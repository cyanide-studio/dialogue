using System.Collections.Generic;
using System.Linq;

namespace DialogueEditor
{
    public class DialogueController
    {
        #region Helper class
        private class DialogueViewHolder
        {
            public DocumentDialogueView DialogueView = null;

            public DialogueViewHolder(DocumentDialogueView inDialogueView)
            {
                DialogueView = inDialogueView;
            }
        };
        #endregion

        #region Variables
        public Dialogue Dialogue = null;
        public DialogueNode SelectedDialogueNode = null;
        private List<DialogueViewHolder> DialogueViews = new List<DialogueViewHolder>();
        public bool Dirty = false;
        public bool PendingDirty = false;   // Handles textbox trickiness: onChanged (as opposed to onValidated) dirties but does not save state, so it's a funny situation...
        protected List<State> previousStates = new List<State>();
        protected int indexState = 0;
        #endregion

        #region Constructor
        public DialogueController(Dialogue inDialogue)
        {
            Dialogue = inDialogue;
        }
        #endregion

        #region Views management
        public void AddView(DocumentDialogueView inDialogueView)
        {
            DialogueViews.Add(new DialogueViewHolder(inDialogueView));
        }

        public void RefreshDirtyFlags()
        {
            foreach (DialogueViewHolder holder in DialogueViews)
            {
                if (holder != null && holder.DialogueView != null)
                    holder.DialogueView.RefreshTitle();
            }
        }

        public void SetPendingDirty(bool inPendingDirty = true)
        {
            PendingDirty = inPendingDirty;
        }

        public void CancelPendingDirty()
        {
            PendingDirty = false;
        }

        public void ResolvePendingDirty()
        {
            if (PendingDirty)
            {
                // Properties are kind of an unregistered view
                ProjectController.ResolvePendingDirty();

                // Now the regular views - they can also have pending dirty actions
                foreach (DialogueViewHolder holder in DialogueViews)
                {
                    if (holder != null && holder.DialogueView != null)
                        holder.DialogueView.ResolvePendingDirty();
                }

                if (PendingDirty)   //ResolvePendingDirty may call CancelPendingDirty
                {
                    SetDirty();    //Raise dirty + store Undo State
                }
            }
        }

        public void SetDirty()
        {
            PendingDirty = false;
            Dirty = true;

            RefreshDirtyFlags();
            SaveState();
        }

        private void RefreshSelectedDialogueNode(DocumentDialogueView originView)
        {
            // Tell the views to refresh just their selection (if they can - if they want they can do a full refresh...)
            foreach (DialogueViewHolder holder in DialogueViews)
            {
                if (holder != null && holder.DialogueView != null && holder.DialogueView != originView)
                    holder.DialogueView.SelectDialogueNode(SelectedDialogueNode);
            }
        }

        private void RefreshDialogueNode(DialogueNode dialogueNode, DocumentDialogueView originView = null, List<string> preciseElements = null)
        {
            // Tell the views to refresh just this node (if they can - if they want they can do a full refresh...)
            foreach (DialogueViewHolder holder in DialogueViews)
            {
                if (holder != null && holder.DialogueView != null && holder.DialogueView != originView)
                    holder.DialogueView.RefreshDialogueNode(dialogueNode);
            }
            if (originView != null)
            {
                // Meaning that the change has been triggered by a DocumentDialogueView, so Properties need to be refreshed 
                ProjectController.UpdateProperties(this, dialogueNode, preciseElements);
            }
        }

        private void RefreshDialogueData(DocumentDialogueView originView = null, List<string> preciseElements = null)
        {
            // Tell the views to refresh just this node (if they can - if they want they can do a full refresh...)
            foreach (DialogueViewHolder holder in DialogueViews)
            {
                if (holder != null && holder.DialogueView != null && holder.DialogueView != originView)
                    holder.DialogueView.RefreshDialogueData();
            }
            if (originView != null)
            {
                // Meaning that the change has been triggered by a DocumentDialogueView, so Properties need to be refreshed 
                ProjectController.UpdateProperties(this, Dialogue.RootNode, preciseElements);
            }
        }
        
        private void RefreshViews(DocumentDialogueView originView = null)
        {
            // Tell the views to fully refresh
            foreach (DialogueViewHolder holder in DialogueViews)
            {
                if (holder != null && holder.DialogueView != null && holder.DialogueView != originView)
                    holder.DialogueView.RefreshDocument();
            }
        }

