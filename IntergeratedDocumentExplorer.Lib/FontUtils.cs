using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace IntergeratedDocumentExplorer.Lib
{
    public class FontUtils : IFontUtils
    {
        private Color _backColor = Color.Red;
        private Color _foreColor = Color.Red;
        private Font _font;
        private int _minSize = 0;
        private int _maxSize = 1639;
        /// <summary>
        /// Gets the Highlighted Color.
        /// </summary>
        public Color BackColor => _backColor;
        /// <summary>
        /// Gets the Fore Color of Font.
        /// </summary>
        public Color foreColor => _foreColor;
        /// <summary>
        /// Gets the Font.
        /// </summary>
        public Font font => _font;
        /// <summary>
        /// Gets or sets the minimum Font Size allowed
        /// </summary>
        public int minSize
        {
            get => _minSize;
            set
            {
                _minSize = value;
            }
        }/// <summary>
        /// Gets or sets the maximum Font Size allowed
        /// </summary>
        public int maxSize
        {
            get => _maxSize;
            set
            {
                _maxSize = value;
            }
        }

        public event EventHandler<FontEventArgs> ChangedForeColor;
        public event EventHandler<FontEventArgs> ChangedBackColor;
        public event EventHandler<FontEventArgs> FontChanged;
        public event EventHandler<FontEventArgs> ChangedFont;

        
        /// <summary>
        /// this method called when font changes.
        /// </summary>
        /// <param name="FontValue"></param>
        public void FontChange(Font FontValue)
        {
            _font = FontValue;
            FontEventArgs args = new FontEventArgs();
            args.font = FontValue;
            OnFontChanged(args);
        }/// <summary>
         /// This method is called to change the Font Family of selected Text.
         /// </summary>
         /// <param name="FontValue">A Font Value</param>
         /// <param name="FontFamilyName">A string value</param>
         /// <returns>A string contains Font Family Name</returns>
        public string ChangeFont(Font FontValue, string FontFamilyName)
        {
            if (FontFamilyName != String.Empty)
            {
                FontValue = new Font(FontFamilyName, FontValue.Size, FontValue.Style, FontValue.Unit, FontValue.GdiCharSet, FontValue.GdiVerticalFont);
            }
            FontEventArgs args = new FontEventArgs();
            args.font = FontValue;
            OnChangedFont(args);
            return FontValue.FontFamily.Name;
        }
        /// <summary>
        /// This method is called to change the size of selected Text.
        /// </summary>
        /// <param name="FontValue">A Font Value</param>
        /// <param name="fontSize">A float Value</param>
        /// <returns>returns the Font Size</returns>
        public float ChangeFont(Font FontValue, float fontSize)
        {
            if (fontSize > _maxSize || fontSize < _minSize)
            {
                fontSize = FontValue.Size;
            }
            FontValue = new Font(FontValue.FontFamily, fontSize, FontValue.Style, FontValue.Unit, FontValue.GdiCharSet, FontValue.GdiVerticalFont);
            FontEventArgs args = new FontEventArgs();
            args.font = FontValue;
            OnChangedFont(args);
            return FontValue.Size;
        }/// <summary>
        /// This method is called to change FontStyle of selected Text.
        /// </summary>
        /// <param name="fontValue">A Font Value</param>
        /// <param name="fontStyle">AFontStyle Value</param>
        /// <param name="checkForPress">A boolean</param>
        public void ChangeFont(Font fontValue, FontStyle fontStyle, bool checkForPress)
        {
            var New_FontStyle = fontValue.Style;
            if (!checkForPress)
            {
                New_FontStyle = New_FontStyle & ~fontStyle;
            }
            else
            {
                New_FontStyle = New_FontStyle | fontStyle;
            }
            fontValue = new Font(fontValue, New_FontStyle);
            FontEventArgs args = new FontEventArgs();
            args.font = fontValue;
            OnChangedFont(args);
        }
        /// <summary>
        /// This mehtod is used to change the Fore Color of Font.
        /// </summary>
        /// <param name="color">A color value</param>
        public void ChangeForeColor(Color color)
        {
            _foreColor = color;
            FontEventArgs fontEvnentArgs = new FontEventArgs();
            fontEvnentArgs.fontForeColor = color;
            OnChangedFontForeColor(fontEvnentArgs);
        }
        /// <summary>
        /// This mehtod is used to change the Highlighted Color of Font.
        /// </summary>
        /// <param name="color">A color value</param>
        public void ChangeBackColor(Color color)
        {
            _backColor = color;
            FontEventArgs fontEvnentArgs = new FontEventArgs();
            fontEvnentArgs.fontBackColor = color;
            OnChangedFontBackColor(fontEvnentArgs);
        }
        /// <summary>
        /// Invoke the FontChanged Event
        /// </summary>
        /// <param name="e">FontEventArgs</param>
        protected virtual void OnFontChanged(FontEventArgs e)
        {
            if (this.FontChanged != null)
            {
                this.FontChanged(this, e);
            }
        }
        /// <summary>
        /// Invoke the ChangedFont Event
        /// </summary>
        /// <param name="e">FontEventArgs</param>
        protected virtual void OnChangedFont(FontEventArgs e)
        {
            if (this.ChangedFont != null)
            {
                this.ChangedFont(this, e);
            }
        }
        /// <summary>
        /// Invoke the ChangedForeColor Event
        /// </summary>
        /// <param name="e">FontEventArgs</param>
        protected virtual void OnChangedFontForeColor(FontEventArgs e)
        {
            if (this.ChangedForeColor != null)
            {
                this.ChangedForeColor(this, e);
            }
        }
        /// <summary>
        /// Invoke the ChangedBackColor Event
        /// </summary>
        /// <param name="e">FontEventArgs</param>
        protected virtual void OnChangedFontBackColor(FontEventArgs e)
        {
            if (this.ChangedBackColor != null)
            {
                this.ChangedBackColor(this, e);
            }
        }
        public void PutFormatPainter(Font font, Color foreColor, Color backColor)
        {
            _font = font;
            _foreColor = foreColor;
            _backColor = backColor;
        }

    }
}