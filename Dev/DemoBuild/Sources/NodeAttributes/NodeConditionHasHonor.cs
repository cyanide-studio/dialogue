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
            return "[Has Honor]" + " [" + Min + "]" + " [" + Max + "]" + " [" + ProjectController.Project.GetActorName(Character) + "]";
        }
    }
}
