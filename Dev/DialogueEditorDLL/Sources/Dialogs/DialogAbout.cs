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
    public partial class DialogAbout : Form
    {
        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogAbout()
        {
            InitializeComponent();

            labelVersion.Text += EditorVersion.GetVersion();
            labelVersionProject.Text += EditorCore.VersionProject;

            labelVersionNet.Text += System.Environment.Version.ToString();
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
