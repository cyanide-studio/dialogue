using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DialogueEditor
{
    public partial class WindowMain : Form
    {
        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        private DeserializeDockContent deserializeDockContent;
        private Timer statusTimer;
        private string lastClosedDialogue = "";

        private bool ignoreMenuItemEvents = false;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public WindowMain()
        {
            InitializeComponent();

            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        public void Init()
        {
            //Panels
            deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            LoadPanels();

            //Status Bar
            statusProgressBar.Visible = false;
            statusLabel.Visible = false;

            //Settings
            EditorCore.Settings.Load();    //Load if file exists
            EditorCore.Settings.Save();    //Ensure file exists

            //Load last project
            if (File.Exists(EditorCore.Settings.LastProject))
            {
                ProjectController.LoadProjectFile(EditorCore.Settings.LastProject);
                ProjectController.ResyncAllFiles();
            }

            //Memory usage
            RefreshMemoryUsage();

            statusTimer = new Timer();
            statusTimer.Interval = 2000;
            statusTimer.Tick += new EventHandler(delegate
            {
                RefreshMemoryUsage();
            });
            statusTimer.Start();

//#if DEBUG
/*
            //Debug code to generate 11250 dummy dialogues (337500 sentences, 6075000 words)
            //
            //  * USAGE
            //
            // uncomment and tweak this section
            // open the tool
            // wait one minute for the generation to finish (you can look at the chapter folders being created as a countdown)
            // close the tool
            // comment this section
            // reopen the tool
            //
            // * NOTES
            //
            // it's faster in release ;)
            // you must have a project already opened previously (LastProject != null)
            // you can create a 'ProjectBig' folder to store this test project, its included in the git ignore file
            //

            int indexFile = 0;
            for (int a = 1; a <= 15; ++a)
            {
                string chapter = string.Format("Chapter_{0:000000}", a);

                for (int b = 1; b <= 15; ++b)
                {
                    string quest = string.Format("Quest_{0:000000}", b);

                    for (int c = 1; c <= 50; ++c)
                    {
                        ++indexFile;
                        string file = string.Format("File_{0:000000}", indexFile);
                        Dialogue dialogue = ProjectController.CreateDialogueFile(Path.Combine(chapter, quest, file));

                        DialogueNode current = dialogue.RootNode;
                        for (int s = 1; s <= 30; ++s)
                        {
                            DialogueNodeSentence sentence = new DialogueNodeSentence();
                            dialogue.AddNode(sentence);
                            sentence.Sentence = "Hello, I'm a dialogue sentence. I'm just here to fill this void space. Please enjoy your day - " + indexFile + "_" + sentence.ID;
                            current.Next = sentence;

                            current = sentence;
                        }
                    }
                }
            }

            ProjectController.SaveDialogues();
            ProjectController.ResyncAllFiles();
*/
//#endif
        }

        public void AddCustomMenu(ToolStripMenuItem menuItem)
        {
            int index = menuMain.Items.IndexOf(menuItemAsk);
            menuMain.Items.Insert(index, menuItem);
        }

        //Obsolete, prefer the OutputLog (and the label is used to display memory)
        /*private void DisplayStatus(string message)
        {
            if (statusTimer != null)
            {
                statusTimer.Stop();
                statusTimer = null;
            }

            statusProgressBar.Visible = false;
            statusLabel.Visible = true;

            statusLabel.Text = message;

            statusTimer = new Timer();
            statusTimer.Interval = 2000;
            statusTimer.Tick += new EventHandler(delegate
            {
                statusTimer.Stop();
                statusProgressBar.Visible = false;
                statusLabel.Visible = false;
            });
            statusTimer.Start();
        }*/

        private void RefreshMemoryUsage()
        {
            statusLabel.Visible = true;

            //statusLabel.Text = string.Format("{0:0,0}", GC.GetTotalMemory(false));

            var proc = Process.GetCurrentProcess();
            statusLabel.Text = string.Format("Estimated Memory Usage : {0:0,0}", proc.PrivateMemorySize64);
        }

        private void LoadPanels()
        {
            string configFile = EditorCore.PathPanelsConfig;
            if (File.Exists(configFile))
            {
                dockPanel.LoadFromXml(configFile, deserializeDockContent);
                EnsurePanels();
            }
            else
            {
                ResetPanels();
            }
        }

        private void SavePanels()
        {
            string configFile = EditorCore.PathPanelsConfig;
            //if (m_bSaveLayout)
            dockPanel.SaveAsXml(configFile);
            // else if (File.Exists(configFile))
            //     File.Delete(configFile);
        }

        private void EnsurePanels()
        {
            ProjectController.EnsurePanels(dockPanel);
        }

        private void ResetPanels()
        {
            ProjectController.ResetPanels(dockPanel);
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            return ProjectController.GetContentFromPersistString(persistString);
        }

        public void OpenDocumentDialogue(string name)
        {
            OpenDocumentDialogue(name, DialogueNode.ID_NULL);
        }

        public void OpenDocumentDialogue(string name, int node)
        {
            var dialogue = ProjectController.GetDialogue(name);
            if (dialogue != null)
                OpenDocumentDialogue(dialogue, node);
        }

        public void OpenDocumentDialogue(Dialogue dialogue, bool forceOpenNewView = false)
        {
            OpenDocumentDialogue(dialogue, DialogueNode.ID_NULL, forceOpenNewView);
        }

        public void OpenDocumentDialogue(Dialogue dialogue, int node, bool forceOpenNewView = false)
        {
            ProjectController.OpenDocumentDialogue(dialogue, node, forceOpenNewView);
        }

        public void CloseViews(Dialogue dialogue, bool force)
        {
            ProjectController.CloseViews(dialogue, force);
        }

        public void ShowDocument(DocumentView documentView)
        {
            documentView.Show(dockPanel, DockState.Document);
        }

        public bool CloseAllDocuments()
        {
            return ProjectController.CloseAllDocuments();
        }

        public bool ShowPopupCloseDocuments(Project dirtyProject, List<DialogueController> dirtyDialogueControllers)
        {
            if (dirtyProject != null || dirtyDialogueControllers.Count > 0)
            {
                DialogSaveOnClose dialog = new DialogSaveOnClose(dirtyProject, (from item in dirtyDialogueControllers select item.Dialogue).ToList());
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return false;
                }
                else if (result == DialogResult.OK)
                {
                    if (dirtyProject != null)
                        ProjectController.SaveProject();

                    foreach (DialogueController dialogueController in dirtyDialogueControllers)
                    {
                        dialogueController.Save();
                    }
                }
                else    //DialogResult.Ignore > Close without saving
                {
                    if (dirtyProject != null)
                        ProjectController.ReloadProject();

                    foreach (DialogueController dialogueController in dirtyDialogueControllers)
                    {
                        dialogueController.Reload();
                    }
                }
            }

            return true;
        }

        public bool CloseProjectWithCheck()
        {
            if (CloseAllDocuments())
            {
                ProjectController.ClearProperties();
                ProjectController.ClearProjectExplorer();
                ProjectController.Clear();
                return true;
            }

            return false;
        }

        public void SetMenuItemProjectExplorer(bool visible)
        {
            ignoreMenuItemEvents = true;
            menuItemProjectExplorer.Checked = visible;
            ignoreMenuItemEvents = false;
        }

        public void SetMenuItemProjectProperties(bool visible)
        {
            ignoreMenuItemEvents = true;
            menuItemProjectProperties.Checked = visible;
            ignoreMenuItemEvents = false;
        }

        public void SetMenuItemOutputLog(bool visible)
        {
            ignoreMenuItemEvents = true;
            menuItemOutputLog.Checked = visible;
            ignoreMenuItemEvents = false;
        }

        public void SetMenuItemSearchResults(bool visible)
        {
            ignoreMenuItemEvents = true;
            menuItemSearchResults.Checked = visible;
            ignoreMenuItemEvents = false;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events from other forms

        public bool OnDocumentDialogueClosed(DocumentDialogueView document, bool force)
        {
            if (force || !document.DialogueController.Dirty || document.DialogueController.HasView(2) || ShowPopupCloseDocuments(null, new List<DialogueController>() { document.DialogueController }))
            {
                if (document == dockPanel.ActiveContent)
                {
                    ProjectController.ClearProperties();
                }

                lastClosedDialogue = document.DialogueController.Dialogue.GetName();
                ProjectController.OnDocumentDialogueClosed(document);
                return true;
            }
            return false;
        }

        public bool OnDocumentProjectClosed(bool force)
        {
            if (force || !ProjectController.Dirty || ShowPopupCloseDocuments(ProjectController.Project, new List<DialogueController>()))
            {
                ProjectController.OnDocumentProjectClosed();
                return true;
            }
            return false;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Shift | Keys.T))
            {
                OpenDocumentDialogue(lastClosedDialogue);
                return true;
            }
            else if (dockPanel.ActiveContent is PanelProjectExplorer)
            {
                if (ProjectController.ProcessCmdKeyForProjectExplorer(keyData))
                {
                    return true;
                }
            }
            else if (/*dockPanel.ActiveContent is DocumentDialogue &&*/ dockPanel.ActiveDocument is DocumentDialogueTreeView)   //Allow the current Dialogue to control the keys, even if it hasn't the focus
            {
                var dialogue = dockPanel.ActiveDocument as DocumentDialogueTreeView;
                if (dialogue.ProcessCmdKey_Impl(keyData))
                {
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            if (!CloseProjectWithCheck())
            {
                e.Cancel = true;
                return;
            }

            EditorCore.Settings.Save();
            SavePanels();
        }

        public void OnExit(object sender, EventArgs e)
        {
            //if (!CloseProjectWithCheck())
            //    return;

            Close();
        }

        private void OnOpenProject(object sender, EventArgs e)
        {
            if (!CloseProjectWithCheck())
                return;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Open Project";
            //dialog.Filter = "Project Files|*.project|Dialogue Files|*.dlg";
            dialog.Filter = "Project Files|*.project";
            dialog.InitialDirectory = Environment.CurrentDirectory;

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                ProjectController.LoadProjectFile(dialog.FileName);
                ProjectController.ResyncAllFiles();

                EditorCore.Settings.LastProject = dialog.FileName;
                EditorCore.Settings.Save();
            }
        }

        private void OnNewProject(object sender, EventArgs e)
        {
            if (!CloseProjectWithCheck())
                return;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Create Project";
            //dialog.Filter = "Project Files|*.project|Dialogue Files|*.dlg";
            dialog.Filter = "Project Files|*.project";
            dialog.InitialDirectory = Environment.CurrentDirectory;

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                ProjectController.CreateProjectFile(dialog.FileName);
                ProjectController.ResyncAllFiles();

                EditorCore.Settings.LastProject = dialog.FileName;
                EditorCore.Settings.Save();
            }
        }

        private void OnCloseProject(object sender, EventArgs e)
        {
            if (!CloseProjectWithCheck())
                return;

            ProjectController.ResyncAllFiles();
        }

        private void OnCheckProjectExplorer(object sender, EventArgs e)
        {
            if (ignoreMenuItemEvents)
                return;

            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ProjectController.ShowProjectExplorerPanel(menuItem.Checked);
        }

        private void OnCheckProperties(object sender, EventArgs e)
        {
            if (ignoreMenuItemEvents)
                return;

            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ProjectController.ShowPropertiesPanel(menuItem.Checked);
        }

        private void OnCheckOutputLog(object sender, EventArgs e)
        {
            if (ignoreMenuItemEvents)
                return;

            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ProjectController.ShowOutputLogPanel(menuItem.Checked);
        }

        private void OnCheckSearchResults(object sender, EventArgs e)
        {
            if (ignoreMenuItemEvents)
                return;

            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ProjectController.ShowSearchResultsPanel(menuItem.Checked);
        }

        private void OnResetPanels(object sender, EventArgs e)
        {
            ResetPanels();
        }

        private void OnCloseAllFiles(object sender, EventArgs e)
        {
            CloseAllDocuments();
        }

        private void OnNewDialogue(object sender, EventArgs e)
        {
            if (ProjectController.Project == null)
                return;

            string projectDirectory = EditorHelper.GetProjectDirectory();

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Create Dialogue";
            dialog.Filter = "Dialogue Files|*" + Dialogue.GetExtension();
            dialog.InitialDirectory = projectDirectory;

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Dialogue newDialogue = ProjectController.CreateDialogueFile(dialog.FileName);
                if (newDialogue != null)
                {
                    ProjectController.ResyncFile(newDialogue, true);

                    OpenDocumentDialogue(newDialogue);
                }
            }
        }

        private void OnImportDialogues(object sender, EventArgs e)
        {
            if (ProjectController.Project != null)
            {
                ExporterDialogues.ImportFromCsv();
            }
        }

        private void OnExportDialogues(object sender, EventArgs e)
        {
            if (ProjectController.Project != null)
            {
                if (ExporterDialogues.ExportToCsv())
                    ProjectController.LogInfo("Export Dialogues Finished");
            }
        }

        private void OnExportLocalizationUnreal4(object sender, EventArgs e)
        {
            if (ProjectController.Project != null)
            {
                if (ExporterLocalization.ExportToUnreal4())
                    ProjectController.LogInfo("Export Localization Finished");
            }
        }

        private void OnExportVoicing(object sender, EventArgs e)
        {
            if (ProjectController.Project != null)
            {
                if (ExporterVoicing.ExportAll())
                {
                    ProjectController.RefreshDirtyFlags();

                    ProjectController.LogInfo("Export Voicing Finished");
                }
            }
        }

        private void OnExportStats(object sender, EventArgs e)
        {
            if (ProjectController.Project != null)
            {
                if (ExporterStats.ExportAll())
                    ProjectController.LogInfo("Export Stats Finished");
            }
        }

        private void OnExportLipsyncFaceFX(object sender, EventArgs e)
        {
            if (ProjectController.Project != null)
            {
                if (ExporterLipsync.ExportFaceFx())
                    ProjectController.LogInfo("Export Face FX Finished");
            }
        }

        private void OnShowStats(object sender, EventArgs e)
        {
            if (dockPanel.ActiveDocument is DocumentDialogueTreeView)
            {
                var document = dockPanel.ActiveDocument as DocumentDialogueTreeView;

                DialogStats dialog = new DialogStats(new List<Dialogue>() { document.DialogueController.Dialogue }, EditorHelper.CurrentLanguage);
                dialog.ShowDialog();
            }
        }

        private void OnShowAllStats(object sender, EventArgs e)
        {
            DialogStats dialog = new DialogStats(ProjectController.GetAllDialogues());
            dialog.ShowDialog();
        }

        private void OnActiveDocumentChanged(object sender, EventArgs e)
        {
            if (dockPanel.ActiveDocument is DocumentView)
            {
                var document = dockPanel.ActiveDocument as DocumentView;
                document.RefreshDocument();
            }
        }

        private void OnPlayDialogue(object sender, EventArgs e)
        {
            if (dockPanel.ActiveDocument is DocumentDialogueTreeView)
            {
                var document = dockPanel.ActiveDocument as DocumentDialogueTreeView;
                var viewer = new WindowDialoguePlayer(document.DialogueController, true);
                viewer.ShowDialog(this);
            }
        }

        private void OnPlayDialogueNode(object sender, EventArgs e)
        {
            if (dockPanel.ActiveDocument is DocumentDialogueTreeView)
            {
                var document = dockPanel.ActiveDocument as DocumentDialogueTreeView;
                var viewer = new WindowDialoguePlayer(document.DialogueController, false);
                viewer.ShowDialog(this);
            }
        }

        private void OnSaveFile(object sender, EventArgs e)
        {
            if (dockPanel.ActiveDocument is DocumentProjectView)
            {
                ProjectController.SaveProject();
            }
            else if (dockPanel.ActiveDocument is DocumentDialogueView)
            {
                var document = dockPanel.ActiveDocument as DocumentDialogueView;
                document.DialogueController.ResolvePendingDirty();
                document.DialogueController.Save();
            }
        }

        private void OnSaveAllFiles(object sender, EventArgs e)
        {
            ProjectController.SaveAllFiles();
        }

        private void OnForceSaveAll(object sender, EventArgs e)
        {
            ProjectController.ForceSaveAll();
        }

        private void OnCheckAll(object sender, EventArgs e)
        {
            if (ProjectController.Project == null)
                return;

            ProjectController.LogInfo("Checking all Dialogues - Begin");

            ProjectController.CheckAll();

            ProjectController.LogInfo("Checking all Dialogues - End");
        }

        private void OnCheckCurrent(object sender, EventArgs e)
        {
            if (ProjectController.Project == null)
                return;

            if (dockPanel.ActiveDocument is DocumentDialogueTreeView)
            {
                ProjectController.LogInfo("Checking Dialogue - Begin");

                var dialogue = dockPanel.ActiveDocument as DocumentDialogueTreeView;
                ProjectController.Check(dialogue.DialogueController.Dialogue);

                ProjectController.LogInfo("Checking Dialogue - End");
            }
        }

        private void OnReloadFile(object sender, EventArgs e)
        {
            if (ProjectController.Project == null)
                return;

            if (dockPanel.ActiveDocument is DocumentProjectView && ProjectController.Dirty)
            {
                var dialog = new DialogConfirmReload(ProjectController.Project, new List<Dialogue>());
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ProjectController.ReloadProject();

                    ProjectController.LogInfo("Reloaded project file");
                }
            }
            else if (dockPanel.ActiveDocument is DocumentDialogueTreeView)  // TODO: fix about other views
            {
                var document = dockPanel.ActiveDocument as DocumentDialogueTreeView;
                document.DialogueController.ResolvePendingDirty();
                if (document.DialogueController.Dirty)
                {
                    var dialog = new DialogConfirmReload(null, new List<Dialogue>() { document.DialogueController.Dialogue });
                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        document.DialogueController.Reload();
                        document.DialogueController.OnPostReload();

                        ProjectController.LogInfo("Reloaded current dialogue file");
                    }
                }
            }
        }

        private void OnReloadAllFiles(object sender, EventArgs e)
        {
            ProjectController.ReloadAllFiles();
        }

        private void OnShowHelp(object sender, EventArgs e)
        {
            DialogHelpShortcuts dialog = new DialogHelpShortcuts();
            dialog.ShowDialog();
        }

        private void OnShowAbout(object sender, EventArgs e)
        {
            DialogAbout dialog = new DialogAbout();
            dialog.ShowDialog();
        }

        private void OnEditProjectProperties(object sender, EventArgs e)
        {
            if (ProjectController.Project == null)
                return;

            ProjectController.OpenDocumentProject();
        }

        private void OnReplaceActor(object sender, EventArgs e)
        {
            if (ProjectController.Project == null)
                return;

            if (dockPanel.ActiveDocument is DocumentDialogueTreeView)
            {
                var document = dockPanel.ActiveDocument as DocumentDialogueTreeView;

                var dialog = new DialogReplaceActor();
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    document.DialogueController.UpdateActorID(dialog.ActorIDFrom, dialog.ActorIDTo);
                }
            }
        }

        private void OnExportConstantsUnreal4(object sender, EventArgs e)
        {
            if (ProjectController.Project != null)
            {
                if (ExporterConstants.ExportToUnreal4())
                    ProjectController.LogInfo("Export Constants Finished");
            }
        }

        public DocumentDialogueTreeView GetActiveDocument()
        {
            if (dockPanel.ActiveDocument is DocumentDialogueTreeView)
            {
                var document = dockPanel.ActiveDocument as DocumentDialogueTreeView;
                if (document != null)
                    return document;
            }

            return null;
        }
    }
}
