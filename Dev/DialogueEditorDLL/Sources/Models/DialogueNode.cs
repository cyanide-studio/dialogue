using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DialogueEditor
{
    public abstract class DialogueNode : ICloneable
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        [JsonIgnore]
        public const int ID_NULL = -1;
        public int ID { get; set;}

        /// <summary>
        /// Next node to play in normal flow
        /// </summary>
        [JsonIgnore]
        public DialogueNode Next { get; set; }
        public int NextID { get; set; }

        public List<NodeCondition> Conditions { get; set; }
        public List<NodeAction> Actions { get; set; }
        public List<NodeFlag> Flags { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods
        
        public DialogueNode()
        {
            ID = ID_NULL;
            Next = null;
            NextID = ID_NULL;

            Conditions = new List<NodeCondition>();
            Actions = new List<NodeAction>();
            Flags = new List<NodeFlag>();
        }

        public DialogueNode(DialogueNode other)
        {
            ID = other.ID;  //Should be overriden by the dialogue, just use this for resolving goto nodes (Dialogue.AddNode())
            Next = null;
            NextID = ID_NULL;

            Conditions = other.Conditions.Clone() as List<NodeCondition>;
            Actions = other.Actions.Clone() as List<NodeAction>;
            Flags = other.Flags.Clone() as List<NodeFlag>;
        }

        public abstract object Clone();
    }
}
