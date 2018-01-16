using System;
using System.IO;

namespace DialogueEditor
{
    public static class Utility
    {
        //--------------------------------------------------------------------------------------------------------------
        // Enums

        public enum ETriBool
        {
            TB_undefined,
            TB_true,
            TB_false,
        }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public static string GetRelativePathFromCurrentDir(string file)
        {
            return GetRelativePath(file, Environment.CurrentDirectory);
        }

        public static string GetRelativePath(string file, string folder)
        {
            Uri pathUri = new Uri(file);
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public static bool IsFileLocked(string path)
        {
            try
            {
                //Use exception on file open to determine if the file is locked
                using (Stream stream = new FileStream(path, FileMode.Open))
                {
                }
            }
            catch (IOException)
            {
                // e.Message
                return true;
            }
            return false;
        }

        public static DateTime GetCurrentTime()
        {
            return DateTime.UtcNow;
        }

        public static string GetCurrentTimeAsString()
        {
            return GetTimeAsString(DateTime.UtcNow);
        }

        public static string GetTimeAsString(DateTime date)
        {
            return date.ToString("HH:mm:ss");
        }

        public static string GetCurrentDateAsString()
        {
            return GetDateAsString(DateTime.UtcNow);
        }

        public static string GetDateAsString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd_HH-mm-ss");
        }

        public static ETriBool GetTriBoolFromString(string value)
        {
            if (value == "true")
                return ETriBool.TB_true;
            if (value == "false")
                return ETriBool.TB_false;
            return ETriBool.TB_undefined;
        }

        public static string GetStringFromTriBool(ETriBool value)
        {
            if (value == ETriBool.TB_true)
                return "true";
            if (value == ETriBool.TB_false)
                return "false";
            return"undefined";
        }
    }
}
