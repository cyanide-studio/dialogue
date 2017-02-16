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
        public float Timer { get; set; }
        public bool HideTimer { get; set; }
        public string Blueprint { get; set; }

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
            Timer = 0.0f;
            HideTimer = false;
            Blueprint = "";

            Replies = new List<DialogueNodeReply>();
            RepliesIDs = new List<int>();
        }

        public DialogueNodeChoice(DialogueNodeChoice other)
            : base(other)
        {
            Choice = other.Choice;
            Timer = other.Timer;
            HideTimer = other.HideTimer;
            Blueprint = other.Blueprint;
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
