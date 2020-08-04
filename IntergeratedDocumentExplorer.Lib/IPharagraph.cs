using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IntergeratedDocumentExplorer.Lib
{
    public interface IPharagraph
    {
        event EventHandler<ParagraphEventArgs> IndentationChanged;
        event EventHandler<ParagraphEventArgs> AlignmentChanged;
        event EventHandler<ParagraphEventArgs> ChangedAlignment;
        HorizontalAlignment textAlignValue { get; }
        int identationCount { get; set; }
        void AlignmentChange(HorizontalAlignment alignTextValue);
        void ChangeTextAlign(HorizontalAlignment alignTextValue);
        void RightIndent(int IndentValue);
    }
}