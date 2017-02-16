using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueEditor
{
    //--------------------------------------------------------------------------------------------------------------
    // Enums

    public enum ETranslationResult
    {
        Accepted,

        Accepted_IdenticalTimestamp,    //The current localization has the same timestamp as the new entry, but a different text
        Accepted_IdenticalText,         //The current localization has the same text as the new entry, but a more recent timestamp

        Refused_Identical,              //The current localization has the same text and timestamp as the new entry (loca is up to date)
        Refused_EmptyText,              //The localized entry text is empty (invalid)
        Refused_Outdated,               //The current localization is more recent than the new entry (invalid)
    }

    //--------------------------------------------------------------------------------------------------------------
    // Helper Class

    public class TranslationEntry
    {
        public string LanguageName;
        public string Text;
        public DateTime SourceDate;
    }

    public class TranslationTable
    {
        //--------------------------------------------------------------------------------------------------------------
        // Public vars

        public const string DialogueNodePrefix = "Node_";

        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public Dictionary<string, List<TranslationEntry>> Entries;

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public TranslationTable()
        {
            Entries = new Dictionary<string, List<TranslationEntry>>();
        }

        public static int GetNodeIDFromPrefixedString(string nodeID)
        {
            nodeID = nodeID.Replace(DialogueNodePrefix, String.Empty);
            return int.Parse(nodeID);
        }

        public TranslationEntry GetEntry(string key, Language language)
        {
            if (Entries.ContainsKey(key) && language != null)
            {
                var entry = Entries[key].Find(item => item.LanguageName == language.Name);
                return entry;
            }
            return null;
        }

        public TranslationEntry GetNodeEntry(int nodeID, Language language)
        {
            return GetEntry(DialogueNodePrefix + nodeID, language);
        }

        public TranslationEntry GetNodeEntry(DialogueNode node, Language language)
        {
            return GetNodeEntry(node.ID, language);
        }

        public bool AddEntry(string key)
        {
            if (!Entries.ContainsKey(key))
            {
                Entries.Add(key, new List<TranslationEntry>());
                return true;
            }
            return false;
        }

        public bool AddEntry(string key, Language language, string text)
        {
            TranslationEntry entry = new TranslationEntry();
            entry.Text = text;
            entry.LanguageName = language.Name;
            entry.SourceDate = DateTime.MinValue;

            if (!Entries.ContainsKey(key))
            {
                Entries.Add(key, new List<TranslationEntry>() { entry });
                return true;
            }
            else
            {
                int index = Entries[key].FindIndex(item => item.LanguageName == entry.LanguageName);
                if (index == -1)
                {
                    Entries[key].Add(entry);
                    return true;
                }
            }
            return false;
        }

        public ETranslationResult AddOrUpdateEntry(string key, Language language, DateTime timestampLoca, string text)
        {
            //Refuse empty translations
            if (text == null || text == String.Empty)
                return ETranslationResult.Refused_EmptyText;

            TranslationEntry entry = new TranslationEntry();
            entry.Text = text;
            entry.LanguageName = language.Name;
            entry.SourceDate = timestampLoca;

            ETranslationResult result = ETranslationResult.Accepted;
            if (!Entries.ContainsKey(key))
            {
                Entries.Add(key, new List<TranslationEntry>() { entry });
            }
            else
            {
                int index = Entries[key].FindIndex(item => item.LanguageName == entry.LanguageName);
                if (index == -1)
                {
                    Entries[key].Add(entry);
                }
                else
                {
                    if (Entries[key][index].SourceDate > entry.SourceDate)
                        return ETranslationResult.Refused_Outdated;

                    if (Entries[key][index].SourceDate == entry.SourceDate && Entries[key][index].Text == entry.Text)
                        return ETranslationResult.Refused_Identical;

                    if (Entries[key][index].SourceDate == entry.SourceDate)
                        result = ETranslationResult.Accepted_IdenticalTimestamp;
                    else if (Entries[key][index].Text == entry.Text)
                        result = ETranslationResult.Accepted_IdenticalText;

                    Entries[key][index] = entry;
                }
            }

            return result;
        }

        public ETranslationResult AddOrUpdateNodeEntry(int nodeID, Language language, DateTime date, string text)
        {
            return AddOrUpdateEntry(DialogueNodePrefix + nodeID, language, date, text);
        }

        public ETranslationResult AddOrUpdateNodeEntry(DialogueNode node, Language language, DateTime date, string text)
        {
            return AddOrUpdateNodeEntry(node.ID, language, date, text);
        }
    }
}
