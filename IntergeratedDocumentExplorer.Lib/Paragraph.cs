using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace IntergeratedDocumentExplorer.Lib
{
    public class Paragraph : IPharagraph
    {
        private HorizontalAlignment _textAlignValue;
        private int _indentationValue;
        private int _identationCount = 25;
        /// <summary>
        /// An event which occurs when Indentation Changes
        /// </summary>
        public event EventHandler<ParagraphEventArgs> IndentationChanged;
        /// <summary>
        /// An event occurs when Alignment of Text Changes
        /// </summary>
        public event EventHandler<ParagraphEventArgs> AlignmentChanged;
        /// <summary>
        /// An event occurs when user want to changes the textAlign
        /// </summary>
        public event EventHandler<ParagraphEventArgs> ChangedAlignment;
        /// <summary>
        /// Gets the Text Align Value.
        /// </summary>
        public HorizontalAlignment textAlignValue => _textAlignValue;
        /// <summary>
        /// Gets or sets the indencation count , the value by which we increase or decrease the indent.
        /// </summary>
        public int identationCount
        {
            get => _identationCount;
            set
            {
                _identationCount = value;
            }
        }
        /// <summary>
        /// This method is called when the text align changes.
        /// </summary>
        /// <param name="alignTextValue">A HorizontalAlignment value</param>
        public void AlignmentChange(HorizontalAlignment alignTextValue)
        {
            _textAlignValue = alignTextValue;
            ParagraphEventArgs args = new ParagraphEventArgs();
            args.TextAlign = alignTextValue;
            onAlignmentChange(args);
        }
        /// <summary>
        /// This method changes the Text Align of SelectedText.
        /// </summary>
        /// <param name="alignTextValue">A HorizontalAlignment value</param>
        public void ChangeTextAlign(HorizontalAlignment alignTextValue)
        {
            _textAlignValue = alignTextValue;
            ParagraphEventArgs args = new ParagraphEventArgs();
            args.TextAlign = alignTextValue;
            onTextAlignChange(args);
        }
        /// <summary>
        /// This increases the indent Value by indentationCount.
        /// </summary>
        /// <param name="IndentValue">An intger Value</param>
        public void RightIndent(int IndentValue)
        {
            IndentValue += _identationCount;
            _indentationValue = IndentValue;
            ParagraphEventArgs args = new ParagraphEventArgs();
            args.Indentation = IndentValue;
            IndentationChange(args);
        }
        /// <summary>
        /// This decreases the indent Value by indentationCount.
        /// </summary>
        /// <param name="IndentValue">An intreger Value</param>
        public void LeftIndent(int IndentValue)
        {
            if(IndentValue !=0)
            {
                IndentValue -= _identationCount;
            }
            _indentationValue = IndentValue;
            ParagraphEventArgs args = new ParagraphEventArgs();
            args.Indentation = IndentValue;
            IndentationChange(args);
        }
        /// <summary>
        /// Invokes an event of Text Alignment Changed
        /// </summary>
        /// <param name="e">ParagraphEventsArgs</param>

        protected virtual void onAlignmentChange(ParagraphEventArgs e)
        {
            if (this.ChangedAlignment != null)
            {
                this.ChangedAlignment(this, e);
            }
        }
        /// <summary>
        /// Invokes an event of Change the Text alignment 
        /// </summary>
        /// <param name="e">ParagraphEventsArgs</param>
        protected virtual void onTextAlignChange(ParagraphEventArgs e)
        {
            if (this.AlignmentChanged != null)
            {
                this.AlignmentChanged(this, e);
            }
        }
        /// <summary>
        /// Invokes an event of Indentation Change
        /// </summary>
        /// <param name="e">ParagraphEventsArgs</param>
        protected virtual void IndentationChange(ParagraphEventArgs e)
        {
            if (this.IndentationChanged != null)
            {
                this.IndentationChanged(this, e);
            }
        }
    }
}