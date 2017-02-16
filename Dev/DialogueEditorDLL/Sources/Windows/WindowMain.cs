using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

namespace DialogueEditor
{
    public partial class WindowMain : Form
    {
        //--------------------------------------------------------------------------------------------------------------
        // Internal vars

        private DeserializeDockContent deserializeDockContent;

        private DocumentProject documentProject = null;
        private List<DocumentDialogue> documentDialogues = new List<DocumentDialogue>();
        private Timer statusTimer;

        private string lastClosedDialogue = "";

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public WindowMain()
        {
            InitializeComponent();

            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
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
                ResourcesHandler.LoadProjectFile(EditorCore.Settings.LastProject);
                EditorCore.ProjectExplorer.ResyncAllFiles();
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
            //Debug code to generate 11250 dummy dialogues (337500 sentences)
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
            //
            /*
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
                        Dialogue dialogue = ResourcesHandler.CreateDialogueFile(Path.Combine(chapter, quest, file));

                        DialogueNode current = dialogue.RootNode;
                        for (int s = 1; s <= 30; ++s)
                        {
                            DialogueNodeSentence sentence = new DialogueNodeSentence();
                            dialogue.AddNode(sentence);
                            sentence.Sentence = "Hello, I'm a dialogue sentence. I'm just here to fill this void space. Please enjoy your day - " + indexFile + "_" + sentence.ID;
                            current.Next = sentence;

                            current = sentence;
                        }

                        ResourcesHandler.SaveDialogue(dialogue);
                    }
                }
            }
            EditorCore.ProjectExplorer.ResyncAllFiles();
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

            var proc = System.Diagnostics.Process.GetCurrentProcess();
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
            if (!dockPanel.Contains(EditorCore.ProjectExplorer))
                EditorCore.ProjectExplorer.Show(dockPanel, DockState.DockLeft);
            if (!dockPanel.Contains(EditorCore.Properties))
                EditorCore.Properties.Show(dockPanel, DockState.DockRight);
            if (!dockPanel.Contains(EditorCore.OutputLog))
                EditorCore.OutputLog.Show(dockPanel, DockState.DockBottom);
        }

        private void ResetPanels()
        {
            EditorCore.ProjectExplorer.Show(dockPanel, DockState.DockLeft);
            EditorCore.Properties.Show(dockPanel, DockState.DockRight);
            EditorCore.OutputLog.Show(dockPanel, DockState.DockBottom);
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(PanelProjectExplorer).ToString())
                return EditorCore.ProjectExplorer;
            else if (persistString == typeof(PanelProperties).ToString())
                return EditorCore.Properties;
            else if (persistString == typeof(OutputLog).ToString())
                return EditorCore.OutputLog;
            return null;
        }

        public void OpenDocumentDialogue(string name)
        {
            OpenDocumentDialogue(name, DialogueNode.ID_NULL);
        }

        public void OpenDocumentDialogue(string name, int node)
        {
            var dialogue = ResourcesHandler.GetDialogue(name);
            if (dialogue != null)
                OpenDocumentDialogue(dialogue, node);
        }

        public void OpenDocumentDialogue(Dialogue dialogue)
        {
            OpenDocumentDialogue(dialogue, DialogueNode.ID_NULL);
        }

        public void OpenDocumentDialogue(Dialogue dialogue, int node)
        {
            foreach (DocumentDialogue document in documentDialogues)
            {
                if (document.Dialogue == dialogue)
                {
                    document.Activate();   //OnActiveDocumentChanged will handle the refresh

                    document.SelectNode(node);
                    return;
                }
            }

            DocumentDialogue newDocument = new DocumentDialogue(dialogue);
            newDocument.Show(dockPanel, DockState.Document);
            documentDialogues.Add(newDocument);

            newDocument.SelectNode(node);

            EditorHelper.CheckDialogueErrors(dialogue);
        }

        public bool IsDocumentFocused(Dialogue dialogue)
        {
            foreach (DocumentDialogue document in documentDialogues)
            {
                if (document.Dialogue == dialogue)
                {
                    return document.Focused;
                }
            }

            return false;
        }

        public bool IsDocumentOpened(Dialogue dialogue)
        {
            foreach (DocumentDialogue document in documentDialogues)
            {
                if (document.Dialogue == dialogue)
                {
                    return !document.IsHidden;
                }
            }

            return false;
        }

        public bool CloseDocumentDialogue(Dialogue dialogue, bool force)
        {
            foreach (DocumentDialogue document in documentDialogues)
            {
                if (document.Dialogue == dialogue)
                {
                    return CloseDocumentDialogue(document, force);
                }
            }

            return false;
        }

        public bool CloseDocumentDialogue(DocumentDialogue document, bool force)
        {
            if (document != null)
            {
                document.ForceClose = force;
                document.Close();
                return true;
            }
            return false;
        }

        public void OpenDocumentProject()
        {
            if (documentProject != null)
            {
                documentProject.Activate();   //OnActiveDocumentChanged will handle the refresh
                return;
            }

            documentProject = new DocumentProject();
            documentProject.Show(dockPanel, DockState.Document);
        }

        public bool CloseDocumentProject(bool force)
        {
            if (documentProject != null)
            {
                documentProject.ForceClose = force;
                documentProject.Close();
                return true;
            }
            return false;
        }

        public bool CloseAllDocuments()
        {
            if (ResourcesHandler.Project != null)
            {
                Project dirtyProject = (ResourcesHandler.Project.Dirty) ? ResourcesHandler.Project : null;
                var dirtyDialogues = ResourcesHandler.GetAllDialoguesDirty();

                if (!ShowPopupCloseDocuments(dirtyProject, dirtyDialogues))
                    return false;
            }

            //At this point, what should have been saved is saved, so we can force close every document
            while (documentDialogues.Count > 0)
            {
                CloseDocumentDialogue(documentDialogues[0], true);
            }

            CloseDocumentProject(true);

            return true;
        }

        private bool ShowPopupCloseDocuments(Project dirtyProject, List<Dialogue> dirtyDialogues)
        {
            if (dirtyProject != null || dirtyDialogues.Count > 0)
            {
                DialogSaveOnClose dialog = new DialogSaveOnClose(dirtyProject, dirtyDialogues);
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return false;
                }
                else if (result == DialogResult.OK)
                {
                    if (dirtyProject != null)
                        ResourcesHandler.SaveProject();

                    foreach (Dialogue dialogue in dirtyDialogues)
                    {
                        ResourcesHandler.SaveDialogue(dialogue);
                    }
                }
                else    //DialogResult.Ignore > Close without saving
                {
                    if (dirtyProject != null)
                        ResourcesHandler.ReloadProject();

                    foreach (Dialogue dialogue in dirtyDialogues)
                    {
                        ResourcesHandler.ReloadDialogue(dialogue);
                    }
                }
            }

            return true;
        }

        public bool CloseProjectWithCheck()
        {
            if (CloseAllDocuments())
            {
                EditorCore.Properties.Clear();
                EditorCore.ProjectExplorer.Clear();
                ResourcesHandler.Clear();
                return true;
            }

            return false;
        }

        public void RefreshDirtyFlags()
        {
            foreach (DocumentDialogue document in documentDialogues)
            {
                document.RefreshTitle();
            }

            if (documentProject != null)
                documentProject.RefreshTitle();
        }

        public void SyncMenuItemFromPanel(DockContent panel)
        {
            if (panel == EditorCore.ProjectExplorer)
            {
                menuItemProjectExplorer.Checked = panel.Visible;
            }
            else if (panel == EditorCore.Properties)
            {
                menuItemProjectProperties.Checked = panel.Visible;
            }
            else if (panel == EditorCore.OutputLog)
            {
                menuItemOutputLog.Checked = panel.Visible;
            }
        }

        public void SyncPanelFromMenuItem(DockContent panel, ToolStripMenuItem menuItem)
        {
            if (menuItem.Checked)
            {
                if (!panel.Visible)
                    panel.Show();
            }
            else
            {
                if (panel.Visible)
                    panel.Hide();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Events from other forms

        public bool OnDocumentDialogueClosed(DocumentDialogue document, bool force)
        {
            if (force || !ResourcesHandler.IsDirty(document.Dialogue) || ShowPopupCloseDocuments(null, new List<Dialogue>() { document.Dialogue }))
            {
                if (document == dockPanel.ActiveContent)
                {
                    if (EditorCore.Properties != null)
                        EditorCore.Properties.Clear();
                }

                lastClosedDialogue = document.Dialogue.GetName();
                documentDialogues.Remove(document);
                return true;
            }
            return false;
        }

        public bool OnDocumentProjectClosed(bool force)
        {
            if (force || !ResourcesHandler.Project.Dirty || ShowPopupCloseDocuments(ResourcesHandler.Project, new List<Dialogue>()))
            {
                documentProject = null;
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
            else if (dockPanel.ActiveContent is PanelProjectExplorer && EditorCore.ProjectExplorer != null)
            {
                if (EditorCore.ProjectExplorer.ProcessCmdKey_Impl(keyData))
                {
                    return true;
                }
            }
            else if (/*dockPanel.ActiveContent is DocumentDialogue &&*/ dockPanel.ActiveDocument is DocumentDialogue)   //Allow the current Dialogue to control the keys, even if it hasn't the focus
            {
                var dialogue = dockPanel.ActiveDocument as DocumentDialogue;
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
            dialog.InitialDirectory = System.Environment.CurrentDirectory;

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                ResourcesHandler.LoadProjectFile(dialog.FileName);
                EditorCore.ProjectExplorer.ResyncAllFiles();

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
            dialog.InitialDirectory = System.Environment.CurrentDirectory;

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                ResourcesHandler.CreateProjectFile(dialog.FileName);
                EditorCore.ProjectExplorer.ResyncAllFiles();

                EditorCore.Settings.LastProject = dialog.FileName;
                EditorCore.Settings.Save();
            }
        }

