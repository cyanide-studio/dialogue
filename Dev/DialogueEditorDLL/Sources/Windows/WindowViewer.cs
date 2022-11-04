using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class WindowViewer : Form
    {
        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        protected DocumentDialogue currentDocument = null;
        protected Dialogue currentDialogue = null;
        protected DialogueNode currentNode = null;

        protected bool runningDialogue = false;
        protected List<DialogueNode> previousNodes = new List<DialogueNode>();
        protected List<DialogueNodeReply> currentReplies = null;

        private Font fontReplies = null;
        private Font fontConditions = null;

        protected enum EOptionConditions
        {
            AskEveryBranch,
            //AskEveryTime,
            AlwaysTrue,
            AlwaysFalse,
        }

        protected bool UseGameActions => checkUseGameActions.Checked;
        protected bool UseGameConditions => checkUseGameConditions.Checked;
        protected EOptionConditions OptionConditions { get { return GetOptionConditions(comboBoxOptionConditions.SelectedItem); } }
        protected bool ShowReplyConditions => checkBoxShowReplyConditions.Checked;
        protected bool ApplyConstants => checkBoxOptionConstants.Checked;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public WindowViewer(Dialogue dialogue, DialogueNode nodeFrom, DocumentDialogue document)
        {
            InitializeComponent();

            comboBoxOptionConditions.DataSource = new BindingSource(Enum.GetValues(typeof(EOptionConditions)), null);

            // Put some group boxes atop each other
            groupBoxGoto.Location = groupBoxChoice.Location;

            fontReplies = listBoxReplies.Font;
            listBoxReplies.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxReplies.DrawItem += OnListBoxReplies_DrawItem;

            fontConditions = listBoxConditions.Font;
            listBoxConditions.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxConditions.DrawItem += OnListBoxConditions_DrawItem;

            checkUseGameActions.Checked = EditorCore.Settings.PlayViewerUseGameActions;
            checkUseGameConditions.Checked = EditorCore.Settings.PlayViewerUseGameConditions;
            checkBoxOptionConstants.Checked = true; // EditorCore.Settings.UseConstants ?

            comboBoxOptionConditions.Enabled = !UseGameConditions;

            currentDocument = document;
            currentDialogue = dialogue;

            Play(nodeFrom);
        }

        public void Play(DialogueNode nodeFrom = null)
        {
            if (nodeFrom != null && (nodeFrom is DialogueNodeReply))
            {
                foreach (var node in currentDialogue.ListNodes)
                {
                    if (node is DialogueNodeChoice)
                    {
                        if (((DialogueNodeChoice)node).Replies.Contains(nodeFrom))
                        {
                            nodeFrom = node;
                            break;
                        }
                    }
                }
            }

            runningDialogue = true;

            if (EditorCore.OnPlayDialogueStart != null)
            {
                PlayDialogueContext context = new PlayDialogueContext();
                context.FullDialogue = nodeFrom == null;
                EditorCore.OnPlayDialogueStart(context);
            }

            if (nodeFrom != null  && !(nodeFrom is DialogueNodeRoot))
            {
                PlayNode(nodeFrom);
            }
            else if (currentDialogue.RootNode != null)
            {
                PlayNode(currentDialogue.RootNode.Next);
            }
            else
            {
                Stop();
            }
        }

        public void Finish()
        {
            // Do not call this here, because the user can still use the "back" command and go backwards.
            //EditorCore.OnPlayDialogueEnd();

            currentReplies = null;
            currentNode = null;

            ResetGroups();
            ResetSentence();
        }

        public void Stop()
        {
            runningDialogue = false;

            if (EditorCore.OnPlayDialogueEnd != null)
                EditorCore.OnPlayDialogueEnd();

            previousNodes.Clear();
            currentReplies = null;
            currentNode = null;

            ResetGroups();
            ResetSentence();
        }

        public void Restart()
        {
            Stop();
            Play();
        }

        protected void ResetSentence()
        {
            pictureBoxSpeaker.Image = null;
            pictureBoxListener.Image = null;
            labelSpeaker.Text = "";
            labelListener.Text = "";
            labelSentence.Text = "";
        }

        protected void ResetGroups()
        {
            groupBoxConditions.Visible = false;
            radioButtonConditionsTrue.Checked = true;   //reset this to true, it is used when jumping to next node, even if it's invisible
            listBoxConditions.DataSource = null;

            groupBoxGoto.Visible = false;

            groupBoxChoice.Visible = false;
            listBoxReplies.DataSource = null;

            labelRepliesConditions.Enabled = false;
            checkBoxShowReplyConditions.Enabled = false;
            radioButtonReplyConditionsTrue.Enabled = false;
            radioButtonReplyConditionsFalse.Enabled = false;
        }

        protected virtual void PlayNextNode()
        {
            if (currentNode != null)
            {
                if (radioButtonConditionsFalse.Checked)
                {
                    PlayNode(currentNode.Next);
                }
                else if (currentNode is DialogueNodeChoice)
                {
                    var reply = listBoxReplies.SelectedItem as DialogueNodeReply;
                    if (reply != null)
                    {
                        PlayNode(reply);
                    }
                }
                else if (currentNode is DialogueNodeGoto)
                {
                    var nodeGoto = currentNode as DialogueNodeGoto;
                    PlayNode(nodeGoto.Goto);
                }
                else if (currentNode is DialogueNodeBranch)
                {
                    var nodeBranch = currentNode as DialogueNodeBranch;
                    PlayNode(nodeBranch.Branch);
                }
                else
                {
                    PlayNode(currentNode.Next);
                }
            }
        }

        protected virtual void PlayPreviousNode()
        {
            if (previousNodes.Count > 0)
            {
                DialogueNode previous = previousNodes.Last();
                previousNodes.RemoveAt(previousNodes.Count - 1);
                PlayNode(previous, false);
            }
        }

        protected void PlayNode(DialogueNode nextNode, bool forward = true)
        {
            if (forward && currentNode != null && !(currentNode is DialogueNodeReply) && !(currentNode is DialogueNodeGoto))
                previousNodes.Add(currentNode);

            currentReplies = null;

            if (currentNode != null)
            {
                PlayNodeActions(false);
            }

            if (nextNode == null)
            {
                Finish();
                return;
            }

            currentNode = nextNode;

            currentDocument.SelectNode(currentNode);

            ResetGroups();

            PlayNodeActions(true);

            if (!TestNodeConditions(currentNode))
            {
                PlayNode(currentNode.Next);
                return;
            }

            if (currentNode is DialogueNodeSentence)
            {
                var sentence = currentNode as DialogueNodeSentence;

                ResetSentence();

                Actor speaker = ResourcesHandler.Project.GetActorFromID(sentence.SpeakerID);
                if (speaker != null)
                {
                    labelSpeaker.Text = speaker.Name;

                    string strPathPortraitSpeaker = Path.Combine(EditorHelper.GetProjectDirectory(), speaker.Portrait);
                    if (File.Exists(strPathPortraitSpeaker))
                        pictureBoxSpeaker.Image = Image.FromFile(strPathPortraitSpeaker);
                }

                Actor listener = ResourcesHandler.Project.GetActorFromID(sentence.ListenerID);
                if (listener != null)
                {
                    labelListener.Text = listener.Name;

                    string strPathPortraitListener = Path.Combine(EditorHelper.GetProjectDirectory(), listener.Portrait);
                    if (File.Exists(strPathPortraitListener))
                        pictureBoxListener.Image = Image.FromFile(strPathPortraitListener);
                }

                if (ApplyConstants)
                    labelSentence.Text = EditorHelper.FormatTextEntry(sentence.Sentence, EditorCore.LanguageWorkstring);
                else
                    labelSentence.Text = sentence.Sentence;
            }
            else if (currentNode is DialogueNodeChoice)
            {
                var nodeChoice = currentNode as DialogueNodeChoice;

                currentReplies = new List<DialogueNodeReply>();
                foreach (var nodeReply in nodeChoice.Replies)
                {
                    if (TestNodeConditions(nodeReply))
                    {
                        currentReplies.Add(nodeReply);
                    }
                }

                groupBoxChoice.Visible = true;
                labelChoice.Text = String.Format("Choice : {0}", nodeChoice.Choice);
                listBoxReplies.DataSource = new BindingSource(currentReplies, null);

                checkBoxShowReplyConditions.Checked = false;
                radioButtonReplyConditionsTrue.Checked = true;

                foreach (DialogueNodeReply reply in currentReplies)
                {
                    if (reply.Conditions.Count > 0)
                    {
                        labelRepliesConditions.Enabled = true;
                        checkBoxShowReplyConditions.Enabled = true;
                        radioButtonReplyConditionsTrue.Enabled = true;
                        radioButtonReplyConditionsFalse.Enabled = true;
                        break;
                    }
                }
            }
            else if (currentNode is DialogueNodeReply)
            {
                var nodeReply = currentNode as DialogueNodeReply;
                PlayNode(nodeReply.Next);
            }
            else if (currentNode is DialogueNodeGoto)
            {
                var nodeGoto = currentNode as DialogueNodeGoto;

                if (ShouldWaitForNodeConditions(nodeGoto, branchingNode: true))
                {
                    groupBoxGoto.Visible = true;
                    labelGoto.Text = String.Format("Goto > \"{0}\"", GetGotoText(nodeGoto.Goto));

                    ShowConditions(nodeGoto);
                }
                else if (TestNodeConditions(nodeGoto))
                {
                    PlayNode(nodeGoto.Goto);
                }
                else
                {
                    PlayNode(nodeGoto.Next);
                }
            }
            else if (currentNode is DialogueNodeBranch)
            {
                var nodeBranch = currentNode as DialogueNodeBranch;

                if (ShouldWaitForNodeConditions(nodeBranch, branchingNode: true))
                {
                    groupBoxGoto.Visible = true;
                    labelGoto.Text = String.Format("Branch > \"{0}\"", GetGotoText(nodeBranch.Branch));

                    ShowConditions(nodeBranch);
                }
                else if (TestNodeConditions(nodeBranch))
                {
                    PlayNode(nodeBranch.Branch);
                }
                else
                {
                    PlayNode(nodeBranch.Next);
                }
            }
        }

        protected void PlayNodeActions(bool nodeStart)
        {
            if (UseGameActions)
            {
                foreach (var action in currentNode.Actions)
                {
                    if (action.OnNodeStart == nodeStart)
                    {
                        action.OnPlayNodeAction(nodeStart);
                    }
                }
            }
        }

        protected bool ShouldWaitForNodeConditions(DialogueNode node, bool branchingNode)
        {
            if (node.Conditions.Count > 0)
            {
                if (UseGameConditions)
                {
                    return false;
                }
                //else if (OptionConditions == EOptionConditions.AskEveryTime)
                //{
                //    return true;
                //}
                else if (branchingNode && OptionConditions == EOptionConditions.AskEveryBranch)
                {
                    return true;
                }
            }

            return false;
        }

        protected bool TestNodeConditions(DialogueNode node)
        {
            if (node.Conditions.Count > 0)
            {
                if (UseGameConditions)
                {
                    bool result = true;
                    foreach (var condition in node.Conditions)
                    {
                        result &= condition.IsPlayConditionValid();
                    }

                    return result;
                }
                else if (OptionConditions == EOptionConditions.AlwaysFalse)
                {
                    return false;
                }
                else if (OptionConditions == EOptionConditions.AlwaysTrue)
                {
                    return true;
                }
            }

            return true;
        }

        protected void ShowConditions(DialogueNode node)
        {
            groupBoxConditions.Visible = true;
            radioButtonConditionsTrue.Checked = true;

            if (node.Conditions.Count > 0)
                listBoxConditions.DataSource = new BindingSource(node.Conditions, null);
        }

        protected string GetGotoText(DialogueNode node)
        {
            if (node != null)
            {
                if (node is DialogueNodeSentence)
                {
                    if (ApplyConstants)
                        return EditorHelper.FormatTextEntry((node as DialogueNodeSentence).Sentence, EditorCore.LanguageWorkstring);
                    else
                        return (node as DialogueNodeSentence).Sentence;
                }
                else if (node is DialogueNodeChoice)
                {
                    return (node as DialogueNodeChoice).Choice;
                }
                else if (node is DialogueNodeGoto)
                {
                    return GetGotoText((node as DialogueNodeGoto).Goto);
                }
            }
            return "";
        }

        protected EOptionConditions GetOptionConditions(object item)
        {
            EOptionConditions value = (EOptionConditions)Enum.Parse(typeof(EOptionConditions), item.ToString());
            return value;
        }

        protected virtual void EditCurrentNode()
        {
            if (currentDocument != null && currentNode != null)
            {
                currentDocument.SelectNode(currentNode);
            }

            Close();
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events

        private void OnRestart(object sender, EventArgs e)
        {
            Restart();
        }

        private void OnNextNode(object sender, EventArgs e)
        {
            PlayNextNode();
        }

        private void OnPreviousNode(object sender, EventArgs e)
        {
            PlayPreviousNode();
        }

        private void OnListBoxReplies_DrawItem(object sender, DrawItemEventArgs e)
        {
            var listBox = sender as ListBox;

            if (e.Index < 0 || e.Index >= listBox.Items.Count)
                return;

            var g = e.Graphics;
            var item = listBox.Items[e.Index] as DialogueNodeReply;

            var color = Color.Black;
            if (!groupBoxChoice.Enabled)
            {
                color = Color.LightGray;
            }
            else if (item.Conditions.Count > 0 && radioButtonReplyConditionsFalse.Checked)
            {
                color = Color.Gray;
            }

            StringBuilder stringBuilder = new StringBuilder();

            if (ShowReplyConditions)
            {
                foreach (NodeCondition condition in item.Conditions)
                    stringBuilder.AppendFormat("[{0}] ", condition.GetDisplayText());
            }

            if (ApplyConstants)
                stringBuilder.Append(EditorHelper.FormatTextEntry(item.Reply, EditorCore.LanguageWorkstring));
            else
                stringBuilder.Append(item.Reply);

            e.DrawBackground();

            if (e.State.HasFlag(DrawItemState.Selected))
                g.FillRectangle(new SolidBrush(Color.LightBlue), e.Bounds);
            //else
            //    g.FillRectangle(new SolidBrush(Color.White), e.Bounds);

            g.DrawString(stringBuilder.ToString(), fontReplies, new SolidBrush(color), new PointF(e.Bounds.X, e.Bounds.Y));

            e.DrawFocusRectangle();
        }

        private void OnListBoxConditions_DrawItem(object sender, DrawItemEventArgs e)
        {
            var listBox = sender as ListBox;

            if (e.Index < 0 || e.Index >= listBox.Items.Count)
                return;

            var g = e.Graphics;
            var item = listBox.Items[e.Index] as NodeCondition;

            var color = Color.Black;

            e.DrawBackground();

            g.FillRectangle(new SolidBrush(Color.White), e.Bounds);

            g.DrawString(item.GetDisplayText(), fontConditions, new SolidBrush(color), new PointF(e.Bounds.X, e.Bounds.Y));

            e.DrawFocusRectangle();
        }

        private void OnComboBoxOptionConditionsFormat(object sender, ListControlConvertEventArgs e)
        {
            EOptionConditions value = GetOptionConditions(e.ListItem);
            if (value == EOptionConditions.AskEveryBranch)
                e.Value = "Ask Every Branch";
            //else if (value == EOptionConditions.AskEveryTime)
            //    e.Value = "Ask Every Time";
            else if (value == EOptionConditions.AlwaysTrue)
                e.Value = "Always True";
            else if (value == EOptionConditions.AlwaysFalse)
                e.Value = "Always False";
        }

        private void OnConditionsChecked(object sender, EventArgs e)
        {
            if (radioButtonConditionsTrue.Checked)
            {
                groupBoxChoice.Enabled = true;
                if (listBoxReplies.Items.Count > 0)
                    listBoxReplies.SelectedIndex = 0;

                groupBoxGoto.Enabled = true;
            }
            else
            {
                groupBoxChoice.Enabled = false;
                listBoxReplies.ClearSelected();

                groupBoxGoto.Enabled = false;
            }
        }

        private void OnReplyConditionsChanged(object sender, EventArgs e)
        {
            listBoxReplies.RefreshItems();
        }

        private void OnReplyConditionsChecked(object sender, EventArgs e)
        {
            listBoxReplies.RefreshItems();
        }

        private void OnEditNode(object sender, EventArgs e)
        {
            EditCurrentNode();
        }

        private void OnOptionConstantsChanged(object sender, EventArgs e)
        {
        }

        private void OnUseGameActionsChanged(object sender, EventArgs e)
        {
            EditorCore.Settings.PlayViewerUseGameActions = UseGameActions;
        }

        private void OnUseGameConditionsChanged(object sender, EventArgs e)
        {
            EditorCore.Settings.PlayViewerUseGameConditions = UseGameConditions;
            comboBoxOptionConditions.Enabled = !UseGameConditions;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape || keyData == Keys.F5)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            if (runningDialogue)
            {
                runningDialogue = false;

                if (EditorCore.OnPlayDialogueEnd != null)
                    EditorCore.OnPlayDialogueEnd();
            }
        }
    }
}
