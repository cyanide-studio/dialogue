using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace DialogueEditor
{
    public class DialogueNodeGoto : DialogueNode
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        /// <summary>
        /// Node to jump to instead of Next
        /// </summary>
        [JsonIgnore]
        public DialogueNode Goto { get; set; }
        public int GotoID { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods
        
        public DialogueNodeGoto()
        {
            Goto = null;
            GotoID = ID_NULL;
        }

        public DialogueNodeGoto(DialogueNodeGoto other)
            : base(other)
        {
            //Should be overriden by the dialogue, just use this for resolving goto nodes (Dialogue.AddNode())
            Goto = null;
            GotoID = other.GotoID;         //this will be used when pasting (copy from the clipboard)
            if (other.Goto != null)
                GotoID = other.Goto.ID;     //this will be used when copying (copy to the clipboard)
        }

        public override object Clone()
        {
            return new DialogueNodeGoto(this);
        }
    }
}
