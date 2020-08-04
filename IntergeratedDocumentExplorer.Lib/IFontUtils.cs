using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace IntergeratedDocumentExplorer.Lib
{
    public interface IFontUtils
    {
        event EventHandler<FontEventArgs> ChangedForeColor;
        event EventHandler<FontEventArgs> ChangedBackColor;
        event EventHandler<FontEventArgs> FontChanged;
        event EventHandler<FontEventArgs> ChangedFont;

        string ChangeFont(Font FontValue, string FontFamilyName);
        float ChangeFont(Font FontValue, float fontSize);
        void ChangeFont(Font fontValue, FontStyle fontStyle, bool checkForPress);
        void FontChange(Font FontValue);
        void ChangeBackColor(Color color);
        void ChangeForeColor(Color color);
    }
}