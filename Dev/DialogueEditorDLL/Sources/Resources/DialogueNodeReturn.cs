namespace DialogueEditor
{
    public class DialogueNodeReturn : DialogueNode
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods
        
        public DialogueNodeReturn()
        {
        }

        public DialogueNodeReturn(DialogueNodeReturn other)
            : base(other)
        {
        }

        public override object Clone()
        {
            return new DialogueNodeReturn(this);
        }
    }
}
