using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueEditor
{
    public class DialogueNodeBranch : DialogueNode
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public string Workstring { get; set; }

        /// <summary>
        /// Node to execute instead of Next
        /// </summary>
        [JsonIgnore]
        public DialogueNode Branch { get; set; }
        public int BranchID { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods
        
        public DialogueNodeBranch()
        {
            Workstring = "";
            Branch = null;
            BranchID = ID_NULL;
        }

        public DialogueNodeBranch(DialogueNodeBranch other)
            : base(other)
        {
            Workstring = other.Workstring;
            Branch = null;
            BranchID = ID_NULL;

            if (other.Branch != null)
            {
                Branch = other.Branch.Clone() as DialogueNode;

                //Clone all child nodes
                DialogueNode currentNode = Branch;
                DialogueNode currentOther = other.Branch;
                while (currentOther != null && currentOther.Next != null)
                {
                    currentNode.Next = currentOther.Next.Clone() as DialogueNode;
                    currentNode = currentNode.Next;
                    currentOther = currentOther.Next;
                }
            }
        }

        public override object Clone()
        {
            return new DialogueNodeBranch(this);
        }
    }
}
