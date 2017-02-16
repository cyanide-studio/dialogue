using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DialogueEditor;


namespace TestSingleWindow
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();

            //Dialogue pDialogue = ResourcesHandler.CreateEmptyDialogueInstance("TestDialogue");    //This creates an empty document (no root !)
            Dialogue dialogue = ResourcesHandler.CreateDialogueInstance("TestDialogue");           //This creates a document with a root, ready to go

            EditorCore.Properties = new PanelProperties();
            EditorCore.Properties.Owner = this;
            EditorCore.Properties.Show();

            DocumentDialogue documentDialogue = new DocumentDialogue(dialogue);
            documentDialogue.Owner = this;
            documentDialogue.Show();
        }
    }
}
