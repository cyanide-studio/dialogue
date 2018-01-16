using System;

namespace DialogueEditor
{
    public class NodeAction : ICloneable
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars
        
        public bool OnNodeStart { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public NodeAction()
        {
            OnNodeStart = true;
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
            return "[NodeAction]";
        }
    }
}
