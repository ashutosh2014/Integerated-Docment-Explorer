using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntergeratedDocumentExplorer.Lib
{
    public class FileEventsArgs : EventArgs
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}