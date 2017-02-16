using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DialogueEditor
{
    public class NodeFlag : ICloneable
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public NodeFlag()
        {
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        public virtual string GetTreeText()
        {
            return GetDisplayText();
        }

        public virtual string GetDisplayText()
        {
            return "[NodeFlag]";
        }
    }
}
