using System.Collections.Generic;
using DialogueEditor;

namespace DemoProjectPlugin
{
    public class Plugin : IModule
    {
        public string Name => "Demo Project Plugin";
        public string Description => "Add extra attributes for the demo project";

        public void Initialize(WindowMain windowMain)
        {
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
        }

        public void Shutdown()
        {
        }

        public IDictionary<string, IModuleAddIn> AddIns => new Dictionary<string, IModuleAddIn>();
    }
}
