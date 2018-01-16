using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class FormPropertiesChoice : FormProperties
    {
        #region Internal vars
        protected DialogueNodeChoice dialogueNode;
        #endregion

        #region Constructor
        public FormPropertiesChoice()
        {
            InitializeComponent();

            Dock = DockStyle.Fill;
        }
        #endregion

        #region Init
        public void Init(DialogueController inDialogueController, DialogueNodeChoice inDialogueNode)
        {
            dialogueController = inDialogueController;
            dialogueNode = inDialogueNode;

            textBoxWorkstring.Text = dialogueNode.Choice;
            textBoxTimer.Text = dialogueNode.Timer.ToString();

            //TODO: Unreal Specific
            Dictionary<string, string> list;
            if (EditorCore.CustomLists.TryGetValue("DialogueChoices", out list))
            {
                comboBoxBlueprint.DataSource = new BindingSource(list, null);
                comboBoxBlueprint.ValueMember = "Key";
                comboBoxBlueprint.DisplayMember = "Value";
                comboBoxBlueprint.SelectedValue = dialogueNode.Blueprint;
            }

            checkBoxTimer.Checked = dialogueNode.HideTimer;

            ready = true;
        }
        #endregion

        #region Events
        public override void ForceFocus()
        {
            textBoxWorkstring.Focus();
            textBoxWorkstring.Select(textBoxWorkstring.TextLength, 0);  //jump to text end
        }

        public override bool IsEditingWorkstring()
        {
            return textBoxWorkstring.Focused;
        }

        private void OnWorkstringChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            RichTextBox textBox = sender as RichTextBox;
            dialogueNode.Choice = EditorHelper.SanitizeText(textBox.Text);
            dialogueController.NotifyModifiedDialogueNode(dialogueNode, true);
        }

        private void OnWorkstringValidated(object sender, EventArgs e)
        {
            dialogueController.ResolvePendingDirty();
        }

        private void OnTimerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;
            UpdateTimer(false);
            dialogueController.NotifyModifiedDialogueNode(dialogueNode, true);
        }

        private void OnTimerValidated(object sender, EventArgs e)
        {
            if (!ready)
                return;
            UpdateTimer(true);
            dialogueController.ResolvePendingDirty();
        }

        private void OnBlueprintChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.Blueprint = (sender as ComboBox).SelectedValue as string;
            dialogueController.NotifyModifiedDialogueNode(dialogueNode, true);
        }

        private void OnHideTimerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.HideTimer = checkBoxTimer.Checked;
            dialogueController.NotifyModifiedDialogueNode(dialogueNode);
        }

        private void UpdateTimer(bool updateTextBox)
        {
            float value = 0.0f;
            if (float.TryParse(textBoxTimer.Text, out value))
                dialogueNode.Timer = Math.Max(0.0f, value);
            if (updateTextBox)
                textBoxTimer.Text = dialogueNode.Timer.ToString();
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
                textBoxWorkstring.Text = dialogueNode.Choice;
                preciseUpdatePerformed = true;
            }

            //Finalize
            ready = true;

            return preciseUpdatePerformed;
        }
        #endregion
    }
}
