using System;

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
