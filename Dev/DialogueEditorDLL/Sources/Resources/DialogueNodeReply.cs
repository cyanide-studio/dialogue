using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueEditor
{
    public class DialogueNodeReply : DialogueNode
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public string Reply { get; set; }
        public DateTime LastEditDate { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogueNodeReply()
        {
            Reply = "";
            LastEditDate = Utility.GetCurrentTime();
        }

        public DialogueNodeReply(DialogueNodeReply other)
            : base(other)
        {
            Reply = other.Reply;

            //Clone all child nodes
            DialogueNode currentNode = this;
            DialogueNode currentOther = other;
            while (currentOther != null && currentOther.Next != null)
            {
                currentNode.Next = currentOther.Next.Clone() as DialogueNode;
                currentNode = currentNode.Next;
                currentOther = currentOther.Next;
            }
        }

        public override object Clone()
        {
            return new DialogueNodeReply(this);
        }
    }
}
