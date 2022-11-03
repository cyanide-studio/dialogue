using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

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

        // Called by the PlayDialogue tool when the node is played.
        // Parameter nodeStart will tell if the action is played at the start or end of the node execution.
        public virtual void OnPlayNode(bool nodeStart)
        {
        }
    }
}
