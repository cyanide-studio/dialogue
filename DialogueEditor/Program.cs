using System;
using System.Windows.Forms;

namespace DialogueEditor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Initialise editor with a main window
            EditorCore.Initialize(true);

            // MainWindow
            ProjectController.MainWindow.Init();

            // Run
            Application.Run(ProjectController.MainWindow);
        }
    }
}
