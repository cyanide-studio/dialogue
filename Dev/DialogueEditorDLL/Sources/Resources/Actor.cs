using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueEditor
{
    public class Actor
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public string ID { get; set; }

        public string Name { get; set; }
        public string Species { get; set; }
        public string Gender { get; set; }
        public string Build { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public string Personality { get; set; }
        public string Description { get; set; }

        public string VoiceKit { get; set; }

        public string Portrait { get; set; }
        public int Color { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> ExtensionData { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public Actor()
        {
            ID = "";

            Name = "";
            Species = "";
            Gender = "";
            Build = "";
            Age = 30;
            Height = 170;
            Personality = "";
            Description = "";

            VoiceKit = "";

            Portrait = "";
            Color = System.Drawing.Color.DarkSlateGray.ToArgb();

            if (EditorCore.CustomLists["Species"].Count > 0)
            {
                Species = EditorCore.CustomLists["Species"].First().Key;
            }
            if (EditorCore.CustomLists["Genders"].Count > 0)
            {
                Gender = EditorCore.CustomLists["Genders"].First().Key;
            }
            if (EditorCore.CustomLists["Builds"].Count > 0)
            {
                Build = EditorCore.CustomLists["Builds"].First().Key;
            }
        }
    }
}
