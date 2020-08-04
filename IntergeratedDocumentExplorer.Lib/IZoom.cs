using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntergeratedDocumentExplorer.Lib
{
    public interface IZoom
    {
        event System.EventHandler<ZoomEventsArgs> ZoomChanged;
        float ChangeZoom(float value);
        
    }
}