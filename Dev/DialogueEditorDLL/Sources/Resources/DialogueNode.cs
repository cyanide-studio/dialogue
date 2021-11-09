using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DialogueEditor
{
    public abstract class DialogueNode : ICloneable
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        [JsonIgnore]
        public const int ID_NULL = -1;
        public int ID { get; set; }

        /// <summary>
        /// Next node to play in normal flow
        /// </summary>
        [JsonIgnore]
        public DialogueNode Next { get; set; }
        public int NextID { get; set; }

        public List<NodeCondition> Conditions { get; set; }
        public List<NodeAction> Actions { get; set; }
        public List<NodeFlag> Flags { get; set; }

        public List<NodeCustomProperties> CustomProperties { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> ExtensionData { get; set; }

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

            CustomProperties = new List<NodeCustomProperties>();
        }

        public DialogueNode(DialogueNode other)
        {
            ID = other.ID;  //Should be overriden by the dialogue, just use this for resolving goto nodes (Dialogue.AddNode())
            Next = null;
            NextID = ID_NULL;

            Conditions = other.Conditions.Clone() as List<NodeCondition>;
            Actions = other.Actions.Clone() as List<NodeAction>;
            Flags = other.Flags.Clone() as List<NodeFlag>;

            CustomProperties = other.CustomProperties.Clone() as List<NodeCustomProperties>;
        }

        public abstract object Clone();

        public NodeCustomProperties GetCustomProperties(Type customPropertiesType)
        {
            foreach (NodeCustomProperties properties in CustomProperties)
            {
                if (properties.GetType().Equals(customPropertiesType))
                {
                    return properties;
                }
            }

            return null;
        }
    }
}
