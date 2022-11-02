using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DialogueEditor
{
    public partial class FormPropertiesReply : UserControl, IFormProperties
    {
        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        protected DocumentDialogue document;
        protected TreeNode treeNode;
        protected DialogueNodeReply dialogueNode;

        protected bool isEditingWorkstring = false;
        protected string originalWorkstring = "";

        protected AutoComplete autoComplete;

        protected bool ready = false;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public FormPropertiesReply()
        {
            InitializeComponent();

            Dock = System.Windows.Forms.DockStyle.Fill;
        }

        public void Clear()
        {
            ready = false;

            autoComplete.Dispose();

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
                if (originalWorkstring != dialogueNode.Reply)
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
            dialogueNode = inDialogueNode as DialogueNodeReply;

            Project project = ResourcesHandler.Project;

            textBoxWorkstring.Text = dialogueNode.Reply;
            RefreshWordCount();

            //AutoComplete
            autoComplete = new AutoComplete(this, new BindingSource(project.ListConstants, null));
            autoComplete.OnValidate = ValidateAutoComplete;
            autoComplete.OnClose = CloseAutoComplete;
            autoComplete.OnDrawItem = DrawItemAutoComplete;

            ready = true;
        }

        private void RefreshWordCount()
        {
            labelWordCount.Text = "(" + dialogueNode.Reply.Length + " chars)";

            Color color = Color.FromArgb(255, 0, 0, 0);
            if (dialogueNode.Reply.Length > ResourcesHandler.Project.MaxLengthReply)
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
                originalWorkstring = dialogueNode.Reply;
            }

            dialogueNode.Reply = EditorHelper.SanitizeText(textBox.Text);

            RefreshWordCount();

            document.RefreshTreeNodeForWorkstringEdit(treeNode);
            //document.SetDirty();
            document.SetPendingDirty();
        }

        private void OnWorkstringValidated(object sender, EventArgs e)
        {
            if (!ready)
                return;

            ValidateEditedWorkstring();
        }

        public void ValidateEditedWorkstring()
        {
            document.RefreshTreeNodeForWorkstringValidation(treeNode);
            document.ResolvePendingDirty();
        }
    }
}