using System.ComponentModel;
using DialogueEditor;

namespace DemoBuild
{
    class NodeActionAddHonor : NodeAction
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public int Value { get; set; }

        [PropertySearchable]
        [PropertyCharacterName]
        [TypeConverter(typeof(PropertyCharacterNameConverter))]
        public string Character { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public NodeActionAddHonor()
        {
            Value = 0;
            Character = "";
        }

        public override string GetDisplayText()
        {
            return "[Add Honor]" + " [" + Value + "]" + " [" + ProjectController.Project.GetActorName(Character) + "]";
        }
    }
}
