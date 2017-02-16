using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace DialogueEditor
{
    static public class ExporterVoicing
    {
        //--------------------------------------------------------------------------------------------------------------
        // Exports

        static public bool ExportAll()
        {
            Project project = ResourcesHandler.Project;

            string projectDirectory = EditorHelper.GetProjectDirectory();
            string exportDirectory = Path.Combine(projectDirectory, EditorCore.Settings.DirectoryExportVoicing);

            var dialog = new DialogExport("Export Voicing",
                                            exportDirectory,
                                            true, false,
                                            false, true,
                                            true, project.LastVoicingExportDate);
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.Cancel)
                return false;

            EditorCore.Settings.DirectoryExportVoicing = Utility.GetRelativePath(dialog.ExportPath, projectDirectory);

            DateTime dateNow = Utility.GetCurrentTime();
            project.LastVoicingExportDate = dateNow;
            project.Dirty = true;
            
            exportDirectory = dialog.ExportPath;
            if (dialog.UseDateDirectory)
                exportDirectory = Path.Combine(exportDirectory, Utility.GetDateAsString(dateNow));

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

            var listLanguages = new List<Language>() { EditorCore.LanguageWorkstring };
            if (!dialog.WorkstringOnly)
                listLanguages = dialog.ListLanguages;

            ExporterStats.ProjectStats projectStats = new ExporterStats.ProjectStats();

            foreach (var packageGroup in packageGroups)     //Either a list of individual packages, or 1 entry with all the packages
            {
                var packageDirectory = exportDirectory;
                if (dialog.UsePackagesDirectory)
                    packageDirectory = Path.Combine(packageDirectory, packageGroup[0].Name);

                var dialogues = dialog.SelectedDialogues;
                if (!dialog.UseCustomSelection)
                {
                    dialogues = ResourcesHandler.GetAllDialoguesFromPackages(packageGroup);
                }

                var dateFrom = dialog.DateFrom;

                foreach (var language in listLanguages)
                {
                    string languageDirectory = Path.Combine(packageDirectory, language.VoicingCode);

                    if (!Directory.Exists(languageDirectory))
                        Directory.CreateDirectory(languageDirectory);

                    ExportLocasToCSVFile(languageDirectory, project, dialogues, language, dialog.WorkstringOnly, dialog.WorkstringFallback, dateFrom, projectStats);
                    ExportDialoguesToCSVFile(languageDirectory, project, dialogues, language, dateFrom);
                    ExportActorsToCSVFile(languageDirectory, project, dialogues, language, dateFrom);
                    ExportDialoguesToWwiseTextFile(languageDirectory, project, dialogues, language, dateFrom);
                }
            }

            ExporterStats.Export("TodoVoicingCount", projectStats, exportDirectory, dialog.ListPackages, listLanguages, dialog.WorkstringOnly, dialog.WorkstringFallback);

            System.Diagnostics.Process.Start(exportDirectory);

            return true;
        }

        static public void ExportLocasToCSVFile(string directory, Project project, List<Dialogue> dialogues, Language language, bool workstringOnly, bool workstringFallback, DateTime dateFrom, ExporterStats.ProjectStats projectStats)
        {
            string path = Path.Combine(directory, "Loca_" + project.GetName() + "_" + language.VoicingCode + ".csv");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false, Encoding.UTF8))
            {
                //status, scene, voicing id, index, speaker, context, voicing comments, intensity, sentence, comments
                {
                    ExporterCsv.CsvLineWriter header = new ExporterCsv.CsvLineWriter();
                    header.AddField("Status");
                    header.AddField("Dialogue");
                    header.AddField("Voicing ID");
                    header.AddField("Index");
                    header.AddField("Speaker");
                    header.AddField("Context");
                    header.AddField("Voicing Comments");
                    header.AddField("Voicing Intensity");
                    header.AddField("Sentence");
                    header.AddField("Comments");
                    header.WriteLine(file);
                }

                foreach (Dialogue dialogue in dialogues)
                {
                    int index = 1;

                    var orderedListNodes = new List<DialogueNode>();
                    dialogue.GetOrderedNodes(ref orderedListNodes);
                    foreach (DialogueNode dialogueNode in orderedListNodes)
                    {
                        if (dialogueNode is DialogueNodeSentence)
                        {
                            DialogueNodeSentence dialogueNodeSentence = dialogueNode as DialogueNodeSentence;

                            bool done = false;
                            if (dialogueNodeSentence.LastEditDate < dateFrom)
                                done = true;

                            string nodeID = EditorHelper.GetPrettyNodeVoicingID(dialogue, dialogueNodeSentence);
                            string speaker = project.GetActorName(dialogueNodeSentence.SpeakerID);
                            string scene = dialogue.GetName();
                            string context = dialogueNodeSentence.Context;
                            string comment = dialogueNodeSentence.Comment;
                            string intensity = dialogueNodeSentence.VoiceIntensity;
                            string status = (done) ? "OK" : "TODO";

                            string voicedText = dialogueNodeSentence.Sentence;
                            if (!workstringOnly)
                            {
                                TranslationEntry entry = dialogue.Translations.GetNodeEntry(dialogueNode, language);
                                if (entry == null)
                                {
                                    if (!workstringFallback)
                                        voicedText = "<Missing Translation>";
                                }
                                else
                                {
                                    voicedText = entry.Text;
                                }
                            }

                            // use null as langage to force constant replacement in csv file
                            voicedText = EditorHelper.FormatTextEntry(voicedText, null);  //language = workstring if workstringOnly

                            //voicedText = voicedText.Replace("’", "'");
                            //voicedText = voicedText.Replace("…", "...");

                            if (speaker == "")
                                speaker = "<Undefined>";

                            //status, scene, node id, index, speaker, context, voicing comments, intensity, sentence, comments
                            ExporterCsv.CsvLineWriter line = new ExporterCsv.CsvLineWriter();
                            line.AddField(status);
                            line.AddField(scene);
                            line.AddField(nodeID);
                            line.AddField(index);
                            line.AddField(speaker);
                            line.AddField(context);
                            line.AddField(comment);
                            line.AddField(intensity);
                            line.AddField(voicedText);
                            line.AddEmptyField();
                            line.WriteLine(file);

                            ++index;

                            if (!done)
                            {
                                projectStats.AddSentence(dialogue, dialogueNodeSentence.SpeakerID, voicedText);
                            }
                        }
                    }
                }
            }
        }

        static public void ExportDialoguesToCSVFile(string directory, Project project, List<Dialogue> dialogues, Language language, DateTime dateFrom)
        {
            string path = Path.Combine(directory, "Dialogues_" + project.GetName() + "_" + language.VoicingCode + ".csv");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false, Encoding.UTF8))
            {
                //Scene, Scene Type, Voicing Context, Voicing Comments, Comments
                {
                    ExporterCsv.CsvLineWriter header = new ExporterCsv.CsvLineWriter();
                    header.AddField("Dialogue");
                    header.AddField("Scene Type");
                    header.AddField("Context");
                    header.AddField("Comments");
                    header.WriteLine(file);
                }

                foreach (Dialogue dialogue in dialogues)
                {
                    string name = dialogue.GetName();
                    string context = dialogue.Context;
                    string sceneType = dialogue.SceneType;

                    //Scene, Scene Type, Voicing Context, Voicing Comments, Comments
                    ExporterCsv.CsvLineWriter line = new ExporterCsv.CsvLineWriter();
                    line.AddField(name);
                    line.AddField(sceneType);
                    line.AddField(context);
                    line.AddEmptyField();
                    line.WriteLine(file);
                }
            }
        }

        static public void ExportActorsToCSVFile(string directory, Project project, List<Dialogue> dialogues, Language language, DateTime dateFrom)
        {
            string path = Path.Combine(directory, "Actors_" + project.GetName() + "_" + language.VoicingCode + ".csv");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false, Encoding.UTF8))
            {
                //Name, Voice Actor, Species, Gender, Build, Age, Height, Personality, Voice Kit, ID
                {
                    ExporterCsv.CsvLineWriter header = new ExporterCsv.CsvLineWriter();
                    header.AddField("Name");
                    header.AddField("Voice Actor");
                    header.AddField("Species");
                    header.AddField("Gender");
                    header.AddField("Build");
                    header.AddField("Age");
                    header.AddField("Height");
                    header.AddField("Personality");
                    header.AddField("Voice Kit");
                    header.AddField("Actor ID");
                    header.WriteLine(file);
                }

                foreach (Actor actor in project.ListActors)
                {
                    string actorID = actor.ID;
                    string name = actor.Name;
                    string voiceActor = project.GetLocalizedVoiceActorFromKit(actor.VoiceKit, language);
                    string voiceKit = actor.VoiceKit;
                    string species = actor.Species;
                    string gender = actor.Gender;
                    string build = actor.Build;
                    string age = actor.Age.ToString();
                    string height = actor.Height.ToString();
                    string personality = actor.Personality;

                    if (voiceActor == "")
                        voiceActor = project.GetVoiceActorNameFromKit(actor.VoiceKit);

                    //Name, Voice Actor, Species, Gender, Build, Age, Height, Personality, Voice Kit, ID
                    ExporterCsv.CsvLineWriter line = new ExporterCsv.CsvLineWriter();
                    line.AddField(name);
                    line.AddField(voiceActor);
                    line.AddField(species);
                    line.AddField(gender);
                    line.AddField(build);
                    line.AddField(age);
                    line.AddField(height);
                    line.AddField(personality);
                    line.AddField(voiceKit);
                    line.AddField(actorID);
                    line.WriteLine(file);
                }
            }
        }

        static public void ExportDialoguesToWwiseTextFile(string directory, Project project, List<Dialogue> dialogues, Language language, DateTime dateFrom)
        {
            string path = Path.Combine(directory, "Wwise_" + project.GetName() + "_" + language.VoicingCode + ".txt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false, Encoding.Unicode))
            {
                file.WriteLine("Filename\tContainer\tSVComment\tComment");

                foreach (Dialogue dialogue in dialogues)
                {
                    var orderedListNodes = new List<DialogueNode>();
                    dialogue.GetOrderedNodes(ref orderedListNodes);
                    foreach (DialogueNode dialogueNode in orderedListNodes)
                    {
                        if (dialogueNode is DialogueNodeSentence)
                        {
                            DialogueNodeSentence dialogueNodeSentence = dialogueNode as DialogueNodeSentence;
                            //if (dialogueNodeSentence.LastEditDate < dateFrom)   //No need for that, reexport everything
                            //    continue;

                            string nodeID = EditorHelper.GetPrettyNodeVoicingID(dialogue, dialogueNodeSentence);
                            string container = dialogue.SceneType;
                            string speaker = project.GetActorName(dialogueNodeSentence.SpeakerID);
                            string voicedText = EditorHelper.FormatTextEntry(dialogueNodeSentence.Sentence, language);  //language = workstring if workstringOnly

                            if (speaker == "")
                                speaker = "Undefined";

                            //Filename, Container, SVComment, Comment
                            file.WriteLine(nodeID + "\t" + container + "\t" + speaker + "\t" + voicedText);
                        }
                    }
                }
            }
        }

    }
}
