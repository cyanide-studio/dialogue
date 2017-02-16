using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class DialogExport : Form
    {
        //--------------------------------------------------------------------------------------------------------------
        // Public vars

        public List<Package> ListPackages = new List<Package>();
        public List<Language> ListLanguages = new List<Language>();

        public bool UseCustomSelection = false;
        public List<Dialogue> SelectedDialogues = new List<Dialogue>();
        public bool Constants = true;

        public bool WorkstringOnly = false;
        public bool WorkstringFallback = true;
        public string ExportPath = "";
        public bool UseDateDirectory = true;
        public bool UsePackagesDirectory = true;

        public bool ExportFromDate = false;
        public DateTime DateFrom = DateTime.MinValue;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogExport(string title, string path, bool defaultDateDirectory, bool defaultPackageDirectory, bool allowConstants, bool allowWorkstringFallback, bool allowDateFrom, DateTime dateFrom)
        {
            InitializeComponent();

            Text = title;

            var project = ResourcesHandler.Project;

            checkedListBoxPackages.DataSource = new BindingSource(project.ListPackages, null);
            checkedListBoxPackages.DisplayMember = "Name";
            for (int i = 0; i < checkedListBoxPackages.Items.Count; ++i)
                checkedListBoxPackages.SetItemChecked(i, project.ListPackages[i].Export);

            checkedListBoxLanguages.DataSource = new BindingSource(project.ListLanguages, null);
            checkedListBoxLanguages.DisplayMember = "Name";
            for (int i = 0; i < checkedListBoxLanguages.Items.Count; ++i)
                checkedListBoxLanguages.SetItemChecked(i, true);

            checkBoxCustomSelection.Checked = UseCustomSelection;
            buttonChooseDialogues.Enabled = UseCustomSelection;

            if (!allowConstants)
                Constants = false;
            checkBoxConstants.Checked = Constants;
            checkBoxConstants.Visible = allowConstants;
            checkBoxConstants.Enabled = allowConstants;

            checkBoxWorkstringOnly.Checked = WorkstringOnly;

            if (!allowWorkstringFallback)
                WorkstringFallback = false;
            checkBoxWorkstringFallback.Checked = WorkstringFallback;
            checkBoxWorkstringFallback.Visible = allowWorkstringFallback;
            checkBoxWorkstringFallback.Enabled = allowWorkstringFallback;

            ExportPath = path;
            textBoxPath.Text = ExportPath;

            UseDateDirectory = defaultDateDirectory;
            checkBoxDateDirectory.Visible = true;
            checkBoxDateDirectory.Enabled = true;
            checkBoxDateDirectory.Checked = UseDateDirectory;
            labelNoDateDirectory.Visible = false;
            labelNoDateDirectory.Location = new Point(checkBoxDateDirectory.Location.X - 2, checkBoxDateDirectory.Location.Y - 2);

            UsePackagesDirectory = defaultPackageDirectory;
            checkBoxPackagesDirectory.Checked = UsePackagesDirectory;

            checkBoxExportFromDate.Visible = allowDateFrom;
            checkBoxExportFromDate.Enabled = allowDateFrom;
            checkBoxExportFromDate.Checked = false;
            dateTimePicker.Visible = allowDateFrom;
            dateTimePicker.Enabled = false;
            dateTimePicker.Value = new DateTime(Math.Max(dateTimePicker.MinDate.Ticks, dateFrom.Ticks));
        }

        private void OnExportClick(object sender, EventArgs e)
        {
            ListPackages = checkedListBoxPackages.CheckedItems.Cast<Package>().ToList();
            ListLanguages = checkedListBoxLanguages.CheckedItems.Cast<Language>().ToList();

            UseCustomSelection = checkBoxCustomSelection.Checked;
            Constants = checkBoxConstants.Checked;

            WorkstringOnly = checkBoxWorkstringOnly.Checked;
            WorkstringFallback = checkBoxWorkstringFallback.Checked;

            if (WorkstringOnly)
                ListLanguages.Clear();

            ExportPath = textBoxPath.Text;

            UseDateDirectory = checkBoxDateDirectory.Checked;
            UsePackagesDirectory = checkBoxPackagesDirectory.Checked;

            ExportFromDate = checkBoxExportFromDate.Checked;
            DateFrom = (!ExportFromDate) ? new DateTime(0) : dateTimePicker.Value;
        }

        private void OnEditPathClick(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select the export directory :";

            dialog.SelectedPath = EditorHelper.GetProjectDirectory();
            if (Directory.Exists(textBoxPath.Text))
                dialog.SelectedPath = textBoxPath.Text;

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxPath.Text = dialog.SelectedPath;
            }
        }

        private void OnExportFromDateChanged(object sender, EventArgs e)
        {
            dateTimePicker.Enabled = checkBoxExportFromDate.Checked;
        }

        private void OnExportWorkstringOnlyChanged(object sender, EventArgs e)
        {
            checkedListBoxLanguages.Enabled = !checkBoxWorkstringOnly.Checked;
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

        private void buttonChooseDialogues_Click(object sender, EventArgs e)
        {
            var project = ResourcesHandler.Project;
            string projectDirectory = EditorHelper.GetProjectDirectory();
            string exportDirectory = Path.Combine(projectDirectory, EditorCore.Settings.DirectoryExportDialogues);

            var dialog = new DialogDocumentSelector("Choose Dialogues..", SelectedDialogues);
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            SelectedDialogues = dialog.checkedDialogues;
        }

        private void checkBoxCustomSelection_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxCustomSelection.Checked)
            {
                //Disable Packages list
                checkedListBoxPackages.SelectionMode = SelectionMode.None;
                checkedListBoxPackages.BackColor = Color.FromKnownColor(KnownColor.Control);

                //Enable Dialogue Selector
                buttonChooseDialogues.Enabled = true;
            }
            else
            {
                //Enable Packages list
                checkedListBoxPackages.SelectionMode = SelectionMode.One;
                checkedListBoxPackages.BackColor = Color.FromKnownColor(KnownColor.Window);

                //Disable Dialogue Selector
                buttonChooseDialogues.Enabled = false;
            }
        }
    }
}
