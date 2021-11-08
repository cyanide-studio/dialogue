using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogueEditor
{
    public interface IFormProperties
    {
        void Init(DocumentDialogue inDocument, TreeNode inTreeNode, DialogueNode inDialogueNode);
        void Clear();
        void ForceFocus();
        bool IsEditingWorkstring();
        void OnResolvePendingDirty();
    }
}
