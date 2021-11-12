using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace DialogueEditor
{
    public class DialogueNodeChoice : DialogueNode
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public string Choice { get; set; }

        /// <summary>
        /// List of available replies
        /// </summary>
        [JsonIgnore]
        public List<DialogueNodeReply> Replies { get; set; }
        public List<int> RepliesIDs { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogueNodeChoice()
        {
            Choice = "";

            Replies = new List<DialogueNodeReply>();
            RepliesIDs = new List<int>();
        }

        public DialogueNodeChoice(DialogueNodeChoice other)
            : base(other)
        {
            Choice = other.Choice;
            RepliesIDs = new List<int>();

            //Clone all child nodes
            Replies = other.Replies.Clone() as List<DialogueNodeReply>;
        }

        public override object Clone()
        {
            return new DialogueNodeChoice(this);
        }
    }
}
