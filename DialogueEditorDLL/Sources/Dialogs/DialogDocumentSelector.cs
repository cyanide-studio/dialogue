using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class DialogDocumentSelector : Form
    {
        //--------------------------------------------------------------------------------------------------------------
        // Helper Class

        private class Folder
        {
            public string Path = "";
        };

        //--------------------------------------------------------------------------------------------------------------
        // Internal Vars

        public List<Dialogue> checkedDialogues = new List<Dialogue>();

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogDocumentSelector(string title, List<Dialogue> inCheckedDialogues = null)
            : base()
        {
            InitializeComponent();

            Text = title;

            if (inCheckedDialogues != null)
                checkedDialogues = inCheckedDialogues;

            dialogueTreeView.ImageList = EditorCore.DefaultImageList;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            populateDialogueTreeView(checkedDialogues);
        }

        private void populateDialogueTreeView(List<Dialogue> inCheckedDialogues)
        {
            var project = ProjectController.Project;

            if (project == null)
                return;

            dialogueTreeView.BeginUpdate();

            foreach (Package package in project.ListPackages)
            {
                TreeNode nodePackage = dialogueTreeView.Nodes.Add(GetPackageKey(package.Name), package.Name);
                nodePackage.Tag = package;
                EditorHelper.SetNodeIcon(nodePackage, ENodeIcon.Package);
            }

            foreach (Dialogue dialogue in ProjectController.GetAllDialogues())
            {
                if (dialogue == null)
                    continue;

                if (dialogue.Package == null)
                    continue;

                TreeNode[] nodePackages = dialogueTreeView.Nodes.Find(GetPackageKey(dialogue.Package.Name), false);
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
                            EditorHelper.SetNodeIcon(nodeParent, ENodeIcon.Folder);
                        }
                    }

                    TreeNode nodeDialogue = nodeParent.Nodes.Add(dialogue.FileName, dialogue.Name);
                    nodeDialogue.Checked = checkedDialogues.Contains(dialogue);
                    nodeDialogue.Tag = dialogue;
                    EditorHelper.SetNodeIcon(nodeDialogue, ENodeIcon.Dialogue);
                }
            }

            dialogueTreeView.EndUpdate();

            foreach (TreeNode node in dialogueTreeView.Nodes)
            {
                node.Expand();
            }
        }

        public List<Dialogue> GetSelectedDialogues()
        {
            List<Dialogue> selectedDialogues = new List<Dialogue>();
            GetSelectedDialogues(dialogueTreeView.Nodes, selectedDialogues);
            return selectedDialogues;
        }

        private void GetSelectedDialogues(TreeNodeCollection nodeCollection, List<Dialogue> dialogues)
        {
            if (nodeCollection.Count != 0)
            {
                foreach (TreeNode childNode in nodeCollection)
                {
                    if (childNode.Tag is Dialogue && childNode.Checked)
                        dialogues.Add(childNode.Tag as Dialogue);
                    GetSelectedDialogues(childNode.Nodes, dialogues);
                }
            }
        }

        private bool GetLeaves(TreeNodeCollection nodeCollection, List<TreeNode> leaves)
        {
            if (nodeCollection.Count == 0)
                return true;

            foreach (TreeNode childNode in nodeCollection)
            {
                if (GetLeaves(childNode.Nodes, leaves))
                    leaves.Add(childNode);
            }

            return false;
        }

        protected string GetPackageKey(string name)
        {
            return "Package_" + name;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            checkedDialogues = GetSelectedDialogues();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
