using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class FormPropertiesSentence : FormProperties
    {
        #region Internal vars
        protected DialogueNodeSentence dialogueNode;
        protected bool isEditingWorkstring = false;
        protected string originalWorkstring = "";
        protected AutoComplete autoComplete;
        #endregion

        #region Constructor
        public FormPropertiesSentence()
        {
            InitializeComponent();

            Dock = DockStyle.Fill;
        }
        #endregion

        #region Init
        public void Init(DialogueController inDialogueController, DialogueNodeSentence inDialogueNode)
        {
            dialogueController = inDialogueController;
            dialogueNode = inDialogueNode;

            Project project = ProjectController.Project;

            //Text
            textBoxWorkstring.Text = dialogueNode.Sentence;
            RefreshWordCount();

            //Actors
            if (ProjectController.Project.ListActors.Count > 0)
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

            //Delays
            textBoxPreDelay.Text = dialogueNode.PreDelay.ToString();
            textBoxPostDelay.Text = dialogueNode.PostDelay.ToString();

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

            comboBoxCamera.DataSource = new BindingSource(EditorCore.CustomLists["Cameras"], null);
            comboBoxCamera.ValueMember = "Key";
            comboBoxCamera.DisplayMember = "Value";
            comboBoxCamera.SelectedValue = dialogueNode.Camera;
            textBoxCameraBlendTime.Text = dialogueNode.CameraBlendTime.ToString();

            //Ready !
            ready = true;
        }
        #endregion

        #region Events
        public override void Clear()
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

        public override void ForceFocus()
        {
            textBoxWorkstring.Focus();
            textBoxWorkstring.Select(textBoxWorkstring.TextLength, 0);  //jump to text end
        }

        public override bool IsEditingWorkstring()
        {
            return textBoxWorkstring.Focused;
        }

        public override void OnResolvePendingDirty()
        {
            if (isEditingWorkstring)
            {
                if (originalWorkstring != dialogueNode.Sentence)
                {
                    dialogueNode.LastEditDate = Utility.GetCurrentTime();
                }
                else
                {
                    dialogueController.CancelPendingDirty();
                }

                isEditingWorkstring = false;
                originalWorkstring = "";
            }
        }

        private void RefreshWordCount()
        {
            labelWordCount.Text = "(" + dialogueNode.Sentence.Length + " chars)";

            Color color = Color.FromArgb(255, 0, 0, 0);
            if (dialogueNode.Sentence.Length > ProjectController.Project.MaxLengthSentence)
            {
                color = Color.FromArgb(255, 250, 100, 0);
            }

            labelWordCount.ForeColor = color;
        }

        private void RefreshPortraits()
        {
            Project project = ProjectController.Project;
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
            Project project = ProjectController.Project;
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
            List<string> animationList;
            if (EditorCore.Animations.TryGetValue(animset, out animationList))
            {
                try
                {
                    anims.Add("", "<Auto>");
                    anims = anims.Concat(animationList.ToDictionary(item => item))
                                 .ToDictionary(item => item.Key, item => item.Value);
                }
                catch (ArgumentException)
                {
                    ProjectController.LogError($"Animset \"{animset}\" contains duplicate animation names");
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
                textBoxWorkstring.Text = textBoxWorkstring.Text.Insert(position, $"{{constant.ID}}");
                textBoxWorkstring.SelectionStart = position + constant.ID.Count() + 2;    //size of ID + braces

                CloseAutoComplete();
            }
        }

        private string DrawItemAutoComplete(object item)
        {
            var constant = item as Constant;
            return $"{constant.ID}  ({constant.Workstring})";
        }

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
            dialogueController.NotifyModifiedDialogueNode(dialogueNode, true);
        }

        private void OnWorkstringValidated(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueController.ResolvePendingDirty();
        }

        private void OnSpeakerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.SpeakerID = comboBoxSpeaker.SelectedValue as string;

            RefreshPortraits();
            RefreshAnimSetList(comboBoxAnimsetSpeaker, dialogueNode.SpeakerID);
            dialogueController.NotifyModifiedDialogueNode(dialogueNode);
        }

        private void OnListenerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.ListenerID = comboBoxListener.SelectedValue as string;

            RefreshPortraits();
            RefreshAnimSetList(comboBoxAnimsetListener, dialogueNode.ListenerID);
            dialogueController.NotifyModifiedDialogueNode(dialogueNode);
        }

        private void OnCameraChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.Camera = (sender as ComboBox).SelectedValue as string;
            dialogueController.NotifyModifiedDialogueNode(dialogueNode);
        }

        private void OnHideSubtitleChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.HideSubtitle = checkBoxHideSubtitle.Checked;
            dialogueController.NotifyModifiedDialogueNode(dialogueNode);
        }

        private void OnIntensityChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.VoiceIntensity = comboBoxIntensity.SelectedValue as string;
            dialogueController.NotifyModifiedDialogueNode(dialogueNode);
        }

        private void OnCommentChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.Comment = textBoxComment.Text;
            dialogueController.NotifyModifiedDialogueNode(dialogueNode, true);
        }

        private void OnCommentValidated(object sender, EventArgs e)
        {
            dialogueController.ResolvePendingDirty();
        }

        private void OnCameraBlendTimeChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            float Value = 0.0f;
            if (float.TryParse(textBoxCameraBlendTime.Text, out Value))
            {
                dialogueNode.CameraBlendTime = Value;
            }
            else if (textBoxCameraBlendTime.Text == "")
                dialogueNode.CameraBlendTime = 0.0f;
            dialogueController.NotifyModifiedDialogueNode(dialogueNode, true);
        }
        private void OnCameraBlendTimeValidated(object sender, EventArgs e)
        {
            textBoxCameraBlendTime.Text = dialogueNode.CameraBlendTime.ToString();

            dialogueController.ResolvePendingDirty();
        }

        private void OnPreDelayChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            float Value = 0.0f;
            if (float.TryParse(textBoxPreDelay.Text, out Value))
            {
                dialogueNode.PreDelay = Value;
            }
            else if (textBoxPreDelay.Text == "")
                dialogueNode.PreDelay = 0.0f;

            dialogueController.SetPendingDirty();
        }

        private void OnPostDelayChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            float Value = 0.0f;
            if (float.TryParse(textBoxPostDelay.Text, out Value))
            {
                dialogueNode.PostDelay = Value;
            }
            else if (textBoxPostDelay.Text == "")
                dialogueNode.PostDelay = 0.0f;

            dialogueController.SetPendingDirty();
        }

        private void OnDelayValidated(object sender, EventArgs e)
        {
            textBoxPreDelay.Text = dialogueNode.PreDelay.ToString();
            textBoxPostDelay.Text = dialogueNode.PostDelay.ToString();

            dialogueController.ResolvePendingDirty();
        }

        private void OnContextChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.Context = textBoxContext.Text;
            dialogueController.NotifyModifiedDialogueNode(dialogueNode, true);
        }

        private void OnContextValidated(object sender, EventArgs e)
        {
            dialogueController.ResolvePendingDirty();
        }

        private void OnMoodSpeakerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            RefreshAnimList(comboBoxAnimsetSpeaker, comboBoxAnimSpeaker);
            dialogueNode.SpeakerAnimset = (sender as ComboBox).SelectedValue as string;
            dialogueController.NotifyModifiedDialogueNode(dialogueNode);
        }

        private void OnMoodListenerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            RefreshAnimList(comboBoxAnimsetListener, comboBoxAnimListener);
            dialogueNode.ListenerAnimset = (sender as ComboBox).SelectedValue as string;
            dialogueController.NotifyModifiedDialogueNode(dialogueNode);
        }

        private void OnAnimSpeakerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            string value = (sender as ComboBox).SelectedValue as string;
            if (value != null && value != dialogueNode.SpeakerAnim)
            {
                dialogueNode.SpeakerAnim = value;
                dialogueController.NotifyModifiedDialogueNode(dialogueNode);
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
                dialogueController.NotifyModifiedDialogueNode(dialogueNode);
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
            dialogueController.NotifyModifiedDialogueNode(dialogueNode);
        }

        public override bool UpdatePreciseElements(DialogueController inDialogueController, DialogueNode inDialogueNode, List<string> preciseElements = null)
        {
            if (dialogueController != inDialogueController || inDialogueNode != dialogueNode)
                return false;

            if (!ready)
                return false;

            bool preciseUpdatePerformed = false;

            ready = false;

            if (preciseElements.Contains("workstring"))
            {
                textBoxWorkstring.Text = dialogueNode.Sentence;
                RefreshWordCount();
                preciseUpdatePerformed = true;
            }
            if (preciseElements.Contains("comment"))
            {
                textBoxComment.Text = dialogueNode.Comment;
                preciseUpdatePerformed = true;
            }
            if (preciseElements.Contains("context"))
            {
                textBoxContext.Text = dialogueNode.Context;
                preciseUpdatePerformed = true;
            }

            //Finalize
            ready = true;

            return preciseUpdatePerformed;
        }
        #endregion
    }
}
