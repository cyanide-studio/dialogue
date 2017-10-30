using System;
using System.Windows.Forms;
using DialogueEditor;

namespace TestSingleWindow
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            EditorCore.FillDefaultImageList();
            EditorCore.InitDefaultLists();

            ProjectController.CreateProjectInstance("TestProject");

            //Declare some actors
            Actor actorA = new Actor();
            actorA.ID = "Actor01";
            actorA.Name = "Jim";
            ProjectController.AddActor(actorA);

            Actor actorB = new Actor();
            actorB.ID = "Actor02";
            actorB.Name = "Bob";
            ProjectController.AddActor(actorB);

            //Dialogue is created in Window constructor
            Application.Run(new Window());
        }
    }
}
