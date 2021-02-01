using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Newtonsoft.Json;

namespace DialogueEditor
{
    static public class ResourcesHandler
    {
        //--------------------------------------------------------------------------------------------------------------
        // Helper Class

        private class ResourceHolder
        {
            public Dialogue Dialogue = null;
            public bool Dirty = false;
        };

        //--------------------------------------------------------------------------------------------------------------
        // Public vars

        static public Project Project = null;

        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        //Legu note: I prefer to keep this one private, to force the use of GetDialogue and handle potential loading on demand
        static private Dictionary<string, ResourceHolder> dialogues = new Dictionary<string, ResourceHolder>();

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        static public Dialogue GetDialogue(string name)
        {
            if (dialogues.ContainsKey(name))
            {
                return dialogues[name].Dialogue;
            }
            return null;
        }

        static public List<Dialogue> GetAllDialogues()
        {
            return new List<Dialogue>(from item in dialogues
                                      orderby item.Key ascending
                                      select item.Value.Dialogue    );
        }

        static public List<Dialogue> GetAllDialoguesDirty()
        {
            return new List<Dialogue>(from item in dialogues
                                      where item.Value.Dirty
                                      orderby item.Key ascending
                                      select item.Value.Dialogue    );
        }

        static public List<Dialogue> GetAllDialoguesFromPackages(List<Package> packages)
        {
            return new List<Dialogue>(from item in dialogues
                                      where packages.Contains(item.Value.Dialogue.Package)
                                      orderby item.Key ascending
                                      select item.Value.Dialogue    );
        }

        static public bool IsDirty(Dialogue dialogue)
        {
            if (dialogue != null)
                return IsDirty(dialogue.GetName());
            return false;
        }

        static public bool IsDirty(string name)
        {
            if (dialogues.ContainsKey(name))
                return dialogues[name].Dirty;
            return false;
        }

        static public void SetDirty(Dialogue dialogue)
        {
            if (dialogue != null)
                SetDirty(dialogue.GetName());
        }

        static public void SetDirty(string name)
        {
            if (dialogues.ContainsKey(name))
                dialogues[name].Dirty = true;
        }

        //--------------------------------------------------------------------------------------------------------------
        // 

        static private bool CheckDialogueNameAvailable(string path, string name, bool logErrorIfNotAvailable)
        {
            if (!dialogues.ContainsKey(name))
            {
                return true;
            }

            if (logErrorIfNotAvailable)
            {
                EditorCore.LogError("Dialogue already exists (ignored) : " + name + " at \"" + path + "\" and \"" + dialogues[name].Dialogue.GetFilePath() + "\"");
            }

            return false;
        }

        static private bool AddDialogue(Dialogue dialogue)
        {
            string name = dialogue.GetName();
            if (CheckDialogueNameAvailable(dialogue.GetFilePath(), name, true))
            {
                dialogues.Add(name, new ResourceHolder { Dialogue=dialogue });
                return true;
            }

            return false;
        }

        static private bool RemoveDialogue(Dialogue dialogue)
        {
            string name = dialogue.GetName();
            if (dialogues.ContainsKey(name))
            {
                dialogues.Remove(name);
                return true;
            }
            return false;
        }

        //--------------------------------------------------------------------------------------------------------------
        // 

        static public void CreateProjectFile(string path)
        {
            Clear();

            string projectPath = Utility.GetRelativePathFromCurrentDir(path);

            Project = new Project();
            Project.Init(Path.GetDirectoryName(projectPath), Path.GetFileNameWithoutExtension(projectPath));

            ParseProject();

            ExporterJson.SaveProjectFile(Project);
            LoadAllDialogues();

            if (EditorCore.OnProjectLoad != null)
                EditorCore.OnProjectLoad();
        }

        static public Project CreateProjectInstance(string name)
        {
            Clear();

            Project = new Project();
            Project.Init("", name);

            if (EditorCore.OnProjectLoad != null)
                EditorCore.OnProjectLoad();

            return Project;
        }

        static public void LoadProjectFile(string path)
        {
            Clear();

            string projectPath = Utility.GetRelativePathFromCurrentDir(path);

            Project = new Project();
            Project.Init(Path.GetDirectoryName(projectPath), Path.GetFileNameWithoutExtension(projectPath));

            ParseProject();

            ExporterJson.LoadProjectFile(Project);
            LoadAllDialogues();

            if (EditorCore.OnProjectLoad != null)
                EditorCore.OnProjectLoad();
        }

        static public Dialogue CreateDialogueFile(string path, Package package = null)
        {
            string projectDirectory = Path.Combine(System.Environment.CurrentDirectory, Project.GetFilePath());
            string filePath = "";
            try
            {
                filePath = Utility.GetRelativePath(path, projectDirectory);
            }
            catch (System.UriFormatException)
            {
                filePath = path;    //In case the given path is already relative (or consider it as relative if it's invalid)
            }

            Dialogue dialogue = new Dialogue();
            dialogue.ResetFilePathName(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));
            dialogue.Package = (package != null) ? package : Project.GetDefaultPackage();
            if (AddDialogue(dialogue))
            {
                DialogueNodeRoot root = new DialogueNodeRoot();
                dialogue.AddNode(root);
                dialogue.RootNode = root;

                ExporterJson.SaveDialogueFile(Project, dialogue);

                return dialogue;
            }
            return null;
        }

        static public Dialogue CreateDialogueInstance(string name)
        {
            Dialogue dialogue = new Dialogue();
            dialogue.ResetFilePathName("", name);
            dialogue.Package = Project.GetDefaultPackage();
            if (AddDialogue(dialogue))
            {
                DialogueNodeRoot root = new DialogueNodeRoot();
                dialogue.AddNode(root);
                dialogue.RootNode = root;

                return dialogue;
            }
            return null;
        }

