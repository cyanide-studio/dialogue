using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueEditor
{
    public abstract class NodeCustomProperties : ICloneable
    {
        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
