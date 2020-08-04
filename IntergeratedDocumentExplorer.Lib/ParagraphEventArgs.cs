using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace IntergeratedDocumentExplorer.Lib
{
    public class ParagraphEventArgs : EventArgs
    {
        public HorizontalAlignment TextAlign { get; set; }

        public int Indentation { get; set; }
    }
}