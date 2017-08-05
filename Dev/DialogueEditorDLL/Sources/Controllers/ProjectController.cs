using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DialogueEditor
{
    public static class ProjectController
    {
        #region Internal vars
        public static Project Project = null;   // Should be private too! Only keeping it public during refactoring
        public static bool Dirty = false;  // i.e. is the project (object/model) dirty - does not tell anything about the dialogues
        private static Dictionary<string, DialogueController> DialogueControllers = new Dictionary<string, DialogueController>();

        public static WindowMain MainWindow = null; // Should be private too! Only keeping it public during refactoring
        private static DocumentView ProjectView = null; // No point in caring about multiple project views
        private static PanelProjectExplorer ProjectExplorer = null;
        private static PanelProperties Properties = null;
        private static PanelOutputLog OutputLog = null;
        private static PanelSearchResults SearchResults = null;
        #endregion

        #region Dialogues management
        public static bool AddDialogue(Dialogue dialogue)
        {
            string name = dialogue.GetName();
            if (!DialogueControllers.ContainsKey(name))
            {
                DialogueControllers.Add(name, new DialogueController(dialogue));
                return true;
            }

            LogError("Dialogue already exists (ignored) : " + name + " at \"" + dialogue.GetFilePath() + "\" and \"" + DialogueControllers[name].Dialogue.GetFilePath() + "\"");
            return false;
        }

        public static bool RemoveDialogue(Dialogue dialogue)
        {
            string name = dialogue.GetName();
            if (DialogueControllers.ContainsKey(name))
            {
                DialogueControllers.Remove(name);
                return true;
            }
            return false;
        }

        public static Dialogue CreateDialogueFile(string path, Package package = null)
        {
            string projectDirectory = Path.Combine(Environment.CurrentDirectory, Project.GetFilePath());
            string filePath = "";
            try
            {
                filePath = Utility.GetRelativePath(path, projectDirectory);
            }
            catch (UriFormatException)
            {
                filePath = path;    //In case the given path is already relative (or consider it as relative if it's invalid)
            }

            Dialogue dialogue = new Dialogue();
            dialogue.Init(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));
            dialogue.Package = package != null ? package : Project.GetDefaultPackage();
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

        public static Dialogue CreateDialogueInstance(string name, bool empty = false)
        {
            Dialogue dialogue = new Dialogue();
            dialogue.Init("", name);
            dialogue.Package = Project.GetDefaultPackage();
            if (AddDialogue(dialogue))
            {
                if (!empty)
                {
                    DialogueNodeRoot root = new DialogueNodeRoot();
                    dialogue.AddNode(root);
                    dialogue.RootNode = root;
                }
                return dialogue;
            }
            return null;
        }

        public static void LoadAllDialogues()
        {
            List<string> toRemove = new List<string>();

            foreach (var kvp in DialogueControllers)
            {
                if (!ExporterJson.LoadDialogueFile(Project, kvp.Value.Dialogue))
                {
                    toRemove.Add(kvp.Key);
                }
            }

            foreach (var key in toRemove)
            {
                DialogueControllers.Remove(key);
            }
        }

        public static Dialogue GetDialogue(string name)
        {
            if (DialogueControllers.ContainsKey(name))
            {
                return DialogueControllers[name].Dialogue;
            }
            return null;
        }

        public static List<Dialogue> GetAllDialogues()
        {
            return new List<Dialogue>(from item in DialogueControllers
                                      orderby item.Key ascending
                                      select item.Value.Dialogue);
        }

        public static List<DialogueController> GetAllDialogueControllersDirty()
        {
            return new List<DialogueController>(from item in DialogueControllers
                                                where item.Value.Dirty
                                                orderby item.Key ascending
                                                select item.Value);
        }

        public static List<Dialogue> GetAllDialoguesFromPackages(List<Package> packages)
        {
            return new List<Dialogue>(from item in DialogueControllers
                                      where packages.Contains(item.Value.Dialogue.Package)
                                      orderby item.Key ascending
                                      select item.Value.Dialogue);
        }
        #endregion

        #region Project management
        public static void CreateProjectFile(string path)
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

        public static Project CreateProjectInstance(string name)
        {
            Clear();

            Project = new Project();
            Project.Init("", name);

            if (EditorCore.OnProjectLoad != null)
                EditorCore.OnProjectLoad();

            return Project;
        }

        public static void LoadProjectFile(string path)
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

        public static void ReloadProject()
        {
            if (Project != null)
            {
                ExporterJson.LoadProjectFile(Project);
                Dirty = false;
            }
            if (ProjectView != null)
                ProjectView.OnPostReload();
        }

        public static void ParseProject()
        {
            string projectDirectory = Path.Combine(Environment.CurrentDirectory, Project.GetFilePath());
            DirectoryInfo rootDir = new DirectoryInfo(projectDirectory);

            ParseDirectory(rootDir);
        }

        public static void ParseDirectory(DirectoryInfo root)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;

            try
            {
                files = root.GetFiles("*.*");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                string projectDirectory = Path.Combine(Environment.CurrentDirectory, Project.GetFilePath());

                foreach (FileInfo fi in files)
                {
                    if (fi.Extension == Dialogue.GetExtension())
                    {
                        string filePath = Utility.GetRelativePath(fi.FullName, projectDirectory);

                        Dialogue dialogue = new Dialogue();
                        dialogue.Init(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));
                        AddDialogue(dialogue);
                    }
                }

                subDirs = root.GetDirectories();

                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    ParseDirectory(dirInfo);
                }
            }
        }

        public static void SaveProject()
        {
            ExporterJson.SaveProjectFile(Project);
            Dirty = false;
            RefreshDirtyFlags();
        }

        public static bool AddActor(Actor newActor)
        {
            bool added = Project.AddActor(newActor);
            if (added)
                Dirty = true;
            return added;
        }
        #endregion

        #region Views management
        public static void InitDefaultWindow()
        {
            MainWindow = new WindowMain();
            ProjectExplorer = new PanelProjectExplorer();
            Properties = new PanelProperties();
            OutputLog = new PanelOutputLog();
            SearchResults = new PanelSearchResults();
        }

        public static void ForcePropertiesFocus()
        {
            if (Properties != null)
                Properties.ForceFocus();
        }

        public static void ShowProperties(DialogueController dialogueController, DialogueNode dialogueNode)
        {
            if (Properties != null)
                Properties.ShowDialogueNodeProperties(dialogueController, dialogueNode);
        }

        public static void UpdateProperties(DialogueController dialogueController, DialogueNode dialogueNode, List<string> preciseElements = null)
        {
            if (Properties != null)
                Properties.UpdateDialogueNodeProperties(dialogueController, dialogueNode, preciseElements);
        }

        public static void ClearProjectExplorer()
        {
            if (ProjectExplorer != null)
                ProjectExplorer.Clear();
        }

        public static void ClearProperties()
        {
            if (Properties != null)
                Properties.Clear();
        }

        public static void EnsurePanels(DockPanel dockPanel)
        {
            if (!dockPanel.Contains(ProjectExplorer))
                ProjectExplorer.Show(dockPanel, DockState.DockLeft);
            if (!dockPanel.Contains(Properties))
                Properties.Show(dockPanel, DockState.DockRight);
            if (!dockPanel.Contains(OutputLog))
                OutputLog.Show(dockPanel, DockState.DockBottom);
            if (!dockPanel.Contains(SearchResults))
                SearchResults.Show(dockPanel, DockState.DockBottom);

            SyncMenuItemFromPanel(ProjectExplorer);
            SyncMenuItemFromPanel(Properties);
            SyncMenuItemFromPanel(OutputLog);
            SyncMenuItemFromPanel(SearchResults);
        }

        public static void ResetPanels(DockPanel dockPanel)
        {
            ProjectExplorer.Show(dockPanel, DockState.DockLeft);
            Properties.Show(dockPanel, DockState.DockRight);
            SearchResults.Show(dockPanel, DockState.DockBottom);
            OutputLog.Show(dockPanel, DockState.DockBottom);
        }

        private static void ShowDockContent(DockContent panel, bool show = true)
        {
            if (!show && (panel.Visible || !panel.IsHidden))
            {
                panel.Hide();
            }
            else if (show && (!panel.Visible || panel.IsHidden))
            {
                panel.Show();
            }
        }

        public static void ShowProjectExplorerPanel(bool show = true)
        {
            ShowDockContent(ProjectExplorer, show);
        }

        public static void ShowPropertiesPanel(bool show = true)
        {
            ShowDockContent(Properties, show);
        }

        public static void ShowOutputLogPanel(bool show = true)
        {
            ShowDockContent(OutputLog, show);
        }

        public static void ShowSearchResultsPanel(bool show = true)
        {
            ShowDockContent(SearchResults, show);
        }

        public static void CreateCustomProperties(Form owner, bool show = true)
        {
            // Basically it's only for TestSingleWindow project
            Properties = new PanelProperties();
            Properties.Owner = owner;
            if (show)
                Properties.Show();
        }

        public static void SyncMenuItemFromPanel(DockContent panel)
        {
            if (MainWindow != null)
            {
                if (panel == ProjectExplorer)
                {
                    MainWindow.SetMenuItemProjectExplorer(!panel.IsHidden);   // panel.Visible means the panel is fully visible, IsHidden means it's closed
                }
                else if (panel == Properties)
                {
                    MainWindow.SetMenuItemProjectProperties(!panel.IsHidden);
                }
                else if (panel == OutputLog)
                {
                    MainWindow.SetMenuItemOutputLog(!panel.IsHidden);
                }
                else if (panel == SearchResults)
                {
                    MainWindow.SetMenuItemSearchResults(!panel.IsHidden);
                }
            }
        }

        public static bool ProcessCmdKeyForProjectExplorer(Keys keyData)
        {
            return ProjectExplorer != null && ProjectExplorer.ProcessCmdKey_Impl(keyData);
        }

        public static void OpenDocumentProject()
        {
            if (ProjectView != null)
            {
                ProjectView.Activate();   //OnActiveDocumentChanged will handle the refresh
                return;
            }

            ProjectView = new DocumentProjectView();
            MainWindow.ShowDocument(ProjectView);
        }

        public static bool CloseDocumentProject(bool force)
        {
            if (ProjectView != null)
            {
                ProjectView.ForceClose = force;
                ProjectView.Close();
                return true;
            }
            return false;
        }

        public static void OnDocumentProjectClosed()
        {
            ProjectView = null;
        }

        public static void OnDocumentDialogueClosed(DocumentDialogueView documentView)
        {
            foreach (var entry in DialogueControllers)
            {
                if (entry.Value.OnDocumentDialogueClosed(documentView))
                {
                    return;
                }
            }
        }

        public static void OpenDocumentDialogue(Dialogue dialogue, int node, bool forceOpenNewView = false)
        {
            OpenDocumentDialogue<DocumentDialogueTreeView>(dialogue, node, forceOpenNewView);
        }

        public static void OpenDocumentDialogue<T>(Dialogue dialogue, int node, bool forceOpenNewView = false) where T : DocumentDialogueView
        {
            if (!forceOpenNewView)
            {
                foreach (var entry in DialogueControllers)
                {
                    if (entry.Value.Dialogue == dialogue)
                    {
                        if (entry.Value.HasView<T>())
                        {
                            entry.Value.Activate<T>();
                            if (node != DialogueNode.ID_NULL)
                                entry.Value.SelectNode(node);
                            return;
                        }
                        break;
                    }
                }
            }

            // Spawn new view
            DialogueController dialogueController = null;
            foreach (var entry in DialogueControllers)
            {
                if (entry.Value.Dialogue == dialogue)
                {
                    dialogueController = entry.Value;
                    break;
                }
            }
            if (dialogueController == null)
            {
                dialogueController = new DialogueController(dialogue);
                DialogueControllers[dialogue.GetName()] = dialogueController;
            }

            DocumentDialogueView newDocument = (T) Activator.CreateInstance(typeof(T), dialogueController);
            newDocument.InitView();
            MainWindow.ShowDocument(newDocument);
            if (newDocument is DocumentDialogueTreeView)   // TODO: SelectNode refactoring...
                (newDocument as DocumentDialogueTreeView).SelectNode(node);

            dialogueController.AddView(newDocument);

            EditorHelper.CheckDialogueErrors(dialogue);
        }

        public static void CloseViews(Dialogue dialogue, bool force)
        {
            foreach (var entry in DialogueControllers)
            {
                if (entry.Value.Dialogue == dialogue)
                {
                    entry.Value.CloseViews(force);
                }
            }
        }
        #endregion

        #region Logs
        private static void LogInfo(string message, string dialogue, int node)
        {
            if (OutputLog != null)
                OutputLog.LogInfo(message, dialogue, node);
        }

        private static void LogWarning(string message, string dialogue, int node)
        {
            if (OutputLog != null)
                OutputLog.LogWarning(message, dialogue, node);
        }

        private static void LogError(string message, string dialogue, int node)
        {
            if (OutputLog != null)
                OutputLog.LogError(message, dialogue, node);
        }

        public static void LogInfo(string message)
        {
            LogInfo(message, "", DialogueNode.ID_NULL);
        }

        public static void LogInfo(string message, string dialogue)
        {
            LogInfo(message, dialogue, DialogueNode.ID_NULL);
        }

        public static void LogInfo(string message, Dialogue dialogue)
        {
            LogInfo(message, dialogue.GetName(), DialogueNode.ID_NULL);
        }

        public static void LogInfo(string message, Dialogue dialogue, DialogueNode node)
        {
            LogInfo(message, dialogue.GetName(), node.ID);
        }

        public static void LogWarning(string message)
        {
            LogWarning(message, "", DialogueNode.ID_NULL);
        }

        public static void LogWarning(string message, string dialogue)
        {
            LogWarning(message, dialogue, DialogueNode.ID_NULL);
        }

        public static void LogWarning(string message, Dialogue dialogue)
        {
            LogWarning(message, dialogue.GetName(), DialogueNode.ID_NULL);
        }

        public static void LogWarning(string message, Dialogue dialogue, DialogueNode node)
        {
            LogWarning(message, dialogue.GetName(), node.ID);
        }

        public static void LogError(string message)
        {
            LogError(message, "", DialogueNode.ID_NULL);
        }

        public static void LogError(string message, string dialogue)
        {
            LogError(message, dialogue, DialogueNode.ID_NULL);
        }

        public static void LogError(string message, Dialogue dialogue)
        {
            LogError(message, dialogue.GetName(), DialogueNode.ID_NULL);
        }

        public static void LogError(string message, Dialogue dialogue, DialogueNode node)
        {
            LogError(message, dialogue.GetName(), node.ID);
        }
        #endregion 

        #region Misc
        public static bool IsDirty(Dialogue dialogue)
        {
            if (dialogue != null)
                return IsDirty(dialogue.GetName());
            return false;
        }

        public static bool IsDirty(string name)
        {
            if (DialogueControllers.ContainsKey(name))
                return DialogueControllers[name].Dirty;
            return false;
        }

        public static void SetDirty(Dialogue dialogue)
        {
            if (dialogue != null)
                SetDirty(dialogue.GetName());
        }

        public static void SetDirty(string name)
        {
            if (DialogueControllers.ContainsKey(name))
                DialogueControllers[name].SetDirty();
        }

        public static void SetDirty()
        {
            Dirty = true;
        }

        public static void RefreshDirtyFlags()
        {
            foreach (KeyValuePair<string, DialogueController> entry in DialogueControllers)
            {
                if (entry.Value != null)
                    entry.Value.RefreshDirtyFlags();
            }
            if (ProjectView != null)
                ProjectView.RefreshTitle();
        }

        public static void ResyncAllFiles()
        {
            if (ProjectExplorer != null)
                ProjectExplorer.ResyncAllFiles();
        }

        public static void ResyncFile(Dialogue dialogue, Package previousPackage, bool focus)
        {
            if (ProjectExplorer != null)
                ProjectExplorer.ResyncFile(dialogue, previousPackage, focus);
        }

        public static void ResyncFile(Dialogue dialogue, bool focus)
        {
            if (ProjectExplorer != null)
                ProjectExplorer.ResyncFile(dialogue, focus);
        }

        public static bool IsEditingWorkstring()
        {
            return Properties != null && Properties.IsEditingWorkstring();
        }

        public static void ResolvePendingDirty()
        {
            if (Properties != null)
                Properties.OnResolvePendingDirty();
        }

        public static IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(PanelProjectExplorer).ToString())
                return ProjectExplorer;
            else if (persistString == typeof(PanelProperties).ToString())
                return Properties;
            else if (persistString == typeof(PanelOutputLog).ToString())
                return OutputLog;
            else if (persistString == typeof(PanelSearchResults).ToString())
                return SearchResults;
            return null;
        }

        public static void SaveAllFiles()
        {
            if (Dirty)
                SaveProject();
            foreach (var entry in DialogueControllers)
            {
                entry.Value.Save();
            }
        }

        public static bool CloseAllDocuments()
        {
            if (Project != null)
            {
                Project dirtyProject = Dirty ? Project : null;
                var dirtyDialogueControllers = GetAllDialogueControllersDirty();

                if (!MainWindow.ShowPopupCloseDocuments(dirtyProject, dirtyDialogueControllers))
                    return false;
            }

            //At this point, what should have been saved is saved, so we can force close every document
            // TODO: there's probably a more elegant way to do it
            while (DialogueControllers.Count > 0)
            {
                DialogueControllers.ElementAt(0).Value.CloseViews(true);
                DialogueControllers.Remove(DialogueControllers.ElementAt(0).Key);
            }
            DialogueControllers.Clear();

            CloseDocumentProject(true);

            return true;
        }

        public static void ForceSaveAll()
        {
            if (Project == null)
                return;

            foreach (var entry in DialogueControllers)
            {
                entry.Value.ResolvePendingDirty();
            }

            LogInfo("Checking all Dialogues - Begin");
            CheckAll();
            LogInfo("Checking all Dialogues - End");
            SaveAll();

            RefreshDirtyFlags();
            LogInfo("All Project Files Saved");
        }

        public static void ReloadAllFiles()
        {
            if (Project == null)
                return;

            Project project = null;
            var newDialogues = new List<Dialogue>();

            if (ProjectView != null && Dirty)
            {
                project = Project;
            }

            if (DialogueControllers.Count > 0)
            {
                foreach (var entry in DialogueControllers)
                {
                    entry.Value.ResolvePendingDirty();
                    if (entry.Value.Dirty)
                        newDialogues.Add(entry.Value.Dialogue);
                }
            }

            bool reloadOK = true;
            if (project != null || newDialogues.Count > 0)
            {
                var dialog = new DialogConfirmReload(project, newDialogues);
                DialogResult result = dialog.ShowDialog();
                reloadOK = result == DialogResult.OK;
            }

            if (reloadOK)
            {
                ReloadAll();

                foreach (var entry in DialogueControllers)
                {
                    entry.Value.OnPostReload();
                }

                LogInfo("Reloaded all project files");
            }
        }

        public static void SaveAllDirty()
        {
            if (Dirty)
            {
                ExporterJson.SaveProjectFile(Project);
                Dirty = false;
            }

            foreach (var kvp in DialogueControllers)
            {
                kvp.Value.Save();
            }

            RefreshDirtyFlags();
        }

        public static void SaveAll()
        {
            ExporterJson.SaveProjectFile(Project);
            Dirty = false;
            SaveDialogues();
        }

        public static void SaveDialogues()
        {
            foreach (var kvp in DialogueControllers)
            {
                kvp.Value.Save();
            }
        }

        public static void ReloadAll()
        {
            ReloadProject();

            foreach (var kvp in DialogueControllers)
            {
                kvp.Value.Reload();
            }
        }

        public static void Clear()
        {
            Project = null;
            DialogueControllers.Clear();
        }

        public static void CheckAll()
        {
            foreach (var kvp in DialogueControllers)
            {
                EditorHelper.CheckDialogueErrors(kvp.Value.Dialogue);
            }
        }

        public static void Check(Dialogue dialogue)
        {
            EditorHelper.CheckDialogueErrors(dialogue);
        }

        public static void UpdateActorID(string actorIDFrom, string actorIDTo)
        {
            foreach (var kvp in DialogueControllers)
            {
                kvp.Value.UpdateActorID(actorIDFrom, actorIDTo);
            }

            SetDirty();
            RefreshDirtyFlags();
        }
        #endregion
    }
}
