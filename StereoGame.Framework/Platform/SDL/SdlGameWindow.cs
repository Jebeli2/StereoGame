﻿namespace StereoGame.Framework.Platform.SDL
{
    using StereoGame.Framework.Input;
    using StereoGame.Framework.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    internal class SdlGameWindow : GameWindow, IDisposable
    {
        private readonly Game game;
        private IntPtr handle;
        private int width;
        private int height;
        private bool resizable;
        private bool borderless;
        private bool willBeFullScreen;
        private bool mouseVisible;
        private bool hardwareSwitch;
        private string screenDeviceName = "";
        private bool wasMoved;
        private bool supressMoved;
        private bool isFullScreen;
        private bool disposed;
        private uint id;
        private IntPtr cursor;

        public SdlGameWindow(Game game)
        {
            resizable = true;
            borderless = false;
            mouseVisible = true;
            this.game = game;
            width = GraphicsDeviceManager.DefaultBackBufferWidth;
            height = GraphicsDeviceManager.DefaultBackBufferHeight;
            CreateWindow();
            //handle = Sdl.Window.Create("", 0, 0, width, height, Sdl.Window.State.Hidden | Sdl.Window.State.AllowHighDPI);
        }

        ~SdlGameWindow()
        {
            Dispose(false);
        }
        public uint ID => id;
        internal void CreateWindow()
        {
            var initflags =
                Sdl.Window.State.AllowHighDPI |
                Sdl.Window.State.Hidden |
                Sdl.Window.State.InputFocus |
                Sdl.Window.State.MouseFocus;

            if (handle != IntPtr.Zero) Sdl.Window.DestroyWindow(handle);

            var winx = Sdl.Window.PosCentered;
            var winy = Sdl.Window.PosCentered;

            //// if we are on Linux, start on the current screen
            //if (CurrentPlatform.OS == OS.Linux)
            //{
            //    winx |= GetMouseDisplay();
            //    winy |= GetMouseDisplay();
            //}

            width = GraphicsDeviceManager.DefaultBackBufferWidth;
            height = GraphicsDeviceManager.DefaultBackBufferHeight;

            handle = Sdl.Window.CreateWindow("", winx, winy, width, height, initflags);

            id = Sdl.Window.GetWindowId(handle);

            //if (_icon != IntPtr.Zero)
            //    Sdl.Window.SetIcon(_handle, _icon);

            Sdl.Window.SetWindowBordered(handle, !borderless);
            Sdl.Window.SetWindowResizable(handle, resizable);

            SetCursorVisible(mouseVisible);
        }
        public override string ScreenDeviceName => screenDeviceName;
        public override IntPtr Handle => handle;

        public override bool IsBorderless
        {
            get { return borderless; }
            set
            {
                Sdl.Window.SetWindowBordered(handle, !value);
                borderless = value;
            }
        }

        public void SetCursorVisible(bool visible)
        {
            mouseVisible = visible;
            //Sdl.Mouse.ShowCursor(visible ? 1 : 0);
        }

        protected internal override void SetSystemCursor(SystemCursor cursor)
        {
            ClearSystemCursor();
            this.cursor = Sdl.Mouse.CreateSystemCursor((Sdl.Mouse.SystemCursor)cursor);
            if (this.cursor != IntPtr.Zero)
            {
                Sdl.Mouse.SetCursor(this.cursor);
            }
        }

        protected internal override void ClearSystemCursor()
        {
            if (cursor != IntPtr.Zero)
            {
                Sdl.Mouse.FreeCursor(cursor);
                cursor = IntPtr.Zero;
            }
        }
        public override DisplayOrientation CurrentOrientation => DisplayOrientation.Default;

        public override Point Position
        {
            get
            {
                int x = 0;
                int y = 0;
                if (!isFullScreen) { Sdl.Window.GetWindowPosition(handle, out x, out y); }
                return new Point(x, y);
            }
            set
            {
                Sdl.Window.SetWindowPosition(handle, value.X, value.Y);
                wasMoved = true;
            }
        }

        public override Rectangle ClientBounds
        {
            get
            {
                Sdl.Window.GetWindowPosition(handle, out int x, out int y);
                return new Rectangle(x, y, width, height);
            }
        }

        public override Size Size
        {
            get { return new Size(width, height); }
            set
            {
                if (value.Width != width || value.Height != height)
                {
                    var prevBounds = ClientBounds;
                    width = value.Width;
                    height = value.Height;
                    Sdl.Window.GetWindowBordersSize(handle, out int miny, out int minx, out _, out _);
                    var centerX = Math.Max(prevBounds.X + ((prevBounds.Width - width) / 2), minx);
                    var centerY = Math.Max(prevBounds.Y + ((prevBounds.Height - height) / 2), miny);
                    Sdl.Window.SetWindowSize(handle, width, height);
                    Sdl.Window.SetWindowPosition(handle, centerX, centerY);
                }
            }
        }

        public override bool AllowUserResizing
        {
            get { return !IsBorderless && resizable; }
            set
            {
                Sdl.Window.SetWindowResizable(handle, value);
                resizable = value;
            }
        }

        protected override void SetTitle(string title)
        {
            Sdl.Window.SetWindowTitle(handle, title);
        }

        protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
        {

        }

        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
            this.willBeFullScreen = willBeFullScreen;
        }

        public override void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight)
        {
            this.screenDeviceName = screenDeviceName;
            var prevBounds = ClientBounds;
            var displayIndex = Sdl.Window.GetWindowDisplayIndex(handle);
            Sdl.Display.GetDisplayBounds(displayIndex, out Rectangle displayRect);
            if (willBeFullScreen != isFullScreen || hardwareSwitch != game.GraphicsDeviceManager.HardwareModeSwitch)
            {
                uint fullScreenFlag = (uint)(game.GraphicsDeviceManager.HardwareModeSwitch ? Sdl.Window.State.Fullscreen : Sdl.Window.State.FullscreenDesktop);
                Sdl.Window.SetWindowFullscreen(handle, willBeFullScreen ? fullScreenFlag : 0);
                hardwareSwitch = game.GraphicsDeviceManager.HardwareModeSwitch;
            }
            if (CurrentPlatform.OS == OS.Windows)
            {
                Sdl.SetHint("SDL_VIDEO_MINIMIZE_ON_FOCUS_LOSS", willBeFullScreen && hardwareSwitch ? "1" : "0");
            }
            if (!willBeFullScreen || game.GraphicsDeviceManager.HardwareModeSwitch)
            {
                Sdl.Window.SetWindowSize(handle, clientWidth, clientHeight);
                width = clientWidth;
                height = clientHeight;
            }
            else
            {
                width = displayRect.Width;
                height = displayRect.Height;
            }
            Sdl.Window.GetWindowBordersSize(handle, out int miny, out int minx, out _, out _);
            var centerX = Math.Max(prevBounds.X + ((prevBounds.Width - clientWidth) / 2), minx);
            var centerY = Math.Max(prevBounds.Y + ((prevBounds.Height - clientHeight) / 2), miny);
            if (isFullScreen && !willBeFullScreen)
            {
                Sdl.Display.GetDisplayBounds(displayIndex, out displayRect);
                centerX = displayRect.X + displayRect.Width / 2 - clientWidth / 2;
                centerY = displayRect.Y + displayRect.Height / 2 - clientHeight / 2;
            }
            Sdl.Window.SetWindowPosition(handle, centerX, centerY);
            if (isFullScreen != willBeFullScreen)
            {
                OnClientSizeChanged();
            }
            isFullScreen = willBeFullScreen;
            supressMoved = true;
        }

        internal void Moved()
        {
            if (supressMoved)
            {
                supressMoved = false;
                return;
            }

            wasMoved = true;
        }

        public void ClientResize(int width, int height)
        {
            if (this.width == width && this.height == height) { return; }
            //_game.GraphicsDevice.PresentationParameters.BackBufferWidth = width;
            //_game.GraphicsDevice.PresentationParameters.BackBufferHeight = height;
            //_game.GraphicsDevice.Viewport = new Viewport(0, 0, width, height);

            Sdl.Window.GetWindowSize(handle, out this.width, out this.height);

            OnClientSizeChanged();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            Sdl.Window.DestroyWindow(handle);
            handle = IntPtr.Zero;

            //if (_icon != IntPtr.Zero)
            //    Sdl.FreeSurface(_icon);

            disposed = true;
        }
    }
}
