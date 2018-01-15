using System;
using System.Collections.Generic;
using System.Linq;

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
            nodeID = nodeID.Replace(DialogueNodePrefix, string.Empty);
            return int.Parse(nodeID);
        }

        public TranslationEntry GetEntry(string key, Language language)
        {
            List<TranslationEntry> entryList;
            if (language != null && Entries.TryGetValue(key, out entryList))
            {
                return entryList.Find(x => x.LanguageName == language.Name);
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

            List<TranslationEntry> entryList;
            if (!Entries.TryGetValue(key, out entryList))
            {
                Entries.Add(key, new List<TranslationEntry>() { entry });
                return true;
            }

            if (entryList.FirstOrDefault(x => x.LanguageName == entry.LanguageName) == null)
            {
                entryList.Add(entry);
                return true;
            }

            return false;
        }

        public ETranslationResult AddOrUpdateEntry(string key, Language language, DateTime timestampLoca, string text)
        {
            //Refuse empty translations
            if (text == null || text == string.Empty)
                return ETranslationResult.Refused_EmptyText;

            TranslationEntry entry = new TranslationEntry();
            entry.Text = text;
            entry.LanguageName = language.Name;
            entry.SourceDate = timestampLoca;

            ETranslationResult result = ETranslationResult.Accepted;
            List<TranslationEntry> entryList;
            if (!Entries.TryGetValue(key, out entryList))
            {
                Entries.Add(key, new List<TranslationEntry>() { entry });
                return ETranslationResult.Accepted;
            }

            TranslationEntry existing = entryList.FirstOrDefault(x => x.LanguageName == entry.LanguageName);
            if (existing == null)
            {
                entryList.Add(entry);
                return ETranslationResult.Accepted;
            }

            if (existing.SourceDate > entry.SourceDate)
                return ETranslationResult.Refused_Outdated;

            if (existing.SourceDate == entry.SourceDate && existing.Text == entry.Text)
                return ETranslationResult.Refused_Identical;

            if (existing.SourceDate == entry.SourceDate)
                result = ETranslationResult.Accepted_IdenticalTimestamp;
            else if (existing.Text == entry.Text)
                result = ETranslationResult.Accepted_IdenticalText;

            existing = entry;

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
