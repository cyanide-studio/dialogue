using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DialogueEditor;

namespace EditorBuild
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Handle float edition in PropertyGrid
            //http://visualhint.com/blog/70/how-to-format-a-number-with-a-specific-cultureinfonumberformatinfo-in-the-propert
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Core
            EditorCore.FillDefaultImageList();
            EditorCore.InitDefaultWindow();
            EditorCore.InitDefaultLists();

            EditorCore.VersionProject = "1.0.0";

            //......
            // Custom overrides should come here !
            //......

            // MainWindow
            ProjectController.MainWindow.Init();

            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(WIN32.ShowCrashMessage);

            // Run
            Application.Run(ProjectController.MainWindow);
        }
    }
}
