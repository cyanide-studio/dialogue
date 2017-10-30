using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class DialogHelpShortcuts : Form
    {
        public DialogHelpShortcuts()
        {
            InitializeComponent();
        }
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape || keyData == Keys.F1)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
