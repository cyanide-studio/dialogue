using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueEditor
{
    public interface IFormProperties
    {
        void Clear();
        void ForceFocus();
        bool IsEditingWorkstring();
        void OnResolvePendingDirty();
    }
}
