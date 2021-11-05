using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DialogueEditor
{
    public static class ExporterJson
    {
        //--------------------------------------------------------------------------------------------------------------
        // Binding

        public class TypeNameSerializationBinder : SerializationBinder
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
                    ProjectController.LogError($"Unknown type {type} on serialization");

                assemblyName = null;
                typeName = Bindings.Single(item => item.Value == type).Key;
            }

            public override Type BindToType(string assemblyName, string typeName)
            {
                Type binding;
                if (Bindings.TryGetValue(typeName, out binding))
                    return binding;

                ProjectController.LogError($"Unknown type {typeName} on deserialization");
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

        public static void SaveProjectFile(Project project)
        {
            //PreSave
            project.PreSave();

            //Serialize
            string path = Path.Combine(EditorHelper.GetProjectDirectory(), project.FileName);
            SerializeToFile(path, project);

            ExporterConstants.ExportToUnreal4();
        }

        public static void SaveDialogueFile(Project project, Dialogue dialogue)
        {
            //PreSave
            dialogue.PreSave(project);

            //Serialize
            string path = Path.Combine(EditorHelper.GetProjectDirectory(), dialogue.FullPath);
            SerializeToFile(path, dialogue);
        }

        public static string SaveDialogueToString(Project project, Dialogue dialogue)
        {
            //PreSave
            dialogue.PreSave(project);

            //Serialize
            return SerializeToString(dialogue);
        }

        //--------------------------------------------------------------------------------------------------------------
        // Imports

        public static void LoadProjectFile(Project project)
        {
            //Deserialize
            string path = Path.Combine(EditorHelper.GetProjectDirectory(), project.FileName);
            DeserializeFromFile(path, project);
        }

        public static bool LoadDialogueFile(Project project, Dialogue dialogue)
        {
            //Deserialize
            string path = Path.Combine(EditorHelper.GetProjectDirectory(), dialogue.FullPath);
            if (!DeserializeFromFile(path, dialogue))
                return false;

            //PostLoad
            dialogue.PostLoad(project);
            return true;
        }

        public static bool LoadDialogueFromString(Project project, Dialogue dialogue, string content)
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

        private static void SerializeToFile<T>(string path, T value)
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

        private static string SerializeToString<T>(T value)
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

        private static bool DeserializeFromFile<T>(string path, T value)
        {
            //var jObject = Newtonsoft.Json.Linq.JObject.Parse(Path.ReadAllText(path));
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
            catch (JsonReaderException e)
            {
                ProjectController.LogError("A malformed file could not be loaded: " + path);
                ProjectController.LogError("> exception: " + e.Message);
                return false;
            }
            catch (JsonSerializationException e)
            {
                //TODO: Here I could plug additionnal handlers with older versions of the SerializationBinder

                ProjectController.LogError("A file containing unknown serialization bindings could not be loaded: " + path);
                ProjectController.LogError("> exception: " + e.Message);
                return false;
            }

            return true;
        }

        private static bool DeserializeFromString<T>(T value, string content)
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
            catch (JsonSerializationException)
            {
                ProjectController.LogError("Errors occured when parsing a file (will not be loaded)");
                return false;
            }

            return true;
        }
    }
}
