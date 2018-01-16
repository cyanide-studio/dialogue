
using System.Collections.Generic;
using System.Windows.Forms;

namespace DialogueEditor
{
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        Module interface
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    public interface IModule
    {
        string Name { get; }
        string Description { get; }

        void Initialize(WindowMain windowMain);
        void Shutdown();

        IDictionary<string, IModuleAddIn> AddIns { get; }
    }


    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        Module Add-In types
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    public interface IModuleAddIn
    {
    }

    public class ToolStripAddIn : Dictionary<string, ToolStripItem>, IModuleAddIn
    {
    }

    public class ToolStripMenuAddIn : Dictionary<string, ToolStripMenuItem>, IModuleAddIn
    {
    }
}
