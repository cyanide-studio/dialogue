using DialogueEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoBuild
{
    public partial class FormPropertiesDirecting : UserControl, IFormProperties
    {
        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        protected DocumentDialogue document;
        protected TreeNode treeNode;
        protected DialogueNode dialogueNode;

        protected DirectingProperties directingProperties;

        protected bool ready = false;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public FormPropertiesDirecting()
        {
            InitializeComponent();

            Dock = System.Windows.Forms.DockStyle.Fill;
        }

        public void Clear()
        {
            ready = false;

            Dispose();
        }

        public void ForceFocus()
        {
        }

        public bool IsEditingWorkstring()
        {
            return false;
        }

        public void OnResolvePendingDirty()
        {
        }

        public void Init(DocumentDialogue inDocument, TreeNode inTreeNode, DialogueNode inDialogueNode)
        {
            document = inDocument;
            treeNode = inTreeNode;
            dialogueNode = inDialogueNode;

            directingProperties = dialogueNode.GetCustomProperties(typeof(DirectingProperties)) as DirectingProperties;

            comboBoxCamera.DataSource = new BindingSource(EditorCore.CustomLists["Cameras"], null);
            comboBoxCamera.ValueMember = "Key";
            comboBoxCamera.DisplayMember = "Value";
            comboBoxCamera.SelectedValue = directingProperties.Camera;

            textBoxCameraBlendTime.Text = directingProperties.CameraBlendTime.ToString();
            textBoxPreDelay.Text = directingProperties.PreDelay.ToString();
            textBoxPostDelay.Text = directingProperties.PostDelay.ToString();

            ready = true;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events

        private void OnCameraChanged(object sender, EventArgs e)
        {

        }

        private void OnBlendTimeValidated(object sender, EventArgs e)
        {

        }

        private void OnPreDelayValidated(object sender, EventArgs e)
        {
            if (!ready)
                return;

            float value = 0.0f;
            float.TryParse(textBoxPreDelay.Text, out value);

            if (directingProperties.PreDelay != value)
            {
                directingProperties.PreDelay = value;
                document.SetDirty();
            }
        }

        private void OnPostDelayValidated(object sender, EventArgs e)
        {
            if (!ready)
                return;

            float value = 0.0f;
            float.TryParse(textBoxPostDelay.Text, out value);

            if (directingProperties.PostDelay != value)
            {
                directingProperties.PostDelay = value;
                document.SetDirty();
            }
        }
    }
}
