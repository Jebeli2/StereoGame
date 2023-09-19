using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework
{
    public abstract class GameWindow
    {
        internal bool allowAltF4 = true;
        private string title;


        [DefaultValue(false)]
        public abstract bool AllowUserResizing { get; set; }

        public abstract Rectangle ClientBounds { get; }
        public virtual bool AllowAltF4 { get { return allowAltF4; } set { allowAltF4 = value; } }
        public abstract Point Position { get; set; }
        public abstract DisplayOrientation CurrentOrientation { get; }
        public abstract IntPtr Handle { get; }
        public abstract string ScreenDeviceName { get; }
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    SetTitle(value);
                    title = value;
                }
            }
        }

        public virtual bool IsBorderless
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected GameWindow()
        {
        }

        public event EventHandler<EventArgs>? ClientSizeChanged;
        public event EventHandler<EventArgs>? OrientationChanged;
        public event EventHandler<EventArgs>? ScreenDeviceNameChanged;

        public abstract void BeginScreenDeviceChange(bool willBeFullScreen);
        public abstract void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight);
        public void EndScreenDeviceChange(string screenDeviceName)
        {
            EndScreenDeviceChange(screenDeviceName, ClientBounds.Width, ClientBounds.Height);
        }
        protected void OnActivated()
        {
        }

        internal void OnClientSizeChanged()
        {
            EventHelpers.Raise(this, ClientSizeChanged, EventArgs.Empty);
        }

        protected void OnDeactivated()
        {
        }
        protected void OnOrientationChanged()
        {
            EventHelpers.Raise(this, OrientationChanged, EventArgs.Empty);
        }

        protected void OnPaint()
        {
        }
        protected void OnScreenDeviceNameChanged()
        {
            EventHelpers.Raise(this, ScreenDeviceNameChanged, EventArgs.Empty);
        }

        protected internal abstract void SetSupportedOrientations(DisplayOrientation orientations);

        protected abstract void SetTitle(string title);

    }
}
