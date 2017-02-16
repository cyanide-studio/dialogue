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

        protected List<DialogueNode> previousNodes = new List<DialogueNode>();

        private Font fontReplies = null;
        private Font fontConditions = null;

        protected enum EOptionConditions
        {
            AskEveryTime,
            AlwaysTrue,
            AlwaysFalse,
        }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public WindowViewer(Dialogue dialogue, DialogueNode nodeFrom, DocumentDialogue document)
        {
            InitializeComponent();

            comboBoxOptionConditions.DataSource = new BindingSource(Enum.GetValues(typeof(EOptionConditions)), null);

            groupBoxGoto.Location = groupBoxChoice.Location;

            fontReplies = listBoxReplies.Font;
            listBoxReplies.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxReplies.DrawItem += OnListBoxReplies_DrawItem;

            fontConditions = listBoxConditions.Font;
            listBoxConditions.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxConditions.DrawItem += OnListBoxConditions_DrawItem;

            checkBoxOptionConstants.Checked = true; // EditorCore.Settings.UseConstants;

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
            currentNode = null;

            ResetGroups();
            ResetSentence();
        }

        public void Stop()
        {
            previousNodes.Clear();
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
                        PlayNode(reply.Next);
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

            if (nextNode == null)
            {
                Finish();
                return;
            }

            currentNode = nextNode;

            currentDocument.SelectNode(currentNode);

            ResetGroups();
            
            if (currentNode.Conditions.Count > 0 && GetCurrentOptionConditions() == EOptionConditions.AlwaysFalse)
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

                if (checkBoxOptionConstants.Checked)
                    labelSentence.Text = EditorHelper.FormatTextEntry(sentence.Sentence, EditorCore.LanguageWorkstring);
                else
                    labelSentence.Text = sentence.Sentence;
            }
            else if (currentNode is DialogueNodeChoice)
            {
                var choice = currentNode as DialogueNodeChoice;
                
                groupBoxChoice.Visible = true;
                labelChoice.Text = String.Format("Choice : {0}", choice.Choice);
                listBoxReplies.DataSource = new BindingSource(choice.Replies, null);

                checkBoxShowReplyConditions.Checked = false;
                radioButtonReplyConditionsTrue.Checked = true;

                foreach (DialogueNodeReply reply in choice.Replies)
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

                if (choice.Conditions.Count > 0 && GetCurrentOptionConditions() == EOptionConditions.AskEveryTime)
                {
                    ShowConditions(choice);
                }
            }
            else if (currentNode is DialogueNodeGoto)
            {
                var nodeGoto = currentNode as DialogueNodeGoto;

                if (nodeGoto.Conditions.Count > 0 && GetCurrentOptionConditions() == EOptionConditions.AskEveryTime)
                {
                    groupBoxGoto.Visible = true;
                    labelGoto.Text = String.Format("Goto > \"{0}\"", GetGotoText(nodeGoto.Goto));

                    ShowConditions(nodeGoto);
                }
                else
                {
                    PlayNode(nodeGoto.Goto);
                }
            }
            else if (currentNode is DialogueNodeBranch)
            {
                var nodeBranch = currentNode as DialogueNodeBranch;

                if (nodeBranch.Conditions.Count > 0 && GetCurrentOptionConditions() == EOptionConditions.AskEveryTime)
                {
                    groupBoxGoto.Visible = true;
                    labelGoto.Text = String.Format("Branch > \"{0}\"", GetGotoText(nodeBranch.Branch));

                    ShowConditions(nodeBranch);
                }
                else
                {
                    PlayNode(nodeBranch.Branch);
                }
            }
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
                    if (checkBoxOptionConstants.Checked)
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

        protected EOptionConditions GetCurrentOptionConditions()
        {
            return GetOptionConditions(comboBoxOptionConditions.SelectedItem);
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

            if (checkBoxShowReplyConditions.Checked)
            {
                foreach (NodeCondition condition in item.Conditions)
                    stringBuilder.AppendFormat("[{0}] ", condition.GetDisplayText());
            }

            if (checkBoxOptionConstants.Checked)
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
            if (value == EOptionConditions.AskEveryTime)
                e.Value = "Ask Every Time";
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape || keyData == Keys.F5)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