        private void OnCloseProject(object sender, EventArgs e)
        {
            if (!CloseProjectWithCheck())
                return;

            EditorCore.ProjectExplorer.ResyncAllFiles();
        }

        private void OnCheckProjectExplorer(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            SyncPanelFromMenuItem(EditorCore.ProjectExplorer, menuItem);
        }

        private void OnCheckProperties(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            SyncPanelFromMenuItem(EditorCore.Properties, menuItem);
        }

        private void OnCheckOutputLog(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            SyncPanelFromMenuItem(EditorCore.OutputLog, menuItem);
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
            if (ResourcesHandler.Project == null)
                return;

            string projectDirectory = EditorHelper.GetProjectDirectory();

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Create Dialogue";
            dialog.Filter = "Dialogue Files|*" + Dialogue.GetExtension();
            dialog.InitialDirectory = projectDirectory;

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Dialogue newDialogue = ResourcesHandler.CreateDialogueFile(dialog.FileName);
                if (newDialogue != null)
                {
                    EditorCore.ProjectExplorer.ResyncFile(newDialogue, true);

                    OpenDocumentDialogue(newDialogue);
                }
            }
        }

        private void OnImportDialogues(object sender, EventArgs e)
        {
            if (ResourcesHandler.Project != null)
            {
                ExporterDialogues.ImportFromCsv();
            }
        }

        private void OnExportDialogues(object sender, EventArgs e)
        {
            if (ResourcesHandler.Project != null)
            {
                if (ExporterDialogues.ExportToCsv())
                    EditorCore.LogInfo("Export Dialogues Finished");
            }
        }

        private void OnExportLocalizationUnreal4(object sender, EventArgs e)
        {
            if (ResourcesHandler.Project != null)
            {
                if (ExporterLocalization.ExportToUnreal4())
                    EditorCore.LogInfo("Export Localization Finished");
            }
        }

        private void OnExportVoicing(object sender, EventArgs e)
        {
            if (ResourcesHandler.Project != null)
            {
                if (ExporterVoicing.ExportAll())
                {
                    RefreshDirtyFlags();

                    EditorCore.LogInfo("Export Voicing Finished");
                }
            }
        }

        private void OnExportStats(object sender, EventArgs e)
        {
            if (ResourcesHandler.Project != null)
            {
                if (ExporterStats.ExportAll())
                    EditorCore.LogInfo("Export Stats Finished");
            }
        }

        private void OnExportLipsyncFaceFX(object sender, EventArgs e)
        {
            if (ResourcesHandler.Project != null)
            {
                if (ExporterLipsync.ExportFaceFx())
                    EditorCore.LogInfo("Export Face FX Finished");
            }
        }

        private void OnShowStats(object sender, EventArgs e)
        {
            if (dockPanel.ActiveDocument is DocumentDialogue)
            {
                var document = dockPanel.ActiveDocument as DocumentDialogue;

                DialogStats dialog = new DialogStats(new List<Dialogue>() { document.Dialogue }, EditorHelper.CurrentLanguage);
                dialog.ShowDialog();
            }
        }

        private void OnShowAllStats(object sender, EventArgs e)
        {
            DialogStats dialog = new DialogStats(ResourcesHandler.GetAllDialogues());
            dialog.ShowDialog();
        }

        private void OnActiveDocumentChanged(object sender, EventArgs e)
        {
            if (dockPanel.ActiveDocument is IDocument)
            {
                var document = dockPanel.ActiveDocument as IDocument;
                document.RefreshDocument();
            }
        }

        private void OnPlayDialogue(object sender, EventArgs e)
        {
            if (dockPanel.ActiveDocument is DocumentDialogue)
            {
                var document = dockPanel.ActiveDocument as DocumentDialogue;
                var viewer = new WindowViewer(document.Dialogue, null, document);
                viewer.ShowDialog(this);
            }
        }

        private void OnPlayDialogueNode(object sender, EventArgs e)
        {
            if (dockPanel.ActiveDocument is DocumentDialogue)
            {
                var document = dockPanel.ActiveDocument as DocumentDialogue;
                var viewer = new WindowViewer(document.Dialogue, document.GetSelectedDialogueNode(), document);
                viewer.ShowDialog(this);
            }
        }

        private void OnSaveFile(object sender, EventArgs e)
        {
            if (dockPanel.ActiveDocument is DocumentProject)
            {
                ResourcesHandler.SaveProject();
                documentProject.RefreshTitle();
            }
            else if (dockPanel.ActiveDocument is DocumentDialogue)
            {
                var document = dockPanel.ActiveDocument as DocumentDialogue;

                document.ResolvePendingDirty();
                ResourcesHandler.SaveDialogue(document.Dialogue);
                document.OnPostSave();
                document.RefreshTitle();
            }
        }

        private void OnSaveAllFiles(object sender, EventArgs e)
        {
            if (documentProject != null)
            {
                if (ResourcesHandler.Project.Dirty)
                {
                    ResourcesHandler.SaveProject();
                    documentProject.RefreshTitle();
                }
            }

            foreach (DocumentDialogue document in documentDialogues)
            {
                var documentDialogue = document as DocumentDialogue;
                documentDialogue.ResolvePendingDirty();
                if (ResourcesHandler.IsDirty(documentDialogue.Dialogue))
                {
                    ResourcesHandler.SaveDialogue(documentDialogue.Dialogue);
                    documentDialogue.OnPostSave();
                    documentDialogue.RefreshTitle();
                }
            }
        }

        private void OnForceSaveAll(object sender, EventArgs e)
        {
            if (ResourcesHandler.Project == null)
                return;

            foreach (DocumentDialogue document in documentDialogues)
            {
                document.ResolvePendingDirty();
            }

            EditorCore.LogInfo("Checking all Dialogues - Begin");

            ResourcesHandler.CheckAll();

            EditorCore.LogInfo("Checking all Dialogues - End");

            ResourcesHandler.SaveAll();

            foreach (DocumentDialogue document in documentDialogues)
            {
                document.OnPostSave();
            }

            RefreshDirtyFlags();

            EditorCore.LogInfo("All Project Files Saved");
        }

        private void OnCheckAll(object sender, EventArgs e)
        {
            if (ResourcesHandler.Project == null)
                return;

            EditorCore.LogInfo("Checking all Dialogues - Begin");

            ResourcesHandler.CheckAll();

            EditorCore.LogInfo("Checking all Dialogues - End");
        }

        private void OnCheckCurrent(object sender, EventArgs e)
        {
            if (ResourcesHandler.Project == null)
                return;

            if (dockPanel.ActiveDocument is DocumentDialogue)
            {
                EditorCore.LogInfo("Checking Dialogue - Begin");

                var dialogue = dockPanel.ActiveDocument as DocumentDialogue;
                ResourcesHandler.Check(dialogue.Dialogue);

                EditorCore.LogInfo("Checking Dialogue - End");
            }
        }

        private void OnReloadFile(object sender, EventArgs e)
        {
            if (ResourcesHandler.Project == null)
                return;

            if (dockPanel.ActiveDocument is DocumentProject && ResourcesHandler.Project.Dirty)
            {
                var dialog = new DialogConfirmReload(ResourcesHandler.Project, new List<Dialogue>());
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ResourcesHandler.ReloadProject();
                    documentProject.OnPostReload();

                    EditorCore.LogInfo("Reloaded project file");
                }
            }
            else if (dockPanel.ActiveDocument is DocumentDialogue)
            {
                var document = dockPanel.ActiveDocument as DocumentDialogue;
                document.ResolvePendingDirty();
                if (ResourcesHandler.IsDirty(document.Dialogue))
                {
                    var dialog = new DialogConfirmReload(null, new List<Dialogue>() { document.Dialogue });
                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        ResourcesHandler.ReloadDialogue(document.Dialogue);
                        document.OnPostReload();

                        EditorCore.LogInfo("Reloaded current dialogue file");
                    }
                }
            }
        }

        private void OnReloadAllFiles(object sender, EventArgs e)
        {
            if (ResourcesHandler.Project == null)
                return;

            Project project = null;
            var dialogues = new List<Dialogue>();

            if (documentProject != null && ResourcesHandler.Project.Dirty)
            {
                project = ResourcesHandler.Project;
            }

            if (documentDialogues.Count > 0)
            {
                foreach (DocumentDialogue document in documentDialogues)
                {
                    var documentDialogue = document as DocumentDialogue;
                    documentDialogue.ResolvePendingDirty();
                    if (ResourcesHandler.IsDirty(documentDialogue.Dialogue))
                        dialogues.Add(documentDialogue.Dialogue);
                }
            }

            bool reloadOK = true;
            if (project != null || dialogues.Count > 0)
            {
                var dialog = new DialogConfirmReload(project, dialogues);
                DialogResult result = dialog.ShowDialog();
                reloadOK = (result == DialogResult.OK);
            }

            if (reloadOK)
            {
                ResourcesHandler.ReloadAll();

                if (documentProject != null)
                {
                    documentProject.OnPostReload();
                }

                foreach (DocumentDialogue document in documentDialogues)
                {
                    var documentDialogue = document as DocumentDialogue;
                    documentDialogue.OnPostReload();
                }

                EditorCore.LogInfo("Reloaded all project files");
            }
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
            if (ResourcesHandler.Project == null)
                return;

            OpenDocumentProject();
        }

        private void OnReplaceActor(object sender, EventArgs e)
        {
            if (ResourcesHandler.Project == null)
                return;

            if (dockPanel.ActiveDocument is DocumentDialogue)
            {
                var document = dockPanel.ActiveDocument as DocumentDialogue;

                var dialog = new DialogReplaceActor();
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (dialog.ActorIDFrom != "" && dialog.ActorIDTo != "")
                    {
                        if (document.Dialogue.UpdateActorID(dialog.ActorIDFrom, dialog.ActorIDTo))
                        {
                            document.RefreshAllTreeNodes();
                            document.ResyncSelectedNode();
                            document.SetDirty();
                        }
                    }
                }
            }
        }

        private void OnExportConstantsUnreal4(object sender, EventArgs e)
        {
            if (ResourcesHandler.Project != null)
            {
                if (ExporterConstants.ExportToUnreal4())
                    EditorCore.LogInfo("Export Constants Finished");
            }
        }

        public DocumentDialogue GetActiveDocument()
        {
            if (dockPanel.ActiveDocument is DocumentDialogue)
            {
                var document = dockPanel.ActiveDocument as DocumentDialogue;
                if (document != null)
                    return document;
            }

            return null;
        }
    }
}
