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

            return ExporterJson.SerializeToFile(exportDirectory, constants);
        }
    }
}
