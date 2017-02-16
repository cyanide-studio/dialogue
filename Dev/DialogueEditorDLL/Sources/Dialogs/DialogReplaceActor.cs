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
    public partial class DialogReplaceActor : Form
    {
        //--------------------------------------------------------------------------------------------------------------
        // Public vars

        public string ActorIDFrom = "";
        public string ActorIDTo = "";

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogReplaceActor()
        {
            InitializeComponent();

            if (ResourcesHandler.Project.ListActors.Count > 0)
            {
                var actors = new Dictionary<string, string>();
                foreach (Actor actor in ResourcesHandler.Project.ListActors)
                {
                    actors.Add(actor.ID, actor.Name);
                }

                comboBoxActorFrom.DataSource = new BindingSource(actors, null);
                comboBoxActorFrom.ValueMember = "Key";
                comboBoxActorFrom.DisplayMember = "Value";

                comboBoxActorTo.DataSource = new BindingSource(actors, null);
                comboBoxActorTo.ValueMember = "Key";
                comboBoxActorTo.DisplayMember = "Value";
            }
        }

        private void OnValidateClicked(object sender, EventArgs e)
        {
            ActorIDFrom = comboBoxActorFrom.SelectedValue as string;
            ActorIDTo = comboBoxActorTo.SelectedValue as string;
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
