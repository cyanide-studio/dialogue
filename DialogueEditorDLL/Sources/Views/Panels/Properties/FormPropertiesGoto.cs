using System.Windows.Forms;

namespace DialogueEditor
{
    public partial class FormPropertiesGoto : FormProperties
    {
        #region Internal vars
        protected DialogueNodeGoto dialogueNode;
        #endregion

        #region Constructor
        public FormPropertiesGoto()
        {
            InitializeComponent();

            Dock = DockStyle.Fill;
        }
        #endregion

        #region Init
        public void Init(DialogueController inDialogueController, DialogueNodeGoto inDialogueNode)
        {
            dialogueController = inDialogueController;
            dialogueNode = inDialogueNode;

            ready = true;
        }
        #endregion
    }
}
