using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace DialogueEditor
{
    static public class Utility
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

        static public string GetRelativePathFromCurrentDir(string file)
        {
            return GetRelativePath(file, System.Environment.CurrentDirectory);
        }

        static public string GetRelativePath(string file, string folder)
        {
            Uri pathUri = new Uri(file);
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        static public bool IsFileLocked(string path)
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

        static public DateTime GetCurrentTime()
        {
            return DateTime.UtcNow;
        }

        static public string GetCurrentTimeAsString()
        {
            return GetTimeAsString(DateTime.UtcNow);
        }

        static public string GetTimeAsString(DateTime date)
        {
            return date.ToString("HH:mm:ss");
        }

        static public string GetCurrentDateAsString()
        {
            return GetDateAsString(DateTime.UtcNow);
        }

        static public string GetDateAsString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd_HH-mm-ss");
        }

        static public ETriBool GetTriBoolFromString(string value)
        {
            if (value == "true")
                return ETriBool.TB_true;
            if (value == "false")
                return ETriBool.TB_false;
            return ETriBool.TB_undefined;
        }

        static public string GetStringFromTriBool(ETriBool value)
        {
            if (value == ETriBool.TB_true)
                return "true";
            if (value == ETriBool.TB_false)
                return "false";
            return"undefined";
        }
    }
}
