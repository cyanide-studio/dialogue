using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace DialogueEditor
{
    public class EditorSettings
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public string LastProject { get; set; }

        public string DirectoryExportDialogues { get; set; }
        public string DirectoryExportLocalization { get; set; }
        public string DirectoryExportVoicing { get; set; }
        public string DirectoryExportLipsync { get; set; }
        public string DirectoryExportStats { get; set; }
        public string DirectoryPortraits { get; set; }

        public bool DisplaySpeaker { get; set; }
        public bool DisplayListener { get; set; }
        public bool DisplayConditions { get; set; }
        public bool DisplayActions { get; set; }
        public bool DisplayFlags { get; set; }
        public bool DisplayID { get; set; }
        public bool DisplayText { get; set; }

        public bool UseActorColors { get; set; }
        public bool UseConstants { get; set; }
        public bool RefreshTreeViewOnEdit { get; set; }

        public int MaxUndoLevels { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public EditorSettings()
        {
            LastProject = "";

            DirectoryExportDialogues = "Exports\\Dialogues";
            DirectoryExportLocalization = "Exports\\Localization";
            DirectoryExportVoicing = "Exports\\Voicing";
            DirectoryExportLipsync = "Exports\\Lipsync";
            DirectoryExportStats = "Exports\\Stats";
            DirectoryPortraits = "";

            DisplaySpeaker = true;
            DisplayListener = false;
            DisplayConditions = false;
            DisplayActions = false;
            DisplayFlags = false;
            DisplayID = false;
            DisplayText = true;

            UseActorColors = false;
            UseConstants = false;
            RefreshTreeViewOnEdit = false;

            MaxUndoLevels = 50;
        }

        public void Load()
        {
            string path = EditorCore.PathUserConfig;
            if (File.Exists(path))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.Auto;
                JsonConvert.PopulateObject(File.ReadAllText(path), this, settings);
            }
        }

        public void Save()
        {
            string path = EditorCore.PathUserConfig;
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                serializer.Serialize(file, this);
            }
        }
    }
}
