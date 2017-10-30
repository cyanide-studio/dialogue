using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DialogueEditor
{
    //--------------------------------------------------------------------------------------------------------------
    // Helper struct

    public struct ConditionSlot
    {
        public string Text;
        public Type ConditionType;
    };

    public struct ActionSlot
    {
        public string Text;
        public Type ActionType;
    };

    public struct FlagSlot
    {
        public string Text;
        public Type FlagType;
    };

    public struct DocumentDialogueViewSlot
    {
        public string Text;
        public Type DocumentDialogueViewType;
        // TODO: it would also be great to register a list of dialogue types that invoke this view by default
    };

    //--------------------------------------------------------------------------------------------------------------
    // Helper delegates

    public delegate void DelegateProjectLoad();
    public delegate List<string> DelegateActorAnimsets(string ActorID);
    public delegate void DelegateCheckDialogueErrors(Dialogue dialogue);
    public delegate string DelegateProjectStats(ExporterStats.ProjectStats Stats);

    public static class EditorCore
    {
        //--------------------------------------------------------------------------------------------------------------
        // Public vars

        ///<summary> Project specific version number, registered in all saved files </summary>
        public static string VersionProject = "1.0.0";

        ///<summary> User file path used to store the EditorSettings </summary>
        public static string PathUserConfig = Path.Combine(Environment.CurrentDirectory, @"User.config");

        ///<summary> User file path used to store the UI configuration </summary>
        public static string PathPanelsConfig = Path.Combine(Environment.CurrentDirectory, @"DockPanel.config");

        ///<summary> User's settings for the Editor </summary>
        public static EditorSettings Settings = new EditorSettings();

        ///<summary> SerializationBinder used for handling nodes and attributes types during serialization </summary>
        public static ExporterJson.TypeNameSerializationBinder SerializationBinder = new ExporterJson.TypeNameSerializationBinder();

        ///<summary> ImageList shared by all containers </summary>
        public static ImageList DefaultImageList = null;

        // Custom Conditions, Actions and Flags associated with Nodes
        public static List<ConditionSlot> ConditionSlots = new List<ConditionSlot>();
        public static List<ActionSlot> ActionSlots = new List<ActionSlot>();
        public static List<FlagSlot> FlagSlots = new List<FlagSlot>();

        // Document Dialogue Views
        // (only the std tree view is registered here by default - the other views must be registered per-build (BindDocumentDialogueView))
        public static List<DocumentDialogueViewSlot> DocumentDialogueViewSlots = new List<DocumentDialogueViewSlot>() { new DocumentDialogueViewSlot() { Text = "Standard (Tree) View", DocumentDialogueViewType = typeof(DocumentDialogueTreeView) } };

        // Project specific stats
        public static DelegateProjectStats ProjectStats = null;

        ///<summary> Animations groups </summary>
        public static Dictionary<string, List<string>> Animations = new Dictionary<string, List<string>>();

        ///<summary> Custom lists (named dictionaries of Asset -> Display Name) </summary>
        public static Dictionary<string, Dictionary<string, string>> CustomLists = new Dictionary<string, Dictionary<string, string>>();

        ///<summary> Default Workstring language definition </summary>
        public static Language LanguageWorkstring = new Language() { Name = "Workstring", LocalizationCode = "wk", VoicingCode = "wk" };

        ///<summary> Custom callback used when the Project is loaded </summary>
        public static DelegateProjectLoad OnProjectLoad = null;

        ///<summary> Custom callback used to retrieve Animsets usable by the specified Actor </summary>
        public static DelegateActorAnimsets GetActorAnimsets = null;

        ///<summary> Custom callback used when checking a dialogue errors </summary>
        public static DelegateCheckDialogueErrors OnCheckDialogueErrors = null;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public static void InitDefaultWindow()
        {
            ProjectController.InitDefaultWindow();
        }

        public static void InitDefaultLists()
        {
            var sceneTypes = new Dictionary<string, string>();
            sceneTypes.Add("Interactive", "Interactive");
            sceneTypes.Add("Cinematic", "Cinematic");
            sceneTypes.Add("Ambient", "Ambient");
            sceneTypes.Add("Monologue", "Monologue");
            CustomLists["SceneTypes"] = sceneTypes;

            var species = new Dictionary<string, string>();
            species.Add("Undefined", "Undefined");
            species.Add("Human", "Human");
            species.Add("Monster", "Monster");
            species.Add("Alien", "Alien");
            species.Add("Elf", "Elf");
            species.Add("Dwarf", "Dwarf");
            species.Add("Orc", "Orc");
            CustomLists["Species"] = species;

            var genders = new Dictionary<string, string>();
            genders.Add("Undefined", "Undefined");
            genders.Add("Male", "Male");
            genders.Add("Female", "Female");
            CustomLists["Genders"] = genders;

            var builds = new Dictionary<string, string>();
            builds.Add("Undefined", "Undefined");
            builds.Add("Normal", "Normal");
            builds.Add("Slender", "Slender");
            builds.Add("Athletic", "Athletic");
            builds.Add("Curvy", "Curvy");
            builds.Add("Big", "Big");
            builds.Add("Obese", "Obese");
            CustomLists["Builds"] = builds;

            var intensities = new Dictionary<string, string>();
            intensities.Add("", "Normal");
            intensities.Add("Whisper", "Whisper");
            intensities.Add("Loud", "Loud");
            intensities.Add("Onomatopoeia", "Onomatopoeia");
            CustomLists["Intensities"] = intensities;

            var cameras = new Dictionary<string, string>();
            cameras.Add("", "");
            CustomLists["Cameras"] = cameras;
        }

        public static void FillDefaultImageList()
        {
            DefaultImageList = EditorHelper.CreateDefaultImageList();
        }

        public static void BindAttribute(Type type, string serializedName, string textMenu)
        {
            if (type.IsSubclassOf(typeof(NodeCondition)))
            {
                ConditionSlots.Add(new ConditionSlot() { Text = textMenu, ConditionType = type });
            }
            else if (type.IsSubclassOf(typeof(NodeAction)))
            {
                ActionSlots.Add(new ActionSlot() { Text = textMenu, ActionType = type });
            }
            else if (type.IsSubclassOf(typeof(NodeFlag)))
            {
                FlagSlots.Add(new FlagSlot() { Text = textMenu, FlagType = type });
            }
            else
            {
                Debug.Fail("BindAttribute : Invalid type provided, please use a subclass of NodeCondition, NodeAction or NodeFlag.");
            }

            SerializationBinder.AddBinding(serializedName, type);
        }

        public static void BindDocumentDialogueView(Type type, string textMenu)
        {
            DocumentDialogueViewSlots.Add(new DocumentDialogueViewSlot() { Text = textMenu, DocumentDialogueViewType = type });
        }

        public static string GetCustomListKeyFromValue(string listName, string value)
        {
            if (CustomLists.ContainsKey(listName))
            {
                foreach (KeyValuePair<string, string> kvp in CustomLists[listName])
                {
                    if (kvp.Value == value)
                        return kvp.Key;
                }
            }
            return "";
        }

        public static string GetCustomListValueFromKey(string listName, string key)
        {
            if (CustomLists.ContainsKey(listName))
            {
                if (CustomLists[listName].ContainsKey(key))
                {
                    return CustomLists[listName][key];
                }
            }
            return "";
        }
    }
}
