using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogueEditor
{
    static public class ExporterLipsync
    {
        //--------------------------------------------------------------------------------------------------------------
        // Exports

        static public bool ExportFaceFx()
        {
            var project = ResourcesHandler.Project;

            string projectDirectory = EditorHelper.GetProjectDirectory();
            string exportDirectory = Path.Combine(projectDirectory, EditorCore.Settings.DirectoryExportLipsync);

            var dialog = new DialogExport("Export FaceFX",
                                            exportDirectory,
                                            true, false,
                                            false, false,
                                            false, DateTime.MinValue);
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.Cancel)
                return false;

            EditorCore.Settings.DirectoryExportLipsync = Utility.GetRelativePath(dialog.ExportPath, projectDirectory);

            exportDirectory = dialog.ExportPath;
            if (dialog.UseDateDirectory)
                exportDirectory = Path.Combine(exportDirectory, Utility.GetDateTimeAsString(Utility.GetCurrentTime()));

            if (!Directory.Exists(exportDirectory))
                Directory.CreateDirectory(exportDirectory);

            var packageGroups = new List<List<Package>>();
            if (dialog.UsePackagesDirectory)
            {
                foreach (var package in dialog.ListPackages)
                    packageGroups.Add(new List<Package>() { package });
            }
            else
            {
                packageGroups.Add(dialog.ListPackages);
            }

            ExporterStats.ProjectStats projectStats = new ExporterStats.ProjectStats();

            var listLanguages = new List<Language>() { EditorCore.LanguageWorkstring };
            if (!dialog.WorkstringOnly)
                listLanguages = dialog.ListLanguages;

            foreach (var packageGroup in packageGroups)     //Either a list of individual packages, or 1 entry with all the packages
            {
                var packageDirectory = exportDirectory;
                if (dialog.UsePackagesDirectory)
                    packageDirectory = Path.Combine(packageDirectory, packageGroup[0].Name);

                if (!Directory.Exists(packageDirectory))
                    Directory.CreateDirectory(packageDirectory);

                var dialogues = dialog.SelectedDialogues;
                if (!dialog.UseCustomSelection)
                {
                    dialogues = ResourcesHandler.GetAllDialoguesFromPackages(packageGroup);
                }

                ExportFaceFx_Internal(packageDirectory, project, dialogues, listLanguages, dialog.WorkstringOnly);
            }

            System.Diagnostics.Process.Start(exportDirectory);

            return true;
        }

        static public void ExportFaceFx_Internal(string directory, Project project, List<Dialogue> dialogues, List<Language> languages, bool workstringOnly)
        {
            foreach (Language language in languages)
            {
                string languagePath = Path.Combine(directory, language.LocalizationCode);
                foreach (Dialogue dialogue in dialogues)
                {
                    var orderedListNodes = new List<DialogueNode>();
                    dialogue.GetOrderedNodes(ref orderedListNodes);
                    foreach (DialogueNode dialogueNode in orderedListNodes)
                    {
                        if (dialogueNode is DialogueNodeSentence || dialogueNode is DialogueNodeReply)
                        {
                            string nodeVoicingID = EditorHelper.GetPrettyNodeVoicingID(dialogue, dialogueNode);
                            string path = Path.Combine(languagePath, nodeVoicingID + ".txt");

                            string lineToWrite = "";
                            if (dialogueNode is DialogueNodeSentence)
                                lineToWrite = (dialogueNode as DialogueNodeSentence).Sentence;
                            else
                                lineToWrite = (dialogueNode as DialogueNodeReply).Reply;

                            if (!workstringOnly)
                            {
                                var entry = dialogue.Translations.GetNodeEntry(dialogueNode, language);
                                lineToWrite = (entry != null) ? entry.Text : "";
                            }

                            //Export empy files
                            //if (lineToWrite.Count() == 0)
                            //    continue;

                            lineToWrite = EditorHelper.FormatTextEntry(lineToWrite, language);

                            if (!Directory.Exists(languagePath))
                                Directory.CreateDirectory(languagePath);

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false, Encoding.UTF8))
                            {
                                file.WriteLine(lineToWrite);
                            }
                        }
                    }
                }
            }
        }
    }
}