        static public Dialogue CreateEmptyDialogueInstance(string name)
        {
            Dialogue dialogue = new Dialogue();
            dialogue.ResetFilePathName("", name);
            dialogue.Package = Project.GetDefaultPackage();
            if (AddDialogue(dialogue))
            {
                return dialogue;
            }
            return null;
        }

        static public bool RenameDialogueFile(Dialogue dialogue, string path)
        {
            string projectDirectory = Path.Combine(System.Environment.CurrentDirectory, Project.GetFilePath());
            string filePath = "";
            try
            {
                filePath = Utility.GetRelativePath(path, projectDirectory);
            }
            catch (System.UriFormatException)
            {
                filePath = path;    //In case the given path is already relative (or consider it as relative if it's invalid)
            }

            string newPath = Path.GetDirectoryName(filePath);
            string newName = Path.GetFileNameWithoutExtension(filePath);
            if (CheckDialogueNameAvailable(newPath, newName, true))
            {
                RemoveDialogueFile(dialogue);

                dialogue.ResetFilePathName(newPath, newName);

                AddDialogue(dialogue);
                ExporterJson.SaveDialogueFile(Project, dialogue);
                return true;
            }

            return false;
        }

        static public bool RemoveDialogueFile(Dialogue dialogue)
        {
            if (RemoveDialogue(dialogue))
            {
                string filePathName = Path.Combine(EditorHelper.GetProjectDirectory(), dialogue.GetFilePathName());
                if (File.Exists(filePathName))
                    File.Delete(filePathName);

                return true;
            }

            return false;
        }

        static public void LoadAllDialogues()
        {
            List<string> toRemove = new List<string>();

            foreach (var kvp in dialogues)
            {
                if (!ExporterJson.LoadDialogueFile(Project, kvp.Value.Dialogue))
                {
                    toRemove.Add(kvp.Key);
                }
            }

            foreach (var key in toRemove)
            {
                dialogues.Remove(key);
            }
        }

        static public void ReloadProject()
        {
            if (Project != null)
            {
                ExporterJson.LoadProjectFile(Project);
                Project.Dirty = false;
            }
        }

        static public void ReloadDialogue(Dialogue dialogue)
        {
            ReloadDialogue(dialogue.GetName());
        }

        static public void ReloadDialogue(string name)
        {
            var holder = dialogues[name];
            if (holder != null && holder.Dialogue != null)
            {
                ExporterJson.LoadDialogueFile(Project, holder.Dialogue);
                holder.Dirty = false;
            }
        }

        static public void ReloadDialogueFromString(Dialogue dialogue, string content)
        {
            var holder = dialogues[dialogue.GetName()];
            if (dialogue != null && holder != null)
            {
                ExporterJson.LoadDialogueFromString(Project, dialogue, content);
                holder.Dirty = true;
            }
        }

        static public void ParseProject()
        {
            string projectDirectory = Path.Combine(System.Environment.CurrentDirectory, Project.GetFilePath());
            System.IO.DirectoryInfo rootDir = new DirectoryInfo(projectDirectory);

            ParseDirectory(rootDir);
        }

        static public void ParseDirectory(System.IO.DirectoryInfo root)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            try
            {
                files = root.GetFiles("*.*");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                string projectDirectory = Path.Combine(System.Environment.CurrentDirectory, Project.GetFilePath());

                foreach (System.IO.FileInfo fi in files)
                {
                    if (fi.Extension == Dialogue.GetExtension())
                    {
                        string filePath = Utility.GetRelativePath(fi.FullName, projectDirectory);

                        Dialogue dialogue = new Dialogue();
                        dialogue.ResetFilePathName(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));
                        AddDialogue(dialogue);
                    }
                }

                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    ParseDirectory(dirInfo);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // 

        static public void SaveProject()
        {
            ExporterJson.SaveProjectFile(Project);
            Project.Dirty = false;
        }

        static public void SaveDialogue(Dialogue dialogue)
        {
            var holder = dialogues[dialogue.GetName()];
            if (dialogue != null && holder != null)
            {
                ExporterJson.SaveDialogueFile(Project, dialogue);
                holder.Dirty = false;
            }
        }

        static public void SaveAllDirty()
        {
            if (Project.Dirty)
            {
                ExporterJson.SaveProjectFile(Project);
                Project.Dirty = false;
            }

            foreach (var kvp in dialogues)
            {
                if (kvp.Value.Dirty)
                {
                    ExporterJson.SaveDialogueFile(Project, kvp.Value.Dialogue);
                    kvp.Value.Dirty = false;
                }
            }
        }

        static public void SaveAll()
        {
            ExporterJson.SaveProjectFile(Project);
            Project.Dirty = false;

            foreach (var kvp in dialogues)
            {
                ExporterJson.SaveDialogueFile(Project, kvp.Value.Dialogue);
                kvp.Value.Dirty = false;
            }
        }

        static public void ReloadAll()
        {
            ReloadProject();

            foreach (var kvp in dialogues)
            {
                ReloadDialogue(kvp.Value.Dialogue);
            }
        }

        static public void Clear()
        {
            Project = null;
            dialogues.Clear();
        }

        //--------------------------------------------------------------------------------------------------------------
        // 

        static public void CheckAll()
        {
            foreach (var kvp in dialogues)
            {
                EditorHelper.CheckDialogueErrors(kvp.Value.Dialogue);
            }
        }

        static public void Check(Dialogue dialogue)
        {
            EditorHelper.CheckDialogueErrors(dialogue);
        }
    }
}
