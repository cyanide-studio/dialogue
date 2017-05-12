using System.Windows.Forms;
using DialogueEditor;

namespace TestSingleWindow
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();

            //Dialogue dialogue = ProjectController.CreateDialogueInstance("TestDialogue", true);   //This creates an empty document (no root !)
            Dialogue dialogue = ProjectController.CreateDialogueInstance("TestDialogue");           //This creates a document with a root, ready to go

            ProjectController.CreateCustomProperties(this);

            DialogueController dialogueController = new DialogueController(dialogue);
            DocumentDialogueTreeView documentDialogue = new DocumentDialogueTreeView(dialogueController);
            dialogueController.AddView(documentDialogue);
            documentDialogue.Owner = this;
            documentDialogue.Show();
        }
    }
}
