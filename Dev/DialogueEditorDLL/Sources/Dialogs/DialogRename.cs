using System;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class DialogRename : Form
    {
        //--------------------------------------------------------------------------------------------------------------
        // Public vars

        public string NewID = "";

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogRename(string actorID)
        {
            InitializeComponent();

            textBoxCurrent.Text = actorID;
            textBoxNew.Text = actorID;
        }

        private void OnValidateClicked(object sender, EventArgs e)
        {
            NewID = textBoxNew.Text;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
