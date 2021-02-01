using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Newtonsoft.Json;

namespace DialogueEditor
{
    public class Dialogue
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public string VersionProject { get; set; }
        public string VersionEditor { get; set; }

        [JsonIgnore]
        public Package Package { get; set; }
        public string PackageName { get; set; }

        public int LastIndex { get; set; }

        public string SceneType { get; set; }
        public string VoiceBank { get; set; }
        public string Context { get; set; }
        public string Comment { get; set; }

        public string Camera { get; set; }
        public float CameraBlendTime { get; set; }

        //[JsonConverter(typeof(ExporterJson.ConverterTriBool))]
        //public Utility.ETriBool Spatialized { get; set; }         //Should note be useful, and implicit through SceneType

        [JsonIgnore]
        public DialogueNodeRoot RootNode { get; set; }
        public int RootNodeID { get; set; }

        ///<summary> List of all the nodes in the dialogue </summary>
        public List<DialogueNode> ListNodes { get; set; }

        ///<summary> List of involved actors IDs that may not be used in any sentence </summary>
        public List<string> ListAdditionalActors { get; set; }

        public TranslationTable Translations { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        protected string path { get; set; }
        protected string name { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        static public string GetExtension()
        {
            return ".dlg";
        }

        public Dialogue()
        {
            VersionProject = "";
            VersionEditor = "";
            PackageName = "";
            LastIndex = DialogueNode.ID_NULL;
            SceneType = "";
            VoiceBank = "";
            Context = "";
            Comment = "";
            Camera = "";
            CameraBlendTime = -1.0f;
            //Spatialized = Utility.ETriBool.TB_undefined;

            ListNodes = new List<DialogueNode>();
            ListAdditionalActors = new List<string>();

            path = "";
            name = "";

            Translations = new TranslationTable();

            if (EditorCore.CustomLists["SceneTypes"].Count > 0)
            {
                SceneType = EditorCore.CustomLists["SceneTypes"].First().Key;
            }
        }

        public Dialogue(Dialogue other)
        {
            //Only Copy parameters, leave nodes and specific naming/path aside
            Copy(other);
        }

        public void Copy(Dialogue other)
        {
            //Only Copy parameters, leave nodes and specific naming/path aside
            Package = other.Package;
            SceneType = other.SceneType;
            VoiceBank = other.VoiceBank;
            Comment = other.Comment;
            Camera = other.Camera;
            CameraBlendTime = other.CameraBlendTime;
            //Spatialized = other.Spatialized;
            ListAdditionalActors = other.ListAdditionalActors;
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

        public void ResetFilePathName(string inPath, string inName)
        {
            path = inPath;
            name = inName;
        }

        public void PostLoad(Project project)
        {
            //Resolve links with package
            Package = project.GetPackage(PackageName);
            if (Package == null)
            {
                EditorCore.LogError("Loading a Dialogue without Package (forcing default) : " + GetName(), this);
                Package = project.GetDefaultPackage();
            }

            //Resolve links between nodes
            RootNode = GetNodeByID(RootNodeID) as DialogueNodeRoot;

            foreach (DialogueNode node in ListNodes)
            {
                node.Next = GetNodeByID(node.NextID);
                if (node is DialogueNodeChoice)
                {
                    DialogueNodeChoice nodeChoice = node as DialogueNodeChoice;
                    foreach (int replyID in nodeChoice.RepliesIDs)
                    {
                        DialogueNodeReply nodeReply = GetNodeByID(replyID) as DialogueNodeReply;
                        if (nodeReply != null)
                            nodeChoice.Replies.Add(nodeReply);
                    }
                }
                else if (node is DialogueNodeGoto)
                {
                    DialogueNodeGoto nodeGoto = node as DialogueNodeGoto;
                    nodeGoto.Goto = GetNodeByID(nodeGoto.GotoID);
                }
                else if (node is DialogueNodeBranch)
                {
                    DialogueNodeBranch nodebranch = node as DialogueNodeBranch;
                    nodebranch.Branch = GetNodeByID(nodebranch.BranchID);
                }
            }
        }

        public void PreSave(Project project)
        {
            //Prepare File
            VersionProject = EditorCore.VersionProject;
            VersionEditor = EditorVersion.GetVersion();

            if (Package == null)
            {
                EditorCore.LogError("Saving a Dialogue without Package (forcing default) : " + GetName(), this);
                Package = project.GetDefaultPackage();
            }

            if (Package != null)
            {
                PackageName = Package.Name;
            }

            //Sanitize texts
            //dialogue.Comment = EditorCore.SanitizeText(dialogue.Comment);

            //Prepare nodes links
            RootNodeID = (RootNode != null) ? RootNode.ID : DialogueNode.ID_NULL;

            foreach (DialogueNode node in ListNodes)
            {
                node.NextID = (node.Next != null) ? node.Next.ID : DialogueNode.ID_NULL;

                if (node is DialogueNodeSentence)
                {
                    //Generate ID
                    DialogueNodeSentence nodeSentence = node as DialogueNodeSentence;
                    nodeSentence.VoicingID = EditorHelper.GetPrettyNodeVoicingID(this, nodeSentence);

                    //Sanitize texts
                    nodeSentence.Sentence = EditorHelper.SanitizeText(nodeSentence.Sentence);
                    //nodeSentence.Comment = EditorHelper.SanitizeText(nodeSentence.Comment);
                }
                else if (node is DialogueNodeChoice)
                {
                    DialogueNodeChoice nodeChoice = node as DialogueNodeChoice;

                    //Sanitize texts
                    nodeChoice.Choice = EditorHelper.SanitizeText(nodeChoice.Choice);

                    nodeChoice.RepliesIDs.Clear();
                    foreach (DialogueNodeReply nodeReply in nodeChoice.Replies)
                    {
                        //Sanitize texts
                        nodeReply.Reply = EditorHelper.SanitizeText(nodeReply.Reply);

                        nodeChoice.RepliesIDs.Add(nodeReply.ID);
                    }
                }
                else if (node is DialogueNodeGoto)
                {
                    DialogueNodeGoto nodeGoto = node as DialogueNodeGoto;
                    nodeGoto.GotoID = (nodeGoto.Goto != null) ? nodeGoto.Goto.ID : DialogueNode.ID_NULL;
                }
                else if (node is DialogueNodeBranch)
                {
                    DialogueNodeBranch nodeBranch = node as DialogueNodeBranch;
                    nodeBranch.BranchID = (nodeBranch.Branch != null) ? nodeBranch.Branch.ID : DialogueNode.ID_NULL;
                }
            }
        }

        public int GenerateID()
        {
            // By default, LastIndex is DialogueNode.ID_NULL (-1), so the Root node should have an ID of 0, and the first "real" node an ID of 1.
            ++LastIndex;
            return LastIndex;
        }

        public DialogueNode GetNodeByID(int id)
        {
            return ListNodes.Find(item => item.ID == id);
        }

        public bool IsNodeReferencedByGoto(DialogueNode nodeRef)
        {
            foreach (DialogueNode node in ListNodes)
            {
                if (node is DialogueNodeGoto)
                {
                    DialogueNodeGoto nodeGoto = node as DialogueNodeGoto;
                    if (nodeGoto.Goto == nodeRef)
                        return true;
                }
            }
            return false;
        }

        public List<DialogueNode> GetGotoReferencesOnNode(DialogueNode nodeRef)
        {
            var gotos = new List<DialogueNode>();
            foreach (DialogueNode node in ListNodes)
            {
                if (node is DialogueNodeGoto)
                {
                    DialogueNodeGoto nodeGoto = node as DialogueNodeGoto;
                    if (nodeGoto.Goto == nodeRef)
                        gotos.Add(nodeGoto);
                }
            }
            return gotos;
        }

        public void GetOrderedNodes(ref List<DialogueNode> orderedListNodes)
        {
            ParseNodeChildren(RootNode, false, ref orderedListNodes);
        }

        public void GetDependingNodes(DialogueNode nodeFrom, ref List<DialogueNode> dependendingNodes)
        {
            ParseNodeChildren(nodeFrom, true, ref dependendingNodes);
        }

        public void AddNode(DialogueNode newNode)
        {
            //Generate flat list of all children nodes, for easier parsing
            //(only useful when adding a node with children, will not do much when adding a single node)
            var newListNodes = new List<DialogueNode>();
            ParseNodeChildren(newNode, false, ref newListNodes);

            //Resolve goto links while old IDs are still available (first resolve inside copied sequence, then link to existing nodes)
            foreach (var node in newListNodes)
            {
                if (node is DialogueNodeGoto)
                {
                    DialogueNodeGoto nodeGoto = node as DialogueNodeGoto;
                    if (nodeGoto.GotoID != DialogueNode.ID_NULL)
                    {
                        nodeGoto.Goto = newListNodes.Find(item => item.ID == nodeGoto.GotoID);
                        if (nodeGoto.Goto == null)
                        {
                            nodeGoto.Goto = GetNodeByID(nodeGoto.GotoID);
                        }
                        nodeGoto.GotoID = DialogueNode.ID_NULL;
                    }
                }
            }

            //Generate new IDs for all new nodes
            foreach (var node in newListNodes)
            {
                node.ID = GenerateID();
                ListNodes.Add(node);
            }
        }

        static private void ParseNodeChildren(DialogueNode nodeFrom, bool onlyDependencies, ref List<DialogueNode> result)
        {
            //onlyDependencies allows to get all nodes that will be affected when removing or moving nodeFrom.
            //If nodeFrom is a Reply, all the Next nodes are depepndencies.
            //If nodeFrom is a Choice or Branch, the direct Next nodes are NOT depepndencies.

            var node = nodeFrom;
            while (node != null)
            {
                result.Add(node);

                if (node is DialogueNodeChoice)
                {
                    var choice = node as DialogueNodeChoice;
                    foreach (DialogueNodeReply reply in choice.Replies)
                    {
                        ParseNodeChildren(reply, false, ref result);
                    }
                }
                else if (node is DialogueNodeBranch)
                {
                    var branch = node as DialogueNodeBranch;
                    ParseNodeChildren(branch.Branch, false, ref result);
                }

                if (onlyDependencies && !(nodeFrom is DialogueNodeReply))
                    break;

                node = node.Next;
            }
        }

        public void RemoveNode(DialogueNode nodeToRemove)
        {
            if (nodeToRemove == null)
                return;

            if (nodeToRemove is DialogueNodeReply)
            {
                //Remove depending nodes
                DialogueNode next = nodeToRemove.Next;
                while (next != null)
                {
                    DialogueNode nextNext = next.Next;
                    RemoveNode(next);
                    next = nextNext;
                }
            }
            else if (nodeToRemove is DialogueNodeChoice)
            {
                //Remove all replies, and let them remove their depending nodes
                DialogueNodeChoice nodechoice = nodeToRemove as DialogueNodeChoice;
                while (nodechoice.Replies.Count > 0)
                {
                    RemoveNode(nodechoice.Replies[0]);
                }
            }
            else if (nodeToRemove is DialogueNodeBranch)
            {
                DialogueNodeBranch nodeBranch = nodeToRemove as DialogueNodeBranch;

                //Remove depending nodes
                DialogueNode next = nodeBranch.Branch;
                while (next != null)
                {
                    DialogueNode nextNext = next.Next;
                    RemoveNode(next);
                    next = nextNext;
                }
            }
            
            foreach (DialogueNode node in ListNodes)
            {
                //Link next node to previous node
                if (node.Next == nodeToRemove)
                {
                    node.Next = nodeToRemove.Next;
                }

                //Remove from parent choice
                if (node is DialogueNodeChoice && nodeToRemove is DialogueNodeReply)
                {
                    DialogueNodeChoice nodeChoice = node as DialogueNodeChoice;
                    DialogueNodeReply nodeReply = nodeToRemove as DialogueNodeReply;
                    if (nodeChoice.Replies.Contains(nodeReply))
                    {
                        nodeChoice.Replies.Remove(nodeReply);
                    }
                }

                //Remove from parent branch
                if (node is DialogueNodeBranch)
                {
                    DialogueNodeBranch nodeBranch = node as DialogueNodeBranch;
                    if (nodeBranch.Branch == nodeToRemove)
                    {
                        nodeBranch.Branch = nodeToRemove.Next;
                    }
                }

                //Clean Goto references
                if (node is DialogueNodeGoto)
                {
                    DialogueNodeGoto nodeGoto = node as DialogueNodeGoto;
                    if (nodeGoto.Goto == nodeToRemove)
                    {
                        nodeGoto.Goto = null;
                    }
                }
            }

            ListNodes.Remove(nodeToRemove);
        }

        public bool UpdateActorID(string currentID, string newID)
        {
            bool update = false;

            //Update known global actors
            for (int i = 0; i < ListAdditionalActors.Count; ++i)
            {
                if (ListAdditionalActors[i] == currentID)
                {
                    ListAdditionalActors[i] = newID;
                    update = true;
                }
            }

            foreach (DialogueNode node in ListNodes)
            {
                //Update known node properties
                if (node is DialogueNodeSentence)
                {
                    var sentence = node as DialogueNodeSentence;
                    if (sentence.SpeakerID == currentID)
                    {
                        sentence.SpeakerID = newID;
                        update = true;
                    }
                    if (sentence.ListenerID == currentID)
                    {
                        sentence.ListenerID = newID;
                        update = true;
                    }
                }

                //Update custom node properties
                List<object> nodeAttributes = new List<object>();
                nodeAttributes.AddRange(node.Conditions.ToList<object>());
                nodeAttributes.AddRange(node.Actions.ToList<object>());
                nodeAttributes.AddRange(node.Flags.ToList<object>());
                foreach (var nodeAttribute in nodeAttributes)
                {
                    var properties = from p in nodeAttribute.GetType().GetProperties()
                                        let attrs = p.GetCustomAttributes(typeof(PropertyCharacterName), true)
                                        where attrs.Length != 0
                                        select p;

                    foreach (var propertyInfo in properties)
                    {
                        if ((string)propertyInfo.GetValue(nodeAttribute) == currentID)
                        {
                            propertyInfo.SetValue(nodeAttribute, newID);
                            update = true;
                        }
                    }
                }
            }

            return update;
        }
    }
}
