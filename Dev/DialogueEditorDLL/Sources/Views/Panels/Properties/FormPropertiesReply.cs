using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class FormPropertiesReply : FormProperties
    {
        #region Internal vars
        protected DocumentDialogueTreeView document;
        protected TreeNode treeNode;
        protected DialogueNodeReply dialogueNode;
        protected bool isEditingWorkstring = false;
        protected string originalWorkstring = "";
        protected AutoComplete autoComplete;
        #endregion

        #region Constructor
        public FormPropertiesReply()
        {
            InitializeComponent();

            Dock = DockStyle.Fill;
        }
        #endregion

        #region Init
        public void Init(DialogueController inDialogueController, DialogueNodeReply inDialogueNode)
        {
            dialogueController = inDialogueController;
            dialogueNode = inDialogueNode;

            Project project = ProjectController.Project;

            textBoxWorkstring.Text = dialogueNode.Reply;
            checkBoxRepeat.Checked = dialogueNode.Repeat;
            checkBoxDeduction.Checked = dialogueNode.Deduction;
            checkBoxAutoSelect.Checked = dialogueNode.AutoSelect;
            RefreshWordCount();

            //AutoComplete
            autoComplete = new AutoComplete(this, new BindingSource(project.ListConstants, null));
            autoComplete.OnValidate = ValidateAutoComplete;
            autoComplete.OnClose = CloseAutoComplete;
            autoComplete.OnDrawItem = DrawItemAutoComplete;

            ready = true;
        }
        #endregion

        #region Events
        public override void Clear()
        {
            ready = false;

            autoComplete.Dispose();

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
                if (originalWorkstring != dialogueNode.Reply)
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
            labelWordCount.Text = "(" + dialogueNode.Reply.Length + " chars)";

            Color color = Color.FromArgb(255, 0, 0, 0);
            if (dialogueNode.Reply.Length > ProjectController.Project.MaxLengthReply)
            {
                color = Color.FromArgb(255, 250, 100, 0);
            }

            labelWordCount.ForeColor = color;
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
            return string.Format("{0}  ({1})", constant.ID, constant.Workstring);
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
                originalWorkstring = dialogueNode.Reply;
            }

            dialogueNode.Reply = EditorHelper.SanitizeText(textBox.Text);

            RefreshWordCount();
            dialogueController.NotifyModifiedDialogueNode(dialogueNode, true);
        }

        private void OnWorkstringValidated(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueController.ResolvePendingDirty();
        }
        
        private void OnCheckBoxChanged(object sender, EventArgs e)
        {
            UpdateBools(false);
        }

        private void OnCheckBoxValidated(object sender, EventArgs e)
        {
            UpdateBools(true);
        }

        private void UpdateBools(bool validated)
        {
            if (!ready)
                return;
            dialogueNode.Repeat = checkBoxRepeat.Checked;
            dialogueNode.Deduction = checkBoxDeduction.Checked;
            dialogueNode.AutoSelect = checkBoxAutoSelect.Checked;
            if (validated)
                dialogueController.ResolvePendingDirty();
            else
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
                textBoxWorkstring.Text = dialogueNode.Reply;
                RefreshWordCount();
                preciseUpdatePerformed = true;
            }

            //Finalize
            ready = true;

            return preciseUpdatePerformed;
        }
        #endregion
    }
}