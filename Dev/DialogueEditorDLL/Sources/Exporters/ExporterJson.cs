using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Newtonsoft.Json;

namespace DialogueEditor
{
    static public class ExporterJson
    {
        //--------------------------------------------------------------------------------------------------------------
        // Binding

        public class TypeNameSerializationBinder : System.Runtime.Serialization.SerializationBinder
        {
            private Dictionary<string, Type> Bindings = new Dictionary<string, Type>();

            public TypeNameSerializationBinder()
            {
                AddBinding("NodeRoot", typeof(DialogueNodeRoot));
                AddBinding("NodeSentence", typeof(DialogueNodeSentence));
                AddBinding("NodeChoice", typeof(DialogueNodeChoice));
                AddBinding("NodeReply", typeof(DialogueNodeReply));
                AddBinding("NodeGoto", typeof(DialogueNodeGoto));
                AddBinding("NodeBranch", typeof(DialogueNodeBranch));

                AddBinding("ConditionAnd", typeof(NodeConditionAnd));
                AddBinding("ConditionOr", typeof(NodeConditionOr));
            }

            public void AddBinding(string typeName, Type type)
            {
                //No need to check if Serialization Binding already has an entry for this type, Dictionnaries have a built-in assert for this
                Bindings.Add(typeName, type);
            }

            public override void BindToName(Type type, out string assemblyName, out string typeName)
            {
                if (!Bindings.Values.Contains(type))
                    EditorCore.LogError("Unknown type on serialization : " + type);

                assemblyName = null;
                typeName = Bindings.Single(item => item.Value == type).Key;
            }

            public override Type BindToType(string assemblyName, string typeName)
            {
                if (Bindings.ContainsKey(typeName))
                    return Bindings[typeName];

                EditorCore.LogError("Unknown type on deserialization : " + typeName);
                return null;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Converters

        /*public class ConverterDialogueRef : JsonConverter
        {
            public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(((Dialogue)value).GetName());
            }

            public override object ReadJson (JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
            {
                Dialogue pDialogue = ResourcesHandler.GetDialogue(reader.Value.ToString());
                return pDialogue;
            }

            public override bool CanConvert (Type type)
            {
                //Legu: je passe jamais ici, wtf ??
                return true;
            }
        }*/

        public class ConverterTriBool : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(Utility.GetStringFromTriBool((Utility.ETriBool)value));
            }

            public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
            {
                Utility.ETriBool value = Utility.GetTriBoolFromString(reader.Value.ToString());
                return value;
            }

            public override bool CanConvert(Type type)
            {
                return true;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Exports

        static public void SaveProjectFile(Project project)
        {
            //PreSave
            project.PreSave();

            //Serialize
            string path = Path.Combine(EditorHelper.GetProjectDirectory(), project.GetFileName());
            SerializeToFile(path, project);

            ExporterConstants.ExportToUnreal4();
        }

        static public void SaveDialogueFile(Project project, Dialogue dialogue)
        {
            //PreSave
            dialogue.PreSave(project);

            //Serialize
            string path = Path.Combine(EditorHelper.GetProjectDirectory(), dialogue.GetFilePathName());
            SerializeToFile(path, dialogue);
        }

        static public string SaveDialogueToString(Project project, Dialogue dialogue)
        {
            //PreSave
            dialogue.PreSave(project);

            //Serialize
            return SerializeToString(dialogue);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Imports

        static public void LoadProjectFile(Project project)
        {
            //Deserialize
            string path = Path.Combine(EditorHelper.GetProjectDirectory(), project.GetFileName());
            DeserializeFromFile(path, project);
        }

        static public bool LoadDialogueFile(Project project, Dialogue dialogue)
        {
            //Deserialize
            string path = Path.Combine(EditorHelper.GetProjectDirectory(), dialogue.GetFilePathName());
            if (!DeserializeFromFile(path, dialogue))
                return false;

            //PostLoad
            dialogue.PostLoad(project);
            return true;
        }

        static public bool LoadDialogueFromString(Project project, Dialogue dialogue, string content)
        {
            //Deserialize
            if (!DeserializeFromString(dialogue, content))
                return false;

            //PostLoad
            dialogue.PostLoad(project);
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Helpers

        static private void SerializeToFile<T>(string path, T value)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                serializer.Binder = EditorCore.SerializationBinder;
                serializer.Serialize(file, value);
            }
        }

        static private string SerializeToString<T>(T value)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(memoryStream))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.TypeNameHandling = TypeNameHandling.Auto;
                    serializer.Binder = EditorCore.SerializationBinder;
                    serializer.Serialize(writer, value);

                    writer.Flush();
                    memoryStream.Position = 0;

                    StreamReader reader = new StreamReader(memoryStream);
                    return reader.ReadToEnd();
                }
            }
        }

        static private bool DeserializeFromFile<T>(string path, T value)
        {
            //var jObject = Newtonsoft.Json.Linq.JObject.Parse(File.ReadAllText(path));
            //int version = jObject.GetValue("Version");

            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.Auto;                      //default
                settings.NullValueHandling = NullValueHandling.Include;                 //default
                settings.ObjectCreationHandling = ObjectCreationHandling.Replace;       //not default
                settings.PreserveReferencesHandling = PreserveReferencesHandling.None;  //default

                settings.Binder = EditorCore.SerializationBinder;

                JsonConvert.PopulateObject(File.ReadAllText(path), value, settings);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                EditorCore.LogError("A malformed file could not be loaded : " + path);
                EditorCore.LogError("> exception : " + e.Message);
                return false;
            }
            catch (Newtonsoft.Json.JsonSerializationException e)
            {
                //TODO: Here I could plug additionnal handlers with older versions of the SerializationBinder

                EditorCore.LogError("A file containing unknown serialization bindings could not be loaded : " + path);
                EditorCore.LogError("> exception : " + e.Message);
                return false;
            }

            return true;
        }

        static private bool DeserializeFromString<T>(T value, string content)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.Auto;                      //default
                settings.NullValueHandling = NullValueHandling.Include;                 //default
                settings.ObjectCreationHandling = ObjectCreationHandling.Replace;       //not default
                settings.PreserveReferencesHandling = PreserveReferencesHandling.None;  //default

                settings.Binder = EditorCore.SerializationBinder;

                JsonConvert.PopulateObject(content, value, settings);
            }
            catch (Newtonsoft.Json.JsonSerializationException)
            {
                EditorCore.LogError("Errors occured when parsing a file (will not be loaded)");
                return false;
            }

            return true;
        }
    }
}
