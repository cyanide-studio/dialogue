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
    public partial class FormPropertiesGoto : UserControl, IFormProperties
    {
        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        protected DocumentDialogue document;
        protected TreeNode treeNode;
        protected DialogueNodeGoto dialogueNode;

        protected bool ready = false;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public FormPropertiesGoto()
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
        }

        public bool IsEditingWorkstring()
        {
            return false;
        }

        public void OnResolvePendingDirty()
        {
        }

        public void Init(DocumentDialogue inDocument, TreeNode inTreeNode, DialogueNode inDialogueNode)
        {
            document = inDocument;
            treeNode = inTreeNode;
            dialogueNode = inDialogueNode as DialogueNodeGoto;

            ready = true;
        }
    }
}
