using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DialogueEditor
{
    public partial class PanelProjectExplorer : DockContent
    {
        //--------------------------------------------------------------------------------------------------------------
        // Helper Class

        private class Folder
        {
            public string Path = "";
        };

        public class ProjectNodeSorter : System.Collections.IComparer
        {
            public int Compare(object left, object right)
            {
                TreeNode nodeLeft = left as TreeNode;
                TreeNode nodeRight = right as TreeNode;

                //Project above Packages
                if (nodeLeft.Tag is Project)
                    return -1;
                if (nodeRight.Tag is Project)
                    return 1;

                return string.Compare(nodeLeft.Text, nodeRight.Text);
            }
        };

        //--------------------------------------------------------------------------------------------------------------
        // Internal Vars

        private Timer timerSearch = null;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public PanelProjectExplorer()
        {
            InitializeComponent();

            listBoxSearchResults.Location = tree.Location;
            listBoxSearchResults.Size = tree.Size;

            comboBoxSearchSceneType.Enabled = false;
            comboBoxSearchActor.Enabled = false;

            timerSearch = new Timer();
            timerSearch.Interval = 500;
            timerSearch.Tick += new EventHandler(delegate
            {
                ProcessSearchName();

                timerSearch.Stop();
            });

            tree.ImageList = EditorCore.DefaultImageList;
            tree.TreeViewNodeSorter = new ProjectNodeSorter();
        }

        public void ResyncAllFiles()
        {
            Clear();

            Project project = ResourcesHandler.Project;
            if (project == null)
                return;

            if (EditorCore.CustomLists["SceneTypes"].Count > 0)
            {
                var sceneTypes = new Dictionary<string, string>();
                sceneTypes.Add("", "");
                foreach (var kvp in EditorCore.CustomLists["SceneTypes"])
                {
                    sceneTypes.Add(kvp.Key, kvp.Value);
                }

                comboBoxSearchSceneType.DataSource = new BindingSource(sceneTypes, null);
                comboBoxSearchSceneType.ValueMember = "Key";
                comboBoxSearchSceneType.DisplayMember = "Value";

                comboBoxSearchSceneType.Enabled = true;
            }

            if (project.ListActors.Count > 0)
            {
                var actors = new Dictionary<string, string>();
                actors.Add("", "");
                foreach (var actor in project.ListActors)
                {
                    actors.Add(actor.ID, actor.Name);
                }

                comboBoxSearchActor.DataSource = new BindingSource(actors, null);
                comboBoxSearchActor.ValueMember = "Key";
                comboBoxSearchActor.DisplayMember = "Value";

                comboBoxSearchActor.Enabled = true;
            }

            tree.BeginUpdate();

            TreeNode nodeProject = tree.Nodes.Add("Project", String.Format("Project {0}", project.GetName()));
            nodeProject.Tag = project;
            EditorHelper.SetNodeIcon(nodeProject, ENodeIcon.Project);

            foreach (Package package in project.ListPackages)
            {
                TreeNode nodePackage = tree.Nodes.Add(GetPackageKey(package.Name), package.Name);
                nodePackage.Tag = package;
                nodePackage.ContextMenuStrip = menuPackage;
                EditorHelper.SetNodeIcon(nodePackage, ENodeIcon.Package);
            }

            foreach (Dialogue dialogue in ResourcesHandler.GetAllDialogues())
            {
                ResyncFile(dialogue, false);
            }

            tree.EndUpdate();

            //tree.CollapseAll();
            foreach (TreeNode node in tree.Nodes)
            {
                node.Expand();
            }
        }

        public void DeleteIfEmptyFolder(TreeNode node)
        {
            if (node != null && node.Tag is Folder && node.Nodes.Count == 0)
            {
                TreeNode parent = node.Parent;
                node.Remove();
                DeleteIfEmptyFolder(parent);
            }
        }

        public void ResyncFile(Dialogue dialogue, Package previousPackage, bool focus)
        {
            if (dialogue == null)
                return;

            if (previousPackage != null)
            {
                TreeNode[] nodePackages = tree.Nodes.Find(GetPackageKey(previousPackage.Name), false);
                if (nodePackages.Count() > 0)
                {
                    TreeNode nodeParent = nodePackages[0];

                    TreeNode[] nodeChildren = nodeParent.Nodes.Find(dialogue.GetFileName(), true);
                    if (nodeChildren.Count() > 0)
                    {
                        TreeNode nodeDialogue = nodeChildren[0];
                        TreeNode nodeFolder = nodeDialogue.Parent;
                        nodeDialogue.Remove();

                        DeleteIfEmptyFolder(nodeFolder);
                    }
                }
                else
                {
                    EditorCore.LogError("Unable to update a Dialogue with unknown Package in project explorer : " + dialogue.GetName() + " in " + previousPackage.Name);
                }
            }

            ResyncFile(dialogue, focus);
        }

        public void ResyncFile(Dialogue dialogue, bool focus)
        {
            if (dialogue == null)
                return;

            if (dialogue.Package == null)
            {
                EditorCore.LogError("Unable to show a Dialogue without Package in project explorer : " + dialogue.GetName());
                return;
            }

            TreeNode[] nodePackages = tree.Nodes.Find(GetPackageKey(dialogue.Package.Name), false);
            if (nodePackages.Count() > 0)
            {
                TreeNode nodeParent = nodePackages[0];

                string path = dialogue.GetFilePath();
                string[] folders = path.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                string folderPath = "";
                foreach (string folder in folders)
                {
                    folderPath = Path.Combine(folderPath, folder);

                    TreeNode[] nodeFolders = nodeParent.Nodes.Find(folder, false);
                    if (nodeFolders.Count() > 0)
                    {
                        nodeParent = nodeFolders[0];
                    }
                    else
                    {
                        nodeParent = nodeParent.Nodes.Add(folder, folder);
                        nodeParent.Tag = new Folder() { Path = folderPath };
                        nodeParent.ContextMenuStrip = menuFolder;
                        EditorHelper.SetNodeIcon(nodeParent, ENodeIcon.Folder);
                    }
                }

                TreeNode nodeDialogue = nodeParent.Nodes.Add(dialogue.GetFileName(), dialogue.GetName());
                nodeDialogue.ContextMenuStrip = menuDocument;
                EditorHelper.SetNodeIcon(nodeDialogue, ENodeIcon.Dialogue);

                nodeDialogue.Tag = dialogue;

                if (focus)
                    nodeDialogue.EnsureVisible();
            }
            else
            {
                EditorCore.LogError("Unable to show a Dialogue with unknown Package in project explorer : " + dialogue.GetName() + " in " + dialogue.Package.Name);
            }
        }

        protected string GetPackageKey(string name)
        {
            return "Package_" + name;
        }

        public void Clear()
        {
            comboBoxSearchSceneType.DataSource = null;
            comboBoxSearchSceneType.Enabled = false;
            comboBoxSearchActor.DataSource = null;
            comboBoxSearchActor.Enabled = false;

            ClearSearchName();

            tree.Nodes.Clear();
        }

        private void ClearSearchName()
        {
            timerSearch.Stop();

            tree.Visible = true;
            listBoxSearchResults.Visible = false;

            listBoxSearchResults.Items.Clear();
        }

        private void ProcessSearchName()
        {
            tree.Visible = false;
            listBoxSearchResults.Visible = true;

            listBoxSearchResults.BeginUpdate();

            listBoxSearchResults.Items.Clear();

            string fileName = textBoxSearchName.Text;
            string sceneType = comboBoxSearchSceneType.SelectedValue as string;
            string actor = comboBoxSearchActor.SelectedValue as string;

            var dialogues = ResourcesHandler.GetAllDialogues();
            foreach (var dialogue in dialogues)
            {
                bool validActor = true;
                if (actor != String.Empty)
                {
                    validActor = false;
                    foreach (var node in dialogue.ListNodes)
                    {
                        var sentence = node as DialogueNodeSentence;
                        if (sentence != null && (sentence.SpeakerID == actor || sentence.ListenerID == actor))
                        {
                            validActor = true;
                            break;
                        }
                    }
                }

                if ((fileName == String.Empty || dialogue.GetName().ContainsIgnoreCase(fileName))
                && (sceneType == String.Empty || dialogue.SceneType == sceneType)
                && validActor)
                {
                    listBoxSearchResults.Items.Add(dialogue.GetName());
                }
            }

            if (listBoxSearchResults.Items.Count == 0)
                listBoxSearchResults.Items.Add("No Result");

            listBoxSearchResults.EndUpdate();
        }

        private bool OpenDocument(TreeNode node, bool keepFocus)
        {
            if (node == null)
                return false;

            if (node.Tag is Dialogue)
            {
                if (EditorCore.MainWindow != null)
                {
                    EditorCore.MainWindow.OpenDocumentDialogue(node.Tag as Dialogue);
                    if (keepFocus)
                        tree.Focus();
                    return true;
                }
            }
            else if (node.Tag is Project)
            {
                if (EditorCore.MainWindow != null)
                {
                    EditorCore.MainWindow.OpenDocumentProject();
                    if (keepFocus)
                        tree.Focus();
                    return true;
                }
            }
            return false;
        }

        private void RefreshSearch(bool tickNow)
        {
            string fileName = textBoxSearchName.Text;
            string sceneType = comboBoxSearchSceneType.SelectedValue as string;
            string actor = comboBoxSearchActor.SelectedValue as string;

            if ((fileName != null && fileName != String.Empty)
            ||  (sceneType != null && sceneType != String.Empty)
            ||  (actor != null && actor != String.Empty))
            {
                timerSearch.Stop();
                if (tickNow)
                {
                    ProcessSearchName();
                }
                else
                {
                    timerSearch.Start();
                }
            }
            else
            {
                ClearSearchName();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events

        public bool ProcessCmdKey_Impl(Keys keyData)
        {
            if (keyData == (Keys.Enter))
            {
                if (tree.Focused)
                {
                    if (OpenDocument(tree.SelectedNode, true))
                    {
                        return true;
                    }
                }
                else if (listBoxSearchResults.Focused)
                {
                    if (listBoxSearchResults.SelectedItem != null)
                    {
                        if (EditorCore.MainWindow != null)
                        {
                            var item = listBoxSearchResults.SelectedItem as string;
                            EditorCore.MainWindow.OpenDocumentDialogue(item);
                            listBoxSearchResults.Focus();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void OnNodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tree.SelectedNode = e.Node;
        }

        private void OnNodeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            OpenDocument(e.Node, false);
        }

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            if (EditorCore.MainWindow != null)
                EditorCore.MainWindow.SyncMenuItemFromPanel(this);
        }

        private void OnFolderNewDialogue(object sender, EventArgs e)
        {
            Folder folder = tree.SelectedNode.Tag as Folder;
            if (folder != null)
            {
                var root = tree.SelectedNode.GetRootNode();
                
                Package package = root.Tag as Package;
                if (package != null)
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Title = "Create Dialogue";
                    dialog.Filter = "Dialogue Files|*" + Dialogue.GetExtension();
                    dialog.InitialDirectory = Path.Combine(EditorHelper.GetProjectDirectory(), folder.Path);

                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        Dialogue newDialogue = ResourcesHandler.CreateDialogueFile(dialog.FileName, package);
                        if (newDialogue != null)
                        {
                            ResyncFile(newDialogue, true);

                            EditorCore.MainWindow.OpenDocumentDialogue(newDialogue);
                        }
                    }
                }
            }
        }

        private void OnPackageNewDialogue(object sender, EventArgs e)
        {
            Package package = tree.SelectedNode.Tag as Package;
            if (package != null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "Create Dialogue";
                dialog.Filter = "Dialogue Files|*" + Dialogue.GetExtension();
                dialog.InitialDirectory = EditorHelper.GetProjectDirectory();

                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Dialogue newDialogue = ResourcesHandler.CreateDialogueFile(dialog.FileName, package);
                    if (newDialogue != null)
                    {
                        ResyncFile(newDialogue, true);

                        EditorCore.MainWindow.OpenDocumentDialogue(newDialogue);
                    }
                }
            }
        }

        private void OnDocumentOpenDirectory(object sender, EventArgs e)
        {
            Dialogue dialogue = tree.SelectedNode.Tag as Dialogue;
            if (dialogue != null)
            {
                Process.Start(Path.Combine(EditorHelper.GetProjectDirectory(), dialogue.GetFilePath()));
            }
        }

        private void OnFolderOpenDirectory(object sender, EventArgs e)
        {
            Folder folder = tree.SelectedNode.Tag as Folder;
            if (folder != null)
            {
                Process.Start(Path.Combine(EditorHelper.GetProjectDirectory(), folder.Path));
            }
        }

        private void OnDocumentShowStats(object sender, EventArgs e)
        {
            Dialogue dialogue = tree.SelectedNode.Tag as Dialogue;
            if (dialogue != null)
            {
                DialogStats dialog = new DialogStats(new List<Dialogue>() { dialogue });
                dialog.ShowDialog();
            }
        }

        private void OnFolderShowStats(object sender, EventArgs e)
        {
            Folder folder = tree.SelectedNode.Tag as Folder;
            if (folder != null)
            {
                var dialogues = new List<Dialogue>();

                var list = tree.SelectedNode.FlattenList(true);
                foreach (var node in list)
                {
                    Dialogue dialogue = node.Tag as Dialogue;
                    if (dialogue != null)
                    {
                        dialogues.Add(dialogue);
                    }
                }

                DialogStats dialog = new DialogStats(dialogues);
                dialog.ShowDialog();
            }
        }

        private void OnPackageShowStats(object sender, EventArgs e)
        {
            Package package = tree.SelectedNode.Tag as Package;
            if (package != null)
            {
                var dialogues = new List<Dialogue>();

                var list = tree.SelectedNode.FlattenList(true);
                foreach (var node in list)
                {
                    Dialogue dialogue = node.Tag as Dialogue;
                    if (dialogue != null)
                    {
                        dialogues.Add(dialogue);
                    }
                }

                DialogStats dialog = new DialogStats(dialogues);
                dialog.ShowDialog();
            }
        }

        private void OnDocumentCopyName(object sender, EventArgs e)
        {
            Dialogue dialogue = tree.SelectedNode.Tag as Dialogue;
            if (dialogue != null)
            {
                Clipboard.SetText(dialogue.GetName());
            }
        }

        private void OnDocumentDelete(object sender, EventArgs e)
        {
            Dialogue dialogue = tree.SelectedNode.Tag as Dialogue;
            if (dialogue != null)
            {
                var dialog = new DialogConfirmDelete(dialogue);
                DialogResult eResult = dialog.ShowDialog();
                if (eResult == DialogResult.Cancel)
                {
                    return;
                }

                if (EditorCore.MainWindow != null)
                    EditorCore.MainWindow.CloseDocumentDialogue(dialogue, true);

                TreeNode nodeFolder = tree.SelectedNode.Parent;

                tree.Nodes.Remove(tree.SelectedNode);
                tree.SelectedNode = null;

                DeleteIfEmptyFolder(nodeFolder);

                ResourcesHandler.RemoveDialogue(dialogue);

                string filePathName = Path.Combine(EditorHelper.GetProjectDirectory(), dialogue.GetFilePathName());
                if (File.Exists(filePathName))
                    File.Delete(filePathName);
            }
        }

        private void OnNodeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            //Forbid the collapsing of desired nodes
            if (e.Node.Tag is Package)
            {
                e.Cancel = true;
            }
        }

        private void OnExpandAll(object sender, EventArgs e)
        {
            tree.SelectedNode.ExpandAll();
        }

        private void OnCollapseAll(object sender, EventArgs e)
        {
            tree.SelectedNode.Collapse(false);
        }

        private void OnTextSearchChanged(object sender, EventArgs e)
        {
            RefreshSearch(false);
        }

        private void OnSearchSceneTypeChanged(object sender, EventArgs e)
        {
            RefreshSearch(true);
        }

        private void OnSearchActorChanged(object sender, EventArgs e)
        {
            RefreshSearch(true);
        }

        private void OnSearchResultDoubleClick(object sender, EventArgs e)
        {
            if (listBoxSearchResults.SelectedItem != null)
            {
                var item = listBoxSearchResults.SelectedItem as string;

                if (EditorCore.MainWindow != null)
                    EditorCore.MainWindow.OpenDocumentDialogue(item);
            }
        }
    }
}
