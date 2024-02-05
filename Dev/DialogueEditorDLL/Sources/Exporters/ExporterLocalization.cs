using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogueEditor
{
    static public class ExporterLocalization
    {
        //--------------------------------------------------------------------------------------------------------------
        // Exports

        static public bool ExportToUnreal4()
        {
            var project = ResourcesHandler.Project;

            string projectDirectory = EditorHelper.GetProjectDirectory();
            string exportDirectory = Path.Combine(projectDirectory, EditorCore.Settings.DirectoryExportLocalization);

            var dialog = new DialogExport("Export Localization Unreal 4",
                                            path: exportDirectory,
                                            defaultDateDirectory: true,
                                            defaultPackageDirectory: false,
                                            allowConstants: false,
                                            allowLanguages: true,
                                            allowWorkstringFallback: true,
                                            allowDateFrom: true,
                                            dateFrom: DateTime.MinValue);

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.Cancel)
                return false;

            EditorCore.Settings.DirectoryExportLocalization = Utility.GetRelativePath(dialog.ExportPath, projectDirectory);

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

                foreach (var language in listLanguages)
                {
                    string languageDirectory = Path.Combine(packageDirectory, language.LocalizationCode);

                    if (!Directory.Exists(languageDirectory))
                        Directory.CreateDirectory(languageDirectory);

                    ExportToPO(languageDirectory, project, dialogues, language, dialog.WorkstringOnly, dialog.WorkstringFallback);
                }

                ExportToUnrealManifest(packageDirectory, project, dialogues);
            }

            System.Diagnostics.Process.Start(exportDirectory);

            return true;
        }

        static public bool ExportToUnity()
        {
            var project = ResourcesHandler.Project;

            string projectDirectory = EditorHelper.GetProjectDirectory();
            string exportDirectory = Path.Combine(projectDirectory, EditorCore.Settings.DirectoryExportLocalization);

            var dialog = new DialogExport("Export Localization Unity",
                                            path: exportDirectory,
                                            defaultDateDirectory: true,
                                            defaultPackageDirectory: false,
                                            allowConstants: false,
                                            allowLanguages: true,
                                            allowWorkstringFallback: true,
                                            allowDateFrom: true,
                                            dateFrom: DateTime.MinValue);

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.Cancel)
                return false;

            EditorCore.Settings.DirectoryExportLocalization = Utility.GetRelativePath(dialog.ExportPath, projectDirectory);

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

                ExportToCSV(packageDirectory, project, dialogues, listLanguages, workstringOnly: dialog.WorkstringOnly, workstringFallback: dialog.WorkstringFallback);
            }

            System.Diagnostics.Process.Start(exportDirectory);

            return true;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Common export utilities

        public static string GetLocalizedText(Dialogue dialogue, Language language, DialogueNode dialogueNode, bool workstringOnly, bool workstringFallback)
        {
            string workstring;
            if (dialogueNode is DialogueNodeSentence)
                workstring = (dialogueNode as DialogueNodeSentence).Sentence;
            else
                workstring = (dialogueNode as DialogueNodeReply).Reply;

            string localizedText = workstring;
            if (!workstringOnly)
            {
                TranslationEntry translation = dialogue.Translations.GetNodeEntry(dialogueNode, language);
                if (translation == null)
                {
                    if (!workstringFallback)
                        localizedText = "";
                }
                else
                {
                    localizedText = translation.Text;
                }
            }

            localizedText = EditorHelper.FormatTextEntry(localizedText, language);  //language = workstring if workstringOnly
            return localizedText;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Export PO

        private class POEntry
        {
            public string text = "";
            public List<string> nodes = new List<string>();
        }

        static public void ExportToPO(string directory, Project project, List<Dialogue> dialogues, Language language, bool workstringOnly, bool workstringFallback)
        {
            string path = Path.Combine(directory, "Dialogues.po");

            var locas = new Dictionary<string, POEntry>();
            bool isDefault = workstringOnly || (project.GetDefaultLanguage() == language);

            //Fill existing locas
            /*if (File.Exists(path))
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(path, Encoding.UTF8))
                {
                    while (!file.EndOfStream)
                    {
                        string lineId = file.ReadLine();
                        if (lineId.StartsWith("msgid"))
                        {
                            lineId = lineId.Replace("msgid", "");
                            lineId = lineId.Trim();
                            lineId = lineId.Substring(1);
                            lineId = lineId.Substring(0, lineId.Length - 1);

                            string lineStr = file.ReadLine();
                            if (lineStr.StartsWith("msgstr"))
                            {
                                lineStr = lineStr.Replace("msgstr", "");
                                lineStr = lineStr.Trim();
                                lineStr = lineStr.Substring(1);
                                lineStr = lineStr.Substring(0, lineStr.Length - 1);

                                if (!locas.ContainsKey(lineId))
                                    locas.Add(lineId, new POEntry() { text = lineStr });
                            }
                        }
                    }
                }
            }*/

            //Parse texts to localize and fill nodes references
            foreach (Dialogue dialogue in dialogues)
            {
                var orderedListNodes = new List<DialogueNode>();
                dialogue.GetOrderedNodes(ref orderedListNodes);
                foreach (DialogueNode dialogueNode in orderedListNodes)
                {
                    if (dialogueNode is DialogueNodeSentence || dialogueNode is DialogueNodeReply)
                    {
                        string workstring;
                        if (dialogueNode is DialogueNodeSentence)
                            workstring = (dialogueNode as DialogueNodeSentence).Sentence;
                        else
                            workstring = (dialogueNode as DialogueNodeReply).Reply;

                        POEntry entry;
                        if (!locas.TryGetValue(workstring, out entry))
                        {
                            string localizedText = GetLocalizedText(dialogue, language, dialogueNode, workstringOnly: workstringOnly, workstringFallback: workstringFallback);

                            entry = new POEntry() { text = localizedText };
                            locas.Add(workstring, entry);
                        }

                        entry.nodes.Add(EditorHelper.GetPrettyNodeID(dialogue, dialogueNode));
                    }
                }
            }

            //Re-write file
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false, Encoding.UTF8))
            {
                string date = Utility.GetDateTimeAsString(Utility.GetCurrentTime());

                //Header
                file.WriteLine("# Dialogues " + language.Name + " translation.");
                file.WriteLine("# ");
                file.WriteLine("msgid \"\"");
                file.WriteLine("msgstr \"\"");
                file.WriteLine("\"Project-Id-Version: Dialogues\\n\"");
                file.WriteLine("\"POT-Creation-Date: " + date + "\\n\"");
                file.WriteLine("\"PO-Revision-Date: " + date + "\\n\"");
                file.WriteLine("\"Language-Team: \\n\"");
                file.WriteLine("\"Language: " + language.LocalizationCode + "\\n\"");
                file.WriteLine("\"MIME-Version: 1.0\\n\"");
                file.WriteLine("\"Content-Type: text/plain; charset=UTF-8\\n\"");
                file.WriteLine("\"Content-Transfer-Encoding: 8bit\\n\"");
                file.WriteLine("\"Plural-Forms: nplurals=2; plural=(n != 1);\\n\"");
            }

            //Write entries
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true, Encoding.UTF8))
            {
                foreach (var kvp in locas)
                {
                    if (kvp.Value.nodes.Count > 0)
                    {
                        file.WriteLine("");
                        foreach (string node in kvp.Value.nodes)
                            file.WriteLine(String.Format("#: {0}", node));
                        file.WriteLine(String.Format("msgid \"{0}\"", kvp.Key));
                        file.WriteLine(String.Format("msgstr \"{0}\"", kvp.Value.text));
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Export CSV

        static public void ExportToCSV(string directory, Project project, List<Dialogue> dialogues, List<Language> languages, bool workstringOnly, bool workstringFallback)
        {
            string path = Path.Combine(directory, $"Localization_{project.GetName()}.csv");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false, Encoding.UTF8))
            {
                // Key, ID, ...Languages...
                {
                    ExporterCsv.CsvLineWriter header = new ExporterCsv.CsvLineWriter();
                    header.AddField("Key");
                    header.AddField("Id");
                    foreach (var language in languages)
                    {
                        header.AddField($"{language.Name}({language.LocalizationCode})");
                    }
                    header.WriteLine(file);
                }

                foreach (Dialogue dialogue in dialogues)
                {
                    var orderedListNodes = new List<DialogueNode>();
                    dialogue.GetOrderedNodes(ref orderedListNodes);

                    foreach (var dialogueNode in orderedListNodes)
                    {
                        if (dialogueNode is DialogueNodeSentence || dialogueNode is DialogueNodeReply)
                        {
                            // Key, ID, ...Languages...
                            ExporterCsv.CsvLineWriter line = new ExporterCsv.CsvLineWriter();

                            line.AddField(EditorHelper.GetPrettyNodeID(dialogue, dialogueNode));
                            line.AddEmptyField();

                            foreach (var language in languages)
                            {
                                string localizedText = GetLocalizedText(dialogue, language, dialogueNode, workstringOnly: workstringOnly, workstringFallback: workstringFallback);
                                line.AddField(localizedText);
                            }

                            line.WriteLine(file);
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        // Export UE Manifest

        private class UEManifestRoot
        {
            public int FormatVersion;
            public string Namespace;
            public List<UEManifestEntry> Children;
        };
        private class UEManifestEntry
        {
            public UEManifestSource Source;
            public List<UEManifestKey> Keys;
        };
        private class UEManifestSource
        {
            public string Text;
        };
        private class UEManifestKey
        {
            public string Key;
            public string Path;
        };

        static private void AddUEManifestEntry(UEManifestRoot root, string key)
        {
            //In manifest, we will use Key = Text, because Text here will be used as an ID in .po files
            UEManifestEntry entry = new UEManifestEntry();
            entry.Source = new UEManifestSource()
            {
                Text = key
            };
            entry.Keys = new List<UEManifestKey>()
            {
                new UEManifestKey()
                {
                    Key = key,
                    Path = ""
                }
            };
            root.Children.Add(entry);
        }

        static public void ExportToUnrealManifest(string directory, Project project, List<Dialogue> dialogues)
        {
            string path = Path.Combine(directory, "Dialogues.manifest");

            UEManifestRoot root = new UEManifestRoot();
            root.FormatVersion = 1;
            root.Namespace = "";
            root.Children = new List<UEManifestEntry>();

            foreach (Dialogue dialogue in dialogues)
            {
                var orderedListNodes = new List<DialogueNode>();
                dialogue.GetOrderedNodes(ref orderedListNodes);
                foreach (DialogueNode dialogueNode in orderedListNodes)
                {
                    if (dialogueNode is DialogueNodeSentence)
                    {
                        DialogueNodeSentence sentence = (dialogueNode as DialogueNodeSentence);
                        AddUEManifestEntry(root, sentence.Sentence);
                    }
                    else if (dialogueNode is DialogueNodeReply)
                    {
                        DialogueNodeReply reply = (dialogueNode as DialogueNodeReply);
                        AddUEManifestEntry(root, reply.Reply);
                    }
                }
            }

            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, root);
            }
        }
    }
}
