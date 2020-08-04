using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace IntergeratedDocumentExplorer.Lib
{
    public class FontEventArgs : EventArgs
    {
        public Font font { get; set; }
        public Color fontForeColor { get; set; }

        public Color fontBackColor { get; set; }
    }
}