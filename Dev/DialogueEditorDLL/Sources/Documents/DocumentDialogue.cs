using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DialogueEditor
{
    public partial class DocumentDialogue : DockContent, IDocument
    {
        //--------------------------------------------------------------------------------------------------------------
        // Helper Class

        protected class NodeWrap
        {
            public DialogueNode DialogueNode;

            public NodeWrap(DialogueNode dialogueNode)
            {
                DialogueNode = dialogueNode;
            }
        }

        protected class State
        {
            public string Content;
            public int NodeID = DialogueNode.ID_NULL;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Public vars

        public Dialogue Dialogue;

        public bool ForceClose = false;

        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        protected bool lockCheckDisplayEvents = false;
        protected int copyReference = -1;

        protected bool pendingDirty = false;
        protected List<State> previousStates = new List<State>();
        protected int indexState = 0;

        private Font font = null;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DocumentDialogue(Dialogue inDialogue)
        {
            InitializeComponent();

            Dialogue = inDialogue;
            Name = Dialogue.GetName();
            tree.ImageList = EditorCore.DefaultImageList;

            //Use this to have multiple colors on a single node.
            //If there are visual glitches, you can try commenting this block.
            //Note: Allowing default rendering will allow drag&drop with left mouse button.
            font = tree.Font;
            tree.DrawMode = TreeViewDrawMode.OwnerDrawText;
            tree.DrawNode += OnTreeViewDrawNode;

            //Ensure custom properties were generated for this dialogue
            Dialogue.GenerateCustomProperties();

            SaveState();

            ResyncDisplayOptions();
            ResyncDocument(false);
            SelectRootNode();
            RefreshTitle();
        }

        public virtual void SetDirty()
        {
            pendingDirty = false;
            ResourcesHandler.SetDirty(Dialogue);

            RefreshTitle();

            SaveState();
        }

        public void SetPendingDirty()
        {
            pendingDirty = true;
        }

        public void CancelPendingDirty()
        {
            pendingDirty = false;
        }

        public void ResolvePendingDirty()
        {
            if (pendingDirty)
            {
                EditorCore.Properties?.OnResolvePendingDirty();
                EditorCore.CustomProperties?.OnResolvePendingDirty();

                if (pendingDirty)   //OnResolvePendingDirty may call CancelPendingDirty
                {
                    SetDirty();    //Raise dirty + store Undo State
                }
            }
        }

        public void OnPostSave()
        {
            if (pendingDirty)
            {
                pendingDirty = false;
                SaveState();    //No need to raise dirty since we just saved, but the Undo State needs to be stored
            }
        }

        public void OnPostReload()
        {
            pendingDirty = false;
            //ResetStates();
            SaveState();

            ResyncDocument();
            SelectRootNode();
            RefreshTitle();
        }

        public void RefreshDocument()
        {
            ResyncDisplayOptions();
            RefreshAllTreeNodes();
            ResyncSelectedNode();
        }

        public void RefreshTitle()
        {
            Text = Dialogue.GetName();
            if (ResourcesHandler.IsDirty(Dialogue))
                Text += "*";
        }

        public void ResyncDocument(bool redraw = true)
        {
            WIN32.StopRedraw(this);
            tree.BeginUpdate();

            Clear();

            if (Dialogue.RootNode != null)
            {
                TreeNode newTreeNodeRoot = tree.Nodes.Add(GetNodeKey(Dialogue.RootNode.ID));
                newTreeNodeRoot.Tag = new NodeWrap(Dialogue.RootNode);
                newTreeNodeRoot.ContextMenuStrip = contextMenu;
                EditorHelper.SetNodeIcon(newTreeNodeRoot, ENodeIcon.Dialogue);

                AddTreeNodeChild(Dialogue.RootNode.Next, newTreeNodeRoot);
                newTreeNodeRoot.Expand();
            }

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);

            if (redraw)
            {
                RefreshAllTreeNodes();
            }
        }

        public void CreateNodeSentence(TreeNode treeNodeFrom, bool branch)
        {
            if (branch && (!(GetDialogueNode(treeNodeFrom) is DialogueNodeBranch)))
                return;

            DialogueNodeSentence nodeSentence = new DialogueNodeSentence();
            Dialogue.AddNode(nodeSentence);

            TreeNode newTreeNode = AddNodeSentence(treeNodeFrom, nodeSentence, branch);
            SelectTreeNode(newTreeNode);

            if (EditorCore.Properties != null)
                EditorCore.Properties.ForceFocus();

            SetDirty();
        }

        public void CreateNodeChoice(TreeNode treeNodeFrom, bool branch)
        {
            if (branch && (!(GetDialogueNode(treeNodeFrom) is DialogueNodeBranch)))
                return;

            DialogueNodeChoice nodeChoice = new DialogueNodeChoice();
            Dialogue.AddNode(nodeChoice);

            TreeNode newTreeNode = AddNodeChoice(treeNodeFrom, nodeChoice, branch);
            SelectTreeNode(newTreeNode);

            if (EditorCore.Properties != null)
                EditorCore.Properties.ForceFocus();

            SetDirty();
        }

        public void CreateNodeReply(TreeNode treeNodeFrom)
        {
            if (!IsTreeNodeChoice(tree.SelectedNode))
                return;

            DialogueNodeReply nodeReply = new DialogueNodeReply();
            Dialogue.AddNode(nodeReply);

            TreeNode newTreeNode = AddNodeReply(treeNodeFrom, nodeReply);
            SelectTreeNode(newTreeNode);

            if (EditorCore.Properties != null)
                EditorCore.Properties.ForceFocus();

            SetDirty();
        }

        public void CreateNodeGoto(TreeNode treeNodeFrom, bool branch)
        {
            if (branch && (!(GetDialogueNode(treeNodeFrom) is DialogueNodeBranch)))
                return;

            DialogueNodeGoto nodeGoto = new DialogueNodeGoto();
            Dialogue.AddNode(nodeGoto);

            TreeNode newTreeNode = AddNodeGoto(treeNodeFrom, nodeGoto, branch);
            SelectTreeNode(newTreeNode);

            if (EditorCore.Properties != null)
                EditorCore.Properties.ForceFocus();

            SetDirty();
        }

        public void CreateNodeBranch(TreeNode treeNodeFrom, bool branch)
        {
            if (branch && (!(GetDialogueNode(treeNodeFrom) is DialogueNodeBranch)))
                return;

            DialogueNodeBranch nodeBranch = new DialogueNodeBranch();
            Dialogue.AddNode(nodeBranch);

            TreeNode newTreeNode = AddNodeBranch(treeNodeFrom, nodeBranch, branch);
            SelectTreeNode(newTreeNode);

            if (EditorCore.Properties != null)
                EditorCore.Properties.ForceFocus();

            SetDirty();
        }

        public virtual TreeNode AddNodeSentence(TreeNode treeNodeFrom, DialogueNodeSentence sentence, bool branch)
        {
            if (treeNodeFrom == null || sentence == null)
                return null;

            WIN32.StopRedraw(this);
            tree.BeginUpdate();

            TreeNode newTreeNode = null;
            if (branch || IsTreeNodeRoot(treeNodeFrom) || IsTreeNodeReply(treeNodeFrom))
            {
                newTreeNode = AddTreeNodeChild(sentence, treeNodeFrom);
                treeNodeFrom.Expand();
            }
            else
            {
                newTreeNode = AddTreeNodeSibling(sentence, treeNodeFrom);
            }

            ResolvePostNodeInsertion(treeNodeFrom, sentence, branch);

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);
            this.Refresh();

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
                newTreeNode = AddTreeNodeChild(choice, treeNodeFrom);
                treeNodeFrom.Expand();
            }
            else
            {
                newTreeNode = AddTreeNodeSibling(choice, treeNodeFrom);
            }

            ResolvePostNodeInsertion(treeNodeFrom, choice, branch);

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);
            this.Refresh();

            return newTreeNode;
        }

        public virtual TreeNode AddNodeReply(TreeNode treeNodeFrom, DialogueNodeReply reply)
        {
            if (!IsTreeNodeChoice(treeNodeFrom) || reply == null)
                return null;

            WIN32.StopRedraw(this);
            tree.BeginUpdate();

            TreeNode newTreeNode = null;
            newTreeNode = AddTreeNode(reply, treeNodeFrom, treeNodeFrom.LastNode);
            treeNodeFrom.Expand();

            var nodeDialogueFrom = GetDialogueNode(treeNodeFrom) as DialogueNodeChoice;
            nodeDialogueFrom.Replies.Add(reply);

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);
            this.Refresh();

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
                newTreeNode = AddTreeNodeChild(nodeGoto, treeNodeFrom);
                treeNodeFrom.Expand();
            }
            else
            {
                newTreeNode = AddTreeNodeSibling(nodeGoto, treeNodeFrom);
            }

            ResolvePostNodeInsertion(treeNodeFrom, nodeGoto, branch);

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);
            this.Refresh();

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
                newTreeNode = AddTreeNodeChild(nodeBranch, treeNodeFrom);
                treeNodeFrom.Expand();
            }
            else
            {
                newTreeNode = AddTreeNodeSibling(nodeBranch, treeNodeFrom);
            }

            ResolvePostNodeInsertion(treeNodeFrom, nodeBranch, branch);

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);
            this.Refresh();

            return newTreeNode;
        }

        private void ResolvePostNodeInsertion(TreeNode treeNodeFrom, DialogueNode newNode, bool branch)
        {
            var nodeDialogueFrom = GetDialogueNode(treeNodeFrom);

            //Find the last node of the inserted sequence
            var lastNode = newNode;
            while (lastNode.Next != null)
            {
                lastNode = lastNode.Next;
            }

            if (branch)
            {
                var nodeBranch = nodeDialogueFrom as DialogueNodeBranch;
                lastNode.Next = nodeBranch.Branch;
                nodeBranch.Branch = newNode;
            }
            else
            {
                lastNode.Next = nodeDialogueFrom.Next;
                nodeDialogueFrom.Next = newNode;
            }
        }

        private TreeNode AddTreeNodeChild(DialogueNode node, TreeNode parentTreeNode)
        {
            if (node == null || parentTreeNode == null)
                return null;

            return AddTreeNode(node, parentTreeNode, null);
        }

        private TreeNode AddTreeNodeSibling(DialogueNode node, TreeNode previousTreeNode)
        {
            if (node == null || previousTreeNode == null)
                return null;

            return AddTreeNode(node, previousTreeNode.Parent, previousTreeNode);
        }

        private TreeNode AddTreeNode(DialogueNode node, TreeNode parentTreeNode, TreeNode previousTreeNode)
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

            if (node is DialogueNodeSentence)
            {
                DialogueNodeSentence nodeSentence = node as DialogueNodeSentence;

                newTreeNode = parentTreeNode.Nodes.Insert(insertIndex, GetNodeKey(node.ID), "");
                newTreeNode.Tag = new NodeWrap(node);
                newTreeNode.ContextMenuStrip = contextMenu;
                EditorHelper.SetNodeIcon(newTreeNode, ENodeIcon.Sentence);

                AddTreeNodeSibling(node.Next, newTreeNode);
            }
            else if (node is DialogueNodeChoice)
            {
                DialogueNodeChoice nodeChoice = node as DialogueNodeChoice;

                newTreeNode = parentTreeNode.Nodes.Insert(insertIndex, GetNodeKey(node.ID), "");
                newTreeNode.Tag = new NodeWrap(node);
                newTreeNode.ContextMenuStrip = contextMenu;
                EditorHelper.SetNodeIcon(newTreeNode, ENodeIcon.Choice);

                foreach (DialogueNodeReply reply in nodeChoice.Replies)
                {
                    AddTreeNode(reply, newTreeNode, newTreeNode.LastNode);
                }

                AddTreeNodeSibling(node.Next, newTreeNode);
                newTreeNode.Expand();
            }
            else if (node is DialogueNodeReply)
            {
                DialogueNodeReply nodeReply = node as DialogueNodeReply;

                newTreeNode = parentTreeNode.Nodes.Insert(insertIndex, GetNodeKey(node.ID), "");
                newTreeNode.Tag = new NodeWrap(node);
                newTreeNode.ContextMenuStrip = contextMenu;
                EditorHelper.SetNodeIcon(newTreeNode, ENodeIcon.Reply);

                AddTreeNodeChild(node.Next, newTreeNode);
                newTreeNode.Expand();
            }
            else if (node is DialogueNodeGoto)
            {
                DialogueNodeGoto nodeGoto = node as DialogueNodeGoto;

                newTreeNode = parentTreeNode.Nodes.Insert(insertIndex, GetNodeKey(node.ID), "");
                newTreeNode.Tag = new NodeWrap(node);
                newTreeNode.ContextMenuStrip = contextMenu;
                EditorHelper.SetNodeIcon(newTreeNode, ENodeIcon.Goto);

                AddTreeNodeSibling(node.Next, newTreeNode);
            }
            else if (node is DialogueNodeBranch)
            {
                DialogueNodeBranch nodeBranch = node as DialogueNodeBranch;

                newTreeNode = parentTreeNode.Nodes.Insert(insertIndex, GetNodeKey(node.ID), "");
                newTreeNode.Tag = new NodeWrap(node);
                newTreeNode.ContextMenuStrip = contextMenu;
                EditorHelper.SetNodeIcon(newTreeNode, ENodeIcon.Branch);

                AddTreeNodeChild(nodeBranch.Branch, newTreeNode);

                AddTreeNodeSibling(node.Next, newTreeNode);
                newTreeNode.Expand();
            }

            if (newTreeNode != null)
            {
                RefreshTreeNode_Impl(newTreeNode);
            }

            return newTreeNode;
        }

        public void RemoveNode(TreeNode treeNode)
        {
            DialogueNode dialogueNode = ((NodeWrap)tree.SelectedNode.Tag).DialogueNode;
            RemoveNode(dialogueNode, tree.SelectedNode);
        }

        public virtual void RemoveNode(DialogueNode node, TreeNode treeNode)
        {
            if (node == null || treeNode == null)
                return;

            if (treeNode == GetRootNode())
                return;

            if (copyReference == node.ID)
                copyReference = -1;

            Dialogue.RemoveNode(node);
            treeNode.Nodes.Remove(treeNode);

            RefreshAllTreeNodes();

            SetDirty();
        }

        public void RemoveAllNodes()
        {
            copyReference = -1;

            Dialogue.ListNodes.RemoveAll(item => item != Dialogue.RootNode);
            Dialogue.RootNode.Next = null;

            GetRootNode().Nodes.Clear();

            RefreshAllTreeNodes();

            SetDirty();
        }

        private enum EMoveTreeNode
        {
            Sibling,
            Drop,
            DropSpecial,
        }

        private bool MoveTreeNode(TreeNode nodeMove, TreeNode nodeTarget, EMoveTreeNode moveType)
        {
            if (nodeMove == null || nodeTarget == null || nodeMove == nodeTarget)
                return false;

            if (IsTreeNodeRoot(nodeMove))
                return false;

            if (IsTreeNodeReply(nodeMove) && !IsTreeNodeReply(nodeTarget) && !IsTreeNodeChoice(nodeTarget))
                return false;

            //Check we are not attaching a node on a depending node (loop)
            List<DialogueNode> dependendingNodes = new List<DialogueNode>();
            Dialogue.GetDependingNodes(GetDialogueNode(nodeMove), ref dependendingNodes);
            if (dependendingNodes.Contains(GetDialogueNode(nodeTarget)))
                return false;

            if (IsTreeNodeReply(nodeMove))
            {
                TreeNode nodeChoiceFrom = nodeMove.Parent;
                DialogueNodeReply dialogueNodeMove = GetDialogueNode(nodeMove) as DialogueNodeReply;
                DialogueNodeChoice dialogueNodeChoiceFrom = GetDialogueNode(nodeChoiceFrom) as DialogueNodeChoice;

                if (IsTreeNodeReply(nodeTarget))
                {
                    TreeNode nodeChoiceTo = nodeTarget.Parent;
                    DialogueNodeReply dialogueNodeTarget = GetDialogueNode(nodeTarget) as DialogueNodeReply;
                    DialogueNodeChoice dialogueNodeChoiceTo = GetDialogueNode(nodeChoiceTo) as DialogueNodeChoice;

                    //remove reply from its choice
                    dialogueNodeChoiceFrom.Replies.Remove(dialogueNodeMove);
                    nodeChoiceFrom.Nodes.Remove(nodeMove);

                    //insert reply after another reply inside a choice
                    dialogueNodeChoiceTo.Replies.Insert(dialogueNodeChoiceTo.Replies.IndexOf(dialogueNodeTarget) + 1, dialogueNodeMove);
                    nodeChoiceTo.Nodes.Insert(nodeChoiceTo.Nodes.IndexOf(nodeTarget) + 1, nodeMove);
                }
                else if (IsTreeNodeChoice(nodeTarget))
                {
                    TreeNode nodeChoiceTo = nodeTarget;
                    DialogueNodeChoice dialogueNodeChoiceTo = GetDialogueNode(nodeChoiceTo) as DialogueNodeChoice;

                    //remove reply from its choice
                    dialogueNodeChoiceFrom.Replies.Remove(dialogueNodeMove);
                    nodeChoiceFrom.Nodes.Remove(nodeMove);

                    //insert reply as first reply of a choice
                    dialogueNodeChoiceTo.Replies.Insert(0, dialogueNodeMove);
                    nodeChoiceTo.Nodes.Insert(0, nodeMove);
                }
                else
                {
                    return false;   //this should not happen, the case is checked above
                }
            }
            else
            {
                TreeNode nodeParentFrom = nodeMove.Parent;
                TreeNode nodePrev = nodeMove.PrevNode;
                DialogueNode dialogueNodeMove = GetDialogueNode(nodeMove);
                
                bool branchTarget = false;
                if (IsTreeNodeBranch(nodeTarget))
                {
                    //We are using a move up/down on a node inside a targetted branch
                    //Or using a special drop to force the branch target
                    if ((moveType == EMoveTreeNode.Sibling && nodeMove.Parent == nodeTarget)
                    ||   moveType == EMoveTreeNode.DropSpecial)
                    {
                        branchTarget = true;
                    }
                }

                //remove node from current position
                if (nodePrev != null)
                {
                    DialogueNode dialogueNodePrev = GetDialogueNode(nodePrev);
                    dialogueNodePrev.Next = dialogueNodeMove.Next;
                    dialogueNodeMove.Next = null;
                    nodeParentFrom.Nodes.Remove(nodeMove);
                }
                else
                {
                    DialogueNode dialogueNodeParentFrom = GetDialogueNode(nodeParentFrom);
                    if (IsTreeNodeBranch(nodeParentFrom) && nodeParentFrom.FirstNode == nodeMove)
                    {
                        //node is a branch child, we need to redirect the branch
                        ((DialogueNodeBranch)dialogueNodeParentFrom).Branch = dialogueNodeMove.Next;
                        dialogueNodeMove.Next = null;
                        nodeParentFrom.Nodes.Remove(nodeMove);
                    }
                    else
                    {
                        dialogueNodeParentFrom.Next = dialogueNodeMove.Next;
                        dialogueNodeMove.Next = null;
                        nodeParentFrom.Nodes.Remove(nodeMove);
                    }
                }

                //insert node on new position
                DialogueNode dialogueNodeTarget = GetDialogueNode(nodeTarget);
                if (branchTarget)
                {
                    dialogueNodeMove.Next = ((DialogueNodeBranch)dialogueNodeTarget).Branch;
                    ((DialogueNodeBranch)dialogueNodeTarget).Branch = dialogueNodeMove;
                }
                else
                {
                    dialogueNodeMove.Next = dialogueNodeTarget.Next;
                    dialogueNodeTarget.Next = dialogueNodeMove;
                }

                if (IsTreeNodeRoot(nodeTarget)
                ||  IsTreeNodeReply(nodeTarget)
                ||  branchTarget)
                {
                    nodeTarget.Nodes.Insert(0, nodeMove);
                }
                else
                {
                    TreeNode nodeParentTo = nodeTarget.Parent;
                    nodeParentTo.Nodes.Insert(nodeParentTo.Nodes.IndexOf(nodeTarget) + 1, nodeMove);
                }
            }

            SelectTreeNode(nodeMove);

            return true;
        }

        public void SelectNode(int id)
        {
            SelectTreeNode(GetTreeNode(id));
        }

        public void SelectNode(DialogueNode node)
        {
            SelectTreeNode(GetTreeNode(node));
        }

        public void SelectRootNode()
        {
            SelectTreeNode(GetRootNode());
        }

        public virtual void SelectTreeNode(TreeNode treeNode)
        {
            if (treeNode == null)
                return;

            tree.SelectedNode = treeNode;
        }

        public void ResyncSelectedNode()
        {
            if (tree.SelectedNode == null)
                return;
            
            //tree.BeginUpdate();   //Doesnt seem necessary since we will not edit much most of the time, and keeping it adds a permanent flickering

            UnHighlightAll();

            DialogueNodeGoto nodeGoto = ((NodeWrap)tree.SelectedNode.Tag).DialogueNode as DialogueNodeGoto;
            if (nodeGoto != null)
            {
                Highlight(nodeGoto.Goto);

                List<DialogueNode> gotos = Dialogue.GetGotoReferencesOnNode(nodeGoto.Goto);
                Highlight(gotos, Color.DarkGray);
            }
            else
            {
                List<DialogueNode> gotos = Dialogue.GetGotoReferencesOnNode(GetSelectedDialogueNode());
                if (gotos.Count > 0)
                {
                    Highlight(gotos, Color.DarkGray);
                }
            }

            EditorCore.Properties?.ShowDialogueNodeProperties(this, tree.SelectedNode, ((NodeWrap)tree.SelectedNode.Tag).DialogueNode);
            EditorCore.CustomProperties?.ShowDialogueNodeProperties(this, tree.SelectedNode, ((NodeWrap)tree.SelectedNode.Tag).DialogueNode);

            //tree.EndUpdate();
        }

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
            this.Refresh();
        }

        public void RefreshTreeNode(TreeNode treeNode)
        {
            WIN32.StopRedraw(this);
            tree.BeginUpdate();

            RefreshTreeNode_Impl(treeNode);

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);
            this.Refresh();
        }

        public void RefreshSelectedTreeNode()
        {
            WIN32.StopRedraw(this);
            tree.BeginUpdate();

            RefreshTreeNode_Impl(tree.SelectedNode);

            tree.EndUpdate();
            WIN32.ResumeRedraw(this);
            this.Refresh();
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

            DialogueNode dialogueNode = ((NodeWrap)treeNode.Tag).DialogueNode;

            //Refresh Goto nodes targeting this node (only if not inside a recursive parsing)
            if (!isTreeRefresh)
            {
                List<DialogueNode> gotos = Dialogue.GetGotoReferencesOnNode(dialogueNode);
                foreach (var nodeGoto in gotos)
                    RefreshTreeNode_Impl(GetTreeNode(nodeGoto));
            }

            //Style
            FontStyle style = FontStyle.Regular;

            if (dialogueNode is DialogueNodeRoot
            ||  dialogueNode is DialogueNodeChoice
            ||  dialogueNode is DialogueNodeGoto
            ||  dialogueNode is DialogueNodeBranch)
            {
                style |= FontStyle.Italic;
            }

            if (Dialogue.IsNodeReferencedByGoto(dialogueNode))
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

                treeNode.Text = textID + textAttributes + textActors + textContent;
            }
        }

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
                    Actor speaker = ResourcesHandler.Project.GetActorFromID((dialogueNode as DialogueNodeSentence).SpeakerID);
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
                return String.Format("[{0}] ", dialogueNode.ID);

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
                    stringBuilder.AppendFormat("[{0}] ", ResourcesHandler.Project.GetActorName(nodeSentence.SpeakerID));
                if (EditorCore.Settings.DisplayListener)
                    stringBuilder.AppendFormat("[{0}] ", ResourcesHandler.Project.GetActorName(nodeSentence.ListenerID));

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
                    var entry = Dialogue.Translations.GetNodeEntry(dialogueNode, EditorHelper.CurrentLanguage);
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
                return String.Format("Choice > {0}", nodeChoice.Choice);
            }
            else if (dialogueNode is DialogueNodeReply)
            {
                DialogueNodeReply nodeReply = dialogueNode as DialogueNodeReply;
                if (EditorHelper.CurrentLanguage != EditorCore.LanguageWorkstring)
                {
                    var entry = Dialogue.Translations.GetNodeEntry(dialogueNode, EditorHelper.CurrentLanguage);
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
                    return String.Format("Goto > [{0}] {1}", nodeGoto.Goto.ID, GetTreeNodeTextContent(nodeGoto.Goto));
                else
                    return String.Format("Goto > {0}", GetTreeNodeTextContent(nodeGoto.Goto));
            }
            else if (dialogueNode is DialogueNodeBranch)
            {
                DialogueNodeBranch nodeBranch = dialogueNode as DialogueNodeBranch;
                return String.Format("Branch > {0}", nodeBranch.Workstring);
            }

            return "";
        }

        private void OnTreeViewDrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.DrawDefault = false;

            var node = e.Node;
            var dialogueNode = GetDialogueNode(node);

            Font nodeFont = node.NodeFont;
            if (nodeFont == null)
                nodeFont = font;

            // Retrieve texts
            string textID = GetTreeNodeTextID(dialogueNode);
            string textAttributes = GetTreeNodeTextAttributes(dialogueNode);
            string textActors = GetTreeNodeTextActors(dialogueNode);
            string textContent = GetTreeNodeTextContent(dialogueNode);

            // Highlight
            Rectangle bounds = node.Bounds;
            bounds.Width = 0;
            int nbParts = 0;

            if (textID != String.Empty)
            {
                bounds.Width += TextRenderer.MeasureText(e.Graphics, textID, nodeFont).Width;
                //bounds.Width += e.Graphics.MeasureString(textID, nodeFont).ToSize().Width;
                nbParts += 1;
            }

            if (textAttributes != String.Empty)
            {
                bounds.Width += TextRenderer.MeasureText(e.Graphics, textAttributes, nodeFont).Width;
                //bounds.Width += e.Graphics.MeasureString(textAttributes, nodeFont).ToSize().Width;
                nbParts += 1;
            }

            if (textActors != String.Empty)
            {
                bounds.Width += TextRenderer.MeasureText(e.Graphics, textActors, nodeFont).Width;
                //bounds.Width += e.Graphics.MeasureString(textActors, nodeFont).ToSize().Width;
                nbParts += 1;
            }

            if (textContent != String.Empty)
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
            if (text != null && text != String.Empty)
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

            labelFont.Text = String.Format("{0} {1}", font.Name, font.Size);
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

        public void Clear()
        {
            tree.Nodes.Clear();
        }

        protected string GetNodeKey(int nodeID)
        {
            return "Node_" + nodeID.ToString();
        }

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

        public DialogueNode GetDialogueNode(TreeNode node)
        {
            if (node != null)
            {
                return ((NodeWrap)node.Tag).DialogueNode;
            }
            return null;
        }

        public DialogueNode GetSelectedDialogueNode()
        {
            if (tree.SelectedNode != null)
            {
                return ((NodeWrap)tree.SelectedNode.Tag).DialogueNode;
            }
            return null;
        }

        protected bool IsTreeNodeRoot(TreeNode node)
        {
            if (((NodeWrap)node.Tag).DialogueNode is DialogueNodeRoot)
                return true;
            return false;
        }

        protected bool IsTreeNodeSentence(TreeNode node)
        {
            if (((NodeWrap)node.Tag).DialogueNode is DialogueNodeSentence)
                return true;
            return false;
        }

        protected bool IsTreeNodeChoice(TreeNode node)
        {
            if (((NodeWrap)node.Tag).DialogueNode is DialogueNodeChoice)
                return true;
            return false;
        }

        protected bool IsTreeNodeReply(TreeNode node)
        {
            if (((NodeWrap)node.Tag).DialogueNode is DialogueNodeReply)
                return true;
            return false;
        }

        protected bool IsTreeNodeGoto(TreeNode node)
        {
            if (((NodeWrap)node.Tag).DialogueNode is DialogueNodeGoto)
                return true;
            return false;
        }

        protected bool IsTreeNodeBranch(TreeNode node)
        {
            if (((NodeWrap)node.Tag).DialogueNode is DialogueNodeBranch)
                return true;
            return false;
        }

        private void ResyncDisplayOptions()
        {
            lockCheckDisplayEvents = true;

            if (ResourcesHandler.Project != null)
            {
                var listLanguages = new List<Language>() { EditorCore.LanguageWorkstring };
                listLanguages.AddRange(ResourcesHandler.Project.ListLanguages);

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

        private void SaveState()
        {
            //Remove all States following the current State
            if (indexState < previousStates.Count - 1)
            {
                previousStates.RemoveRange(indexState + 1, previousStates.Count - indexState - 1);
            }

            //Append the new State
            previousStates.Add(new State()
            {
                Content = ExporterJson.SaveDialogueToString(ResourcesHandler.Project, Dialogue),
                NodeID = ((tree.SelectedNode != null) ? GetSelectedDialogueNode().ID : DialogueNode.ID_NULL)
            } );

            indexState = previousStates.Count - 1;

            //Shrink list if needed by removing older States
            int maxStates = EditorCore.Settings.MaxUndoLevels + 1;  //I add +1 because the first state stored is the original file
            if (previousStates.Count > maxStates)
            {
                previousStates.RemoveRange(0, previousStates.Count - maxStates);
                indexState = maxStates - 1;
            }
        }

        private void ResetStates()
        {
            previousStates.Clear();
            indexState = 0;
        }

        public void UndoState()
        {
            if (previousStates.Count >= 2 && indexState > 0)
            {
                //TODO: should be cleaner to clear properties and tree before reloading, but I need to handle the flicker

                State currentState = previousStates.ElementAt(indexState);
                State previousState = previousStates.ElementAt(indexState - 1);
                indexState -= 1;

                ResourcesHandler.ReloadDialogueFromString(Dialogue, previousState.Content);

                ResyncDocument();
                RefreshTitle();
                SelectNode(currentState.NodeID);
                if (tree.SelectedNode == null)
                    SelectRootNode();
            }
        }

        public void RedoState()
        {
            if (previousStates.Count >= 2 && indexState < previousStates.Count - 1)
            {
                //TODO: should be cleaner to clear properties and tree before reloading, but I need to handle the flicker

                //State currentState = previousStates.ElementAt(indexState);
                State nextState = previousStates.ElementAt(indexState + 1);
                indexState += 1;

                ResourcesHandler.ReloadDialogueFromString(Dialogue, nextState.Content);

                ResyncDocument();
                RefreshTitle();
                SelectNode(nextState.NodeID);
                if (tree.SelectedNode == null)
                    SelectRootNode();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events

        public bool ProcessCmdKey_Impl(Keys keyData)
        {
            if (tree.Focused && (keyData == (Keys.F2) || keyData == (Keys.Enter)))
            {
                if (tree.SelectedNode != null)
                {
                    if (IsTreeNodeSentence(tree.SelectedNode)
                    || IsTreeNodeChoice(tree.SelectedNode)
                    || IsTreeNodeReply(tree.SelectedNode))
                    {
                        if (EditorCore.Properties != null)
                            EditorCore.Properties.ForceFocus();

                        return true;
                    }
                }
            }
            else if (!tree.Focused && keyData == (Keys.F2))
            {
                tree.Focus();
                return true;
            }
            else if (keyData == (Keys.Enter))
            {
                if (tree.SelectedNode != null && EditorCore.Properties != null && EditorCore.Properties.IsEditingWorkstring())
                {
                    if (IsTreeNodeSentence(tree.SelectedNode))
                    {
                        var next = GetSelectedDialogueNode().Next;
                        if (next != null)
                        {
                            ResolvePendingDirty();

                            SelectNode(next);

                            if (EditorCore.Properties != null)
                                EditorCore.Properties.ForceFocus();
                        }
                    }
                    return true;
                }
            }
            else if (keyData == (Keys.Control | Keys.Enter) || keyData == (Keys.Control | Keys.Shift | Keys.Enter))
            {
                if (tree.SelectedNode != null && IsTreeNodeSentence(tree.SelectedNode))
                {
                    DialogueNodeSentence newSentence = new DialogueNodeSentence();
                    Dialogue.AddNode(newSentence);

                    var newTreeNode = AddNodeSentence(tree.SelectedNode, newSentence, false);
                    var prevSentence = GetSelectedDialogueNode() as DialogueNodeSentence;

                    if (keyData == (Keys.Control | Keys.Enter))
                    {
                        newSentence.SpeakerID = prevSentence.SpeakerID;
                        newSentence.ListenerID = prevSentence.ListenerID;
                    }
                    else
                    {
                        newSentence.SpeakerID = prevSentence.ListenerID;
                        newSentence.ListenerID = prevSentence.SpeakerID;
                    }

                    RefreshTreeNode(newTreeNode);
                    SelectTreeNode(newTreeNode);

                    if (EditorCore.Properties != null)
                        EditorCore.Properties.ForceFocus();

                    SetDirty();
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

                if (dialogueNode is DialogueNodeRoot)
                {
                    var tempDialogue = new Dialogue(Dialogue);
                    tempDialogue.RootNode = dialogueNode.Clone() as DialogueNodeRoot;
                    EditorHelper.Clipboard = tempDialogue;
                    EditorHelper.ClipboardInfos = new ClipboardInfos() { sourceDialogue = Dialogue.GetName() };
                }
                else
                {
                    EditorHelper.Clipboard = dialogueNode.Clone();
                    EditorHelper.ClipboardInfos = new ClipboardInfos() { sourceDialogue = Dialogue.GetName(), sourceNodeID = dialogueNode.ID };
                }

                if (e.KeyCode == Keys.X)
                {
                    if (dialogueNode is DialogueNodeRoot)
                    {
                        RemoveAllNodes();
                    }
                    else
                    {
                        RemoveNode(dialogueNode, tree.SelectedNode);
                    }
                }
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                e.Handled = true;

                bool asBranch = false;

                //Special paste
                if (e.Shift)
                {
                    if (dialogueNode is DialogueNodeGoto)
                    {
                        if (EditorHelper.Clipboard is DialogueNode && EditorHelper.ClipboardInfos != null && EditorHelper.ClipboardInfos.sourceDialogue == Dialogue.GetName())
                        {
                            DialogueNode nodeTarget = Dialogue.GetNodeByID(EditorHelper.ClipboardInfos.sourceNodeID);
                            if (nodeTarget != null)
                            {
                                (dialogueNode as DialogueNodeGoto).Goto = nodeTarget;
                                ResyncSelectedNode();

                                SetDirty();
                            }
                        }

                        return;
                    }
                    else if (dialogueNode is DialogueNodeBranch)
                    {
                        asBranch = true;
                    }
                    else
                    {
                        //Ignore the paste if trying to special-copy an undefined case
                        return;
                    }
                }

                //if (EditorCore.Clipboard is DialogueNodeRoot)
                if (EditorHelper.Clipboard is Dialogue)
                {
                    //var newRoot = (EditorCore.Clipboard as DialogueNodeRoot).Clone() as DialogueNodeRoot;

                    var tempDialogue = EditorHelper.Clipboard as Dialogue;
                    var newRoot = tempDialogue.RootNode.Clone() as DialogueNodeRoot;

                    Package previousPackage = Dialogue.Package;

                    //Only Copy parameters if we copy a root on another root
                    if (dialogueNode is DialogueNodeRoot)
                    {
                        Dialogue.Copy(tempDialogue);
                    }

                    //Insert from the first child, and discard the new root
                    var firsNode = newRoot.Next;
                    Dialogue.AddNode(firsNode);

                    TreeNode newTreeNode = null;
                    if (firsNode is DialogueNodeSentence)
                        newTreeNode = AddNodeSentence(tree.SelectedNode, firsNode as DialogueNodeSentence, asBranch);
                    else if (firsNode is DialogueNodeChoice)
                        newTreeNode = AddNodeChoice(tree.SelectedNode, firsNode as DialogueNodeChoice, asBranch);
                    else if (firsNode is DialogueNodeReply)
                        newTreeNode = AddNodeReply(tree.SelectedNode, firsNode as DialogueNodeReply);
                    else if (firsNode is DialogueNodeGoto)
                        newTreeNode = AddNodeGoto(tree.SelectedNode, firsNode as DialogueNodeGoto, asBranch);
                    else if (firsNode is DialogueNodeBranch)
                        newTreeNode = AddNodeBranch(tree.SelectedNode, firsNode as DialogueNodeBranch, asBranch);

                    if (dialogueNode is DialogueNodeRoot)
                    {
                        ResyncSelectedNode();   //root node is already selected, we just need a resync

                        if (EditorCore.ProjectExplorer != null)
                            EditorCore.ProjectExplorer.ResyncFile(Dialogue, previousPackage, true);
                    }
                    else
                    {
                        SelectTreeNode(newTreeNode);
                    }

                    SetDirty();
                }
                else if (EditorHelper.Clipboard is DialogueNodeSentence)
                {
                    var newNode = (EditorHelper.Clipboard as DialogueNodeSentence).Clone() as DialogueNodeSentence;
                    Dialogue.AddNode(newNode);

                    var newTreeNode = AddNodeSentence(tree.SelectedNode, newNode, asBranch);
                    SelectTreeNode(newTreeNode);

                    SetDirty();
                }
                else if (EditorHelper.Clipboard is DialogueNodeChoice)
                {
                    var newNode = (EditorHelper.Clipboard as DialogueNodeChoice).Clone() as DialogueNodeChoice;
                    Dialogue.AddNode(newNode);

                    var newTreeNode = AddNodeChoice(tree.SelectedNode, newNode, asBranch);
                    SelectTreeNode(newTreeNode);

                    SetDirty();
                }
                else if (EditorHelper.Clipboard is DialogueNodeReply)
                {
                    if (IsTreeNodeChoice(tree.SelectedNode))
                    {
                        var newNode = (EditorHelper.Clipboard as DialogueNodeReply).Clone() as DialogueNodeReply;
                        Dialogue.AddNode(newNode);

                        var newTreeNode = AddNodeReply(tree.SelectedNode, newNode);
                        SelectTreeNode(newTreeNode);

                        SetDirty();
                    }
                }
                else if (EditorHelper.Clipboard is DialogueNodeGoto)
                {
                    var newNode = (EditorHelper.Clipboard as DialogueNodeGoto).Clone() as DialogueNodeGoto;
                    Dialogue.AddNode(newNode);

                    var newTreeNode = AddNodeGoto(tree.SelectedNode, newNode, asBranch);
                    SelectTreeNode(newTreeNode);

                    SetDirty();
                }
                else if (EditorHelper.Clipboard is DialogueNodeBranch)
                {
                    var newNode = (EditorHelper.Clipboard as DialogueNodeBranch).Clone() as DialogueNodeBranch;
                    Dialogue.AddNode(newNode);

                    var newTreeNode = AddNodeBranch(tree.SelectedNode, newNode, asBranch);
                    SelectTreeNode(newTreeNode);

                    SetDirty();
                }
            }
            else if (e.Control && e.KeyCode == Keys.Z)
            {
                e.Handled = true;

                UndoState();
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                e.Handled = true;

                RedoState();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;

                RemoveNode(dialogueNode, tree.SelectedNode);
            }
        }

        private void OnNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SelectTreeNode(e.Node);     //Will trigger OnNodeSelect
        }

        private void OnNodeSelect(object sender, TreeViewEventArgs e)
        {
            ResyncSelectedNode();
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

        private void OnNodeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            //Forbid the collapsing of desired nodes
            if (((NodeWrap)e.Node.Tag).DialogueNode is DialogueNodeRoot)
            {
                e.Cancel = true;
            }
        }

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

            DialogueNode node = ((NodeWrap)tree.SelectedNode.Tag).DialogueNode;

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
                menuItemPasteReference.Enabled = (copyReference != -1);
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

        private void OnOpenDirectory(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(System.IO.Path.Combine(EditorHelper.GetProjectDirectory(), Dialogue.GetFilePath()));
        }

        private void OnCopyName(object sender, EventArgs e)
        {
            Clipboard.SetText(Dialogue.GetName());
        }

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
            if (!IsTreeNodeChoice(tree.SelectedNode))
                return;

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
            DialogueNodeBranch nodeBranch = GetDialogueNode(tree.SelectedNode) as DialogueNodeBranch;
            if (nodeBranch == null)
                return;

            CreateNodeSentence(tree.SelectedNode, true);
        }

        private void OnBranchNodeChoice(object sender, EventArgs e)
        {
            DialogueNodeBranch nodeBranch = GetDialogueNode(tree.SelectedNode) as DialogueNodeBranch;
            if (nodeBranch == null)
                return;

            CreateNodeChoice(tree.SelectedNode, true);
        }

        private void OnBranchNodeGoto(object sender, EventArgs e)
        {
            DialogueNodeBranch nodeBranch = GetDialogueNode(tree.SelectedNode) as DialogueNodeBranch;
            if (nodeBranch == null)
                return;

            CreateNodeGoto(tree.SelectedNode, true);
        }

        private void OnBranchNodeBranch(object sender, EventArgs e)
        {
            DialogueNodeBranch nodeBranch = GetDialogueNode(tree.SelectedNode) as DialogueNodeBranch;
            if (nodeBranch == null)
                return;

            CreateNodeBranch(tree.SelectedNode, true);
        }

        protected void OnDeleteNode(object sender, EventArgs e)
        {
            RemoveNode(tree.SelectedNode);
        }

        protected virtual void OnCopyReference(object sender, EventArgs e)
        {
            DialogueNode dialogueNode = ((NodeWrap)tree.SelectedNode.Tag).DialogueNode;
            copyReference = dialogueNode.ID;
        }

        protected virtual void OnPasteReference(object sender, EventArgs e)
        {
            if (!IsTreeNodeGoto(tree.SelectedNode))
                return;

            DialogueNodeGoto nodeGoto = ((NodeWrap)tree.SelectedNode.Tag).DialogueNode as DialogueNodeGoto;

            DialogueNode newTarget = Dialogue.GetNodeByID(copyReference);
            if (newTarget != null)
            {
                DialogueNode oldTarget = nodeGoto.Goto;
                nodeGoto.Goto = newTarget;

                RefreshTreeNode(tree.SelectedNode);
                RefreshTreeNode(GetTreeNode(oldTarget));
                RefreshTreeNode(GetTreeNode(newTarget));

                SetDirty();
            }
        }

        protected virtual void OnMoveNodeUp(object sender, EventArgs e)
        {
            bool moved = false;
            if (tree.SelectedNode.PrevNode != null)
            {
                if (tree.SelectedNode.PrevNode.PrevNode != null)
                {
                    moved = MoveTreeNode(tree.SelectedNode, tree.SelectedNode.PrevNode.PrevNode, EMoveTreeNode.Sibling);
                }
                else
                {
                    moved = MoveTreeNode(tree.SelectedNode, tree.SelectedNode.Parent, EMoveTreeNode.Sibling);
                }
            }

            if (moved)
                SetDirty();
        }

        protected virtual void OnMoveNodeDown(object sender, EventArgs e)
        {
            bool moved = false;
            if (tree.SelectedNode.NextNode != null)
            {
                moved = MoveTreeNode(tree.SelectedNode, tree.SelectedNode.NextNode, EMoveTreeNode.Sibling);
            }

            if (moved)
                SetDirty();
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

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            if (lockCheckDisplayEvents)
                return;

            var language = comboBoxLanguages.SelectedItem as Language;
            EditorHelper.CurrentLanguage = language;

            RefreshAllTreeNodes();
        }

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            //UserClosing : cross, middle click, Application.Exit
            //MdiFormClosing : app form close, alt+f4
            if (EditorCore.MainWindow != null && e.CloseReason == CloseReason.UserClosing)
            {
                if (!EditorCore.MainWindow.OnDocumentDialogueClosed(this, ForceClose))
                {
                    e.Cancel = true;
                }
            }
        }

        private void OnCopyID(object sender, EventArgs e)
        {
            DialogueNode dialogueNode = ((NodeWrap)tree.SelectedNode.Tag).DialogueNode;
            Clipboard.SetText(EditorHelper.GetPrettyNodeID(Dialogue, dialogueNode));
        }

        private void OnTreeItemDrag(object sender, ItemDragEventArgs e)
        {
            //EditorCore.LogInfo("Start Dragging");

            if (!IsTreeNodeRoot(e.Item as TreeNode))
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void OnTreeDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            
            /*TreeNode nodeMove = (TreeNode)e.Data.GetData(typeof(TreeNode));

            Point targetPoint = tree.PointToClient(new Point(e.X, e.Y));
            TreeNode nodeTarget = tree.GetNodeAt(targetPoint);

            if (nodeMove != null && nodeTarget != null)
            {
                EditorCore.LogInfo(String.Format("Check node {0} drop on node {1}", GetDialogueNode(nodeMove).ID, GetDialogueNode(nodeTarget).ID));

                if (CanMoveTreeNode(nodeMove, nodeTarget))
                    e.Effect = DragDropEffects.Move;
            }*/
        }

        private bool CanMoveTreeNode(TreeNode nodeMove, TreeNode nodeTarget)
        {
            if (nodeMove == null || nodeTarget == null || nodeMove == nodeTarget)
                return false;

            if (IsTreeNodeRoot(nodeMove))
                return false;

            if (IsTreeNodeReply(nodeMove) && !IsTreeNodeReply(nodeTarget) && !IsTreeNodeChoice(nodeTarget))
                return false;

            //Check we are not attaching a node on a depending node (loop)
            List<DialogueNode> dependendingNodes = new List<DialogueNode>();
            Dialogue.GetDependingNodes(GetDialogueNode(nodeMove), ref dependendingNodes);
            if (dependendingNodes.Contains(GetDialogueNode(nodeTarget)))
                return false;

            return true;
        }

        private void OnTreeDragDrop(object sender, DragEventArgs e)
        {
            //EditorCore.LogInfo("Start Dropping");

            TreeNode nodeMove = (TreeNode)e.Data.GetData(typeof(TreeNode));

            Point targetPoint = tree.PointToClient(new Point(e.X, e.Y));
            TreeNode nodeTarget = tree.GetNodeAt(targetPoint);

            //e.Effect = DragDropEffects.Move;

            if (nodeMove != null && nodeTarget != null)
            {
                //EditorCore.LogInfo(String.Format("Node {0} dropped on node {1}", GetDialogueNode(nodeMove).ID, GetDialogueNode(nodeTarget).ID));

                if (MoveTreeNode(nodeMove, nodeTarget, EMoveTreeNode.Drop))
                {
                    SetDirty();

                    //EditorCore.LogInfo("Drop Success");
                }
            }
        }
    }
}
