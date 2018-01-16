using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class FormPropertiesRoot : FormProperties
    {
        #region Internal vars
        protected DialogueNodeRoot dialogueNode;
        protected List<string> additionalActors = new List<string>();
        protected int currentAdditionalActorIndex = -1;
        #endregion

        #region Constructor
        public FormPropertiesRoot()
        {
            InitializeComponent();

            Dock = DockStyle.Fill;
        }
        #endregion

        #region Init
        public void Init(DialogueController inDialogueController, DialogueNodeRoot inDialogueNode)
        {
            dialogueController = inDialogueController;
            dialogueNode = inDialogueNode;

            textBoxVoiceBank.Text = dialogueController.Dialogue.VoiceBank;
            textBoxContext.Text = dialogueController.Dialogue.Context;
            textBoxComment.Text = dialogueController.Dialogue.Comment;

            comboBoxPackage.DataSource = new BindingSource(ProjectController.Project.ListPackages, null);
            comboBoxPackage.DisplayMember = "Name";
            comboBoxPackage.SelectedItem = dialogueController.Dialogue.Package;

            comboBoxSceneType.DataSource = new BindingSource(EditorCore.CustomLists["SceneTypes"], null);
            comboBoxSceneType.ValueMember = "Key";
            comboBoxSceneType.DisplayMember = "Value";
            comboBoxSceneType.SelectedValue = dialogueController.Dialogue.SceneType;

            comboBoxCamera.DataSource = new BindingSource(EditorCore.CustomLists["Cameras"], null);
            comboBoxCamera.ValueMember = "Key";
            comboBoxCamera.DisplayMember = "Value";
            comboBoxCamera.SelectedValue = dialogueController.Dialogue.Camera;
            textBoxCameraBlendTime.Text = dialogueController.Dialogue.CameraBlendTime.ToString();

            foreach (var actorID in dialogueController.Dialogue.ListAdditionalActors)
            {
                Actor actor = ProjectController.Project.GetActorFromID(actorID);
                if (actor != null)
                    additionalActors.Add(actor.Name);
                else
                    additionalActors.Add("<Undefined>");
            }

            listBoxAdditionalActors.DataSource = new BindingSource(additionalActors, null);

            var actors = new Dictionary<string, string>();
            actors.Add("", "");
            foreach (Actor actor in ProjectController.Project.ListActors)
            {
                actors.Add(actor.ID, actor.Name);
            }

            comboBoxAdditionalActors.DataSource = new BindingSource(actors, null);
            comboBoxAdditionalActors.ValueMember = "Key";
            comboBoxAdditionalActors.DisplayMember = "Value";

            /*comboBoxSpatialized.DataSource = new BindingSource(EditorHelper.GetTriBoolDictionary("<Auto>"), null);
            comboBoxSpatialized.ValueMember = "Key";
            comboBoxSpatialized.DisplayMember = "Value";
            comboBoxSpatialized.SelectedValue = dialogue.Spatialized;*/

            RefreshAdditionalActorView();

            ready = true;
        }
        #endregion

        #region Events
        public override void Clear()
        {
            ready = false;

            comboBoxSceneType.DataSource = null;

            Dispose();
        }

        public void RefreshAdditionalActorView()
        {
            bool setReady = ready;  //only set ready to true if it was true when entering here
            ready = false;

            if (currentAdditionalActorIndex > -1)
            {
                comboBoxAdditionalActors.SelectedValue = dialogueController.Dialogue.ListAdditionalActors[currentAdditionalActorIndex];
                comboBoxAdditionalActors.Enabled = true;
            }
            else
            {
                comboBoxAdditionalActors.SelectedIndex = 0;
                comboBoxAdditionalActors.Enabled = false;
            }

            ready = setReady;
        }

        private void OnPackageChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            Package previous = dialogueController.Dialogue.Package;
            dialogueController.Dialogue.Package = (sender as ComboBox).SelectedValue as Package;
            dialogueController.NotifyModifiedDialogueData();
        }

        private void OnSceneTypeChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueController.Dialogue.SceneType = (sender as ComboBox).SelectedValue as string;
            dialogueController.NotifyModifiedDialogueData();
        }

        private void OnCameraChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueController.Dialogue.Camera = (sender as ComboBox).SelectedValue as string;
            dialogueController.NotifyModifiedDialogueData();
        }

        private void OnCameraBlendTimeChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            float Value = 0.0f;
            if (float.TryParse(textBoxCameraBlendTime.Text, out Value))
            {
                dialogueController.Dialogue.CameraBlendTime = Value;
                dialogueController.NotifyModifiedDialogueData(true);
            }
            else if (textBoxCameraBlendTime.Text == "")
            {
                dialogueController.Dialogue.CameraBlendTime = 0.0f;
                dialogueController.NotifyModifiedDialogueData(true);
            }
        }

        private void OnCameraBlendTimeValidated(object sender, EventArgs e)
        {
            textBoxCameraBlendTime.Text = dialogueController.Dialogue.CameraBlendTime.ToString();

            dialogueController.ResolvePendingDirty();
        }

        private void OnVoiceBankChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueController.Dialogue.VoiceBank = textBoxVoiceBank.Text;
            dialogueController.NotifyModifiedDialogueData(true);
        }

        private void OnVoiceBankValidated(object sender, EventArgs e)
        {
            dialogueController.ResolvePendingDirty();
        }

        private void OnCommentChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueController.Dialogue.Comment = textBoxComment.Text;
            dialogueController.NotifyModifiedDialogueData(true);
        }

        private void OnCommentValidated(object sender, EventArgs e)
        {
            dialogueController.ResolvePendingDirty();
        }

        private void OnContextChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueController.Dialogue.Context = textBoxContext.Text;
            dialogueController.NotifyModifiedDialogueData(true);
        }

        private void OnContextValidated(object sender, EventArgs e)
        {
            dialogueController.ResolvePendingDirty();
        }

        private void OnAdditionalActorsIndexChanged(object sender, EventArgs e)
        {
            currentAdditionalActorIndex = listBoxAdditionalActors.SelectedIndex;
            RefreshAdditionalActorView();
        }

        private void OnAddAdditionalActor(object sender, EventArgs e)
        {
            dialogueController.Dialogue.ListAdditionalActors.Add("");
            additionalActors.Add("<Undefined>");

            (listBoxAdditionalActors.DataSource as BindingSource).ResetBindings(false);
            listBoxAdditionalActors.SelectedIndex = listBoxAdditionalActors.Items.Count - 1;

            if (currentAdditionalActorIndex == -1)   //SelectedIndex will be already set on first insertion, this is a fallback for this special case
            {
                currentAdditionalActorIndex = listBoxAdditionalActors.SelectedIndex;
                RefreshAdditionalActorView();
            }
            dialogueController.NotifyModifiedDialogueData();
        }

        private void OnRemoveAdditionalActor(object sender, EventArgs e)
        {
            int index = listBoxAdditionalActors.SelectedIndex;
            if (dialogueController.Dialogue.ListAdditionalActors.Count == 0)
                return;

            currentAdditionalActorIndex = -1;
            dialogueController.Dialogue.ListAdditionalActors.RemoveAt(index);
            additionalActors.RemoveAt(index);
            (listBoxAdditionalActors.DataSource as BindingSource).ResetBindings(false);

            if (dialogueController.Dialogue.ListAdditionalActors.Count > 0)
            {
                listBoxAdditionalActors.SelectedIndex = 0;

                if (currentAdditionalActorIndex == -1)
                {
                    currentAdditionalActorIndex = listBoxAdditionalActors.SelectedIndex;
                    RefreshAdditionalActorView();
                }
            }
            dialogueController.NotifyModifiedDialogueData();
        }

        private void OnMoveAdditionalActorUp(object sender, EventArgs e)
        {
            int index = listBoxAdditionalActors.SelectedIndex;
            if (dialogueController.Dialogue.ListAdditionalActors.Count < 2 || index == 0)
                return;

            dialogueController.Dialogue.ListAdditionalActors.Reverse(index - 1, 2);
            additionalActors.Reverse(index - 1, 2);
            (listBoxAdditionalActors.DataSource as BindingSource).ResetBindings(false);
            --listBoxAdditionalActors.SelectedIndex;
            dialogueController.NotifyModifiedDialogueData();
        }

        private void OnMoveAdditionalActorDown(object sender, EventArgs e)
        {
            int index = listBoxAdditionalActors.SelectedIndex;
            if (dialogueController.Dialogue.ListAdditionalActors.Count < 2 || index == dialogueController.Dialogue.ListAdditionalActors.Count - 1)
                return;

            dialogueController.Dialogue.ListAdditionalActors.Reverse(index, 2);
            additionalActors.Reverse(index, 2);
            (listBoxAdditionalActors.DataSource as BindingSource).ResetBindings(false);
            ++listBoxAdditionalActors.SelectedIndex;
            dialogueController.NotifyModifiedDialogueData();
        }

        private void OnAdditionalActorNameChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            Actor actor = ProjectController.Project.GetActorFromID((sender as ComboBox).SelectedValue as string);
            if (actor != null)
            {
                dialogueController.Dialogue.ListAdditionalActors[currentAdditionalActorIndex] = actor.ID;
                additionalActors[currentAdditionalActorIndex] = actor.Name;
                (listBoxAdditionalActors.DataSource as BindingSource).ResetBindings(false);
                dialogueController.NotifyModifiedDialogueData();
            }
        }

        public override bool UpdatePreciseElements(DialogueController inDialogueController, DialogueNode inDialogueNode, List<string> preciseElements = null)
        {
            if (dialogueController != inDialogueController || inDialogueNode != dialogueNode)
                return false;

            if (!ready)
                return false;

            bool preciseUpdatePerformed = false;

            ready = false;

            if (preciseElements.Contains("comment"))
            {
                textBoxComment.Text = dialogueController.Dialogue.Comment;
                preciseUpdatePerformed = true;
            }
            if (preciseElements.Contains("context"))
            {
                textBoxContext.Text = dialogueController.Dialogue.Context;
                preciseUpdatePerformed = true;
            }

            //Finalize
            ready = true;

            return preciseUpdatePerformed;
        }
        #endregion
    }
}
