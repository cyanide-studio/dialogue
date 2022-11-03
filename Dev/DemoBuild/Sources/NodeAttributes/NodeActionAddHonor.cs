using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return "[Add Honor]" + " [" + Value + "]" + " [" + ResourcesHandler.Project.GetActorName(Character) + "]";
        }

        // Called by the PlayDialogue tool when the node is played.
        // Parameter nodeStart will tell if the action is played at the start or end of the node execution.
        public override void OnPlayNodeAction(bool nodeStart)
        {
            GameData.PlaySession.AddHonor(Character, Value);
        }
    }
}
