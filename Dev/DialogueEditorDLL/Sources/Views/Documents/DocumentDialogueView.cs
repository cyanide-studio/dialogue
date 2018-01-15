namespace DialogueEditor
{
    public class DocumentDialogueView : DocumentView
    {
        #region Variables
        public DialogueController DialogueController;   // should be protected, but it's helpful for now...
        #endregion

        #region Constructor
        public DocumentDialogueView(DialogueController inDialogueController)
        {
            DialogueController = inDialogueController;
            Name = DialogueController.Dialogue.Name;
        }

        [System.Obsolete("Designer only", true)]
        private DocumentDialogueView()
        {
        }
        #endregion

        #region Init
        public virtual void InitView()
        {

        }
        #endregion

        #region DocumentView overrides
        public override void RefreshTitle()
        {
            Text = DialogueController.Dialogue.Name;
            if (DialogueController.Dirty)
                Text += "*";
        }
        #endregion

        #region Helpers
        protected string GetNodeKey(int? nodeID)
        {
            if (nodeID == null)
                return string.Empty;
            return "Node_" + nodeID.ToString();
        }
        #endregion

        #region Interface
        // The views should implement this to react to model change at dialogue node level - such a refresh should be pretty light (lighter than the impl. of RefreshDocument)
        public virtual void RefreshDialogueNode(DialogueNode DialogueNode) { }
        // Here the only thing that has changed is the selected dialogue node - implementation should be extra light
        // (sure, we'll probably have to amend for multiple selection, but for now let's stick to single selection)
        public virtual void SelectDialogueNode(DialogueNode DialogueNode) { }
        // Here the only thing that has changed is the "header" dialogue data - no node itself has changed
        public virtual void RefreshDialogueData() { }
        #endregion
    };
}
