using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class FormPropertiesChoice : UserControl, IFormProperties
    {
        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        protected DocumentDialogue document;
        protected TreeNode treeNode;
        protected DialogueNodeChoice dialogueNode;

        protected bool ready = false;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public FormPropertiesChoice()
        {
            InitializeComponent();

            Dock = System.Windows.Forms.DockStyle.Fill;
        }

        public void Clear()
        {
            ready = false;
            
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
        }

        public void Init(DocumentDialogue inDocument, TreeNode inTreeNode, DialogueNode inDialogueNode)
        {
            document = inDocument;
            treeNode = inTreeNode;
            dialogueNode = inDialogueNode as DialogueNodeChoice;

            textBoxWorkstring.Text = dialogueNode.Choice;

            ready = true;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events

        private void OnWorkstringChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            RichTextBox textBox = sender as RichTextBox;
            dialogueNode.Choice = EditorHelper.SanitizeText(textBox.Text);

            document.RefreshTreeNodeForWorkstringEdit(treeNode);
            //document.SetDirty();
            document.SetPendingDirty();
        }

        private void OnWorkstringValidated(object sender, EventArgs e)
        {
            ValidateEditedWorkstring();
        }

        public void ValidateEditedWorkstring()
        {
            document.RefreshTreeNodeForWorkstringValidation(treeNode);
            document.ResolvePendingDirty();
        }
    }
}
