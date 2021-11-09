using DialogueEditor;
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
        public float PreDelay { get; set; } = 0.0f;
        public float PostDelay { get; set; } = 0.0f;
    }
}
