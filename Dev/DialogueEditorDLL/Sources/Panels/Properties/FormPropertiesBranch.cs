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
    public partial class FormPropertiesBranch : UserControl, IFormProperties
    {
        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        protected DocumentDialogue document;
        protected TreeNode treeNode;
        protected DialogueNodeBranch dialogueNode;

        protected bool ready = false;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public FormPropertiesBranch()
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

        public void Init(DocumentDialogue inDocument, TreeNode inTreeNode, DialogueNodeBranch inDialogueNode)
        {
            document = inDocument;
            treeNode = inTreeNode;
            dialogueNode = inDialogueNode;

            textBoxWorkstring.Text = dialogueNode.Workstring;

            ready = true;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events

        private void OnWorkstringChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            RichTextBox textBox = sender as RichTextBox;
            dialogueNode.Workstring = EditorHelper.SanitizeText(textBox.Text);

            document.RefreshTreeNode(treeNode);
            //document.SetDirty();
            document.SetPendingDirty();
        }

        private void OnWorkstringValidated(object sender, EventArgs e)
        {
            document.ResolvePendingDirty();
        }
    }
}
