using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DialogueEditor.Properties;

namespace DialogueEditor
{
    //--------------------------------------------------------------------------------------------------------------
    // Enums

    public enum ENodeIcon
    {
        Undefined,

        Project,
        Package,
        Folder,
        Dialogue,

        Sentence,
        Choice,
        Reply,
        Goto,
        Branch,
        VirtualGoto,  // Cycle (defined in advanced view e.g. Graph) detected

        ListRootConditions,
        ListRootActions,
        ListRootFlags,
        ListItemCondition,
        ListItemAction,
        ListItemFlag,
        ListItemConditionGroup,
    }

    //--------------------------------------------------------------------------------------------------------------
    // Custom Attributes

    //Used by UpdateActorID method
    public class PropertyCharacterName : Attribute
    {
    }

    // Use this on searchable properties on nodes tags (conditions, actions, flags)
    public class PropertySearchable : Attribute
    { }

    //--------------------------------------------------------------------------------------------------------------
    // Type Converters (used by PropertyGrids)

    public class PropertyCharacterNameConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return ProjectController.Project.GetActorID(value as string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return ProjectController.Project.GetActorName(value as string);
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(ProjectController.Project.GetAllActorIDs());
        }
    }

    public abstract class PropertyCustomListConverter : TypeConverter
    {
        public string CustomListName;

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return EditorCore.GetCustomListKeyFromValue(CustomListName, value as string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return EditorCore.GetCustomListValueFromKey(CustomListName, value as string);
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(EditorCore.CustomLists[CustomListName].Keys.ToList());
        }
    }

    public class ClipboardInfos
    {
        public string sourceDialogue = "";
        public int sourceNodeID = DialogueNode.ID_NULL;
    }

    public static class EditorHelper
    {
        //--------------------------------------------------------------------------------------------------------------
        // Public vars

        ///<summary> Copy/Paste helper </summary>
        public static object Clipboard = null;
        public static ClipboardInfos ClipboardInfos = null;

        ///<summary> Current Language used on documents edition </summary>
        public static Language CurrentLanguage = null;

        ///<summary> Current Font used on documents edition </summary>
        public static Font CurrentFont = null;   //TODO: store on EditorSettings

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public static string GetProjectDirectory()
        {
            if (ProjectController.Project != null)
                return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, ProjectController.Project.Path));
            return "";
        }

        public static string GetPrettyNodeID(Dialogue dialogue, DialogueNode node)
        {
            return $"{dialogue.Name}_{node.ID}";
        }

        public static string GetPrettyNodeVoicingID(Dialogue dialogue, DialogueNode node)
        {
            return $"VX_{dialogue.Name}_{node.ID}";
        }

        public static string SanitizeText(string text)
        {
            text = text.Replace("\t", string.Empty);
            text = text.Replace("\r", string.Empty);
            text = text.Replace("\n", " ");     //Replace endlines with spaces before the trim, to keep a space between sentences but remove the trailing spaces.
            text = text.Trim();

            return text;
        }

        public static string FormatTextEntry(string text, Language language)
        {
            if (ProjectController.Project != null)
            {
                //TODO: Better parsing of Constants, this seems highly inefficient !
                foreach (var constant in ProjectController.Project.ListConstants)
                {
                    string replacement = constant.Workstring;
                    if (language != null && language != EditorCore.LanguageWorkstring)
                    {
                        var entry = ProjectController.Project.Translations.GetEntry(constant.ID, language);
                        if (entry != null)
                            replacement = entry.Text;
                    }
                    text = text.Replace($"{{constant.ID}}", replacement);
                }
            }
            return text;
        }

        public static Dictionary<Utility.ETriBool, string> GetTriBoolDictionary(string undefined = "Undefined")
        {
            var dictTriBool = new Dictionary<Utility.ETriBool, string>();
            dictTriBool.Add(Utility.ETriBool.TB_undefined, undefined);
            dictTriBool.Add(Utility.ETriBool.TB_true, "True");
            dictTriBool.Add(Utility.ETriBool.TB_false, "False");
            return dictTriBool;
        }

        public static ImageList CreateDefaultImageList()
        {
            var imageList = new ImageList();

            imageList.Images.Add("book", Resources.book);
            imageList.Images.Add("inbox", Resources.inbox);
            imageList.Images.Add("house", Resources.house);
            imageList.Images.Add("folder", Resources.folder);
            imageList.Images.Add("comment", Resources.comment);
            imageList.Images.Add("add", Resources.add);
            imageList.Images.Add("cog", Resources.cog);
            imageList.Images.Add("lightning", Resources.lightning);
            imageList.Images.Add("shield", Resources.shield);
            imageList.Images.Add("note", Resources.note);
            imageList.Images.Add("ArrowBlack", Resources.ArrowBlack);
            imageList.Images.Add("DiamondBlack", Resources.DiamondBlack);
            imageList.Images.Add("DotBlack", Resources.DotBlack);

            return imageList;
        }

        public static void SetNodeIcon(TreeNode node, ENodeIcon nodeIcon)
        {
            // Bind editor icons to actual register icons from the EditorCore.DefaultImageList
            string name = "";
            switch (nodeIcon)
            {
                case ENodeIcon.Project:             name = "book";          break;
                case ENodeIcon.Package:             name = "inbox";         break;
                case ENodeIcon.Folder:              name = "folder";        break;
                case ENodeIcon.Dialogue:            name = "comment";       break;

                case ENodeIcon.Sentence:            name = "ArrowBlack";    break;
                case ENodeIcon.Choice:              name = "DiamondBlack";  break;
                case ENodeIcon.Reply:               name = "DotBlack";      break;
                case ENodeIcon.Goto:                name = "ArrowBlack";    break;
                case ENodeIcon.Branch:              name = "DiamondBlack";  break;

                case ENodeIcon.VirtualGoto:         name = "ArrowBlack";    break;

                case ENodeIcon.ListRootConditions:  name = "shield";        break;
                case ENodeIcon.ListRootActions:     name = "lightning";     break;
                case ENodeIcon.ListRootFlags:       name = "note";          break;
                case ENodeIcon.ListItemCondition:   name = "cog";           break;
                case ENodeIcon.ListItemAction:      name = "cog";           break;
                case ENodeIcon.ListItemFlag:        name = "cog";           break;

                case ENodeIcon.ListItemConditionGroup:  name = "add";       break;
            }

            node.ImageKey = name;
            node.SelectedImageKey = name;
        }

        public static void CheckDialogueErrors(Dialogue dialogue)
        {
            Project project = ProjectController.Project;

            if (!EditorCore.CustomLists["SceneTypes"].ContainsKey(dialogue.SceneType))
            {
                ProjectController.LogError($"{dialogue.Name} - Unknown Scene Type {dialogue.SceneType}", dialogue);
            }

            if (!EditorCore.CustomLists["Cameras"].ContainsKey(dialogue.Camera))
            {
                ProjectController.LogError($"{dialogue.Name} - Unknown Camera {dialogue.Camera}", dialogue);
            }

            var usedIDs = new HashSet<int>();

            foreach (DialogueNode node in dialogue.ListNodes)
            {
                if (usedIDs.Contains(node.ID))
                {
                    ProjectController.LogError($"{dialogue.Name} - Identical ID between two nodes {node.ID}", dialogue, node);
                }
                else
                {
                    usedIDs.Add(node.ID);
                }

                if (node is DialogueNodeSentence)
                {
                    var nodeSentence = node as DialogueNodeSentence;
                    bool validSpeaker = false;

                    if (nodeSentence.SpeakerID == "")
                    {
                        ProjectController.LogError($"{dialogue.Name} {node.ID} - Sentence has no Speaker", dialogue, node);
                    }
                    else
                    {
                        if (project.GetActorFromID(nodeSentence.SpeakerID) == null)
                        {
                            ProjectController.LogError($"{dialogue.Name} {nodeSentence.SpeakerID} - Sentence has invalid Speaker {nodeSentence.SpeakerID}", dialogue, node);
                        }
                        else
                        {
                            validSpeaker = true;
                        }
                    }

                    if (nodeSentence.ListenerID != "")
                    {
                        if (project.GetActorFromID(nodeSentence.ListenerID) == null)
                        {
                            ProjectController.LogError($"{dialogue.Name} {node.ID} - Sentence has invalid Listener {nodeSentence.ListenerID}", dialogue, node);
                        }
                        else if (validSpeaker)
                        {
                            var speaker = project.GetActorFromID(nodeSentence.SpeakerID);
                            var listener = project.GetActorFromID(nodeSentence.ListenerID);

                            if (speaker == listener)
                            {
                                ProjectController.LogError($"{dialogue.Name} {node.ID} - Listener is also Speaker", dialogue, node);
                            }
                            else
                            {
                                if (speaker.VoiceKit != string.Empty)
                                {
                                    if (speaker.VoiceKit == listener.VoiceKit)
                                    {
                                        ProjectController.LogWarning($"{dialogue.Name} {node.ID} - Speaker and Listener have the same Voice Kit", dialogue, node);
                                    }
                                    else if (project.GetVoiceActorNameFromKit(speaker.VoiceKit) == project.GetVoiceActorNameFromKit(listener.VoiceKit))
                                    {
                                        ProjectController.LogWarning($"{dialogue.Name} {node.ID} - Speaker and Listener have the same Voice Actor", dialogue, node);
                                    }
                                }
                            }
                        }
                    }

                    if (nodeSentence.Sentence.Length > project.MaxLengthSentence)
                    {
                        ProjectController.LogWarning($"{dialogue.Name} {node.ID} - Sentence has too many characters", dialogue, node);
                    }
                }
                else if (node is DialogueNodeChoice)
                {
                    var nodeChoice = node as DialogueNodeChoice;

                    if (nodeChoice.Replies.Count == 0)
                    {
                        ProjectController.LogError($"{dialogue.Name} {node.ID} - Choice has no Reply", dialogue, node);
                    }
                }
                else if (node is DialogueNodeReply)
                {
                    var nodeReply = node as DialogueNodeReply;

                    if (nodeReply.Reply.Length > project.MaxLengthReply)
                    {
                        ProjectController.LogWarning($"{dialogue.Name} {node.ID} - Reply has too many characters", dialogue, node);
                    }
                }
                else if (node is DialogueNodeGoto)
                {
                    var nodeGoto = node as DialogueNodeGoto;

                    if (nodeGoto.Goto == null)
                    {
                        ProjectController.LogError($"{dialogue.Name} {node.ID} - Goto has no Target", dialogue, node);
                    }
                }
                else if (node is DialogueNodeBranch)
                {
                    var nodeBranch = node as DialogueNodeBranch;

                    if (nodeBranch.Branch == null)
                    {
                        ProjectController.LogError($"{dialogue.Name} {node.ID} - Branch has no Target", dialogue, node);
                    }
                }
            }

            EditorCore.OnCheckDialogueErrors?.Invoke(dialogue);
        }
    }
}
