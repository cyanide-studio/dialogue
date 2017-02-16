using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueEditor
{
    public class DialogueNodeRoot : DialogueNode
    {
        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public DialogueNodeRoot()
        {
        }

        public DialogueNodeRoot(DialogueNodeRoot other)
            : base(other)
        {
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
            return new DialogueNodeRoot(this);
        }
    }
}
