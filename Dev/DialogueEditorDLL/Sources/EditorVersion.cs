using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueEditor
{
    static public class EditorVersion
    {
        public const int Major = 1;
        public const int Minor = 4;
        public const int Patch = 0;

        static public string GetVersion()
        {
            return string.Format("{0}.{1}.{2}", Major, Minor, Patch);
        }
    }
}