        public void NotifyModifiedDialogueNode(DialogueNode dialogueNode, bool modificationStillInProgress = false, DocumentDialogueView originView = null, List<string> preciseElements = null)
        {
            // For legacy reasons, we allow properties views to directly modify the model.
            // They should notify us afterwards.
            // (ideally we'd have real controller commands like set node value, etc)
            if (modificationStillInProgress)
                SetPendingDirty();
            else
                SetDirty();
            RefreshDialogueNode(dialogueNode, originView, preciseElements);
        }

        public void NotifyModifiedDialogue(bool modificationStillInProgress = false, DocumentDialogueView originView = null, List<string> preciseElements = null)
        {
            // Very blunt
            if (modificationStillInProgress)
                SetPendingDirty();
            else
                SetDirty();
            RefreshViews(originView);
        }

        public void NotifyModifiedDialogueData(bool modificationStillInProgress = false, DocumentDialogueView originView = null, List<string> preciseElements = null)
        {
            // For legacy reasons, we allow properties views to directly modify the model.
            // (here: the data of the dialogue object itself, not any dialogue node)
            // They should notify us afterwards.
            // (ideally we'd have real controller commands like set value, etc)
            if (modificationStillInProgress)
                SetPendingDirty();
            else
                SetDirty();
            RefreshDialogueData(originView, preciseElements);
        }

        private void PostSave()
        {
            if (PendingDirty)
            {
                PendingDirty = false;
                SaveState();    //No need to raise dirty since we just saved, but the Undo State needs to be stored
            }
        }

        public void OnPostReload()
        {
            PendingDirty = false;
            SaveState();

            SelectRootNode();
            foreach (DialogueViewHolder holder in DialogueViews)
            {
                if (holder != null && holder.DialogueView != null)
                    holder.DialogueView.OnPostReload();
            }
        }

        public void Save()
        {
            ResolvePendingDirty();
            if (Dirty)
            {
                ExporterJson.SaveDialogueFile(ProjectController.Project, Dialogue);
                Dirty = false;
                RefreshDirtyFlags();
                PostSave();
            }
        }

        public void Reload()
        {
            ExporterJson.LoadDialogueFile(ProjectController.Project, Dialogue);
            Dirty = false;
            RefreshDirtyFlags();
        }

        public void Activate<T>() where T : DocumentDialogueView
        {
            foreach (DialogueViewHolder holder in DialogueViews)
            {
                if (holder != null && holder.DialogueView is T)
                {
                    // Arbitrarily activate the first view
                    holder.DialogueView.Activate(); //OnActiveDocumentChanged will handle the refresh
                    return;
                }
            }
        }

        public bool OnDocumentDialogueClosed(DocumentDialogueView documentView)
        {
            foreach (DialogueViewHolder holder in DialogueViews)
            {
                if (holder != null && holder.DialogueView != null && holder.DialogueView == documentView)
                {
                    DialogueViews.Remove(holder);
                    return true;
                }
            }
            return false;
        }

        public void CloseViews(bool force)
        {
            // TODO: there's probably a more elegant way to do it
            while (HasView())
            {
                foreach (DialogueViewHolder holder in DialogueViews)
                {
                    if (holder != null && holder.DialogueView != null)
                    {
                        holder.DialogueView.ForceClose = force;
                        holder.DialogueView.Close();
                        break;
                    }
                }
            }
        }

        public bool HasView(int minNumberOfViews = 1)
        {
            return HasView<DocumentDialogueView>(minNumberOfViews);
        }

        public bool HasView<T>(int minNumberOfViews = 1) where T : DocumentDialogueView
        {
            int nViews = 0;
            foreach (DialogueViewHolder holder in DialogueViews)
            {
                if (holder != null && holder.DialogueView is T)
                    nViews++;
            }
            return nViews >= minNumberOfViews;
        }
        #endregion

        #region Undo/redo

        protected class State
        {
            public string Content;
            public int SelectedDialogueNodeID = DialogueNode.ID_NULL;
        }

        public void SaveState()
        {
            //Remove all States following the current State
            if (indexState < previousStates.Count - 1)
            {
                previousStates.RemoveRange(indexState + 1, previousStates.Count - indexState - 1);
            }

            //Append the new State
            previousStates.Add(new State()
            {
                Content = ExporterJson.SaveDialogueToString(ProjectController.Project, Dialogue),
                SelectedDialogueNodeID = SelectedDialogueNode != null ? SelectedDialogueNode.ID : DialogueNode.ID_NULL
            });

            indexState = previousStates.Count - 1;

            //Shrink list if needed by removing older States
            int maxStates = EditorCore.Settings.MaxUndoLevels + 1;  //I add +1 because the first state stored is the original file
            if (previousStates.Count > maxStates)
            {
                previousStates.RemoveRange(0, previousStates.Count - maxStates);
                indexState = maxStates - 1;
            }
        }

