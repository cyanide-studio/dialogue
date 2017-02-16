using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class DialogSaveOnClose : Form
    {
        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogSaveOnClose(Project dirtyProject, List<Dialogue> dirtyDialogues)
        {
            InitializeComponent();

            var items = new Dictionary<string, string>();

            if (dirtyProject != null)
            {
                items.Add(dirtyProject.GetName(), "Project File " + dirtyProject.GetName());
            }

            foreach (Dialogue dialogue in dirtyDialogues)
            {
                items.Add(dialogue.GetName(), dialogue.GetName());
            }

            listBox.DataSource = new BindingSource(items, null);
            listBox.ValueMember = "Key";
            listBox.DisplayMember = "Value";
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
