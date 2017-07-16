using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class DocumentDialogueTreeView : DocumentDialogueView
    {
        #region Helper Classes
        protected class NodeWrap
        {
            public DialogueNode DialogueNode;

            public NodeWrap(DialogueNode dialogueNode)
            {
                DialogueNode = dialogueNode;
            }
        }

        protected class NodeMemo
        {
            public bool isExpanded;

            public NodeMemo(bool inIsExpanded)
            {
                isExpanded = inIsExpanded;
            }
        }

        protected class TreeMemo
        {
            public Dictionary<int, NodeMemo> NodeMemoList;

            public TreeMemo()
            {
                NodeMemoList = new Dictionary<int, NodeMemo>();
            }

            public NodeMemo this[int key]
            {
                get
                {
                    return NodeMemoList[key];
                }
                set
                {
                    NodeMemoList[key] = value;
                }
            }
        }
        
        #endregion

        #region Variables
        protected bool lockCheckDisplayEvents = false;
        protected int copyReference = -1;
        protected Font font = null;
        protected TreeMemo treeMemo = null;
        // Dictionary node ID => TreeNode (the int in the tuple is the number of (direct) cycles detected towards this node)
        protected Dictionary<int, Tuple<TreeNode, int>> treeMap = null;
        // TODO: maybe remove treeMap as a class variable as it's only used during BuildTree
        // (this would involve passing it as a parameter through a few methods though)
        #endregion

        #region Constructor
        public DocumentDialogueTreeView(DialogueController inDialogueController) : base(inDialogueController)
        {
            InitializeComponent();

            tree.ImageList = EditorCore.DefaultImageList;

            // Use this to have multiple colors on a single node.
            // If there are visual glitches, you can try commenting this block.
            // Note: Allowing default rendering will allow drag&drop with left mouse button.
            font = tree.Font;
            tree.DrawMode = TreeViewDrawMode.OwnerDrawText;
            tree.DrawNode += OnTreeViewDrawNode;

            treeMap = new Dictionary<int, Tuple<TreeNode, int>>();

            DialogueController.SaveState(); // TODO: really??
        }
        #endregion

        #region Init
        public override void InitView()
        {
            ResyncDisplayOptions();
            RebuildTree(false);
            SelectRootNode();
            RefreshTitle();
        }
        #endregion

        #region DocumentView overrides
        public override void RefreshDialogueNode(DialogueNode DialogueNode)
        {
            RefreshTreeNode(GetTreeNode(DialogueNode));
        }

        public override void SelectDialogueNode(DialogueNode DialogueNode)
        {
            SelectNode(DialogueNode, true);
        }

        public override void RefreshDocument()
        {
            ResyncDisplayOptions();
            RebuildTree();
            SelectNode(DialogueController.SelectedDialogueNode);
            RestyleSelectedNode();
        }

        public override void OnPostReload()
        {
            RefreshDocument();
        }
        #endregion

        #region Tree building
        private void SaveTreeNodeMemo(TreeNode treeNode)
        {
            DialogueNode dialogueNode = GetDialogueNode(treeNode);
            if (dialogueNode != null && dialogueNode.ID != DialogueController.Dialogue.RootNode.ID)   // Realistically we never want root collapsed anyway
                treeMemo[dialogueNode.ID] = new NodeMemo(treeNode.IsExpanded);
            foreach (TreeNode treeNodeChild in treeNode.Nodes)
            {
                SaveTreeNodeMemo(treeNodeChild);
            }
        }

        private void SaveTreeMemo()
        {
            treeMemo = new TreeMemo();
            foreach (TreeNode treeNode in tree.Nodes)
            {
                SaveTreeNodeMemo(treeNode);
            }
        }

        private bool ShouldExpand(TreeNode treeNode)
        {
            if (treeMemo == null)
                return true;
            DialogueNode dialogueNode = GetDialogueNode(treeNode);
            if (dialogueNode == null || !treeMemo.NodeMemoList.ContainsKey(dialogueNode.ID))
                return true;
            return treeMemo[dialogueNode.ID].isExpanded;
        }

        public void RebuildTree(bool redraw = true)
        {
            WIN32.StopRedraw(this);
            tree.BeginUpdate();

            treeMemo = null;
            if (redraw)
                SaveTreeMemo();
            ClearTree();
            BuildTree();
            treeMemo = null;

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);

            if (redraw)
            {
                RefreshAllTreeNodes();
            }
        }

        public void ClearTree()
        {
            tree.Nodes.Clear();
        }

        private void BuildTree()
        {
            treeMap.Clear();
            if (DialogueController.Dialogue.RootNode != null)
            {
                TreeNode newTreeNodeRoot = InsertTreeNode(DialogueController.Dialogue.RootNode, ENodeIcon.Dialogue);
                AddTreeNodeChild(DialogueController.Dialogue.RootNode.Next, newTreeNodeRoot);
                if (ShouldExpand(newTreeNodeRoot))
                    newTreeNodeRoot.Expand();
            }
        }

        protected TreeNode InsertTreeNode(DialogueNode node, ENodeIcon nodeIcon, TreeNode parentTreeNode = null, int insertIndex = -1)
        {
            TreeNode newTreeNode = null;
            if (treeMap.ContainsKey(node.ID))
            {
                string nodeKey = $"Cycle_{treeMap[node.ID].Item2}_to_{GetNodeKey(node.ID)}";
                if (parentTreeNode == null)
                {
                    newTreeNode = tree.Nodes.Add(nodeKey, "");
                }
                else
                {
                    newTreeNode = parentTreeNode.Nodes.Insert(insertIndex, nodeKey, "");
                }
                newTreeNode.Tag = new NodeWrap(node);
                newTreeNode.ContextMenuStrip = null; // keeping interactivity minimal for those "virtual goto" nodes
                EditorHelper.SetNodeIcon(newTreeNode, nodeIcon);
                treeMap[node.ID] = new Tuple<TreeNode, int>(treeMap[node.ID].Item1, treeMap[node.ID].Item2 + 1);
                return newTreeNode;
            }
            else
            {
                string nodeKey = GetNodeKey(node.ID);
                if (parentTreeNode == null)
                {
                    newTreeNode = tree.Nodes.Add(nodeKey, "");
                }
                else
                {
                    newTreeNode = parentTreeNode.Nodes.Insert(insertIndex, nodeKey, "");
                }
                newTreeNode.Tag = new NodeWrap(node);
                newTreeNode.ContextMenuStrip = contextMenu;
                EditorHelper.SetNodeIcon(newTreeNode, nodeIcon);
                treeMap[node.ID] = new Tuple<TreeNode, int>(newTreeNode, 0);
                return newTreeNode;
            }
        }

        private TreeNode AddTreeNodeChild(DialogueNode node, TreeNode parentTreeNode, bool noRecurs = false)
        {
            if (node == null || parentTreeNode == null)
                return null;

            return AddTreeNode(node, parentTreeNode, null, noRecurs);
        }

        private TreeNode AddTreeNodeSibling(DialogueNode node, TreeNode previousTreeNode, bool noRecurs = false)
        {
            if (node == null || previousTreeNode == null)
                return null;

            return AddTreeNode(node, previousTreeNode.Parent, previousTreeNode, noRecurs);
        }

        private TreeNode AddTreeNode(DialogueNode node, TreeNode parentTreeNode, TreeNode previousTreeNode, bool noRecurs = false)
        {
            if (node == null || parentTreeNode == null)
                return null;

            TreeNode newTreeNode = null;
            int insertIndex = 0;
            if (previousTreeNode != null)
            {
                insertIndex = parentTreeNode.Nodes.IndexOf(previousTreeNode);
                if (insertIndex == -1)
                    insertIndex = 0;
                else
                    ++insertIndex;
            }

            if (treeMap.ContainsKey(node.ID))
            {
                // Cycle (defined in a more capable view) detected
                // Displayed in the form of a very basic "Direct/virtual Goto" tree node
                newTreeNode = InsertTreeNode(node, ENodeIcon.VirtualGoto, parentTreeNode, insertIndex);
            }
            else if (node is DialogueNodeSentence)
            {
                newTreeNode = InsertTreeNode(node, ENodeIcon.Sentence, parentTreeNode, insertIndex);

                if (!noRecurs)
                    AddTreeNodeSibling(node.Next, newTreeNode);
            }
            else if (node is DialogueNodeChoice)
            {
                newTreeNode = InsertTreeNode(node, ENodeIcon.Choice, parentTreeNode, insertIndex);

                foreach (DialogueNodeReply reply in (node as DialogueNodeChoice).Replies)
                {
                    AddTreeNode(reply, newTreeNode, newTreeNode.LastNode);
                }
                if (!noRecurs)
                    AddTreeNodeSibling(node.Next, newTreeNode);
                if (ShouldExpand(newTreeNode))
                    newTreeNode.Expand();
            }
            else if (node is DialogueNodeReply)
            {
                newTreeNode = InsertTreeNode(node, ENodeIcon.Reply, parentTreeNode, insertIndex);

                if (!noRecurs)
                    AddTreeNodeChild(node.Next, newTreeNode);
                if (ShouldExpand(newTreeNode))
                    newTreeNode.Expand();
            }
            else if (node is DialogueNodeGoto)
            {
                newTreeNode = InsertTreeNode(node, ENodeIcon.Goto, parentTreeNode, insertIndex);

                if (!noRecurs)
                    AddTreeNodeSibling(node.Next, newTreeNode);
            }
            else if (node is DialogueNodeBranch)
            {
                newTreeNode = InsertTreeNode(node, ENodeIcon.Branch, parentTreeNode, insertIndex);

                if (!noRecurs)
                {
                    AddTreeNodeChild((node as DialogueNodeBranch).Branch, newTreeNode);
                    AddTreeNodeSibling(node.Next, newTreeNode);
                }
                if (ShouldExpand(newTreeNode))
                    newTreeNode.Expand();
            }

            if (newTreeNode != null)
            {
                RefreshTreeNode_Impl(newTreeNode);
            }

            return newTreeNode;
        }
        #endregion

        #region Dialogue Node events - Add
        private void OnAddNodeSentence(object sender, EventArgs e)
        {
            CreateNodeSentence(tree.SelectedNode, false);
        }

        private void OnAddNodeChoice(object sender, EventArgs e)
        {
            CreateNodeChoice(tree.SelectedNode, false);
        }

        private void OnAddNodeReply(object sender, EventArgs e)
        {
            CreateNodeReply(tree.SelectedNode);
        }

        private void OnAddNodeGoto(object sender, EventArgs e)
        {
            CreateNodeGoto(tree.SelectedNode, false);
        }

        private void OnAddNodeBranch(object sender, EventArgs e)
        {
            CreateNodeBranch(tree.SelectedNode, false);
        }

        private void OnBranchNodeSentence(object sender, EventArgs e)
        {
            CreateNodeSentence(tree.SelectedNode, true);
        }

        private void OnBranchNodeChoice(object sender, EventArgs e)
        {
            CreateNodeChoice(tree.SelectedNode, true);
        }

        private void OnBranchNodeGoto(object sender, EventArgs e)
        {
            CreateNodeGoto(tree.SelectedNode, true);
        }

        private void OnBranchNodeBranch(object sender, EventArgs e)
        {
            CreateNodeBranch(tree.SelectedNode, true);
        }

        private DialogueNode CreateNodeSentence(TreeNode treeNodeFrom, bool branch, string speakerID = null, string listenerID = null)
        {
            DialogueNodeSentence newNode = DialogueController.AddNodeSentence(GetDialogueNode(treeNodeFrom), branch, speakerID, listenerID, this);
            if (newNode != null)    // Add successful
            {
                // Other views are fully rebuilt by the above, but we still prefer to do the hard work ourselves without full rebuild:
                TreeNode newTreeNode = AddNodeSentence(treeNodeFrom, newNode, branch);
                SelectTreeNode(newTreeNode);
            }
            return newNode;
        }

        private DialogueNode CreateNodeChoice(TreeNode treeNodeFrom, bool branch)
        {
            DialogueNodeChoice newNode = DialogueController.AddNodeChoice(GetDialogueNode(treeNodeFrom), branch, this);
            if (newNode != null)    // Add successful
            {
                // Other views are fully rebuilt by the above, but we still prefer to do the hard work ourselves without full rebuild:
                TreeNode newTreeNode = AddNodeChoice(treeNodeFrom, newNode, branch);
                SelectTreeNode(newTreeNode);
            }
            return newNode;
        }

        private DialogueNode CreateNodeReply(TreeNode treeNodeFrom)
        {
            DialogueNodeReply newNode = DialogueController.AddNodeReply(GetDialogueNode(treeNodeFrom), this);
            if (newNode != null)    // Add successful
            {
                // Other views are fully rebuilt by the above, but we still prefer to do the hard work ourselves without full rebuild:
                TreeNode newTreeNode = AddNodeReply(treeNodeFrom, newNode);
                SelectTreeNode(newTreeNode);
            }
            return newNode;
        }

        private DialogueNode CreateNodeGoto(TreeNode treeNodeFrom, bool branch)
        {
            DialogueNodeGoto newNode = DialogueController.AddNodeGoto(GetDialogueNode(treeNodeFrom), branch, this);
            if (newNode != null)    // Add successful
            {
                // Other views are fully rebuilt by the above, but we still prefer to do the hard work ourselves without full rebuild:
                TreeNode newTreeNode = AddNodeGoto(treeNodeFrom, newNode, branch);
                SelectTreeNode(newTreeNode);
            }
            return newNode;
        }

        private DialogueNode CreateNodeBranch(TreeNode treeNodeFrom, bool branch)
        {
            DialogueNodeBranch newNode = DialogueController.AddNodeBranch(GetDialogueNode(treeNodeFrom), branch, this);
            if (newNode != null)    // Add successful
            {
                // Other views are fully rebuilt by the above, but we still prefer to do the hard work ourselves without full rebuild:
                TreeNode newTreeNode = AddNodeBranch(treeNodeFrom, newNode, branch);
                SelectTreeNode(newTreeNode);
            }
            return newNode;
        }
        #endregion

        #region Dialogue Node events - Remove
        protected void OnDeleteNode(object sender, EventArgs e)
        {
            RemoveSelectedNode();
        }

        public void RemoveSelectedNode()
        {
            RemoveNode(tree.SelectedNode);
        }

        private void RemoveNode(TreeNode treeNode)
        {
            DialogueNode node = GetDialogueNode(treeNode);

            if (node != null && copyReference == node.ID)
                copyReference = -1;

            bool removed = DialogueController.RemoveNode(node, this);
            if (removed)
            {
                // Other views are fully rebuilt by the above, but we still prefer to do the hard work ourselves without full rebuild:
                WIN32.StopRedraw(this);
                tree.BeginUpdate();

                treeNode.Nodes.Remove(treeNode);
                RefreshAllTreeNodes();
                RestyleSelectedNode();

                tree.EndUpdate();
                WIN32.ResumeRedraw(this);
                Refresh();
            }
        }

        public void RemoveAllNodes()
        {
            copyReference = -1;

            bool removed = DialogueController.RemoveAllNodes(this);
            if (removed)
            {
                GetRootNode().Nodes.Clear();
            }
        }
        #endregion

        #region Dialogue Node events - Fine view maintenance (view not fully rebuilt)
        private TreeNode AddNodeSentence(TreeNode treeNodeFrom, DialogueNodeSentence sentence, bool branch)
        {
            if (treeNodeFrom == null || sentence == null)
                return null;

            WIN32.StopRedraw(this);
            tree.BeginUpdate();

            TreeNode newTreeNode = null;
            if (branch || IsTreeNodeRoot(treeNodeFrom) || IsTreeNodeReply(treeNodeFrom))
            {
                newTreeNode = AddTreeNodeChild(sentence, treeNodeFrom, true);
                treeNodeFrom.Expand();
            }
            else
            {
                newTreeNode = AddTreeNodeSibling(sentence, treeNodeFrom, true);
            }

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);
            Refresh();

            return newTreeNode;
        }

        public virtual TreeNode AddNodeChoice(TreeNode treeNodeFrom, DialogueNodeChoice choice, bool branch)
        {
            if (treeNodeFrom == null || choice == null)
                return null;

            WIN32.StopRedraw(this);
            tree.BeginUpdate();

            TreeNode newTreeNode = null;
            if (branch || IsTreeNodeRoot(treeNodeFrom) || IsTreeNodeReply(treeNodeFrom))
            {
                newTreeNode = AddTreeNodeChild(choice, treeNodeFrom, true);
                treeNodeFrom.Expand();
            }
            else
            {
                newTreeNode = AddTreeNodeSibling(choice, treeNodeFrom, true);
            }

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);
            Refresh();

            return newTreeNode;
        }

        public virtual TreeNode AddNodeReply(TreeNode treeNodeFrom, DialogueNodeReply reply)
        {
            if (!IsTreeNodeChoice(treeNodeFrom) || reply == null)
                return null;

            WIN32.StopRedraw(this);
            tree.BeginUpdate();

            TreeNode newTreeNode = null;
            newTreeNode = AddTreeNode(reply, treeNodeFrom, treeNodeFrom.LastNode, true);
            treeNodeFrom.Expand();

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);
            Refresh();

            return newTreeNode;
        }

        public virtual TreeNode AddNodeGoto(TreeNode treeNodeFrom, DialogueNodeGoto nodeGoto, bool branch)
        {
            if (treeNodeFrom == null || nodeGoto == null)
                return null;

            WIN32.StopRedraw(this);
            tree.BeginUpdate();

            TreeNode newTreeNode = null;
            if (branch || IsTreeNodeRoot(treeNodeFrom) || IsTreeNodeReply(treeNodeFrom))
            {
                newTreeNode = AddTreeNodeChild(nodeGoto, treeNodeFrom, true);
                treeNodeFrom.Expand();
            }
            else
            {
                newTreeNode = AddTreeNodeSibling(nodeGoto, treeNodeFrom, true);
            }

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);
            Refresh();

            return newTreeNode;
        }

        public virtual TreeNode AddNodeBranch(TreeNode treeNodeFrom, DialogueNodeBranch nodeBranch, bool branch)
        {
            if (treeNodeFrom == null || nodeBranch == null)
                return null;

            WIN32.StopRedraw(this);
            tree.BeginUpdate();

            TreeNode newTreeNode = null;
            if (branch || IsTreeNodeRoot(treeNodeFrom) || IsTreeNodeReply(treeNodeFrom))
            {
                newTreeNode = AddTreeNodeChild(nodeBranch, treeNodeFrom, true);
                treeNodeFrom.Expand();
            }
            else
            {
                newTreeNode = AddTreeNodeSibling(nodeBranch, treeNodeFrom, true);
            }

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);
            Refresh();

            return newTreeNode;
        }
        #endregion

        #region Dialogue Node events - Move
        private enum EMoveTreeNode
        {
            Sibling,
            Drop,
            DropSpecial,    //TODO: gotta implement move as branch and/or copy/paste reference via drag&drop!
        }

        private void MoveTreeNode(TreeNode nodeMoved, TreeNode nodeTo, EMoveTreeNode moveType)
        {
            bool asBranch = false;
            if (IsTreeNodeBranch(nodeTo))
            {
                if (moveType == EMoveTreeNode.Sibling && nodeMoved?.Parent == nodeTo ||
                    moveType == EMoveTreeNode.DropSpecial)
                {
                    // We are using a move up/down on a node inside a targeted branch
                    // Or using a special drop to force the branch target
                    asBranch = true;
                }
            } 

            bool moved = DialogueController.MoveNode(GetDialogueNode(nodeMoved), GetDialogueNode(nodeTo), 
                                                     GetDialogueNode(nodeMoved?.Parent), GetDialogueNode(nodeTo?.Parent),
                                                     GetDialogueNode(nodeMoved?.PrevNode),
                                                     asBranch, this);
            if (moved)
            {
                // Other views are fully rebuilt by the above, but we still prefer to do the hard work ourselves without full rebuild:
                WIN32.StopRedraw(this);
                tree.BeginUpdate();

                if (IsTreeNodeReply(nodeMoved))
                {
                    TreeNode nodeChoiceFrom = nodeMoved.Parent;

                    if (IsTreeNodeReply(nodeTo))
                    {
                        TreeNode nodeChoiceTo = nodeTo.Parent;

                        // Remove reply from its choice...
                        nodeChoiceFrom.Nodes.Remove(nodeMoved);
                        // ... and insert reply after other reply inside other choice
                        nodeChoiceTo.Nodes.Insert(nodeChoiceTo.Nodes.IndexOf(nodeTo) + 1, nodeMoved);
                    }
                    else if (IsTreeNodeChoice(nodeTo))
                    {
                        TreeNode nodeChoiceTo = nodeTo;

                        // Remove reply from its choice...
                        nodeChoiceFrom.Nodes.Remove(nodeMoved);
                        // ... and insert reply as first reply of other choice
                        nodeChoiceTo.Nodes.Insert(0, nodeMoved);
                    }
                }
                else
                {
                    TreeNode nodeParentFrom = nodeMoved.Parent;
                    TreeNode nodePrev = nodeMoved.PrevNode;

                    // Remove node from current position...
                    if (nodePrev != null)
                    {
                        nodeParentFrom.Nodes.Remove(nodeMoved);
                    }
                    else
                    {
                        if (IsTreeNodeBranch(nodeParentFrom) && nodeParentFrom.FirstNode == nodeMoved)
                        {
                            // Node is a branch child, we need to redirect the branch
                            nodeParentFrom.Nodes.Remove(nodeMoved);
                        }
                        else
                        {
                            nodeParentFrom.Nodes.Remove(nodeMoved);
                        }
                    }

                    // ... and insert node on new position
                    if (IsTreeNodeRoot(nodeTo) || IsTreeNodeReply(nodeTo) || asBranch)
                    {
                        nodeTo.Nodes.Insert(0, nodeMoved);
                    }
                    else
                    {
                        TreeNode nodeParentTo = nodeTo.Parent;
                        nodeParentTo.Nodes.Insert(nodeParentTo.Nodes.IndexOf(nodeTo) + 1, nodeMoved);
                    }
                }
                tree.EndUpdate();
                WIN32.ResumeRedraw(this);
                Refresh();

                SelectTreeNode(nodeMoved);
            }
        }

        protected virtual void OnMoveNodeUp(object sender, EventArgs e)
        {
            if (tree.SelectedNode.PrevNode != null)
            {
                TreeNode nodeTarget = tree.SelectedNode.PrevNode.PrevNode ?? tree.SelectedNode.Parent;
                MoveTreeNode(tree.SelectedNode, nodeTarget, EMoveTreeNode.Sibling);
            }
        }

        protected virtual void OnMoveNodeDown(object sender, EventArgs e)
        {
            if (tree.SelectedNode.NextNode != null)
            {
                MoveTreeNode(tree.SelectedNode, tree.SelectedNode.NextNode, EMoveTreeNode.Sibling);
            }
        }
        #endregion

        #region Dialogue Node events - Select
        public void SelectNode(int id)
        {
            SelectTreeNode(GetTreeNode(id));
        }

        public void SelectNode(DialogueNode node, bool force = false)
        {
            SelectTreeNode(GetTreeNode(node), force);
        }

        public void SelectRootNode()
        {
            SelectTreeNode(GetRootNode());
        }

        public virtual void SelectTreeNode(TreeNode treeNode, bool force = false)
        {
            if (treeNode == null)
                return;

            if (force)
                tree.SelectedNode = null;   // Making sure AfterSelect is always triggered
            tree.SelectedNode = treeNode;
        }

        public void RestyleSelectedNode()
        {
            if (tree.SelectedNode == null)
                return;
            
            UnHighlightAll();

            DialogueNodeGoto nodeGoto = GetSelectedDialogueNode() as DialogueNodeGoto;
            if (nodeGoto != null)
            {
                Highlight(nodeGoto.Goto);

                List<DialogueNode> gotos = DialogueController.Dialogue.GetGotoReferencesOnNode(nodeGoto.Goto);
                Highlight(gotos, Color.DarkGray);
            }
            else
            {
                List<DialogueNode> gotos = DialogueController.Dialogue.GetGotoReferencesOnNode(GetSelectedDialogueNode());
                if (gotos.Count > 0)
                {
                    Highlight(gotos, Color.DarkGray);
                }
            }
        }
        #endregion

        #region Refresh
        public void RefreshAllTreeNodes()
        {
            if (tree.Nodes.Count > 0)
                RefreshAllTreeNodes(tree.Nodes[0]);
        }

        public void RefreshAllTreeNodes(TreeNode parent)
        {
            WIN32.StopRedraw(this);
            tree.BeginUpdate();

            RefreshTreeNode_Impl(parent);
            RefreshAllTreeNodes_Impl(parent);

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);
            Refresh();
        }

        public void RefreshTreeNode(TreeNode treeNode)
        {
            WIN32.StopRedraw(this);
            tree.BeginUpdate();

            RefreshTreeNode_Impl(treeNode);

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);
            Refresh();
        }

        private void RefreshAllTreeNodes_Impl(TreeNode parent)
        {
            foreach (TreeNode node in parent.Nodes)
            {
                RefreshTreeNode_Impl(node, true);
                RefreshAllTreeNodes_Impl(node);
            }
        }

        private void RefreshTreeNode_Impl(TreeNode treeNode, bool isTreeRefresh = false)
        {
            if (treeNode == null)
                return;

            DialogueNode dialogueNode = GetDialogueNode(treeNode);

            if (treeNode.Name.StartsWith("Cycle_"))
            {
                // Those nodes look almost like Gotos
                treeNode.NodeFont = new Font(font, FontStyle.Italic);
                treeNode.ForeColor = GetTreeNodeColorContent(dialogueNode);
                treeNode.BackColor = tree.BackColor;
                if (EditorCore.Settings.DisplayID)
                    treeNode.Text = string.Format("Direct/virtual Goto > [{0}] {1}", dialogueNode.ID, GetTreeNodeTextContent(dialogueNode));
                else
                    treeNode.Text = string.Format("Direct/virtual Goto > {0}", GetTreeNodeTextContent(dialogueNode));
            }
            else
            {
                //Refresh Goto nodes targeting this node (only if not inside a recursive parsing)
                if (!isTreeRefresh)
                {
                    List<DialogueNode> gotos = DialogueController.Dialogue.GetGotoReferencesOnNode(dialogueNode);
                    foreach (var nodeGoto in gotos)
                        RefreshTreeNode_Impl(GetTreeNode(nodeGoto));
                }

                //Style
                FontStyle style = FontStyle.Regular;

                if (dialogueNode is DialogueNodeRoot
                || dialogueNode is DialogueNodeChoice
                || dialogueNode is DialogueNodeGoto
                || dialogueNode is DialogueNodeBranch)
                {
                    style |= FontStyle.Italic;
                }

                if (DialogueController.Dialogue.IsNodeReferencedByGoto(dialogueNode))
                {
                    style |= FontStyle.Bold;
                }

                Color color = GetTreeNodeColorContent(dialogueNode);

                treeNode.NodeFont = new Font(font, style);
                treeNode.ForeColor = color;
                treeNode.BackColor = tree.BackColor;

                //Text (I need to fill the line for the drag & drop to work)
                {
                    string textID = GetTreeNodeTextID(dialogueNode);
                    string textAttributes = GetTreeNodeTextAttributes(dialogueNode);
                    string textActors = GetTreeNodeTextActors(dialogueNode);
                    string textContent = GetTreeNodeTextContent(dialogueNode);

                    // NB: still gotta set a decent Text value even if OnTreeViewDrawNode kind of "overrides" it
                    treeNode.Text = textID + textAttributes + textActors + textContent;
                }
            }
        }

        private void ResyncDisplayOptions()
        {
            // TODO: a lot of stuff happening below is not view-specific and should be transfered to e.g. controller
            lockCheckDisplayEvents = true;

            if (ProjectController.Project != null)
            {
                var listLanguages = new List<Language>() { EditorCore.LanguageWorkstring };
                listLanguages.AddRange(ProjectController.Project.ListLanguages);

                if (EditorHelper.CurrentLanguage == null)
                    EditorHelper.CurrentLanguage = EditorCore.LanguageWorkstring;

                comboBoxLanguages.DataSource = new BindingSource(listLanguages, null);
                comboBoxLanguages.DisplayMember = "Name";
                comboBoxLanguages.SelectedItem = EditorHelper.CurrentLanguage;
            }

            if (EditorHelper.CurrentFont == null)
                EditorHelper.CurrentFont = tree.Font;

            RefreshFont();

            checkBoxDisplaySpeaker.Checked = EditorCore.Settings.DisplaySpeaker;
            checkBoxDisplayListener.Checked = EditorCore.Settings.DisplayListener;
            checkBoxDisplayID.Checked = EditorCore.Settings.DisplayID;
            checkBoxDisplayConditions.Checked = EditorCore.Settings.DisplayConditions;
            checkBoxDisplayActions.Checked = EditorCore.Settings.DisplayActions;
            checkBoxDisplayFlags.Checked = EditorCore.Settings.DisplayFlags;
            checkBoxDisplayText.Checked = EditorCore.Settings.DisplayText;
            checkBoxUseActorColors.Checked = EditorCore.Settings.UseActorColors;
            checkBoxUseConstants.Checked = EditorCore.Settings.UseConstants;

            lockCheckDisplayEvents = false;
        }
        #endregion

        #region Display / Style
        public Color GetTreeNodeColorContent(DialogueNode dialogueNode)
        {
            if (dialogueNode is DialogueNodeRoot)
            {
                return Color.Black;
            }
            else if (dialogueNode is DialogueNodeSentence)
            {
                if (EditorCore.Settings.UseActorColors)
                {
                    Actor speaker = ProjectController.Project.GetActorFromID((dialogueNode as DialogueNodeSentence).SpeakerID);
                    if (speaker != null)
                        return Color.FromArgb(speaker.Color);
                }
            }
            else if (dialogueNode is DialogueNodeChoice)
            {
                return Color.FromArgb(220, 100, 0);   //Orange
            }
            else if (dialogueNode is DialogueNodeReply)
            {
                return Color.FromArgb(220, 0, 0);     //Red
            }
            else if (dialogueNode is DialogueNodeGoto)
            {
                if ((dialogueNode as DialogueNodeGoto).Goto != null)
                    return GetTreeNodeColorContent((dialogueNode as DialogueNodeGoto).Goto);
            }
            else if (dialogueNode is DialogueNodeBranch)
            {
                return Color.FromArgb(220, 100, 0);   //Orange
            }

            return Color.FromArgb(0, 0, 220);   //Blue
        }

        public string GetTreeNodeTextID(DialogueNode dialogueNode)
        {
            //if (dialogueNode is DialogueNodeRoot)
            //    return "";

            if (EditorCore.Settings.DisplayID)
                return string.Format("[{0}] ", dialogueNode.ID);

            return "";
        }

        public string GetTreeNodeTextAttributes(DialogueNode dialogueNode)
        {
            //if (dialogueNode is DialogueNodeRoot)
            //    return "";

            StringBuilder stringBuilder = new StringBuilder();

            if (EditorCore.Settings.DisplayConditions)
            {
                foreach (NodeCondition condition in dialogueNode.Conditions)
                    stringBuilder.AppendFormat("[{0}] ", condition.GetDisplayText());
            }

            if (EditorCore.Settings.DisplayActions)
            {
                foreach (NodeAction action in dialogueNode.Actions)
                    stringBuilder.AppendFormat("[{0}] ", action.GetDisplayText());
            }

            if (EditorCore.Settings.DisplayFlags)
            {
                foreach (NodeFlag flag in dialogueNode.Flags)
                    stringBuilder.AppendFormat("[{0}] ", flag.GetDisplayText());
            }

            return stringBuilder.ToString();
        }

        public string GetTreeNodeTextActors(DialogueNode dialogueNode)
        {
            if (dialogueNode is DialogueNodeSentence)
            {
                DialogueNodeSentence nodeSentence = dialogueNode as DialogueNodeSentence;
                StringBuilder stringBuilder = new StringBuilder();

                if (EditorCore.Settings.DisplaySpeaker)
                    stringBuilder.AppendFormat("[{0}] ", ProjectController.Project.GetActorName(nodeSentence.SpeakerID));
                if (EditorCore.Settings.DisplayListener)
                    stringBuilder.AppendFormat("[{0}] ", ProjectController.Project.GetActorName(nodeSentence.ListenerID));

                return stringBuilder.ToString();
            }
            return "";
        }

        private string GetTreeNodeTextContent(DialogueNode dialogueNode)
        {
            if (dialogueNode is DialogueNodeRoot)
                return "Root";

            if (!EditorCore.Settings.DisplayText)
                return "";

            if (dialogueNode is DialogueNodeSentence)
            {
                DialogueNodeSentence nodeSentence = dialogueNode as DialogueNodeSentence;

                if (EditorHelper.CurrentLanguage != EditorCore.LanguageWorkstring)
                {
                    var entry = DialogueController.Dialogue.Translations.GetNodeEntry(dialogueNode, EditorHelper.CurrentLanguage);
                    if (entry != null)
                    {
                        if (EditorCore.Settings.UseConstants)
                            return EditorHelper.FormatTextEntry(entry.Text, EditorHelper.CurrentLanguage);
                        else
                            return entry.Text;
                    }
                }
                else
                {
                    if (EditorCore.Settings.UseConstants)
                        return EditorHelper.FormatTextEntry(nodeSentence.Sentence, EditorCore.LanguageWorkstring);
                    else
                        return nodeSentence.Sentence;
                }
            }
            else if (dialogueNode is DialogueNodeChoice)
            {
                DialogueNodeChoice nodeChoice = dialogueNode as DialogueNodeChoice;
                return string.Format("Choice > {0}", nodeChoice.Choice);
            }
            else if (dialogueNode is DialogueNodeReply)
            {
                DialogueNodeReply nodeReply = dialogueNode as DialogueNodeReply;
                if (EditorHelper.CurrentLanguage != EditorCore.LanguageWorkstring)
                {
                    var entry = DialogueController.Dialogue.Translations.GetNodeEntry(dialogueNode, EditorHelper.CurrentLanguage);
                    if (entry != null)
                    {
                        if (EditorCore.Settings.UseConstants)
                            return EditorHelper.FormatTextEntry(entry.Text, EditorHelper.CurrentLanguage);
                        else
                            return entry.Text;
                    }
                }
                else
                {
                    if (EditorCore.Settings.UseConstants)
                        return EditorHelper.FormatTextEntry(nodeReply.Reply, EditorCore.LanguageWorkstring);
                    else
                        return nodeReply.Reply;
                }
            }
            else if (dialogueNode is DialogueNodeGoto)
            {
                DialogueNodeGoto nodeGoto = dialogueNode as DialogueNodeGoto;
                if (nodeGoto.Goto == null)
                    return "Goto > Undefined";
                else if (EditorCore.Settings.DisplayID)
                    return string.Format("Goto > [{0}] {1}", nodeGoto.Goto.ID, GetTreeNodeTextContent(nodeGoto.Goto));
                else
                    return string.Format("Goto > {0}", GetTreeNodeTextContent(nodeGoto.Goto));
            }
            else if (dialogueNode is DialogueNodeBranch)
            {
                DialogueNodeBranch nodeBranch = dialogueNode as DialogueNodeBranch;
                return string.Format("Branch > {0}", nodeBranch.Workstring);
            }

            return "";
        }

        private void OnTreeViewDrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            // TODO: Refactoring between this and RefreshTreeNode_Impl
            e.DrawDefault = false;

            var node = e.Node;
            var dialogueNode = GetDialogueNode(node);

            Font nodeFont = node.NodeFont;
            if (nodeFont == null)
                nodeFont = font;

            string textID = string.Empty;
            string textAttributes = string.Empty;
            string textActors = string.Empty;
            string textContent = string.Empty;

            if (node.Name.StartsWith("Cycle_"))
            {
                // Those nodes look almost like Gotos
                node.NodeFont = new Font(font, FontStyle.Italic);
                node.ForeColor = GetTreeNodeColorContent(dialogueNode);
                node.BackColor = tree.BackColor;
                if (EditorCore.Settings.DisplayID)
                    textContent = string.Format("Direct/virtual Goto > [{0}] {1}", dialogueNode.ID, GetTreeNodeTextContent(dialogueNode));
                else
                    textContent = string.Format("Direct/virtual Goto > {0}", GetTreeNodeTextContent(dialogueNode));
            }
            else
            {
                // Retrieve texts
                textID = GetTreeNodeTextID(dialogueNode);
                textAttributes = GetTreeNodeTextAttributes(dialogueNode);
                textActors = GetTreeNodeTextActors(dialogueNode);
                textContent = GetTreeNodeTextContent(dialogueNode);
            }

            // Highlight
            Rectangle bounds = node.Bounds;
            bounds.Width = 0;
            int nbParts = 0;

            if (textID != string.Empty)
            {
                bounds.Width += TextRenderer.MeasureText(e.Graphics, textID, nodeFont).Width;
                //bounds.Width += e.Graphics.MeasureString(textID, nodeFont).ToSize().Width;
                nbParts += 1;
            }

            if (textAttributes != string.Empty)
            {
                bounds.Width += TextRenderer.MeasureText(e.Graphics, textAttributes, nodeFont).Width;
                //bounds.Width += e.Graphics.MeasureString(textAttributes, nodeFont).ToSize().Width;
                nbParts += 1;
            }

            if (textActors != string.Empty)
            {
                bounds.Width += TextRenderer.MeasureText(e.Graphics, textActors, nodeFont).Width;
                //bounds.Width += e.Graphics.MeasureString(textActors, nodeFont).ToSize().Width;
                nbParts += 1;
            }

            if (textContent != string.Empty)
            {
                bounds.Width += TextRenderer.MeasureText(e.Graphics, textContent, nodeFont).Width;
                //bounds.Width += e.Graphics.MeasureString(textContent, nodeFont).ToSize().Width;
                nbParts += 1;
            }

            //if (nbParts > 1)
            //    bounds.Width -= 6 * (nbParts - 1);  //Hack to adjust actual width and match drawn content
            bounds.Width = (int)(bounds.Width * 1.05);
            bounds.Width = tree.Width;

            if ((e.State & TreeNodeStates.Selected) != 0)
            //if ((e.State & TreeNodeStates.Focused) != 0)
            {
                e.Graphics.FillRectangle(Brushes.LightBlue, bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(node.BackColor), bounds);
            }

            // Draw Text
            Point location = bounds.Location;
            DrawText(ref location, textID, e.Graphics, nodeFont, Color.Black);
            DrawText(ref location, textAttributes, e.Graphics, nodeFont, Color.MediumOrchid);
            DrawText(ref location, textActors, e.Graphics, nodeFont, Color.DimGray);
            DrawText(ref location, textContent, e.Graphics, nodeFont, GetTreeNodeColorContent(dialogueNode));
        }

        private void DrawText(ref Point location, string text, Graphics g, Font font, Color color)
        {
            if (text != null && text != string.Empty)
            {
                int width = TextRenderer.MeasureText(g, text, font).Width - 6;  //Hack to adjust positions offset
                TextRenderer.DrawText(g, text, font, location, color);          //This rendering seems better than g.DrawString
                //int width = g.MeasureString(text, font).ToSize().Width;
                //SolidBrush brush = new SolidBrush(color);
                //g.DrawString(text, font, brush, location);
                location.X += width;
            }
        }

        public void RefreshFont()
        {
            if (font != EditorHelper.CurrentFont)
            {
                font = EditorHelper.CurrentFont;
                tree.Font = font;
            }

            labelFont.Text = string.Format("{0} {1}", font.Name, font.Size);
        }

        private void OnChangeFont(object sender, EventArgs e)
        {
            if (lockCheckDisplayEvents)
                return;

            var dialog = new FontDialog();
            dialog.Font = font;

            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                EditorHelper.CurrentFont = dialog.Font;

                RefreshFont();
                RefreshAllTreeNodes();
            }
        }

        public void Highlight(TreeNode node)
        {
            Highlight(node, Color.DarkGray);
        }

        public void Highlight(TreeNode node, Color color)
        {
            if (node != null && node.BackColor != color)   //Avoid tree redraw
                node.BackColor = color;
        }

        public void Highlight(DialogueNode node)
        {
            Highlight(GetTreeNode(node));
        }

        public void Highlight(DialogueNode node, Color color)
        {
            Highlight(GetTreeNode(node), color);
        }
        
        public void Highlight(List<DialogueNode> nodes, Color color)
        {
            foreach (DialogueNode node in nodes)
            {
                Highlight(node, color);
            }
        }

        public void UnHighlight(TreeNode node)
        {
            if (node != null && node.BackColor != tree.BackColor)   //Avoid tree redraw
                node.BackColor = tree.BackColor;
        }

        public void UnHighlightAll()
        {
            UnHighlightAll(tree.Nodes);
        }

        public void UnHighlightAll(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                UnHighlight(node);
                UnHighlightAll(node.Nodes);
            }
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            if (lockCheckDisplayEvents)
                return;

            var language = comboBoxLanguages.SelectedItem as Language;
            EditorHelper.CurrentLanguage = language;

            RefreshAllTreeNodes();
        }

        private void OnCheckDisplayOptions(object sender, EventArgs e)
        {
            if (lockCheckDisplayEvents)
                return;

            EditorCore.Settings.DisplaySpeaker = checkBoxDisplaySpeaker.Checked;
            EditorCore.Settings.DisplayListener = checkBoxDisplayListener.Checked;
            EditorCore.Settings.DisplayConditions = checkBoxDisplayConditions.Checked;
            EditorCore.Settings.DisplayActions = checkBoxDisplayActions.Checked;
            EditorCore.Settings.DisplayFlags = checkBoxDisplayFlags.Checked;
            EditorCore.Settings.DisplayID = checkBoxDisplayID.Checked;
            EditorCore.Settings.DisplayText = checkBoxDisplayText.Checked;
            EditorCore.Settings.UseActorColors = checkBoxUseActorColors.Checked;
            EditorCore.Settings.UseConstants = checkBoxUseConstants.Checked;

            RefreshAllTreeNodes();
            //TODO: as those options are EditorCore i.e. shared between views, either be careful about sync between views or make a controller action...
        }
        #endregion

        #region Helpers
        protected TreeNode GetRootNode()
        {
            if (tree.Nodes.Count > 0)
                return tree.Nodes[0];    // GetNodeKey(m_pDialogue.RootNode.ID);
            return null;
        }

        public TreeNode GetTreeNode(DialogueNode dialogueNode)
        {
            if (dialogueNode != null)
            {
                return GetTreeNode(dialogueNode.ID);
            }
            return null;
        }

        public TreeNode GetTreeNode(int dialogueID)
        {
            if (dialogueID != -1 && tree.Nodes.Count > 0)
            {
                TreeNode[] result = tree.Nodes.Find(GetNodeKey(dialogueID), true);
                if (result.Count() > 0)
                    return result.First();
            }
            return null;
        }

        private DialogueNode GetDialogueNode(TreeNode node)
        {
            if (node != null)
            {
                return ((NodeWrap)node.Tag).DialogueNode;
            }
            return null;
        }

        private DialogueNode GetSelectedDialogueNode()
        {
            return GetDialogueNode(tree.SelectedNode);
        }

        protected bool IsTreeNodeRoot(TreeNode node)
        {
            return GetDialogueNode(node) is DialogueNodeRoot;
        }

        protected bool IsTreeNodeSentence(TreeNode node)
        {
            return GetDialogueNode(node) is DialogueNodeSentence;
        }

        protected bool IsTreeNodeChoice(TreeNode node)
        {
            return GetDialogueNode(node) is DialogueNodeChoice;
        }

        protected bool IsTreeNodeReply(TreeNode node)
        {
            return GetDialogueNode(node) is DialogueNodeReply;
        }

        protected bool IsTreeNodeGoto(TreeNode node)
        {
            return GetDialogueNode(node) is DialogueNodeGoto;
        }

        protected bool IsTreeNodeBranch(TreeNode node)
        {
            return GetDialogueNode(node) is DialogueNodeBranch;
        }

        private void OnOpenDirectory(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(EditorHelper.GetProjectDirectory(), DialogueController.Dialogue.GetFilePath()));
        }

        private void OnCopyName(object sender, EventArgs e)
        {
            Clipboard.SetText(DialogueController.Dialogue.GetName());
        }
        #endregion

        #region Interactions - Keyboard
        public bool ProcessCmdKey_Impl(Keys keyData)
        {
            if (tree.Focused && (keyData == Keys.F2 || keyData == Keys.Enter))
            {
                if (tree.SelectedNode != null)
                {
                    DialogueController.EditNode(GetSelectedDialogueNode());
                }
            }
            else if (!tree.Focused && keyData == Keys.F2)
            {
                tree.Focus();
                return true;
            }
            else if (keyData == Keys.Enter)
            {
                if (tree.SelectedNode != null && ProjectController.IsEditingWorkstring())
                {
                    if (IsTreeNodeSentence(tree.SelectedNode))
                    {
                        var next = GetSelectedDialogueNode().Next;
                        if (next != null)
                        {
                            DialogueController.EditNode(next);
                        }
                    }
                    return true;
                }
            }
            else if (keyData == (Keys.Control | Keys.Enter) || keyData == (Keys.Control | Keys.Shift | Keys.Enter))
            {
                if (tree.SelectedNode != null && IsTreeNodeSentence(tree.SelectedNode))
                {
                    DialogueNodeSentence prevSentence = GetSelectedDialogueNode() as DialogueNodeSentence;
                    string speakerID = prevSentence.SpeakerID;
                    string listenerID = prevSentence.ListenerID;
                    if (keyData == (Keys.Control | Keys.Shift | Keys.Enter))
                    {
                        // (Shift is used to create a symmetric cue/sentence (back and forth dialogue) - so swap:)
                        string tempID = speakerID;
                        speakerID = listenerID;
                        listenerID = tempID;
                    }
                    DialogueNode newNode = CreateNodeSentence(tree.SelectedNode, false, speakerID, listenerID);   //NB: ideally this combined with the below tweaks should be a single controller command...
                    if (newNode != null)
                    {
                        DialogueController.EditNode(newNode, true);
                    }
                    return true;
                }
            }

            return false;   //let the caller forward the message to the base class
        }

        protected virtual void OnKeyDown(object sender, KeyEventArgs e)
        {
            var dialogueNode = GetSelectedDialogueNode();
            if (dialogueNode == null)
                return;

            if (e.Control && (e.KeyCode == Keys.C || e.KeyCode == Keys.X))
            {
                e.Handled = true;
                DialogueController.CopyDialogueNode(dialogueNode);
                if (e.KeyCode == Keys.X)
                {
                    if (dialogueNode is DialogueNodeRoot)
                    {
                        RemoveAllNodes();
                    }
                    else
                    {
                        RemoveSelectedNode();
                    }
                }
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                e.Handled = true;
                bool pasted = DialogueController.PasteToDialogueNode(dialogueNode, e.Shift /*, this*/);
                if (pasted)
                {
                    // Nah, this time it's too complicated to minutely amend the view based on the controller's operations.
                    // We leave ourselves be fully refreshed (cf /*, this*/ above)
                }
            }
            // TODO: also handle undo/redo on other views or x-views
            else if (e.Control && e.KeyCode == Keys.Z)
            {
                e.Handled = true;
                DialogueController.UndoState();
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                e.Handled = true;
                DialogueController.RedoState();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                RemoveSelectedNode();
            }
        }
        #endregion

        #region Interactions - Mouse
        private void OnNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SelectTreeNode(e.Node);     //Will trigger OnAfterSelect
        }

        private void OnAfterSelect(object sender, TreeViewEventArgs e)
        {
            RestyleSelectedNode();
            DialogueController.SelectNode(GetDialogueNode(e.Node), false, this);
        }

        private void OnNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var dialogueNode = GetSelectedDialogueNode();
            if (dialogueNode == null)
                return;

            if (dialogueNode is DialogueNodeGoto)
            {
                SelectNode((dialogueNode as DialogueNodeGoto).Goto);
            }
        }

        private void OnTreeItemDrag(object sender, ItemDragEventArgs e)
        {
            //ProjectController.LogInfo("Start Dragging");

            if (!IsTreeNodeRoot(e.Item as TreeNode))
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void OnTreeDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void OnTreeDragDrop(object sender, DragEventArgs e)
        {
            //ProjectController.LogInfo("Start Dropping");

            TreeNode nodeMove = (TreeNode)e.Data.GetData(typeof(TreeNode));

            Point targetPoint = tree.PointToClient(new Point(e.X, e.Y));
            TreeNode nodeTarget = tree.GetNodeAt(targetPoint);

            //e.Effect = DragDropEffects.Move;

            if (nodeMove != null && nodeTarget != null)
            {
                MoveTreeNode(nodeMove, nodeTarget, EMoveTreeNode.Drop);
            }
        }
        #endregion

        #region Context Menu
        private void OnContextMenuOpened(object sender, EventArgs e)
        {
            separatorRoot.Visible = false;
            menuItemOpenDirectory.Visible = false;
            menuItemCopyName.Visible = false;

            separatorReply.Visible = false;
            menuItemAddReply.Visible = false;

            separatorBranch.Visible = false;
            menuItemBranch.Visible = false;

            menuItemAddSentence.Visible = false;
            menuItemAddChoice.Visible = false;
            menuItemAddGoto.Visible = false;
            menuItemAddBranch.Visible = false;

            separatorReference.Visible = false;
            menuItemCopyReference.Visible = false;
            menuItemPasteReference.Visible = false;
            menuItemCopyID.Visible = false;

            separatorMove.Visible = false;
            menuItemMoveNodeUp.Visible = false;
            menuItemMoveNodeDown.Visible = false;

            separatorDelete.Visible = false;
            menuItemDelete.Visible = false;

            DialogueNode node = GetSelectedDialogueNode();

            //Node Insertion
            if (node is DialogueNodeRoot
            || node is DialogueNodeSentence
            || node is DialogueNodeChoice
            || node is DialogueNodeReply
            || node is DialogueNodeGoto
            || node is DialogueNodeBranch)
            {
                menuItemAddSentence.Visible = true;
                menuItemAddChoice.Visible = true;
                menuItemAddGoto.Visible = true;
                menuItemAddBranch.Visible = true;
            }

            //Root
            if (node is DialogueNodeRoot)
            {
                separatorRoot.Visible = true;
                menuItemOpenDirectory.Visible = true;
                menuItemCopyName.Visible = true;
            }

            //Choice
            if (node is DialogueNodeChoice)
            {
                separatorReply.Visible = true;
                menuItemAddReply.Visible = true;
            }

            //Branch
            if (node is DialogueNodeBranch)
            {
                separatorBranch.Visible = true;
                menuItemBranch.Visible = true;
            }

            //Reference copy/paste + Copy ID
            if (node is DialogueNodeGoto)
            {
                separatorReference.Visible = true;
                menuItemPasteReference.Visible = true;
                menuItemPasteReference.Enabled = copyReference != -1;
                menuItemCopyID.Visible = true;
            }
            else if (node is DialogueNodeSentence || node is DialogueNodeChoice || node is DialogueNodeBranch)
            {
                separatorReference.Visible = true;
                menuItemCopyReference.Visible = true;
                menuItemCopyID.Visible = true;
            }
            else if (node is DialogueNodeReply)
            {
                separatorReference.Visible = true;
                menuItemCopyID.Visible = true;
            }

            //Move
            if (!(node is DialogueNodeRoot))
            {
                separatorMove.Visible = true;
                menuItemMoveNodeUp.Visible = true;
                menuItemMoveNodeDown.Visible = true;
            }

            //Delete
            if (!(node is DialogueNodeRoot))
            {
                separatorDelete.Visible = true;
                menuItemDelete.Visible = true;
            }
        }
        #endregion

        #region Copy / Paste
        private void OnCopyID(object sender, EventArgs e)
        {
            Clipboard.SetText(EditorHelper.GetPrettyNodeID(DialogueController.Dialogue, GetSelectedDialogueNode()));
        }

        protected virtual void OnCopyReference(object sender, EventArgs e)
        {
            DialogueNode dialogueNode = GetSelectedDialogueNode();
            copyReference = dialogueNode.ID;
        }

        protected virtual void OnPasteReference(object sender, EventArgs e)
        {
            if (!IsTreeNodeGoto(tree.SelectedNode))
                return;

            DialogueNodeGoto nodeGoto = GetSelectedDialogueNode() as DialogueNodeGoto;

            DialogueNode newTarget = DialogueController.Dialogue.GetNodeByID(copyReference);
            if (newTarget != null)
            {
                DialogueNode oldTarget = nodeGoto.Goto;
                nodeGoto.Goto = newTarget;
                RefreshDialogueNode(nodeGoto);
                RefreshDialogueNode(oldTarget);
                RefreshDialogueNode(newTarget);
                RestyleSelectedNode();
                DialogueController.NotifyModifiedDialogueNode(nodeGoto, false, this);
                DialogueController.NotifyModifiedDialogueNode(oldTarget, false, this);   // Technically not modified but...
                DialogueController.NotifyModifiedDialogueNode(newTarget, false, this);   // Technically not modified but...
                DialogueController.SelectNode(nodeGoto, true, this);
            }
        }
        #endregion

        #region Close
        private void OnClose(object sender, FormClosingEventArgs e)
        {
            //UserClosing : cross, middle click, Application.Exit
            //MdiFormClosing : app form close, alt+f4
            if (ProjectController.MainWindow != null && e.CloseReason == CloseReason.UserClosing)
            {
                if (!ProjectController.MainWindow.OnDocumentDialogueClosed(this, ForceClose))
                {
                    e.Cancel = true;
                }
            }
        }
        #endregion
    }
}
