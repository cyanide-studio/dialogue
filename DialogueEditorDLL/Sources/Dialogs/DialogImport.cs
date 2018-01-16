using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class DialogImport : Form
    {
        //--------------------------------------------------------------------------------------------------------------
        // Public vars

        public string ImportPathDialogues = "";
        public string ImportPathConstants = "";
        public List<Package> ListPackages = new List<Package>();
        public List<Language> ListLanguages = new List<Language>();
        public bool ImportLocalization = true;
        public bool ImportWorkstring = false;
        public bool ImportInformation = false;
        public bool ImportConstants = false;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogImport(string path)
        {
            InitializeComponent();

            ImportPathDialogues = path;
            ImportPathConstants = path;

            var project = ProjectController.Project;

            //textBoxPath.Text = ImportPath;

            checkedListBoxPackages.DataSource = new BindingSource(project.ListPackages, null);
            checkedListBoxPackages.DisplayMember = "Name";
            for (int i = 0; i < checkedListBoxPackages.Items.Count; ++i)
                checkedListBoxPackages.SetItemChecked(i, project.ListPackages[i].Export);

            checkedListBoxLanguages.DataSource = new BindingSource(project.ListLanguages, null);
            checkedListBoxLanguages.DisplayMember = "Name";
            for (int i = 0; i < checkedListBoxLanguages.Items.Count; ++i)
                checkedListBoxLanguages.SetItemChecked(i, true);

            checkBoxImportLocalization.Checked = ImportLocalization;
            checkBoxImportWorkstring.Checked = ImportWorkstring;
            checkBoxImportInformation.Checked = ImportInformation;
            checkBoxImportConstants.Checked = ImportConstants;
        }

        private void OnImportClick(object sender, EventArgs e)
        {
            ImportPathDialogues = textBoxPath.Text;
            ImportPathConstants = textBoxPathConstants.Text;

            ListPackages = checkedListBoxPackages.CheckedItems.Cast<Package>().ToList();
            ListLanguages = checkedListBoxLanguages.CheckedItems.Cast<Language>().ToList();

            ImportLocalization = checkBoxImportLocalization.Checked;
            ImportWorkstring = checkBoxImportWorkstring.Checked;
            ImportInformation = checkBoxImportInformation.Checked;
            ImportConstants = checkBoxImportConstants.Checked;
        }

        private void OnEditPathClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select the import file for Dialogues :";
            dialog.Filter = "Csv Files|*.csv";
            dialog.InitialDirectory = EditorHelper.GetProjectDirectory();
            if (Directory.Exists(ImportPathDialogues))
                dialog.InitialDirectory = ImportPathDialogues;

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxPath.Text = dialog.FileName;
            }
        }

        private void OnEditPathConstantsClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select the import file for Constants :";
            dialog.Filter = "Csv Files|*.csv";
            dialog.InitialDirectory = EditorHelper.GetProjectDirectory();
            if (Directory.Exists(ImportPathConstants))
                dialog.InitialDirectory = ImportPathConstants;

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxPathConstants.Text = dialog.FileName;

                ImportConstants = true;
                checkBoxImportConstants.Checked = ImportConstants;
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
