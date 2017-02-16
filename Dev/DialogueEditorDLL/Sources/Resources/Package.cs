using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Newtonsoft.Json;

namespace DialogueEditor
{
    public class Package
    {
        //--------------------------------------------------------------------------------------------------------------
        // Serialized vars

        public string Name { get; set; }
        public bool Export { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        // Class Methods

        public Package()
        {
            Name = "";
            Export = true;
        }
    }
}
