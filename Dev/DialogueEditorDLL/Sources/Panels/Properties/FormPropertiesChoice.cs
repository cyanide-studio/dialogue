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

        public void Init(DocumentDialogue inDocument, TreeNode inTreeNode, DialogueNodeChoice inDialogueNode)
        {
            document = inDocument;
            treeNode = inTreeNode;
            dialogueNode = inDialogueNode;

            textBoxWorkstring.Text = dialogueNode.Choice;
            textBoxTimer.Text = dialogueNode.Timer.ToString();

            //TODO: Unreal Specific
            if (EditorCore.CustomLists.ContainsKey("DialogueChoices"))
            {
                comboBoxBlueprint.DataSource = new BindingSource(EditorCore.CustomLists["DialogueChoices"], null);
                comboBoxBlueprint.ValueMember = "Key";
                comboBoxBlueprint.DisplayMember = "Value";
                comboBoxBlueprint.SelectedValue = dialogueNode.Blueprint;
            }

            checkBoxTimer.Checked = dialogueNode.HideTimer;

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

            document.RefreshTreeNode(treeNode);
            //document.SetDirty();
            document.SetPendingDirty();
        }

        private void OnWorkstringValidated(object sender, EventArgs e)
        {
            document.ResolvePendingDirty();
        }

        private void OnTimerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;
            UpdateTimer(false);
            document.RefreshTreeNode(treeNode);
            document.SetPendingDirty();
        }

        private void OnTimerValidated(object sender, EventArgs e)
        {
            if (!ready)
                return;
            UpdateTimer(true);
            document.ResolvePendingDirty();
        }

        private void OnBlueprintChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.Blueprint = (sender as ComboBox).SelectedValue as string;

            document.RefreshTreeNode(treeNode);
            document.SetDirty();
        }

        private void OnHideTimerChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            dialogueNode.HideTimer = checkBoxTimer.Checked;

            document.SetDirty();
        }

        private void UpdateTimer(bool updateTextBox)
        {
            float value = 0.0f;
            if (float.TryParse(textBoxTimer.Text, out value))
                dialogueNode.Timer = Math.Max(0.0f, value);
            if (updateTextBox)
                textBoxTimer.Text = dialogueNode.Timer.ToString();
        }
    }
}
