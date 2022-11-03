using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using DialogueEditor;

namespace DemoBuild
{
    class NodeConditionHasHonor : NodeCondition
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public int Min { get; set; }
        public int Max { get; set; }

        [PropertySearchable]
        [PropertyCharacterName]
        [TypeConverter(typeof(PropertyCharacterNameConverter))]
        public string Character { get; set; }
        
        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public NodeConditionHasHonor()
        {
            Min = 0;
            Max = 100;
            Character = "";
        }

        protected override string GetDisplayText_Impl()
        {
            return "[Has Honor]" + " [" + Min + "]" + " [" + Max + "]" + " [" + ResourcesHandler.Project.GetActorName(Character) + "]";
        }

        // Called by the PlayDialogue tool when the node is about to be played.
        // Can be overriden to simulate game flow through custom code.
        public override bool IsPlayConditionValid()
        {
            return true;
        }
    }
}
