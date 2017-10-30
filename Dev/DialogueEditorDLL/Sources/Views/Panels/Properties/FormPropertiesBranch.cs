using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class FormPropertiesBranch : FormProperties
    {
        #region Internal vars
        protected DialogueNodeBranch dialogueNode;
        #endregion

        #region Constructor
        public FormPropertiesBranch()
        {
            InitializeComponent();

            Dock = DockStyle.Fill;
        }
        #endregion

        #region Init
        public void Init(DialogueController inDialogueController, DialogueNodeBranch inDialogueNode)
        {
            dialogueController = inDialogueController;
            dialogueNode = inDialogueNode;

            textBoxWorkstring.Text = dialogueNode.Workstring;

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
            dialogueNode.Workstring = EditorHelper.SanitizeText(textBox.Text);
            dialogueController.NotifyModifiedDialogueNode(dialogueNode, true);
        }

        private void OnWorkstringValidated(object sender, EventArgs e)
        {
            dialogueController.ResolvePendingDirty();
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
                textBoxWorkstring.Text = dialogueNode.Workstring;
                preciseUpdatePerformed = true;
            }

            //Finalize
            ready = true;

            return preciseUpdatePerformed;
        }
        #endregion
    }
}