        private void LoadState(State state)
        {
            ExporterJson.LoadDialogueFromString(ProjectController.Project, Dialogue, state.Content);
            Dirty = true;
            RefreshDirtyFlags();
        }

        public void ResetStates()
        {
            previousStates.Clear();
            indexState = 0;
        }

        public void UndoState()
        {
            if (previousStates.Count >= 2 && indexState > 0)
            {
                State currentState = previousStates.ElementAt(indexState);
                State previousState = previousStates.ElementAt(indexState - 1);
                indexState -= 1;
                LoadState(previousState);
                SelectNodeInternal(currentState.SelectedDialogueNodeID, previousState.SelectedDialogueNodeID);
                RefreshViews();
            }
        }

        public void RedoState()
        {
            if (previousStates.Count >= 2 && indexState < previousStates.Count - 1)
            {
                //State currentState = previousStates.ElementAt(indexState);
                State nextState = previousStates.ElementAt(indexState + 1);
                indexState += 1;
                LoadState(nextState);
                SelectNodeInternal(nextState.SelectedDialogueNodeID);
                RefreshViews();
            }
        }

        #endregion

        #region Copy / Paste
        public bool CopyDialogueNode(DialogueNode dialogueNode)
        {
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
            return true;
        }

        public bool PasteToDialogueNode(DialogueNode nodeTo, bool specialMode, DocumentDialogueView originView = null)
        {
            bool asBranch = false;

            if (specialMode)
            {
                // Special paste mode, when pasting to branch or goto nodes
                if (nodeTo is DialogueNodeGoto)
                {
                    if (EditorHelper.Clipboard is DialogueNode && EditorHelper.ClipboardInfos?.sourceDialogue == Dialogue.GetName())
                    {
                        DialogueNode nodeTarget = Dialogue.GetNodeByID(EditorHelper.ClipboardInfos.sourceNodeID);
                        if (nodeTarget != null)
                        {
                            (nodeTo as DialogueNodeGoto).Goto = nodeTarget;
                            SetDirty();
                            RefreshSelectedDialogueNode(originView);
                            return true;
                        }
                    }
                    return false;
                }
                else if (nodeTo is DialogueNodeBranch)
                {
                    asBranch = true;
                }
                else
                {
                    return false;   // Ignore the paste
                }
            }

            DialogueNode newNode = null;

            if (EditorHelper.Clipboard is Dialogue)
            {
                Dialogue tempDialogue = EditorHelper.Clipboard as Dialogue;
                DialogueNodeRoot newRoot = tempDialogue.RootNode.Clone() as DialogueNodeRoot;

                // Only Copy parameters if we copy a root on another root
                if (nodeTo is DialogueNodeRoot)
                {
                    Dialogue.Copy(tempDialogue);
                }

                // Insert from the first child, and discard the new root
                newNode = newRoot.Next;
                Dialogue.AddNode(newNode);
                GetLastNode(newNode).Next = nodeTo.Next;
                nodeTo.Next = newNode;
            }
            else if (EditorHelper.Clipboard is DialogueNodeReply)
            {
                if (nodeTo is DialogueNodeChoice)
                {
                    newNode = (EditorHelper.Clipboard as DialogueNodeReply).Clone() as DialogueNode;
                    Dialogue.AddNode(newNode);
                    (nodeTo as DialogueNodeChoice).Replies.Add(newNode as DialogueNodeReply);
                }
            }
            else if (EditorHelper.Clipboard is DialogueNode && !(EditorHelper.Clipboard is DialogueNodeRoot))
            {
                newNode = (EditorHelper.Clipboard as DialogueNode).Clone() as DialogueNode;
                Dialogue.AddNode(newNode);
                if (asBranch)
                {
                    GetLastNode(newNode).Next = (nodeTo as DialogueNodeBranch).Branch;
                    (nodeTo as DialogueNodeBranch).Branch = newNode;
                }
                else
                {
                    GetLastNode(newNode).Next = nodeTo.Next;
                    nodeTo.Next = newNode;
                }
            }

            if (newNode != null)
            {
                SelectedDialogueNode = newNode;
                SetDirty();
                RefreshViews(originView);
                return true;
            }
            return false;
        }
        #endregion

        #region Dialogue Node events
        public DialogueNodeSentence AddNodeSentence(DialogueNode parentNode, bool branch, string speakerID = null, string listenerID = null, DocumentDialogueView originView = null)
        {
            DialogueNodeSentence newNode = AddNodeRaw<DialogueNodeSentence>(parentNode, branch, originView);

            if (newNode != null)
            {
                if (speakerID != null)
                    newNode.SpeakerID = speakerID;
                if (listenerID != null)
                    newNode.ListenerID = listenerID;
            }
            return (DialogueNodeSentence)(FinalizeAddNode(newNode, originView));
        }

