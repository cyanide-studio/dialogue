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
    public partial class DialogVoiceActors : Form
    {
        private DocumentProject document;
        private VoiceActor actor;

        private bool ready = false;

        public DialogVoiceActors(DocumentProject inDocument, VoiceActor inActor)
        {
            InitializeComponent();

            document = inDocument;
            actor = inActor;

            listBoxLanguages.DataSource = new BindingSource(ResourcesHandler.Project.ListLanguages, null);
            listBoxLanguages.DisplayMember = "Name";

            ready = true;
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            ready = false;

            textBoxVoiceActor.Text = actor.GetLocalizedActor(listBoxLanguages.SelectedValue as Language);

            ready = true;
        }

        private void OnActorChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            actor.SetLocalizedActor(listBoxLanguages.SelectedValue as Language, textBoxVoiceActor.Text);

            document.SetDirty();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
