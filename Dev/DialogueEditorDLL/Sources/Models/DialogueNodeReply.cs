using System;

namespace DialogueEditor
{
    public class DialogueNodeReply : DialogueNode
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public string Reply { get; set; }
        public bool Repeat { get; set; }
        public bool Deduction { get; set; }
        public bool AutoSelect { get; set; }
        public DateTime LastEditDate { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogueNodeReply()
        {
            Reply = "";
            Repeat = false;
            Deduction = false;
            AutoSelect = false;
            LastEditDate = Utility.GetCurrentTime();
        }

        public DialogueNodeReply(DialogueNodeReply other)
            : base(other)
        {
            Reply = other.Reply;
            Repeat = other.Repeat;
            Deduction = other.Deduction;
            AutoSelect = other.AutoSelect;

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
