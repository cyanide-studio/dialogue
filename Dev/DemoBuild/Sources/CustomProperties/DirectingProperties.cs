using DialogueEditor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBuild
{
    public class DirectingProperties : NodeCustomProperties
    {
        public string Camera { get; set; } = "";
        public float CameraBlendTime { get; set; } = -1.0f;
        public float CameraDelay { get; set; } = 0.0f;

        [JsonConverter(typeof(ExporterJson.ConverterTriBool))]
        public Utility.ETriBool ApplyZoomIn { get; set; } = Utility.ETriBool.TB_undefined;

        [JsonConverter(typeof(ExporterJson.ConverterTriBool))]
        public Utility.ETriBool ApplyOrbitalMove { get; set; } = Utility.ETriBool.TB_undefined;
    }
}
