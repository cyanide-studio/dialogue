using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DialogueEditor
{
    public partial class PanelProperties : DockContent
    {
        #region Constructor
        public PanelProperties()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        public void Clear()
        {
            if (layoutPanel.Controls.Count > 0)
            {
                //Copy the controls into an array, since the Dispose calls will remove them from the source list
                UserControl[] properties = new UserControl[layoutPanel.Controls.Count];
                layoutPanel.Controls.CopyTo(properties, 0);
                foreach (UserControl property in properties)
                {
                    FormProperties propertyBase = property as FormProperties;
                    if (propertyBase != null)
                        propertyBase.Clear();
                }

                layoutPanel.Controls.Clear();
            }
        }

        public void ShowDialogueNodeProperties(DialogueController dialogueController, DialogueNode dialogueNode)
        {
            //SetDoubleBuffered(LayoutPanel);

            WIN32.StopRedraw(this);
            //SuspendLayout();

            Clear();
            
            if (dialogueNode is DialogueNodeRoot)
            {
                DialogueNodeRoot castNode = dialogueNode as DialogueNodeRoot;

                FormPropertiesRoot properties = new FormPropertiesRoot();
                properties.Init(dialogueController, castNode);
                layoutPanel.Controls.Add(properties);

                FormPropertiesCommon propertiesCommon = new FormPropertiesCommon();
                propertiesCommon.Init(dialogueController, castNode);
                layoutPanel.Controls.Add(propertiesCommon);
            }
            else if (dialogueNode is DialogueNodeSentence)
            {
                DialogueNodeSentence castNode = dialogueNode as DialogueNodeSentence;

                FormPropertiesSentence properties = new FormPropertiesSentence();
                properties.Init(dialogueController, castNode);
                layoutPanel.Controls.Add(properties);

                FormPropertiesCommon propertiesCommon = new FormPropertiesCommon();
                propertiesCommon.Init(dialogueController, castNode);
                layoutPanel.Controls.Add(propertiesCommon);
            }
            else if (dialogueNode is DialogueNodeChoice)
            {
                DialogueNodeChoice castNode = dialogueNode as DialogueNodeChoice;

                FormPropertiesChoice properties = new FormPropertiesChoice();
                properties.Init(dialogueController, castNode);
                layoutPanel.Controls.Add(properties);

                FormPropertiesCommon propertiesCommon = new FormPropertiesCommon();
                propertiesCommon.Init(dialogueController, castNode);
                layoutPanel.Controls.Add(propertiesCommon);
            }
            else if (dialogueNode is DialogueNodeReply)
            {
                DialogueNodeReply castNode = dialogueNode as DialogueNodeReply;

                FormPropertiesReply properties = new FormPropertiesReply();
                properties.Init(dialogueController, castNode);
                layoutPanel.Controls.Add(properties);

                FormPropertiesCommon propertiesCommon = new FormPropertiesCommon();
                propertiesCommon.Init(dialogueController, castNode);
                layoutPanel.Controls.Add(propertiesCommon);
            }
            else if (dialogueNode is DialogueNodeGoto)
            {
                DialogueNodeGoto castNode = dialogueNode as DialogueNodeGoto;

                //FormPropertiesGoto properties = new FormPropertiesGoto();
                //properties.Init(dialogue, castNode);
                //layoutPanel.Controls.Add(properties);

                FormPropertiesCommon propertiesCommon = new FormPropertiesCommon();
                propertiesCommon.Init(dialogueController, castNode);
                layoutPanel.Controls.Add(propertiesCommon);
            }
            else if (dialogueNode is DialogueNodeBranch)
            {
                DialogueNodeBranch castNode = dialogueNode as DialogueNodeBranch;

                FormPropertiesBranch properties = new FormPropertiesBranch();
                properties.Init(dialogueController, castNode);
                layoutPanel.Controls.Add(properties);

                FormPropertiesCommon propertiesCommon = new FormPropertiesCommon();
                propertiesCommon.Init(dialogueController, castNode);
                layoutPanel.Controls.Add(propertiesCommon);
            }

            layoutPanel.VerticalScroll.Value = 0;

            //ResumeLayout();

            WIN32.ResumeRedraw(this);
            Refresh();
        }

        public void UpdateDialogueNodeProperties(DialogueController dialogueController, DialogueNode dialogueNode, List<string> preciseElements = null)
        {
            if (preciseElements != null && preciseElements.Any())
            {
                foreach (var formProperties in layoutPanel.Controls)
                {
                    bool preciseUpdatePerformed = (formProperties as FormProperties).UpdatePreciseElements(dialogueController, dialogueNode, preciseElements);
                    if (preciseUpdatePerformed)
                        return;
                }
            }
            ShowDialogueNodeProperties(dialogueController, dialogueNode);
        }

        public void ForceFocus()
        {
            Focus();

            foreach (UserControl property in layoutPanel.Controls)
            {
                FormProperties propertyBase = property as FormProperties;
                if (propertyBase != null)
                    propertyBase.ForceFocus();
            }
        }

        public bool IsEditingWorkstring()
        {
            foreach (UserControl property in layoutPanel.Controls)
            {
                FormProperties propertyBase = property as FormProperties;
                if (propertyBase != null && propertyBase.IsEditingWorkstring())
                    return true;
            }
            return false;
        }

        public void OnResolvePendingDirty()
        {
            foreach (UserControl property in layoutPanel.Controls)
            {
                FormProperties propertyBase = property as FormProperties;
                if (propertyBase != null)
                    propertyBase.OnResolvePendingDirty();
            }
        }

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            ProjectController.SyncMenuItemFromPanel(this);
        }
        #endregion
    }
}