        public DialogueNodeReply AddNodeReplyRaw(DialogueNode choiceNode, DocumentDialogueView originView = null)
        {
            // NB: Always add as last reply (for now)

            if (choiceNode == null)
                return null;

            if (!(choiceNode is DialogueNodeChoice))
                return null;

            DialogueNodeReply newNode = new DialogueNodeReply();
            Dialogue.AddNode(newNode);

            (choiceNode as DialogueNodeChoice).Replies.Add(newNode);

            return newNode;
        }

        public DialogueNodeReply AddNodeReply(DialogueNode choiceNode, DocumentDialogueView originView = null)
        {
            return (DialogueNodeReply)(FinalizeAddNode(AddNodeReplyRaw(choiceNode, originView)));
        }

        public DialogueNodeChoice AddNodeChoice(DialogueNode parentNode, bool branch, DocumentDialogueView originView = null, int nReplies = 0)
        {
            DialogueNodeChoice newNode = AddNodeRaw<DialogueNodeChoice>(parentNode, branch, originView);

            for (int i = 0; i < nReplies; i++)
            {
                AddNodeReplyRaw(newNode, originView);
            }
            return (DialogueNodeChoice)(FinalizeAddNode(newNode, originView));
        }

        public DialogueNodeBranch AddNodeBranch(DialogueNode parentNode, bool branch, DocumentDialogueView originView = null)
        {
            return AddNode<DialogueNodeBranch>(parentNode, branch, originView);
        }

        public DialogueNodeGoto AddNodeGoto(DialogueNode parentNode, bool branch, DocumentDialogueView originView = null)
        {
            return AddNode<DialogueNodeGoto>(parentNode, branch, originView);
        }

        private T AddNodeRaw<T>(DialogueNode parentNode, bool branch, DocumentDialogueView originView = null) where T : DialogueNode, new()
        {
            if (branch && !(parentNode is DialogueNodeBranch))
                return null;

            T newNode = new T();
            Dialogue.AddNode(newNode);

            if (parentNode != null)
            {
                if (branch)
                {
                    newNode.Next = (parentNode as DialogueNodeBranch).Branch;
                    (parentNode as DialogueNodeBranch).Branch = newNode;
                }
                else
                {
                    newNode.Next = parentNode.Next;
                    parentNode.Next = newNode;
                }
            }

            return newNode;
        }

        private DialogueNode FinalizeAddNode(DialogueNode newNode, DocumentDialogueView originView = null)
        {
            if (newNode == null)
                return null;

            SetDirty();
            RefreshViews(originView);
            EditNode(newNode, false, originView);
            return newNode;
        }

        private T AddNode<T>(DialogueNode parentNode, bool branch, DocumentDialogueView originView = null) where T : DialogueNode, new()
        {
            return (T)(FinalizeAddNode(AddNodeRaw<T>(parentNode, branch, originView), originView));
        }

        public bool RemoveNode(DialogueNode nodeToRemove, DocumentDialogueView originView = null)
        {
            if (nodeToRemove == null)
                return false;

            if (nodeToRemove == Dialogue.RootNode)
                return false; // Can't remove root node

            Dialogue.RemoveNode(nodeToRemove);
            SetDirty();
            RefreshViews(originView);
            return true;
        }

        public bool RemoveAllNodes(DocumentDialogueView originView = null)
        {
            Dialogue.ListNodes.RemoveAll(item => item != Dialogue.RootNode);
            Dialogue.RootNode.Next = null;
            SetDirty();
            RefreshViews(originView);
            return true;
        }

