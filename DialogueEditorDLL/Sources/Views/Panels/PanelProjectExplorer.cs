using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public class ProjectNodeSorter : IComparer
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

            int insertIndex = 0;
            foreach (DocumentDialogueViewSlot slot in EditorCore.DocumentDialogueViewSlots)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuItem.Text = slot.Text;
                menuItem.Click += delegate
                {
                    // Unfortunately "clever" code to perform the transition
                    // from reflection (the type of the desired view is known)
                    // to generic (we want to call OpenDocument<desiredView>)
                    var genericOpenDocumentMethod = typeof(PanelProjectExplorer).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                        .Where(m => m.Name == "OpenDocument")
                        .Select(m => new
                        {
                            Method = m,
                            GenericArgs = m.GetGenericArguments()
                        })
                        .Where(x => x.GenericArgs.Length == 1)
                        .Select(x => x.Method)
                        .First();
                    genericOpenDocumentMethod.MakeGenericMethod(slot.DocumentDialogueViewType).Invoke(this, new object[] { tree.SelectedNode, false, false });
                };
                menuDocument.Items.Insert(insertIndex++, menuItem);
            }
        }

        public void ResyncAllFiles()
        {
            Clear();

            Project project = ProjectController.Project;
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

            TreeNode nodeProject = tree.Nodes.Add("Project", $"Project {project.Name}");
            nodeProject.Tag = project;
            EditorHelper.SetNodeIcon(nodeProject, ENodeIcon.Project);

            foreach (Package package in project.ListPackages)
            {
                TreeNode nodePackage = tree.Nodes.Add(GetPackageKey(package.Name), package.Name);
                nodePackage.Tag = package;
                nodePackage.ContextMenuStrip = menuPackage;
                EditorHelper.SetNodeIcon(nodePackage, ENodeIcon.Package);
            }

            foreach (Dialogue dialogue in ProjectController.GetAllDialogues())
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

                    TreeNode[] nodeChildren = nodeParent.Nodes.Find(dialogue.FileName, true);
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
                    ProjectController.LogError($"Unable to update Dialogue {dialogue.Name} with unknown Package {previousPackage.Name} in project explorer");
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
                ProjectController.LogError($"Unable to show Dialogue {dialogue.Name} without Package in project explorer");
                return;
            }

            TreeNode[] nodePackages = tree.Nodes.Find(GetPackageKey(dialogue.Package.Name), false);
            if (nodePackages.Count() > 0)
            {
                TreeNode nodeParent = nodePackages[0];

                string path = dialogue.Path;
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

                TreeNode nodeDialogue = nodeParent.Nodes.Add(dialogue.FileName, dialogue.Name);
                nodeDialogue.ContextMenuStrip = menuDocument;
                EditorHelper.SetNodeIcon(nodeDialogue, ENodeIcon.Dialogue);

                nodeDialogue.Tag = dialogue;

                if (focus)
                    nodeDialogue.EnsureVisible();
            }
            else
            {
                ProjectController.LogError($"Unable to show Dialogue {dialogue.Name} with unknown Package {dialogue.Package.Name} in project explorer");
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

            var dialogues = ProjectController.GetAllDialogues();
            foreach (var dialogue in dialogues)
            {
                bool validActor = true;
                if (actor != string.Empty)
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

                if ((fileName == string.Empty || dialogue.Name.ContainsIgnoreCase(fileName))
                 && (string.IsNullOrEmpty(sceneType) || dialogue.SceneType == sceneType)
                 && validActor)
                {
                    listBoxSearchResults.Items.Add(dialogue.Name);
                }
            }

            if (listBoxSearchResults.Items.Count == 0)
                listBoxSearchResults.Items.Add("No Result");

            listBoxSearchResults.EndUpdate();
        }

        private bool OpenDocument(TreeNode node, bool keepFocus, bool forceOpenNewView = false)
        {
            return OpenDocument<DocumentDialogueTreeView>(node, keepFocus, forceOpenNewView);
        }

        private bool OpenDocument<T>(TreeNode node, bool keepFocus, bool forceOpenNewView = false) where T : DocumentDialogueView
        {
            if (node == null)
                return false;

            if (node.Tag is Dialogue)
            {
                if (ProjectController.MainWindow != null)
                {
                    ProjectController.OpenDocumentDialogue<T>(node.Tag as Dialogue, DialogueNode.ID_NULL, forceOpenNewView);
                    if (keepFocus)
                        tree.Focus();
                    return true;
                }
            }
            else if (node.Tag is Project)
            {
                if (ProjectController.MainWindow != null)
                {
                    ProjectController.OpenDocumentProject();
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

            if (!string.IsNullOrEmpty(fileName) || !string.IsNullOrEmpty(sceneType) || !string.IsNullOrEmpty(actor))
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
            if (keyData == Keys.Enter)
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
                        if (ProjectController.MainWindow != null)
                        {
                            var item = listBoxSearchResults.SelectedItem as string;
                            ProjectController.MainWindow.OpenDocumentDialogue(item);
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
            bool forceOpenNewView = false;
#if DEBUG
            forceOpenNewView = (ModifierKeys & Keys.Shift) != 0;
            // NB: due to architecture changes, I can't summon Hollywood view from here anymore
            // (I used to do it for Keys.Control)
#endif
            OpenDocument(e.Node, false, forceOpenNewView);
        }

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            ProjectController.SyncMenuItemFromPanel(this);
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
                    dialog.Filter = "Dialogue Files|*" + Dialogue.Extension;
                    dialog.InitialDirectory = Path.Combine(EditorHelper.GetProjectDirectory(), folder.Path);

                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        Dialogue newDialogue = ProjectController.CreateDialogueFile(dialog.FileName, package);
                        if (newDialogue != null)
                        {
                            ResyncFile(newDialogue, true);

                            ProjectController.MainWindow.OpenDocumentDialogue(newDialogue);
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
                dialog.Filter = "Dialogue Files|*" + Dialogue.Extension;
                dialog.InitialDirectory = EditorHelper.GetProjectDirectory();

                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Dialogue newDialogue = ProjectController.CreateDialogueFile(dialog.FileName, package);
                    if (newDialogue != null)
                    {
                        ResyncFile(newDialogue, true);

                        ProjectController.MainWindow.OpenDocumentDialogue(newDialogue);
                    }
                }
            }
        }

        private void OnDocumentOpenDirectory(object sender, EventArgs e)
        {
            Dialogue dialogue = tree.SelectedNode.Tag as Dialogue;
            if (dialogue != null)
            {
                Process.Start(Path.Combine(EditorHelper.GetProjectDirectory(), dialogue.Path));
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
                Clipboard.SetText(dialogue.Name);
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

                if (ProjectController.MainWindow != null)
                    ProjectController.MainWindow.CloseViews(dialogue, true);

                TreeNode nodeFolder = tree.SelectedNode.Parent;

                tree.Nodes.Remove(tree.SelectedNode);
                tree.SelectedNode = null;

                DeleteIfEmptyFolder(nodeFolder);

                ProjectController.RemoveDialogue(dialogue);

                string filePathName = Path.Combine(EditorHelper.GetProjectDirectory(), dialogue.FullPath);
                if (File.Exists(filePathName))
                    File.Delete(filePathName);
            }
        }

        private void OnNodeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            //Forbid the collapsing of desired nodes
            /*if (e.Node.Tag is Package)
            {
                e.Cancel = true;
            }*/
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

                if (ProjectController.MainWindow != null)
                    ProjectController.MainWindow.OpenDocumentDialogue(item);
                    // TODO: also handle Ctrl/Shift or even a menu here to be able to choose a non-std view
            }
        }
    }
}
