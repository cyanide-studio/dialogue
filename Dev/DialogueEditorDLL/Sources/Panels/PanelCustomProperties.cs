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
    public partial class PanelCustomProperties : DockContent
    {
        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public PanelCustomProperties()
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

            foreach (CustomPropertiesSlot slot in EditorCore.CustomPropertiesSlots)
            {
                if (slot.FormType != null && slot.DialogueNodeType.IsAssignableFrom(dialogueNode.GetType()))
                {
                    IFormProperties form = Activator.CreateInstance(slot.FormType) as IFormProperties;
                    form.Init(document, treeNode, dialogueNode);
                    layoutPanel.Controls.Add(form as UserControl);
                }
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
            // I prefer to avoid a dedicated checkbox for custom panels for now.
            // Instead, the standard Properties checkbox will affect all properties panels.
            //if (EditorCore.MainWindow != null)
            //    EditorCore.MainWindow.SyncMenuItemFromPanel(this);
        }
    }
}
