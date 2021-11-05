using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    //--------------------------------------------------------------------------------------------------------------
    // Helper delegates

    public delegate void DelegateProjectLoad();
    public delegate List<string> DelegateActorAnimsets(string ActorID);
    public delegate void DelegateCheckDialogueErrors(Dialogue dialogue);
    public delegate string DelegateProjectStats(ExporterStats.ProjectStats Stats);

    static public class EditorCore
    {
        //--------------------------------------------------------------------------------------------------------------
        // Public vars

        ///<summary> Project specific version number, registered in all saved files </summary>
        static public string VersionProject = "1.0.0";

        ///<summary> User file path used to store the EditorSettings </summary>
        static public string PathUserConfig = Path.Combine(System.Environment.CurrentDirectory, @"User.config");

        ///<summary> User file path used to store the UI configuration </summary>
        static public string PathPanelsConfig = Path.Combine(System.Environment.CurrentDirectory, @"DockPanel.config");

        ///<summary> User's settings for the Editor </summary>
        static public EditorSettings Settings = new EditorSettings();

        ///<summary> SerializationBinder used for handling nodes and attributes types during serialization </summary>
        static public ExporterJson.TypeNameSerializationBinder SerializationBinder = new ExporterJson.TypeNameSerializationBinder();

        ///<summary> ImageList shared by all containers </summary>
        static public ImageList DefaultImageList = null;

        // Main panels
        static public WindowMain MainWindow = null;
        static public PanelProjectExplorer ProjectExplorer = null;
        static public PanelProperties Properties = null;
        static public PanelOutputLog OutputLog = null;
        static public PanelSearchResults SearchResults = null;

        // Custom Conditions, Actions and Flags associated with Nodes
        static public List<ConditionSlot> ConditionSlots = new List<ConditionSlot>();
        static public List<ActionSlot> ActionSlots = new List<ActionSlot>();
        static public List<FlagSlot> FlagSlots = new List<FlagSlot>();

        // Project specific stats
        static public DelegateProjectStats ProjectStats = null;

        ///<summary> Animations groups </summary>
        static public Dictionary<string, List<string>> Animations = new Dictionary<string, List<string>>();

        ///<summary> Custom lists (named dictionaries of Asset -> Display Name) </summary>
        static public Dictionary<string, Dictionary<string, string>> CustomLists = new Dictionary<string, Dictionary<string, string>>();

        ///<summary> Default Workstring language definition </summary>
        static public Language LanguageWorkstring = new Language() { Name = "Workstring", LocalizationCode = "wk", VoicingCode = "wk" };

        ///<summary> Custom callback used when the Project is loaded </summary>
        static public DelegateProjectLoad OnProjectLoad = null;

        ///<summary> Custom callback used to retrieve Animsets usable by the specified Actor </summary>
        public static DelegateActorAnimsets GetActorAnimsets = null;

        ///<summary> Custom callback used when checking a dialogue errors </summary>
        static public DelegateCheckDialogueErrors OnCheckDialogueErrors = null;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        static public void InitDefaultWindow()
        {
            MainWindow = new WindowMain();

            ProjectExplorer = new PanelProjectExplorer();
            Properties = new PanelProperties();
            OutputLog = new PanelOutputLog();
            SearchResults = new PanelSearchResults();
        }

        static public void InitDefaultLists()
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

        static public void FillDefaultImageList()
        {
            DefaultImageList = EditorHelper.CreateDefaultImageList();
        }

        static public void BindAttribute(Type type, string serializedName, string textMenu)
        {
            if (type.IsSubclassOf(typeof(NodeCondition)))
            {
                ConditionSlots.Add(new ConditionSlot() { Text = textMenu, ConditionType = type });
                SerializationBinder.AddBinding(serializedName, type);
            }
            else if (type.IsSubclassOf(typeof(NodeAction)))
            {
                ActionSlots.Add(new ActionSlot() { Text = textMenu, ActionType = type });
                SerializationBinder.AddBinding(serializedName, type);
            }
            else if (type.IsSubclassOf(typeof(NodeFlag)))
            {
                FlagSlots.Add(new FlagSlot() { Text = textMenu, FlagType = type });
                SerializationBinder.AddBinding(serializedName, type);
            }
            else
            {
                System.Diagnostics.Debug.Fail("BindAttribute : Invalid type provided, please use a subclass of NodeCondition, NodeAction or NodeFlag.");
            }
        }

        static public void LogInfo(string message)
        {
            LogInfo(message, "", DialogueNode.ID_NULL);
        }
        static public void LogInfo(string message, string dialogue)
        {
            LogInfo(message, dialogue, DialogueNode.ID_NULL);
        }
        static public void LogInfo(string message, Dialogue dialogue)
        {
            LogInfo(message, dialogue.GetName(), DialogueNode.ID_NULL);
        }
        static public void LogInfo(string message, Dialogue dialogue, DialogueNode node)
        {
            LogInfo(message, dialogue.GetName(), node.ID);
        }
        static public void LogInfo(string message, string dialogue, int node)
        {
            if (OutputLog != null)
                OutputLog.WriteLine(LogLevel.Info, message, dialogue, node);
        }

        static public void LogWarning(string message)
        {
            LogWarning(message, "", DialogueNode.ID_NULL);
        }
        static public void LogWarning(string message, string dialogue)
        {
            LogWarning(message, dialogue, DialogueNode.ID_NULL);
        }
        static public void LogWarning(string message, Dialogue dialogue)
        {
            LogWarning(message, dialogue.GetName(), DialogueNode.ID_NULL);
        }
        static public void LogWarning(string message, Dialogue dialogue, DialogueNode node)
        {
            LogWarning(message, dialogue.GetName(), node.ID);
        }
        static public void LogWarning(string message, string dialogue, int node)
        {
            if (OutputLog != null)
                OutputLog.WriteLine(LogLevel.Warning, message, dialogue, node);
        }

        static public void LogError(string message)
        {
            LogError(message, "", DialogueNode.ID_NULL);
        }
        static public void LogError(string message, string dialogue)
        {
            LogError(message, dialogue, DialogueNode.ID_NULL);
        }
        static public void LogError(string message, Dialogue dialogue)
        {
            LogError(message, dialogue.GetName(), DialogueNode.ID_NULL);
        }
        static public void LogError(string message, Dialogue dialogue, DialogueNode node)
        {
            LogError(message, dialogue.GetName(), node.ID);
        }
        static public void LogError(string message, string dialogue, int node)
        {
            if (OutputLog != null)
                OutputLog.WriteLine(LogLevel.Error, message, dialogue, node);
        }

        static public void ClearSearchResult()
        {
            SearchResults.Clear();
        }

        static public void StartSearchResult()
        {
            SearchResults.WriteStartSearch();
        }

        static public void EndSearchResult()
        {
            SearchResults.WriteEndSearch();
        }

        public static void AddSearchResult(string message, Dialogue dialogue, DialogueNode node)
        {
            SearchResults.WriteLine(message, dialogue.GetName(), node.ID);
        }

        static public string GetCustomListKeyFromValue(string listName, string value)
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

        static public string GetCustomListValueFromKey(string listName, string key)
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
