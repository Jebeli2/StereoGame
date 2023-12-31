﻿using StereoGame.Framework.Input;
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
        private string title = "";


        [DefaultValue(false)]
        public abstract bool AllowUserResizing { get; set; }

        public abstract Rectangle ClientBounds { get; }
        public abstract Size Size { get; set; }
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

        internal MouseState MouseState;

        internal protected abstract void SetSystemCursor(SystemCursor cursor);
        internal protected abstract void ClearSystemCursor();

        protected GameWindow()
        {
        }

        public event EventHandler<EventArgs>? ClientSizeChanged;
        public event EventHandler<EventArgs>? OrientationChanged;
        public event EventHandler<EventArgs>? ScreenDeviceNameChanged;
        public event EventHandler<InputKeyEventArgs>? KeyDown;
        public event EventHandler<InputKeyEventArgs>? KeyUp;
        public event EventHandler<TextInputEventArgs>? TextInput;

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
            ClientSizeChanged?.Invoke(this, EventArgs.Empty);
        }

        protected void OnDeactivated()
        {
        }

        internal void OnKeyDown(InputKeyEventArgs e)
        {
            KeyDown?.Invoke(this, e);
        }
        internal void OnKeyUp(InputKeyEventArgs e)
        {
            KeyUp?.Invoke(this, e);
        }

        internal void OnTextInput(TextInputEventArgs e)
        {
            TextInput?.Invoke(this, e);
        }
        protected void OnOrientationChanged()
        {
            OrientationChanged?.Invoke(this, EventArgs.Empty);
        }

        protected void OnPaint()
        {
        }
        protected void OnScreenDeviceNameChanged()
        {
            ScreenDeviceNameChanged?.Invoke(this, EventArgs.Empty);
        }

        protected internal abstract void SetSupportedOrientations(DisplayOrientation orientations);

        protected abstract void SetTitle(string title);

    }
}
