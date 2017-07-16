using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DialogueEditor;

namespace DemoBuild
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

            //..............................................................
            // Here you can declare custom types, bindings and menus

            // Animations
            EditorCore.Animations.Add("Common", new List<string>() { "WaveHands", "CrossArms" });
            EditorCore.Animations.Add("Angry", new List<string>() { "Speak_01", "Speak_02" });

            // Additional lists items
            EditorCore.CustomLists["Builds"].Add("Dwarf", "Dwarf");

            // Bind Nodes Attributes
            EditorCore.BindAttribute(typeof(NodeConditionHasHonor), "ConditionHasHonor", "Has Honor");
            EditorCore.BindAttribute(typeof(NodeActionAddHonor), "ActionAddHonor", "Add Honor");

            // Delegate post-load project
            EditorCore.OnProjectLoad = delegate
            {
                //...
                // Here you can start additional processes like filling the loaded project/dialogues with some imported data
                //...
            };

            // Delegate to check custom errors
            EditorCore.OnCheckDialogueErrors = delegate (Dialogue dialogue)
            {
                //...
                // Here you can plug custom checks, using this kind of messages :
                //ProjectController.LogError(String.Format("{0} {1} - Sentence has no Speaker", dialogue.GetName(), node.ID), dialogue, node);
                //...
            };

            //..............................................................

            // MainWindow
            ProjectController.MainWindow.Init();

            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(WIN32.ShowCrashMessage);

            // Run
            Application.Run(ProjectController.MainWindow);
        }
    }
}
