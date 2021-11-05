using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DialogueEditor
{
    public static class ExporterConstants
    {
        private class Constants
        {
            public List<Constant> constants { get; set; }
        }
        //--------------------------------------------------------------------------------------------------------------
        // Exports

        public static bool ExportToUnreal4()
        {
            var project = ProjectController.Project;

            string projectDirectory = EditorHelper.GetProjectDirectory();
            string exportDirectory = Path.Combine(projectDirectory, "Constants.dlc");
            
            var constants = new Constants();
            constants.constants = project.ListConstants;

            if (!Directory.Exists(Path.GetDirectoryName(exportDirectory)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(exportDirectory));
            }

            using (StreamWriter file = File.CreateText(exportDirectory))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                serializer.Binder = EditorCore.SerializationBinder;
                serializer.Serialize(file, constants);
            }

            return true;
        }
    }
}
