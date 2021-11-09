using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
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
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");

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

            // Cameras
            var cameras = new Dictionary<string, string>();
            cameras.Add("", "");
            cameras.Add("CloseUp", "Close Up");
            cameras.Add("MediumShot", "Medium Shot");
            cameras.Add("FullShot", "Full Shot");
            EditorCore.CustomLists["Cameras"] = cameras;

            // Bind Nodes Attributes
            EditorCore.BindAttribute(typeof(NodeConditionHasHonor), "ConditionHasHonor", "Has Honor");
            EditorCore.BindAttribute(typeof(NodeActionAddHonor), "ActionAddHonor", "Add Honor");

            // Bind Nodes Properties Panels
            EditorCore.BindCustomProperties(typeof(DialogueNodeSentence), "DirectingProperties", typeof(DirectingProperties), typeof(FormPropertiesDirecting));

            // Delegate post-load project
            EditorCore.OnProjectLoad = delegate
            {
                //...
                // Here you can start additional processes like filling the loaded project/dialogues with some imported data
                //...
            };

            // Delegate post-load dialogue, should be used when data needs to migrate between versions
            EditorCore.OnDialoguePostLoad = delegate (Dialogue dialogue)
            {
                //...
                // Here you can handle data migration and properties update, using serialized data from ExtensionData dictionnaries
                //...

                //...
                // Here you can flush all ExtensionData to remove them from serialization once migration is complete
                //foreach (DialogueNode node in dialogue.ListNodes)
                //{
                //    node.ExtensionData?.Clear();
                //}

                //dialogue.ExtensionData?.Clear();
                //...
            };

            // Delegate to check custom errors
            EditorCore.OnCheckDialogueErrors = delegate (Dialogue dialogue)
            {
                //...
                // Here you can plug custom checks, using this kind of messages :
                //EditorCore.LogError(String.Format("{0} {1} - Sentence has no Speaker", dialogue.GetName(), node.ID), dialogue, node);
                //...
            };

            //..............................................................

            // MainWindow
            EditorCore.MainWindow.Init();

            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(WIN32.ShowCrashMessage);

            // Run
            Application.Run(EditorCore.MainWindow);
        }
    }
}
