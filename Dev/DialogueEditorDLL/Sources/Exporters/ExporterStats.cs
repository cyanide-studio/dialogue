using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogueEditor
{
    static public class ExporterStats
    {
        //--------------------------------------------------------------------------------------------------------------
        // Public vars

        static public char[] DelimiterChars = { ' ', ',', '.', ':', ';', '-', '!', '?', '\t', '\r', '\n' };

        //--------------------------------------------------------------------------------------------------------------
        // Helper Class

        public class ProjectStats
        {
            public List<Dialogue> RefDialogues = new List<Dialogue>();

            public int SentenceWords = 0;
            public int Sentences = 0;
            public int ReplyWords = 0;
            public int Replies = 0;

            public Dictionary<string, DialogueStats> Dialogues = new Dictionary<string, DialogueStats>();
            public Dictionary<string, ActorStats> Actors = new Dictionary<string, ActorStats>();
            public HashSet<Package> Packages = new HashSet<Package>();

            public void AddSentence(Dialogue dialogue, string actorID, string text)
            {
                DialogueStats dialogueStats = Dialogues.GetOrAdd(dialogue.GetName(), (key) => new DialogueStats());
                ActorStats actorStats = Actors.GetOrAdd(actorID, (key) => new ActorStats());
                Packages.Add(dialogue.Package);

                string[] split = text.Split(DelimiterChars, StringSplitOptions.RemoveEmptyEntries);
                int words = split.Length;

                dialogueStats.SentenceWords += words;
                dialogueStats.Sentences += 1;
                SentenceWords += words;
                Sentences += 1;

                actorStats.Words += words;
                actorStats.Sentences += 1;
            }

            public void AddReply(Dialogue dialogue, string text)
            {
                DialogueStats dialogueStats = Dialogues.GetOrAdd(dialogue.GetName(), (key) => new DialogueStats());
                Packages.Add(dialogue.Package);

                string[] split = text.Split(DelimiterChars, StringSplitOptions.RemoveEmptyEntries);
                int words = split.Length;

                dialogueStats.ReplyWords += words;
                dialogueStats.Replies += 1;
                ReplyWords += words;
                Replies += 1;
            }
        }

        public class DialogueStats
        {
            public int SentenceWords = 0;
            public int Sentences = 0;
            public int ReplyWords = 0;
            public int Replies = 0;
        }

        public class ActorStats
        {
            public int Words = 0;
            public int Sentences = 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Exports

        static public bool ExportAll()
        {
            var project = ResourcesHandler.Project;

            string projectDirectory = EditorHelper.GetProjectDirectory();
            string exportDirectory = Path.Combine(projectDirectory, EditorCore.Settings.DirectoryExportStats);

            var dialog = new DialogExport("Export Stats",
                                            path: exportDirectory,
                                            defaultDateDirectory: true,
                                            defaultPackageDirectory: false,
                                            allowConstants: false,
                                            allowWorkstringFallback: true,
                                            allowDateFrom: true,
                                            dateFrom: DateTime.MinValue);

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.Cancel)
                return false;

            EditorCore.Settings.DirectoryExportStats = Utility.GetRelativePath(dialog.ExportPath, projectDirectory);

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

                var listLanguages = new List<Language>() { EditorCore.LanguageWorkstring };
                if (!dialog.WorkstringOnly)
                    listLanguages = dialog.ListLanguages;

                foreach (var language in listLanguages)
                {
                    string languageDirectory = Path.Combine(packageDirectory, language.Name);

                    if (!Directory.Exists(languageDirectory))
                        Directory.CreateDirectory(languageDirectory);

                    var projectStats = GatherStats(dialogues, language, dateFrom, dialog.WorkstringOnly, dialog.WorkstringFallback);

                    Export("WordCount", projectStats, languageDirectory, packageGroup, language, dialog.WorkstringOnly, dialog.WorkstringFallback);
                }
            }

            System.Diagnostics.Process.Start(exportDirectory);

            return true;
        }

        public static ProjectStats GatherStats(List<Dialogue> dialogues, Language language, DateTime dateFrom, bool workstringOnly, bool workstringFallback)
        {
            var projectStats = new ProjectStats();

            projectStats.RefDialogues = dialogues;

            foreach (Dialogue dialogue in dialogues)
            {
                foreach (DialogueNode dialogueNode in dialogue.ListNodes)   //No need for GetOrderedNodes here
                {
                    if (dialogueNode is DialogueNodeSentence)
                    {
                        var nodeSentence = (dialogueNode as DialogueNodeSentence);
                        if (nodeSentence.LastEditDate < dateFrom)
                            continue;

                        string text = nodeSentence.Sentence;
                        if (!workstringOnly)
                        {
                            TranslationEntry entry = dialogue.Translations.GetNodeEntry(dialogueNode, language);
                            if (entry == null)
                            {
                                if (!workstringFallback)
                                    continue;
                            }
                            else
                            {
                                text = entry.Text;
                            }
                        }

                        projectStats.AddSentence(dialogue, nodeSentence.SpeakerID, text);
                    }
                    else if (dialogueNode is DialogueNodeReply)
                    {
                        var nodeReply = (dialogueNode as DialogueNodeReply);
                        if (nodeReply.LastEditDate < dateFrom)
                            continue;

                        string text = nodeReply.Reply;
                        if (!workstringOnly)
                        {
                            TranslationEntry entry = dialogue.Translations.GetNodeEntry(dialogueNode, language);
                            if (entry == null)
                            {
                                if (!workstringFallback)
                                    continue;
                            }
                            else
                            {
                                text = entry.Text;
                            }
                        }

                        projectStats.AddReply(dialogue, text);
                    }
                }
            }

            return projectStats;
        }

        /*public static void Export(string name, ProjectStats projectStats, string directory)
        {
            Export(name, projectStats, directory, new List<Package>(), new List<Language>());
        }*/

        public static void Export(string name, ProjectStats projectStats, string directory, List<Package> packages, Language language, bool workstringOnly, bool workstringFallback)
        {
            Export(name, projectStats, directory, packages, new List<Language>() { language }, workstringOnly, workstringFallback);
        }

        public static void Export(string name, ProjectStats projectStats, string directory, List<Package> packages, List<Language> languages, bool workstringOnly, bool workstringFallback)
        {
            string path = Path.Combine(directory, name + ".txt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false, Encoding.Unicode))
            {
                foreach (var package in packages)
                    projectStats.Packages.Add(package);

                WriteStats(file, projectStats, languages, workstringOnly, workstringFallback);
            }
        }

        private class VoiceKitStats
        {
            public int Words = 0;
            public int Sentences = 0;
        }
        
        public static void WriteStats(TextWriter writer, ProjectStats projectStats, List<Language> languages, bool workstringOnly, bool workstringFallback)
        {
            var project = ResourcesHandler.Project;

            //Populate VoiceKit Stats
            Dictionary<string, VoiceKitStats> VoiceKits = new Dictionary<string, VoiceKitStats>();
            foreach (var kvp in projectStats.Actors)
            {
                string voiceKitName = (project.GetActorFromID(kvp.Key) != null) ? project.GetActorFromID(kvp.Key).VoiceKit : null;
                if (voiceKitName != null)
                {
                    VoiceKitStats voiceKitStats = VoiceKits.GetOrAdd(voiceKitName, (key) => new VoiceKitStats());
                    voiceKitStats.Words += kvp.Value.Words;
                    voiceKitStats.Sentences += kvp.Value.Sentences;
                }
            }

            //Append project specific stats
            if (EditorCore.ProjectStats != null)
            {
                writer.WriteLine(EditorCore.ProjectStats(projectStats));
            }

            writer.WriteLine(" * Source");
            writer.WriteLine("");
            if (workstringOnly)
            {
                writer.WriteLine("Workstring only");
            }
            else
            {
                if (languages != null)
                {
                    foreach (var language in languages)
                    {
                        writer.WriteLine(String.Format("Language : {0}", language.Name));
                    }
                }
                if (workstringFallback)
                    writer.WriteLine("Using Workstring fallback");
            }
            writer.WriteLine("");
            foreach (var package in projectStats.Packages)
            {
                if (package != null)
                {
                    writer.WriteLine(String.Format("Package : {0}", package.Name));
                }
            }
            writer.WriteLine("");
            writer.WriteLine("");

            writer.WriteLine(" * General");
            writer.WriteLine("");
            writer.WriteLine(String.Format("Total Sentence Words : {0}", projectStats.SentenceWords));
            writer.WriteLine(String.Format("Total Sentences : {0}", projectStats.Sentences));
            writer.WriteLine(String.Format("Avg Words per Sentence : {0:0.0}", (float)projectStats.SentenceWords / (float)projectStats.Sentences));
            writer.WriteLine("");
            writer.WriteLine(String.Format("Total Reply Words : {0}", projectStats.ReplyWords));
            writer.WriteLine(String.Format("Total Replies : {0}", projectStats.Replies));
            writer.WriteLine(String.Format("Avg Words per Reply : {0:0.0}", (float)projectStats.ReplyWords / (float)projectStats.Replies));
            writer.WriteLine("");
            writer.WriteLine("");

            writer.WriteLine(" * Actors");
            writer.WriteLine("");
            foreach (var kvp in projectStats.Actors)
            {
                float avg = (float)kvp.Value.Words / (float)kvp.Value.Sentences;
                writer.WriteLine(String.Format("{0} (\"{1}\") :  {2} words  ({3:0.0} per sentence)", kvp.Key, project.GetActorName(kvp.Key), kvp.Value.Words, avg));
            }
            writer.WriteLine("");
            writer.WriteLine("");

            writer.WriteLine(" * VoiceKits");
            writer.WriteLine("");
            foreach (var kvp in VoiceKits)
            {
                float avg = (float)kvp.Value.Words / (float)kvp.Value.Sentences;
                writer.WriteLine(String.Format("{0} (\"{1}\") :  {2} words  ({3:0.0} per sentence)", kvp.Key, project.GetVoiceActorNameFromKit(kvp.Key), kvp.Value.Words, avg));
            }
            writer.WriteLine("");
            writer.WriteLine("");

            writer.WriteLine(" * Dialogues");
            writer.WriteLine("");
            foreach (var kvp in projectStats.Dialogues)
            {
                float avg = (float)kvp.Value.SentenceWords / (float)kvp.Value.Sentences;
                writer.WriteLine(String.Format("{0} :  {1} words  ({2:0.0} per sentence)", kvp.Key, kvp.Value.SentenceWords, avg));
            }
        }
    }
}
