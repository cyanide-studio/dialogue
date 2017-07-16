namespace DialogueEditor
{
    public class Package
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public string Name { get; set; }
        public bool Export { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public Package()
        {
            Name = "";
            Export = true;
        }
    }
}
