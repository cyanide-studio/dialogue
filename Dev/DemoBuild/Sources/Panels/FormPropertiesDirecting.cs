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

            EditorHelper.AbsorbMouseWheelEvent(comboBoxCamera);
            EditorHelper.AbsorbMouseWheelEvent(comboBoxApplyZoom);
            EditorHelper.AbsorbMouseWheelEvent(comboBoxApplyOrbitalMove);
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
            textBoxCameraDelay.Text = directingProperties.CameraDelay.ToString();

            comboBoxApplyZoom.DataSource = new BindingSource(EditorHelper.GetTriBoolDictionary("<Auto>"), null);
            comboBoxApplyZoom.ValueMember = "Key";
            comboBoxApplyZoom.DisplayMember = "Value";
            comboBoxApplyZoom.SelectedValue = directingProperties.ApplyZoomIn;

            comboBoxApplyOrbitalMove.DataSource = new BindingSource(EditorHelper.GetTriBoolDictionary("<Auto>"), null);
            comboBoxApplyOrbitalMove.ValueMember = "Key";
            comboBoxApplyOrbitalMove.DisplayMember = "Value";
            comboBoxApplyOrbitalMove.SelectedValue = directingProperties.ApplyOrbitalMove;

            ready = true;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events

        private void OnCameraChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            directingProperties.Camera = (sender as ComboBox).SelectedValue as string;
            document.SetDirty();
        }

        private void OnBlendTimeChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            float value = 0.0f;
            float.TryParse(textBoxCameraBlendTime.Text, out value);

            if (directingProperties.CameraBlendTime != value)
            {
                directingProperties.CameraBlendTime = value;
                document.SetPendingDirty();
            }
        }

        private void OnBlendTimeValidated(object sender, EventArgs e)
        {
            textBoxCameraBlendTime.Text = directingProperties.CameraBlendTime.ToString();
            document.ResolvePendingDirty();
        }

        private void OnDelayChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            float value = 0.0f;
            float.TryParse(textBoxCameraDelay.Text, out value);

            if (directingProperties.CameraDelay != value)
            {
                directingProperties.CameraDelay = value;
                document.SetPendingDirty();
            }
        }

        private void OnDelayValidated(object sender, EventArgs e)
        {
            textBoxCameraDelay.Text = directingProperties.CameraDelay.ToString();
            document.ResolvePendingDirty();
        }

        private void OnApplyZoomChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            directingProperties.ApplyZoomIn = (Utility.ETriBool)comboBoxApplyZoom.SelectedValue;
            document.SetDirty();
        }

        private void OnApplyOrbitalMoveChanged(object sender, EventArgs e)
        {
            if (!ready)
                return;

            directingProperties.ApplyOrbitalMove = (Utility.ETriBool)comboBoxApplyOrbitalMove.SelectedValue;
            document.SetDirty();
        }
    }
}
