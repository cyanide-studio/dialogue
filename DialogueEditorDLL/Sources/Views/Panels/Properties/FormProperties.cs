using System.Collections.Generic;
using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class FormProperties : UserControl
    {
        protected DialogueController dialogueController = null;
        protected bool ready = false;

        public FormProperties() { }

        public virtual void Clear()
        {
            ready = false;
            Dispose();
        }

        public virtual void ForceFocus()
        {
        
        }

        public virtual bool IsEditingWorkstring()
        {
            return false;
        }

        public virtual void OnResolvePendingDirty()
        {

        }

        public virtual bool UpdatePreciseElements(DialogueController inDialogueController, DialogueNode inDialogueNode, List<string> preciseElements = null)
        {
            return false;
        }
    }
}
