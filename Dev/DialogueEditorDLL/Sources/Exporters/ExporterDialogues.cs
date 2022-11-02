using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogueEditor
{
    static public class ExporterDialogues
    {
        //--------------------------------------------------------------------------------------------------------------
        // Exports

        static public bool ExportToCsv()
        {
            var project = ResourcesHandler.Project;

            string projectDirectory = EditorHelper.GetProjectDirectory();
            string exportDirectory = Path.Combine(projectDirectory, EditorCore.Settings.DirectoryExportDialogues);

            var dialog = new DialogExport("Export Dialogues",
                                            path: exportDirectory,
                                            defaultDateDirectory: true,
                                            defaultPackageDirectory: false,
                                            allowConstants: true,
                                            allowWorkstringFallback: false,
                                            allowDateFrom: true,
                                            dateFrom: DateTime.MinValue);

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.Cancel)
                return false;

            EditorCore.Settings.DirectoryExportDialogues = Utility.GetRelativePath(dialog.ExportPath, projectDirectory);

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

            //Ensure no export of a Workstring language
            var listLanguages = new List<Language>();   // { EditorCore.LanguageWorkstring };
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

                ExportDialoguesToCsv(packageDirectory, project, dialogues, listLanguages, dialog.DateFrom, projectStats);

                ExporterStats.Export("TodoTranslationsCount", projectStats, packageDirectory, packageGroup, listLanguages, dialog.WorkstringOnly, dialog.WorkstringFallback);
            }

            if (dialog.Constants)
            {
                ExportConstantsToCsv(exportDirectory, project, listLanguages);
            }

            System.Diagnostics.Process.Start(exportDirectory);

            return true;
        }

        static public void ExportConstantsToCsv(string directory, Project project, List<Language> languages)
        {
            //Ensure no export of a Workstring language
            //languages.Remove(EditorCore.LanguageWorkstring);

            string path = Path.Combine(directory, "Constants_" + project.GetName() + ".csv");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false, Encoding.UTF8))
            {
                //ID, Timestamp, Comment, Workstring, ...Languages...
                {
                    ExporterCsv.CsvLineWriter header = new ExporterCsv.CsvLineWriter();
                    header.AddField("ID");
                    header.AddField("Timestamp");
                    header.AddField("Comment");
                    header.AddField("Workstring");
                    foreach (var language in languages)
                    {
                        header.AddField(language.Name + " State");
                        header.AddField(language.Name + " Text");
                    }
                    header.WriteLine(file);
                }

                //file.Write("\n");

                foreach (var constant in project.ListConstants)
                {
                    ExporterCsv.CsvLineWriter line = new ExporterCsv.CsvLineWriter();
                    line.AddField(constant.ID);
                    line.AddField(constant.LastEditDate);
                    line.AddField(constant.Comment);
                    line.AddField(constant.Workstring);

                    foreach (var language in languages)
                    {
                        var entry = project.Translations.GetEntry(constant.ID, language);
                        if (entry != null)
                        {
                            if (entry.SourceDate < constant.LastEditDate)
                            {
                                line.AddField("UPDATE");
                            }
                            else
                            {
                                line.AddField("OK");
                            }
                            line.AddField(entry.Text);
                        }
                        else
                        {
                            line.AddField("NEW");
                            line.AddEmptyField();
                        }
                    }

                    line.WriteLine(file);
                }
            }
        }

        static public void ExportDialoguesToCsv(string directory, Project project, List<Dialogue> dialogues, List<Language> languages, DateTime dateFrom, ExporterStats.ProjectStats projectStats)
        {
            //Ensure no export of a Workstring language
            //languages.Remove(EditorCore.LanguageWorkstring);

            string path = Path.Combine(directory, "Loca_" + project.GetName() + ".csv");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false, Encoding.UTF8))
            {
                //Dialogue, Node ID, Timestamp, Voicing ID, Index, Package, SceneType, Context, Voicing, Voice Intensity, Speaker, Workstring Text, Words, ...Languages..., comments
                {
                    ExporterCsv.CsvLineWriter header = new ExporterCsv.CsvLineWriter();
                    header.AddField("Dialogue");
                    header.AddField("Node ID");
                    header.AddField("Timestamp");
                    header.AddField("Voicing ID");
                    header.AddField("Index");
                    header.AddField("Package");
                    header.AddField("Scene Type");
                    header.AddField("Context");
                    header.AddField("Voicing");
                    header.AddField("Voice Intensity");
                    header.AddField("Speaker");
                    header.AddField("Workstring Text");
                    //TODO: old workstring for updated texts ?
                    header.AddField("Words");
                    //TODO: characters count ?
                    foreach (var language in languages)
                    {
                        header.AddField(language.Name + " State");
                        header.AddField(language.Name + " Text");
                        header.AddField(language.Name + " Words");
                        //TODO: characters count ?
                    }
                    header.AddField("Comments");
                    header.WriteLine(file);
                }

                //file.Write("\n");

                foreach (Dialogue dialogue in dialogues)
                {
                    int index = 0;

                    var orderedListNodes = new List<DialogueNode>();
                    dialogue.GetOrderedNodes(ref orderedListNodes);

                    //Process global Dialogue information
                    {
                        string package = dialogue.Package.Name;
                        string sceneType = dialogue.SceneType;
                        string context = dialogue.Context;

                        ExporterCsv.CsvLineWriter line = new ExporterCsv.CsvLineWriter();
                        //Dialogue, Node ID, Timestamp, Voicing ID, Index, Package, SceneType, Context, Voicing, Voicing Intensity, Speaker, Workstring Text, Words, ...Languages..., comments
                        line.AddField(dialogue.GetName());
                        line.AddField(TranslationTable.DialogueNodePrefix + dialogue.RootNodeID);
                        line.AddEmptyField();
                        line.AddEmptyField();
                        line.AddField(index);
                        line.AddField(package);
                        line.AddField(sceneType);
                        line.AddField(context);
                        line.AddEmptyField();
                        line.AddEmptyField();
                        line.AddEmptyField();
                        line.AddEmptyField();
                        line.AddEmptyField();

                        //Locas
                        line.AddEmptyField();
                        line.AddEmptyField();
                        line.AddEmptyField();
                        line.AddEmptyField();

                        line.WriteLine(file);
                    }

                    foreach (DialogueNode dialogueNode in orderedListNodes)
                    {
                        if (dialogueNode is DialogueNodeSentence || dialogueNode is DialogueNodeReply)
                        {
                            ++index;

                            string context = "";
                            string speaker = "";
                            string workstring = "";
                            DateTime workstringTimestamp;
                            bool todo = false;
                            string voicingID;
                            string voicing;
                            string voiceIntensity;

                            if (dialogueNode is DialogueNodeSentence)
                            {
                                var dialogueNodeSentence = dialogueNode as DialogueNodeSentence;

                                context = dialogueNodeSentence.Context;
                                workstring = dialogueNodeSentence.Sentence;
                                workstringTimestamp = dialogueNodeSentence.LastEditDate;

                                speaker = project.GetActorName(dialogueNodeSentence.SpeakerID);
                                if (speaker == "")
                                    speaker = "<Undefined>";

                                voicingID = EditorHelper.GetPrettyNodeVoicingID(dialogue, dialogueNodeSentence);
                                voicing = dialogueNodeSentence.Comment;
                                voiceIntensity = dialogueNodeSentence.VoiceIntensity;
                            }
                            else
                            {
                                var dialogueNodeReply = dialogueNode as DialogueNodeReply;

                                context = "<User Interface>";
                                speaker = "<UI>";
                                workstring = dialogueNodeReply.Reply;
                                workstringTimestamp = dialogueNodeReply.LastEditDate;
                                voicingID = "";
                                voicing = "";
                                voiceIntensity = "";
                            }

                            string[] split = workstring.Split(ExporterStats.DelimiterChars, StringSplitOptions.RemoveEmptyEntries);
                            int words = split.Length;

                            //Dialogue, Node ID, Timestamp, Voicing ID, Index, Package, SceneType, Context, Voicing, Voicing Intensity, Speaker, Workstring Text, Words, ...Languages..., comments
                            ExporterCsv.CsvLineWriter line = new ExporterCsv.CsvLineWriter();
                            line.AddField(dialogue.GetName());
                            line.AddField(TranslationTable.DialogueNodePrefix + dialogueNode.ID);
                            line.AddField(workstringTimestamp);
                            line.AddField(voicingID);
                            line.AddField(index);
                            line.AddEmptyField();
                            line.AddEmptyField();
                            line.AddField(context);
                            line.AddField(voicing);
                            line.AddField(voiceIntensity);
                            line.AddField(speaker);
                            line.AddField(workstring);
                            line.AddField(words);

                            foreach (var language in languages)
                            {
                                var entry = dialogue.Translations.GetNodeEntry(dialogueNode, language);
                                if (entry != null)
                                {
                                    if (entry.SourceDate < workstringTimestamp)
                                    {
                                        todo = true;
                                        line.AddField("UPDATE");
                                    }
                                    else
                                    {
                                        line.AddField("OK");
                                    }
                                    line.AddField(entry.Text);
                                    line.AddEmptyField();
                                }
                                else
                                {
                                    todo = true;
                                    line.AddField("NEW");
                                    line.AddEmptyField();
                                    line.AddEmptyField();
                                }
                            }

                            line.AddEmptyField();
                            line.WriteLine(file);

                            if (todo)
                            {
                                if (dialogueNode is DialogueNodeSentence)
                                {
                                    var dialogueNodeSentence = dialogueNode as DialogueNodeSentence;
                                    projectStats.AddSentence(dialogue, dialogueNodeSentence.SpeakerID, workstring);
                                }
                                else
                                {
                                    var dialogueNodeReply = dialogueNode as DialogueNodeReply;
                                    projectStats.AddReply(dialogue, workstring);
                                }
                            }
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Imports

        static public bool ImportFromCsv()
        {
            var project = ResourcesHandler.Project;

            string projectDirectory = EditorHelper.GetProjectDirectory();
            string importPath = Path.Combine(projectDirectory, EditorCore.Settings.DirectoryExportDialogues);

            var dialog = new DialogImport(importPath);
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.Cancel)
                return false;

            bool resultConstants = false;
            if (dialog.ImportConstants)
            {
                resultConstants = ImportConstantsFromCsv(dialog.ImportPathConstants, dialog.ListLanguages, true, false, false);
                if (resultConstants)
                    EditorCore.LogInfo("Import Constants Finished");
                else
                    EditorCore.LogError("Import Constants Failed");
            }

            bool resultDialogues = false;
            if (dialog.ImportLocalization || dialog.ImportWorkstring || dialog.ImportInformation)
            {
                resultDialogues = ImportDialoguesFromCsv(dialog.ImportPathDialogues, dialog.ListPackages, dialog.ListLanguages, dialog.ImportLocalization, dialog.ImportWorkstring, dialog.ImportInformation);
                if (resultDialogues)
                    EditorCore.LogInfo("Import Dialogues Finished");
                else
                    EditorCore.LogError("Import Dialogues Failed");
            }

            return resultConstants || resultDialogues;
        }

        static public bool ImportConstantsFromCsv(string importPath, List<Language> languages, bool importLocalization, bool importWorkstring, bool importInformation)
        {
            var project = ResourcesHandler.Project;

            if (importPath == String.Empty)
            {
                EditorCore.LogError("Import Constants failed : no file specified");
                return false;
            }

            if (!File.Exists(importPath))
            {
                EditorCore.LogError("Import Constants failed : file not found");
                return false;
            }

            while (Utility.IsFileLocked(importPath))
            {
                EditorCore.LogError("Import Constants failed : file is locked");

                var dialogLocked = new DialogLockedFile(importPath);
                DialogResult eResult = dialogLocked.ShowDialog();
                if (eResult == DialogResult.Cancel)
                {
                    return false;
                }
            }

            DateTime currentTime = Utility.GetCurrentTime();

            using (System.IO.StreamReader file = new System.IO.StreamReader(importPath, Encoding.UTF8))
            {
                ExporterCsv.CsvFileReader reader = new ExporterCsv.CsvFileReader();
                if (reader.ParseHeaders(file))
                {
                    while (reader.ParseNextLine())
                    {
                        string id = reader.GetCell("ID");
                        Constant constant = project.ListConstants.Find(item => item.ID == id);
                        if (constant != null)
                        {
                            DateTime timestampLoca = reader.GetCellAsDate("Timestamp");
                            DateTime timestampWorkstring;

                            if (importWorkstring)
                            {
                                //The current workstring is more recent than the modified workstring
                                if (constant.LastEditDate > timestampLoca)
                                {
                                    //TODO: popup or option to choose what to do here
                                    EditorCore.LogWarning(String.Format("Constant {0} - New workstring older than currently registered workstring, but updated anyway", id));
                                }

                                constant.Workstring = reader.GetCell("Workstring");
                                constant.LastEditDate = currentTime;
                                timestampLoca = currentTime;
                            }

                            if (importInformation)
                            {
                                string voicing = reader.GetCell("Comment");
                                constant.Comment = voicing;
                            }

                            timestampWorkstring = constant.LastEditDate;

                            if (importLocalization)
                            {
                                foreach (var language in languages)
                                {
                                    //TODO: extract this part in a function, to be used from other importers

                                    ETranslationResult translationResult = project.Translations.AddOrUpdateEntry(
                                        id,
                                        language,
                                        timestampLoca,
                                        reader.GetCell(language.Name + " Text")
                                    );

                                    if (translationResult == ETranslationResult.Accepted
                                        || translationResult == ETranslationResult.Accepted_IdenticalTimestamp
                                        || translationResult == ETranslationResult.Accepted_IdenticalText)
                                    {
                                        //The current workstring is more recent than the localized entry
                                        if (timestampWorkstring > timestampLoca)
                                        {
                                            EditorCore.LogWarning(String.Format("Constant {0} - [{1}] Translation accepted but based on an outdated workstring, will be re-exported", id, language.Name));
                                        }
                                        else
                                        {
                                            //EditorCore.LogInfo(String.Format("{0} {1} - Translation accepted", dialogue.GetName(), id), dialogue.GetName(), id);
                                        }
                                    }

                                    if (translationResult == ETranslationResult.Accepted_IdenticalTimestamp)
                                    {
                                        EditorCore.LogWarning(String.Format("Constant {0} - [{1}] Translation accepted but with an identical timestamp as the previous entry", id, language.Name));
                                    }
                                    else if (translationResult == ETranslationResult.Accepted_IdenticalText)
                                    {
                                        EditorCore.LogWarning(String.Format("Constant {0} - [{1}] Translation accepted but with an identical text as the previous entry", id, language.Name));
                                    }
                                    else if (translationResult == ETranslationResult.Refused_EmptyText)
                                    {
                                        EditorCore.LogWarning(String.Format("Constant {0} - [{1}] Translation refused : empty text", id, language.Name));
                                    }
                                    else if (translationResult == ETranslationResult.Refused_Outdated)
                                    {
                                        EditorCore.LogWarning(String.Format("Constant {0} - [{1}] Translation refused : outdated timestamp", id, language.Name));
                                    }
                                    else if (translationResult == ETranslationResult.Refused_Identical)
                                    {
                                        //ignored
                                    }
                                }
                            }
                        }
                    }

                    ResourcesHandler.SaveProject();

                    if (EditorCore.MainWindow != null)
                        EditorCore.MainWindow.RefreshDirtyFlags();
                }
            }

            return true;
        }

        static public bool ImportDialoguesFromCsv(string importPath, List<Package> packages, List<Language> languages, bool importLocalization, bool importWorkstring, bool importInformation)
        {
            var project = ResourcesHandler.Project;

            //Dialogue, Node ID, Timestamp, Voicing ID, Index, Package, SceneType, Context, Voicing, Voicing Intensity, Speaker, Workstring Text, Words, ...Languages..., comments
            var headerRedirects = new Dictionary<string, string>();
            headerRedirects.Add("ID", "Node ID");
            headerRedirects.Add("Workstring", "Workstring Text");

            if (importPath == String.Empty)
            {
                EditorCore.LogError("Import Localization failed : no file specified");
                return false;
            }

            if (!File.Exists(importPath))
            {
                EditorCore.LogError("Import Localization failed : file not found");
                return false;
            }

            while (Utility.IsFileLocked(importPath))
            {
                EditorCore.LogError("Import Localization failed : file is locked");

                var dialogLocked = new DialogLockedFile(importPath);
                DialogResult eResult = dialogLocked.ShowDialog();
                if (eResult == DialogResult.Cancel)
                {
                    return false;
                }
            }

            DateTime currentTime = Utility.GetCurrentTime();

            using (System.IO.StreamReader file = new System.IO.StreamReader(importPath, Encoding.UTF8))
            {
                ExporterCsv.CsvFileReader reader = new ExporterCsv.CsvFileReader();
                if (reader.ParseHeaders(file, headerRedirects))
                {
                    while (reader.ParseNextLine())
                    {
                        Dialogue dialogue = ResourcesHandler.GetDialogue(reader.GetCell("Dialogue"));
                        if (dialogue != null)
                        {
                            if (!packages.Contains(dialogue.Package))
                                continue;

                            int id = TranslationTable.GetNodeIDFromPrefixedString(reader.GetCell("ID"));
                            DialogueNode node = dialogue.GetNodeByID(id);
                            if (node == null)
                                continue;

                            if (node is DialogueNodeRoot)
                            {
                                var dialogueNodeRoot = node as DialogueNodeRoot;

                                if (importInformation)
                                {
                                    //Import general Dialogue informations
                                    dialogue.Package = project.GetPackage(reader.GetCell("Package"));
                                    dialogue.SceneType = reader.GetCell("Scene Type");
                                    dialogue.Context = reader.GetCell("Context");
                                }
                            }
                            else
                            {
                                DateTime timestampLoca = reader.GetCellAsDate("Timestamp");
                                DateTime timestampWorkstring;

                                if (node is DialogueNodeSentence)
                                {
                                    var dialogueNodeSentence = node as DialogueNodeSentence;

                                    if (importWorkstring)
                                    {
                                        //The current workstring is more recent than the modified workstring
                                        if (dialogueNodeSentence.LastEditDate > timestampLoca)
                                        {
                                            //TODO: popup or option to choose what to do here
                                            EditorCore.LogWarning(String.Format("{0} {1} - New workstring older than currently registered workstring, but updated anyway", dialogue.GetName(), id), dialogue, node);
                                        }

                                        dialogueNodeSentence.Sentence = reader.GetCell("Workstring");
                                        dialogueNodeSentence.LastEditDate = currentTime;
                                        timestampLoca = currentTime;
                                    }

                                    if (importInformation)
                                    {
                                        //Import Sentence informations (Voicing, Context, Speaker..)
                                        string context = reader.GetCell("Context");
                                        dialogueNodeSentence.Context = context;

                                        string voicing = reader.GetCell("Voicing");
                                        dialogueNodeSentence.Comment = voicing;

                                        string voiceIntensity = reader.GetCell("Voice Intensity");
                                        dialogueNodeSentence.VoiceIntensity = voiceIntensity;

                                        string speakerID = ResourcesHandler.Project.GetActorID(reader.GetCell("Speaker"));
                                        dialogueNodeSentence.SpeakerID = speakerID;
                                    }

                                    timestampWorkstring = dialogueNodeSentence.LastEditDate;
                                }
                                else
                                {
                                    var dialogueNodeReply = node as DialogueNodeReply;

                                    if (importWorkstring)
                                    {
                                        //The current workstring is more recent than the modified workstring
                                        if (dialogueNodeReply.LastEditDate > timestampLoca)
                                        {
                                            //TODO: popup or option to choose what to do here
                                            EditorCore.LogWarning(String.Format("{0} {1} - New workstring older than currently registered workstring, but updated anyway", dialogue.GetName(), id), dialogue, node);
                                        }

                                        dialogueNodeReply.Reply = reader.GetCell("Workstring");
                                        dialogueNodeReply.LastEditDate = currentTime;
                                        timestampLoca = currentTime;
                                    }

                                    timestampWorkstring = dialogueNodeReply.LastEditDate;
                                }

                                if (importLocalization)
                                {
                                    foreach (var language in languages)
                                    {
                                        //TODO: extract this part in a function, to be used from other importers

                                        ETranslationResult translationResult = dialogue.Translations.AddOrUpdateNodeEntry(
                                            id,
                                            language,
                                            timestampLoca,
                                            reader.GetCell(language.Name + " Text")
                                        );

                                        if (translationResult == ETranslationResult.Accepted
                                            || translationResult == ETranslationResult.Accepted_IdenticalTimestamp
                                            || translationResult == ETranslationResult.Accepted_IdenticalText)
                                        {
                                            //The current workstring is more recent than the localized entry
                                            if (timestampWorkstring > timestampLoca)
                                            {
                                                EditorCore.LogWarning(String.Format("{0} {1} - [{2}] Translation accepted but based on an outdated workstring, will be re-exported", dialogue.GetName(), id, language.Name), dialogue, node);
                                            }
                                            else
                                            {
                                                //EditorCore.LogInfo(String.Format("{0} {1} - Translation accepted", dialogue.GetName(), id), dialogue.GetName(), id);
                                            }
                                        }

                                        if (translationResult == ETranslationResult.Accepted_IdenticalTimestamp)
                                        {
                                            EditorCore.LogWarning(String.Format("{0} {1} - [{2}] Translation accepted but with an identical timestamp as the previous entry", dialogue.GetName(), id, language.Name), dialogue, node);
                                        }
                                        else if (translationResult == ETranslationResult.Accepted_IdenticalText)
                                        {
                                            EditorCore.LogWarning(String.Format("{0} {1} - [{2}] Translation accepted but with an identical text as the previous entry", dialogue.GetName(), id, language.Name), dialogue, node);
                                        }
                                        else if (translationResult == ETranslationResult.Refused_EmptyText)
                                        {
                                            EditorCore.LogWarning(String.Format("{0} {1} - [{2}] Translation refused : empty text", dialogue.GetName(), id, language.Name), dialogue, node);
                                        }
                                        else if (translationResult == ETranslationResult.Refused_Outdated)
                                        {
                                            EditorCore.LogWarning(String.Format("{0} {1} - [{2}] Translation refused : outdated timestamp", dialogue.GetName(), id, language.Name), dialogue, node);
                                        }
                                        else if (translationResult == ETranslationResult.Refused_Identical)
                                        {
                                            //ignored
                                        }
                                    }
                                }
                            }
                        }

                        ResourcesHandler.SetDirty(dialogue);
                    }

                    ResourcesHandler.SaveAllDirty();

                    if (EditorCore.MainWindow != null)
                        EditorCore.MainWindow.RefreshDirtyFlags();

                    EditorCore.ProjectExplorer.ResyncAllFiles();
                }
            }
            return true;
        }
    }
}
