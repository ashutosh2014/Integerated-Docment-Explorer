using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntergeratedDocumentExplorer.Lib
{
    public class Zoom : IZoom
    {
        /// <summary>
        /// An event which occurs when users the zoom factor.
        /// </summary>
        public event System.EventHandler<ZoomEventsArgs> ZoomChanged;

        private float _minZoomValue=0;
        private float _maxZoomValue = 2100;
        private float _zoomValue=100;
        /// <summary>
        /// It gets and sets the minimum value of ZoomFactor in multiple of 100.
        /// </summary>
        public float minZomValue
        {
            get => _minZoomValue;
            set
            {
                _minZoomValue = value;
            }
        }
        /// <summary>
        /// It gets and sets the maximum value of ZoomFactor in multiple of 100.
        /// </summary>
        public float maxZomValue
        {
            get => _maxZoomValue;
            set
            {
                _maxZoomValue = value;
            }
        }
        public float zoomValue => _zoomValue;
         
        /// <summary>
        ///  Changes the Zoom Factor take <paramref name="ZoomValue"/> which is in multiple of 100. 
        /// </summary>
        /// <param name="ZoomValue">An Flating Number</param>
        /// <returns>A float number which denotes the new Zoom Factor</returns>
        public float ChangeZoom(float ZoomValue)
        {
            if(ZoomValue>_minZoomValue && ZoomValue <_maxZoomValue)
            {
                _zoomValue = ZoomValue;
            }
            ZoomEventsArgs args = new ZoomEventsArgs();
            args.getZoomFactor = _zoomValue;
            OnZoomChange(args);
            return _zoomValue;
        }
        /// <summary>
        /// Invoke the ZoomChanged event 
        /// </summary>
        /// <param name="e">ZoomEventArgs</param>
        protected virtual void OnZoomChange(ZoomEventsArgs e)
        {
            if(this.ZoomChanged != null)
            {
                this.ZoomChanged(this, e);
            }
        }
    }
}