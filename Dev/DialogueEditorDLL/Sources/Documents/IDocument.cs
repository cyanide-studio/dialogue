using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueEditor
{
    public interface IDocument
    {
        /// <summary>
        /// Refresh the Document when it becomes the Active Document
        /// </summary>
        void RefreshDocument();
        
        /// <summary>
        /// Refresh the Title of the Document, used after a Save action
        /// </summary>
        void RefreshTitle();
    }
}
