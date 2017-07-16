using System;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class DialogAbout : Form
    {
        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogAbout()
        {
            InitializeComponent();

            labelVersion.Text += EditorVersion.GetVersion();
            labelVersionProject.Text += EditorCore.VersionProject;

            labelVersionNet.Text += Environment.Version.ToString();
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