        public bool MoveNode(DialogueNode nodeMoved, DialogueNode nodeTo,
                             DialogueNode nodeMovedParent, DialogueNode nodeToParent,
                             DialogueNode nodeMovedPrev,
                             bool asBranch,
                             DocumentDialogueView originView = null)
        {
            if (nodeMoved == null || nodeTo == null || nodeMoved == nodeTo)
                return false;

            if (nodeMoved is DialogueNodeRoot)
                return false;

            if (nodeMoved is DialogueNodeReply && !(nodeTo is DialogueNodeReply || nodeTo is DialogueNodeChoice))
                return false;

            // Check we are not attaching a node on a depending node (loop)
            List<DialogueNode> dependendingNodes = new List<DialogueNode>();
            Dialogue.GetDependingNodes(nodeMoved, ref dependendingNodes);
            if (dependendingNodes.Contains(nodeTo))
                return false;

            if (nodeMoved is DialogueNodeReply)
            {
                if (nodeTo is DialogueNodeReply)
                {
                    // Remove reply from its choice...
                    (nodeMovedParent as DialogueNodeChoice).Replies.Remove(nodeMoved as DialogueNodeReply);
                    // ... and insert reply after other reply inside other choice
                    (nodeToParent as DialogueNodeChoice).Replies.Insert((nodeToParent as DialogueNodeChoice).Replies.IndexOf(nodeTo as DialogueNodeReply) + 1, nodeMoved as DialogueNodeReply);
                }
                else if (nodeTo is DialogueNodeChoice)
                {
                    // Remove reply from its choice...
                    (nodeMovedParent as DialogueNodeChoice).Replies.Remove(nodeMoved as DialogueNodeReply);
                    // ... and insert reply as first reply of other choice
                    (nodeTo as DialogueNodeChoice).Replies.Insert(0, nodeMoved as DialogueNodeReply);
                }
                else
                {
                    return false;   //this should not happen, the case is checked above
                }
            }
            else
            {
                // Remove node from current position...
                if (nodeMovedPrev != null)
                {
                    nodeMovedPrev.Next = nodeMoved.Next;
                    nodeMoved.Next = null;
                }
                else
                {
                    if (nodeMovedParent is DialogueNodeBranch && (nodeMovedParent as DialogueNodeBranch).Branch == nodeMoved)
                    {
                        // Node is a branch child, we need to redirect the branch
                        (nodeMovedParent as DialogueNodeBranch).Branch = nodeMoved.Next;
                        nodeMoved.Next = null;
                    }
                    else
                    {
                        nodeMovedParent.Next = nodeMoved.Next;
                        nodeMoved.Next = null;
                    }
                }

                // ... and insert node on new position
                if (asBranch)
                {
                    nodeMoved.Next = (nodeTo as DialogueNodeBranch).Branch;
                    (nodeTo as DialogueNodeBranch).Branch = nodeMoved;
                }
                else
                {
                    nodeMoved.Next = nodeTo.Next;
                    nodeTo.Next = nodeMoved;
                }
            }
            SetDirty();
            RefreshViews(originView);
            EditNode(nodeTo, false, originView);
            return true;
        }

        public void SelectNode(DialogueNode nodeToSelect, bool force = false, DocumentDialogueView originView = null, bool bInternal = false)
        {
            bool doSelect = SelectedDialogueNode != nodeToSelect || force;
            if (doSelect)
            {
                ResolvePendingDirty();
                SelectedDialogueNode = nodeToSelect;
            }
            ProjectController.ShowProperties(this, SelectedDialogueNode);   // Must be done anyway for the OnActiveDocumentChanged case
            if (doSelect)
            {
                if (!bInternal)
                    RefreshSelectedDialogueNode(originView);
            }
        }

        private void SelectRootNode()
        {
            SelectNode(Dialogue.RootNode, false, null, true);
        }

        public void SelectNode(int ID, int planB_ID = DialogueNode.ID_NULL, bool bInternal = false)
        {
            DialogueNode node = Dialogue.GetNodeByID(ID);
            if (node == null && planB_ID != DialogueNode.ID_NULL)
                node = Dialogue.GetNodeByID(planB_ID);
            if (node == null)
                node = Dialogue.RootNode;
            SelectNode(node, false, null, bInternal);
        }

        private void SelectNodeInternal(int ID, int planB_ID = DialogueNode.ID_NULL)
        {
            SelectNode(ID, planB_ID, true);
        }

        public void EditNode(DialogueNode nodeToEdit, bool force = false, DocumentDialogueView originView = null)
        {
            SelectNode(nodeToEdit, force, originView);
            if (nodeToEdit is DialogueNodeSentence ||
                nodeToEdit is DialogueNodeChoice ||
                nodeToEdit is DialogueNodeReply)
            {
                ProjectController.ForcePropertiesFocus();
            }
        }
        #endregion

        #region Other events
        public void UpdateActorID(string actorIDFrom, string actorIDTo)
        {
            if (actorIDFrom != "" && actorIDTo != "")
            {
                if (Dialogue.UpdateActorID(actorIDFrom, actorIDTo))
                {
                    SetDirty();
                    RefreshViews();
                }
            }
        }
        #endregion

        #region Helpers
        private DialogueNode GetLastNode(DialogueNode node)
        {
            DialogueNode lastNode = node;
            while (lastNode?.Next != null)
            {
                lastNode = lastNode.Next;
            }
            return lastNode;
        }
        #endregion
    };
}
