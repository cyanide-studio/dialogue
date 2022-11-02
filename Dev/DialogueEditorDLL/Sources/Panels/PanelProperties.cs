using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DialogueEditor
{
    public partial class PanelProperties : DockContent
    {
        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public PanelProperties()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            if (layoutPanel.Controls.Count > 0)
            {
                //Copy the controls into an array, since the Dispose calls will remove them from the source list
                UserControl[] properties = new UserControl[layoutPanel.Controls.Count];
                layoutPanel.Controls.CopyTo(properties, 0);
                foreach (UserControl property in properties)
                {
                    IFormProperties propertyBase = property as IFormProperties;
                    if (propertyBase != null)
                        propertyBase.Clear();
                }

                layoutPanel.Controls.Clear();
            }
        }

        public void ShowDialogueNodeProperties(DocumentDialogue document, TreeNode treeNode, DialogueNode dialogueNode)
        {
            //SetDoubleBuffered(LayoutPanel);

            WIN32.StopRedraw(this);
            //SuspendLayout();

            Clear();
            
            if (dialogueNode is DialogueNodeRoot)
            {
                DialogueNodeRoot castNode = dialogueNode as DialogueNodeRoot;

                FormPropertiesRoot properties = new FormPropertiesRoot();
                properties.Init(document, treeNode, castNode);
                layoutPanel.Controls.Add(properties);

                FormPropertiesCommon propertiesCommon = new FormPropertiesCommon();
                propertiesCommon.Init(document, treeNode, castNode);
                layoutPanel.Controls.Add(propertiesCommon);
            }
            else if (dialogueNode is DialogueNodeSentence)
            {
                DialogueNodeSentence castNode = dialogueNode as DialogueNodeSentence;

                FormPropertiesSentence properties = new FormPropertiesSentence();
                properties.Init(document, treeNode, castNode);
                layoutPanel.Controls.Add(properties);

                FormPropertiesCommon propertiesCommon = new FormPropertiesCommon();
                propertiesCommon.Init(document, treeNode, castNode);
                layoutPanel.Controls.Add(propertiesCommon);
            }
            else if (dialogueNode is DialogueNodeChoice)
            {
                DialogueNodeChoice castNode = dialogueNode as DialogueNodeChoice;

                FormPropertiesChoice properties = new FormPropertiesChoice();
                properties.Init(document, treeNode, castNode);
                layoutPanel.Controls.Add(properties);

                FormPropertiesCommon propertiesCommon = new FormPropertiesCommon();
                propertiesCommon.Init(document, treeNode, castNode);
                layoutPanel.Controls.Add(propertiesCommon);
            }
            else if (dialogueNode is DialogueNodeReply)
            {
                DialogueNodeReply castNode = dialogueNode as DialogueNodeReply;

                FormPropertiesReply properties = new FormPropertiesReply();
                properties.Init(document, treeNode, castNode);
                layoutPanel.Controls.Add(properties);

                FormPropertiesCommon propertiesCommon = new FormPropertiesCommon();
                propertiesCommon.Init(document, treeNode, castNode);
                layoutPanel.Controls.Add(propertiesCommon);
            }
            else if (dialogueNode is DialogueNodeGoto)
            {
                DialogueNodeGoto castNode = dialogueNode as DialogueNodeGoto;

                //FormPropertiesGoto properties = new FormPropertiesGoto();
                //properties.Init(document, treeNode, castNode);
                //layoutPanel.Controls.Add(properties);

                FormPropertiesCommon propertiesCommon = new FormPropertiesCommon();
                propertiesCommon.Init(document, treeNode, castNode);
                layoutPanel.Controls.Add(propertiesCommon);
            }
            else if (dialogueNode is DialogueNodeBranch)
            {
                DialogueNodeBranch castNode = dialogueNode as DialogueNodeBranch;

                FormPropertiesBranch properties = new FormPropertiesBranch();
                properties.Init(document, treeNode, castNode);
                layoutPanel.Controls.Add(properties);

                FormPropertiesCommon propertiesCommon = new FormPropertiesCommon();
                propertiesCommon.Init(document, treeNode, castNode);
                layoutPanel.Controls.Add(propertiesCommon);
            }

            layoutPanel.VerticalScroll.Value = 0;

            //ResumeLayout();

            WIN32.ResumeRedraw(this);
            this.Refresh();
        }

        public void ForceFocus()
        {
            Focus();

            foreach (UserControl property in layoutPanel.Controls)
            {
                IFormProperties propertyBase = property as IFormProperties;
                if (propertyBase != null)
                    propertyBase.ForceFocus();
            }
        }

        public bool IsEditingWorkstring()
        {
            foreach (UserControl property in layoutPanel.Controls)
            {
                IFormProperties propertyBase = property as IFormProperties;
                if (propertyBase != null && propertyBase.IsEditingWorkstring())
                    return true;
            }
            return false;
        }

        public void ValidateEditedWorkstring()
        {
            foreach (UserControl property in layoutPanel.Controls)
            {
                IFormProperties propertyBase = property as IFormProperties;
                if (propertyBase != null && propertyBase.IsEditingWorkstring())
                {
                    propertyBase.ValidateEditedWorkstring();
                    return;
                }
            }
        }

        public void OnResolvePendingDirty()
        {
            foreach (UserControl property in layoutPanel.Controls)
            {
                IFormProperties propertyBase = property as IFormProperties;
                if (propertyBase != null)
                    propertyBase.OnResolvePendingDirty();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events

        private void OnDockStateChanged(object sender, EventArgs e)
        {
            EditorCore.MainWindow?.SyncMenuItemFromPanel(this);
        }
    }
}
