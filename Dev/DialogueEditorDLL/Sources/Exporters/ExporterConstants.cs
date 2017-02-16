using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogueEditor
{
    static public class ExporterConstants
    {
        private class Constants
        {
            public List<Constant> constants { get; set; }
        }
        //--------------------------------------------------------------------------------------------------------------
        // Exports

        static public bool ExportToUnreal4()
        {
            var project = ResourcesHandler.Project;

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
