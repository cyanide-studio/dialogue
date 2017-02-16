using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace DialogueEditor
{
    //--------------------------------------------------------------------------------------------------------------
    // Helper Class

    public class Constant
    {
        public string ID { get; set; }
        public string Workstring { get; set; }
        public DateTime LastEditDate { get; set; }
        public string Comment { get; set; }

        public Constant()
        {
            ID = "";
            Workstring = "";
            LastEditDate = Utility.GetCurrentTime();
            Comment = "";
        }
    }

    public class Project
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public string VersionProject { get; set; }
        public string VersionEditor { get; set; }

        public int MaxLengthSentence { get; set; }
        public int MaxLengthReply { get; set; }

        public DateTime LastVoicingExportDate { get; set; }

        public List<Package> ListPackages { get; set; }
        public List<Language> ListLanguages { get; set; }
        public List<Actor> ListActors { get; set; }

        public List<VoiceKit> ListVoiceKits { get; set; }
        public List<VoiceActor> ListVoiceActors { get; set; }

        public List<Constant> ListConstants { get; set; }
        public TranslationTable Translations { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Public vars

        [JsonIgnore]
        public bool Dirty { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        protected string path { get; set; }
        protected string name { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        static public string GetExtension()
        {
            return ".project";
        }

        public Project()
        {
            VersionProject = "";
            VersionEditor = "";
            MaxLengthSentence = 150;
            MaxLengthReply = 50;
            LastVoicingExportDate = new DateTime(0);

            ListPackages = new List<Package>() { new Package() { Name="Default" } };
            ListLanguages = new List<Language>() { new Language() { Name="English", LocalizationCode="en", VoicingCode="en" } };
            ListActors = new List<Actor>();

            ListVoiceKits = new List<VoiceKit>();
            ListVoiceActors = new List<VoiceActor>();

            ListConstants = new List<Constant>();
            Translations = new TranslationTable();

            Dirty = false;
            path = "";
            name = "";
        }

        public void Init(string inPath, string inName)
        {
            path = inPath;
            name = inName;
        }
        
        public string GetFilePath()
        {
            return path;
        }

        public string GetFilePathName()
        {
            return Path.Combine(path, GetFileName());
        }

        public string GetFileName()
        {
            return name + GetExtension();
        }

        public string GetName()
        {
            return name;
        }

        public void PreSave()
        {
            VersionProject = EditorCore.VersionProject;
            VersionEditor = EditorVersion.GetVersion();
        }

        public Package GetDefaultPackage()
        {
            if (ListPackages.Count > 0)
                return ListPackages[0];

            EditorCore.LogError("No Default package found");
            return null;
        }

        public Package GetPackage(string name)
        {
            return ListPackages.Find(item => item.Name == name);
        }

        public Language GetDefaultLanguage()
        {
            if (ListLanguages.Count > 0)
                return ListLanguages[0];

            EditorCore.LogError("No Default language found");
            return null;
        }

        public Language GetLanguage(string name)
        {
            return ListLanguages.Find(item => item.Name == name);
        }

        public Actor GetActorFromID(string actorID)
        {
            return ListActors.Find(item => item.ID == actorID);
        }

        public string GetActorName(string actorID)
        {
            Actor actor = GetActorFromID(actorID);
            if (actor != null)
                return actor.Name;
            return "";
        }

        public string GetActorID(string actorName)
        {
            Actor actor = ListActors.Find(item => item.Name == actorName);
            if (actor != null)
                return actor.ID;
            return "";
        }

        public List<string> GetAllActorIDs()
        {
            var actorIDs = new List<string>();
            foreach (Actor actor in ListActors)
            {
                actorIDs.Add(actor.ID);
            }
            return actorIDs;
        }

        public List<string> GetAllActorNames()
        {
            var actorNames = new List<string>();
            foreach (Actor actor in ListActors)
            {
                actorNames.Add(actor.Name);
            }
            return actorNames;
        }

        public void AddActor(Actor newActor)
        {
            if (ListActors.Any(item => item.ID == newActor.ID))
                return;

            ListActors.Add(newActor);
            Dirty = true;
        }

        public string GenerateNewActorID()
        {
            string baseID = "ActorID_";
            for (int i = 1; i < Int32.MaxValue; ++i)
            {
                string newID = baseID + i.ToString();
                if (GetActorFromID(newID) == null)
                    return newID;
            }
            return "ActorID_INVALID";
        }

        public bool ChangeActorID(Actor actor, string newID)
        {
            if (newID == String.Empty)   //Forbid empty string
                return false;

            if (ListActors.Any(item => item.ID == newID))   //Forbid two actors with same ID
                return false;

            actor.ID = newID;
            return true;
        }

        public bool ChangeVoiceKitName(VoiceKit kit, string newName)
        {
            if (newName == String.Empty)   //Forbid empty string
                return false;

            if (ListVoiceKits.Any(item => item.Name == newName))   //Forbid two kits with same name
                return false;

            ListActors.FindAll(item => item.VoiceKit == kit.Name).ForEach(item => item.VoiceKit = newName);

            kit.Name = newName;
            return true;
        }

        public bool ChangeVoiceActorName(VoiceActor voiceActor, string newName)
        {
            if (newName == String.Empty)   //Forbid empty string
                return false;

            if (ListVoiceActors.Any(item => item.Name == newName))   //Forbid two voice actors with same name
                return false;

            ListVoiceKits.FindAll(item => item.VoiceActor == voiceActor.Name).ForEach(item => item.VoiceActor = newName);

            voiceActor.Name = newName;
            return true;
        }

        public VoiceKit GetVoiceKit(string name)
        {
            return ListVoiceKits.Find(item => item.Name == name);
        }

        public VoiceActor GetVoiceActor(string name)
        {
            return ListVoiceActors.Find(item => item.Name == name);
        }

        public string GetVoiceActorNameFromKit(string nameKit)
        {
            return GetVoiceActorNameFromKit(GetVoiceKit(nameKit));
        }

        public string GetVoiceActorNameFromKit(VoiceKit kit)
        {
            if (kit != null)
            {
                VoiceActor actor = GetVoiceActor(kit.VoiceActor);
                if (actor != null)
                {
                    return actor.Name;
                }
            }
            return "";
        }

        public string GetLocalizedVoiceActorFromKit(string nameKit, Language language)
        {
            return GetLocalizedVoiceActorFromKit(GetVoiceKit(nameKit), language);
        }

        public string GetLocalizedVoiceActorFromKit(VoiceKit kit, Language language)
        {
            if (kit != null)
            {
                VoiceActor actor = GetVoiceActor(kit.VoiceActor);
                if (actor != null)
                {
                    return actor.GetLocalizedActor(language);
                }
            }
            return "";
        }
    }
}
