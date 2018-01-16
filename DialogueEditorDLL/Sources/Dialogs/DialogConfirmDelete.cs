using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class DialogConfirmDelete : Form
    {
        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogConfirmDelete(Dialogue dialogue)
        {
            InitializeComponent();

            labelInfos.Text = string.Format(labelInfos.Text, dialogue.Name);
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
