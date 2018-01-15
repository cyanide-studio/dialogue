using System.Collections.Generic;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class DialogConfirmReload : Form
    {
        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogConfirmReload(Project dirtyProject, List<Dialogue> dirtyDialogues)
        {
            InitializeComponent();

            var items = new Dictionary<string, string>();

            if (dirtyProject != null)
            {
                items.Add(dirtyProject.Name, "Project File " + dirtyProject.Name);
            }

            foreach (Dialogue dialogue in dirtyDialogues)
            {
                items.Add(dialogue.Name, dialogue.Name);
            }

            if (items.Count > 0)
            {
                listBox.DataSource = new BindingSource(items, null);
                listBox.ValueMember = "Key";
                listBox.DisplayMember = "Value";
            }
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
