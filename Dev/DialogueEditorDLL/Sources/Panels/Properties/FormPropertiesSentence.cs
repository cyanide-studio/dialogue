using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DialogueEditor
{
    public partial class FormPropertiesSentence : UserControl, IFormProperties
    {
        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        protected DocumentDialogue document;
        protected TreeNode treeNode;
        protected DialogueNodeSentence dialogueNode;

        protected bool isEditingWorkstring = false;
        protected string originalWorkstring = "";

        protected AutoComplete autoComplete;

        protected bool ready = false;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public FormPropertiesSentence()
        {
            InitializeComponent();

            Dock = System.Windows.Forms.DockStyle.Fill;
        }

        public void Clear()
        {
            ready = false;

            autoComplete.Dispose();

            comboBoxSpeaker.DataSource = null;
            comboBoxListener.DataSource = null;
            comboBoxAnimsetSpeaker.DataSource = null;
            comboBoxAnimsetListener.DataSource = null;

            comboBoxIntensity.DataSource = null;

            if (picturePortraitSpeaker.Image != null)
                picturePortraitSpeaker.Image.Dispose();
            if (picturePortraitListener.Image != null)
                picturePortraitListener.Image.Dispose();
            picturePortraitSpeaker.Image = null;
            picturePortraitListener.Image = null;

            Dispose();
        }

        public void ForceFocus()
        {
            textBoxWorkstring.Focus();
            textBoxWorkstring.Select(textBoxWorkstring.TextLength, 0);  //jump to text end
        }

        public bool IsEditingWorkstring()
        {
            return textBoxWorkstring.Focused;
        }

        public void OnResolvePendingDirty()
        {
            if (isEditingWorkstring)
            {
                if (originalWorkstring != dialogueNode.Sentence)
                {
                    dialogueNode.LastEditDate = Utility.GetCurrentTime();
                }
                else
                {
                    document.CancelPendingDirty();
                }

                isEditingWorkstring = false;
                originalWorkstring = "";
            }
        }

        public void Init(DocumentDialogue inDocument, TreeNode inTreeNode, DialogueNode inDialogueNode)
        {
            document = inDocument;
            treeNode = inTreeNode;
            dialogueNode = inDialogueNode as DialogueNodeSentence;

            Project project = ResourcesHandler.Project;

            //Text
            textBoxWorkstring.Text = dialogueNode.Sentence;
            RefreshWordCount();

            //Actors
            if (ResourcesHandler.Project.ListActors.Count > 0)
            {
                var actors = new Dictionary<string, string>();
                actors.Add("", "");
                foreach (Actor actor in project.ListActors)
                {
                    actors.Add(actor.ID, actor.Name);
                }

                comboBoxSpeaker.DataSource = new BindingSource(actors, null);
                comboBoxSpeaker.ValueMember = "Key";
                comboBoxSpeaker.DisplayMember = "Value";

                comboBoxListener.DataSource = new BindingSource(actors, null);
                comboBoxListener.ValueMember = "Key";
                comboBoxListener.DisplayMember = "Value";

                comboBoxSpeaker.SelectedValue = dialogueNode.SpeakerID;
                comboBoxListener.SelectedValue = dialogueNode.ListenerID;
            }

            //Anims
            RefreshAnimSetList(comboBoxAnimsetSpeaker, dialogueNode.SpeakerID);
            RefreshAnimSetList(comboBoxAnimsetListener, dialogueNode.ListenerID);

            comboBoxAnimsetSpeaker.SelectedItem = dialogueNode.SpeakerAnimset;
            comboBoxAnimsetListener.SelectedItem = dialogueNode.ListenerAnimset;

            RefreshAnimList(comboBoxAnimsetSpeaker, comboBoxAnimSpeaker);
            RefreshAnimList(comboBoxAnimsetListener, comboBoxAnimListener);

            comboBoxAnimSpeaker.SelectedValue = dialogueNode.SpeakerAnim;
            comboBoxAnimListener.SelectedValue = dialogueNode.ListenerAnim;

            //Voicing
            checkBoxHideSubtitle.Checked = dialogueNode.HideSubtitle;
            textBoxComment.Text = dialogueNode.Comment;
            textBoxContext.Text = dialogueNode.Context;

            comboBoxIntensity.DataSource = new BindingSource(EditorCore.CustomLists["Intensities"], null);
            comboBoxIntensity.ValueMember = "Key";
            comboBoxIntensity.DisplayMember = "Value";
            comboBoxIntensity.SelectedValue = dialogueNode.VoiceIntensity;

            //Portraits
            RefreshPortraits();

            //AutoComplete
            autoComplete = new AutoComplete(this, new BindingSource(project.ListConstants, null));
            autoComplete.OnValidate = ValidateAutoComplete;
            autoComplete.OnClose = CloseAutoComplete;
            autoComplete.OnDrawItem = DrawItemAutoComplete;

            //Ready !
            ready = true;
        }

        private void RefreshWordCount()
        {
            labelWordCount.Text = "(" + dialogueNode.Sentence.Length + " chars)";

            Color color = Color.FromArgb(255, 0, 0, 0);
            if (dialogueNode.Sentence.Length > ResourcesHandler.Project.MaxLengthSentence)
            {
                color = Color.FromArgb(255, 250, 100, 0);
            }

            labelWordCount.ForeColor = color;
        }

        private void RefreshPortraits()
        {
            Project project = ResourcesHandler.Project;
            Actor actorSpeaker = project.GetActorFromID(dialogueNode.SpeakerID);
            Actor actorListener = project.GetActorFromID(dialogueNode.ListenerID);

            picturePortraitSpeaker.Image = null;
            if (actorSpeaker != null)
            {
                string pathPortraitSpeaker = Path.Combine(EditorHelper.GetProjectDirectory(), actorSpeaker.Portrait);
                if (File.Exists(pathPortraitSpeaker))
                {
                    using (Image temp = Image.FromFile(pathPortraitSpeaker))
                    {
                        picturePortraitSpeaker.Image = new Bitmap(temp);
                    }
                }
            }

            picturePortraitListener.Image = null;
            if (actorListener != null)
            {
                string pathPortraitListener = Path.Combine(EditorHelper.GetProjectDirectory(), actorListener.Portrait);
                if (File.Exists(pathPortraitListener))
                {
                    using (Image temp = Image.FromFile(pathPortraitListener))
                    {
                        picturePortraitListener.Image = new Bitmap(temp);
                    }
                }
            }
        }

        private void RefreshAnimSetList(ComboBox comboBoxMood, string actorID)
        {
            Project project = ResourcesHandler.Project;
            if (project == null)
                return;

            var animsets = new List<string>();
            animsets.Add("");

            Actor actor = project.GetActorFromID(actorID);
            if (actor != null && EditorCore.GetActorAnimsets != null)
            {
                List<string> actorAnimsets = EditorCore.GetActorAnimsets(actorID);
                if (actorAnimsets.Count > 0)
                    animsets.AddRange(actorAnimsets);
            }
            else
            {
                animsets.AddRange(EditorCore.Animations.Keys);
            }

            comboBoxMood.DataSource = new BindingSource(animsets, null);
        }

        private void RefreshAnimList(ComboBox comboBoxMood, ComboBox comboBoxAnim)
        {
            string animset = comboBoxMood.SelectedValue as string;

            var anims = new Dictionary<string, string>();
            if (EditorCore.Animations.ContainsKey(animset))
            {
                try
                {
                    anims.Add("", "<Auto>");
                    anims = anims.Concat(EditorCore.Animations[animset].ToDictionary(item => item))
                                 .ToDictionary(item => item.Key, item => item.Value);
                }
                catch (System.ArgumentException)
                {
                    EditorCore.LogError(String.Format("Animset \"{0}\" contains duplicate animation names", animset));
                }

                comboBoxAnim.DataSource = new BindingSource(anims, null);
                comboBoxAnim.ValueMember = "Key";
                comboBoxAnim.DisplayMember = "Value";
                comboBoxAnim.Enabled = true;
            }
            else
            {
                comboBoxAnim.DataSource = null;
                comboBoxAnim.Enabled = false;
            }
        }

        private void OpenAutoComplete()
        {
            autoComplete.Open(textBoxWorkstring);
        }

        private void CloseAutoComplete()
        {
            if (autoComplete.Close())
                textBoxWorkstring.Focus();
        }

        private void ValidateAutoComplete()
        {
            var constant = autoComplete.GetSelectedItem() as Constant;
            if (constant != null)
            {
                int position = textBoxWorkstring.SelectionStart;
                textBoxWorkstring.Text = textBoxWorkstring.Text.Insert(position, "{" + constant.ID + "}");
                textBoxWorkstring.SelectionStart = position + constant.ID.Count() + 2;    //size of ID + braces

                CloseAutoComplete();
            }
        }

        private string DrawItemAutoComplete(object item)
        {
            var constant = item as Constant;
            return String.Format("{0}  ({1})", constant.ID, constant.Workstring);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events

        private void OnWorkstringKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.OemQuotes)   // ²
            if (e.KeyCode == Keys.Tab)
            {
                OpenAutoComplete();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void OnWorkstringChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            RichTextBox textBox = sender as RichTextBox;

            if (!isEditingWorkstring)
            {
                isEditingWorkstring = true;
                originalWorkstring = dialogueNode.Sentence;
            }

            dialogueNode.Sentence = EditorHelper.SanitizeText(textBox.Text);

            RefreshWordCount();

            document.RefreshTreeNode(treeNode);
            //document.SetDirty();
            document.SetPendingDirty();
        }

        private void OnWorkstringValidated(object sender, EventArgs e)
        {
            if (!ready)
                return;

            document.ResolvePendingDirty();
        }

        private void OnSpeakerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.SpeakerID = comboBoxSpeaker.SelectedValue as string;

            RefreshPortraits();
            RefreshAnimSetList(comboBoxAnimsetSpeaker, dialogueNode.SpeakerID);

            document.RefreshTreeNode(treeNode);
            document.SetDirty();
        }

        private void OnListenerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.ListenerID = comboBoxListener.SelectedValue as string;

            RefreshPortraits();
            RefreshAnimSetList(comboBoxAnimsetListener, dialogueNode.ListenerID);

            document.RefreshTreeNode(treeNode);
            document.SetDirty();
        }

        private void OnHideSubtitleChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.HideSubtitle = checkBoxHideSubtitle.Checked;

            document.SetDirty();
        }

        private void OnIntensityChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.VoiceIntensity = comboBoxIntensity.SelectedValue as string;

            document.SetDirty();
        }

        private void OnCommentChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.Comment = textBoxComment.Text;

            //document.SetDirty();
            document.SetPendingDirty();
        }

        private void OnCommentValidated(object sender, EventArgs e)
        {
            document.ResolvePendingDirty();
        }

        private void OnContextChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.Context = textBoxContext.Text;

            //document.SetDirty();
            document.SetPendingDirty();
        }

        private void OnContextValidated(object sender, EventArgs e)
        {
            document.ResolvePendingDirty();
        }

        private void OnMoodSpeakerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            RefreshAnimList(comboBoxAnimsetSpeaker, comboBoxAnimSpeaker);
            dialogueNode.SpeakerAnimset = (sender as ComboBox).SelectedValue as string;

            document.SetDirty();
        }

        private void OnMoodListenerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            RefreshAnimList(comboBoxAnimsetListener, comboBoxAnimListener);
            dialogueNode.ListenerAnimset = (sender as ComboBox).SelectedValue as string;

            document.SetDirty();
        }

        private void OnAnimSpeakerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            string value = (sender as ComboBox).SelectedValue as string;
            if (value != null && value != dialogueNode.SpeakerAnim)
            {
                dialogueNode.SpeakerAnim = value;
                document.SetDirty();
            }
        }

        private void OnAnimListenerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            string value = (sender as ComboBox).SelectedValue as string;
            if (value != null && value != dialogueNode.ListenerAnim)
            {
                dialogueNode.ListenerAnim = value;
                document.SetDirty();
            }
        }

        private void OnSwapActors(object sender, EventArgs e)
        {
            if (!ready)
                return;

            ready = false;

            //Data
            string speakerID = dialogueNode.SpeakerID;
            dialogueNode.SpeakerID = dialogueNode.ListenerID;
            dialogueNode.ListenerID = speakerID;

            string speakerAnimSet = dialogueNode.SpeakerAnimset;
            dialogueNode.SpeakerAnimset = dialogueNode.ListenerAnimset;
            dialogueNode.ListenerAnimset = speakerAnimSet;

            string speakerAnim = dialogueNode.SpeakerAnim;
            dialogueNode.SpeakerAnim = dialogueNode.ListenerAnim;
            dialogueNode.ListenerAnim = speakerAnim;

            //UI
            comboBoxSpeaker.SelectedValue = dialogueNode.SpeakerID;
            comboBoxListener.SelectedValue = dialogueNode.ListenerID;

            comboBoxAnimsetSpeaker.SelectedItem = dialogueNode.SpeakerAnimset;
            comboBoxAnimsetListener.SelectedItem = dialogueNode.ListenerAnimset;

            RefreshAnimList(comboBoxAnimsetSpeaker, comboBoxAnimSpeaker);
            RefreshAnimList(comboBoxAnimsetListener, comboBoxAnimListener);

            comboBoxAnimSpeaker.SelectedValue = dialogueNode.SpeakerAnim;
            comboBoxAnimListener.SelectedValue = dialogueNode.ListenerAnim;

            //Finalize
            ready = true;

            RefreshPortraits();

            document.RefreshTreeNode(treeNode);
            document.SetDirty();
        }
    }
}
