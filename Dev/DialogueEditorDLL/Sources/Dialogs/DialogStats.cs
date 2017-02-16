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
    public partial class DialogStats : Form
    {
        public DialogStats(List<Dialogue> dialogues, Language language = null)
        {
            InitializeComponent();

            bool workstringOnly = true;
            List<Language> languages = null;
            if (language == null || language == EditorCore.LanguageWorkstring)
            {
                workstringOnly = true;
                language = null;
            }
            else
            {
                workstringOnly = false;
                languages = new List<Language>() { language };
            }

            var stats = ExporterStats.GatherStats(dialogues, language, DateTime.MinValue, workstringOnly, false);

            using (System.IO.StringWriter writer = new System.IO.StringWriter())
            {
                ExporterStats.WriteStats(writer, stats, languages, workstringOnly, false);

                textBoxResults.Text = writer.ToString();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape || keyData == Keys.F4)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
