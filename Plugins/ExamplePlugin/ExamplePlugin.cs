using System.Collections.Generic;
using DialogueEditor;

namespace ExamplePlugin
{
    public class Plugin : IModule
    {
        public string Name => "Example Plugin";
        public string Description => "An empty plugin";

        public void Initialize(WindowMain windowMain)
        {
            //ProjectController.LogDebug($"Example plugin initialized.");
        }

        public void Shutdown()
        {
            //ProjectController.LogDebug($"Example plugin shut down.");
        }

        public IDictionary<string, IModuleAddIn> AddIns => new Dictionary<string, IModuleAddIn>();
    }
}

