using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class DialogLockedFile : Form
    {
        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogLockedFile(string path)
        {
            InitializeComponent();

            labelInfos.Text = string.Format(labelInfos.Text, path);
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
