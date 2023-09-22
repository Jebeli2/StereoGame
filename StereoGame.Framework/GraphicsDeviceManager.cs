using StereoGame.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StereoGame.Framework
{
    public partial class GraphicsDeviceManager : IGraphicsDeviceService, IDisposable, IGraphicsDeviceManager
    {
        private readonly Game game;
        private GraphicsDevice? graphicsDevice;
        private bool initialized;
        private SurfaceFormat preferredBackBufferFormat;
        private DepthFormat preferredDepthStencilFormat;
        private bool preferMultiSampling;
        private int preferredBackBufferHeight;
        private int preferredBackBufferWidth;
        private DisplayOrientation supportedOrientations;
        private bool synchronizedWithVerticalRetrace = true;
        private bool disposed;
        private bool drawBegun;
        private bool hardwareModeSwitch = true;
        private bool preferHalfPixelOffset = false;
        private bool wantFullScreen;
        private GraphicsProfile graphicsProfile;
        private bool shouldApplyChanges;

        public static readonly int DefaultBackBufferWidth = 800;
        public static readonly int DefaultBackBufferHeight = 480;
        public GraphicsDeviceManager(Game game)
        {
            this.game = game;
            var clientBounds = this.game.Window.ClientBounds;
            if (clientBounds.Width >= clientBounds.Height)
            {
                preferredBackBufferWidth = clientBounds.Width;
                preferredBackBufferHeight = clientBounds.Height;
            }
            else
            {
                preferredBackBufferWidth = clientBounds.Height;
                preferredBackBufferHeight = clientBounds.Width;
            }
            wantFullScreen = false;
            GraphicsProfile = GraphicsProfile.Reach;

            if (this.game.Services.GetService(typeof(IGraphicsDeviceManager)) != null)
                throw new ArgumentException("A graphics device manager is already registered.  The graphics device manager cannot be changed once it is set.");
            this.game.GraphicsDeviceManager = this;
            this.game.Services.AddService(typeof(IGraphicsDeviceManager), this);
            this.game.Services.AddService(typeof(IGraphicsDeviceService), this);
        }

        public GraphicsProfile GraphicsProfile
        {
            get
            {
                return graphicsProfile;
            }
            set
            {
                shouldApplyChanges = true;
                graphicsProfile = value;
            }
        }

        public bool HardwareModeSwitch
        {
            get { return hardwareModeSwitch; }
            set
            {
                shouldApplyChanges = true;
                hardwareModeSwitch = value;
            }
        }

        public int PreferredBackBufferHeight
        {
            get
            {
                return preferredBackBufferHeight;
            }
            set
            {
                shouldApplyChanges = true;
                preferredBackBufferHeight = value;
            }
        }
        public int PreferredBackBufferWidth
        {
            get
            {
                return preferredBackBufferWidth;
            }
            set
            {
                shouldApplyChanges = true;
                preferredBackBufferWidth = value;
            }
        }
        public GraphicsDevice? GraphicsDevice => graphicsDevice;

        public event EventHandler<EventArgs>? DeviceCreated;

        public event EventHandler<EventArgs>? DeviceDisposing;

        public event EventHandler<EventArgs>? DeviceResetting;

        public event EventHandler<EventArgs>? DeviceReset;

        public event EventHandler<EventArgs>? Disposed;


        protected void OnDeviceCreated(EventArgs e)
        {
            DeviceCreated?.Invoke(this, e);
        }

        protected void OnDeviceDisposing(EventArgs e)
        {
            DeviceDisposing?.Invoke(this, e);
        }

        protected void OnDeviceResetting(EventArgs e)
        {
            DeviceResetting?.Invoke(this, e);
        }

        protected void OnDeviceReset(EventArgs e)
        {
            DeviceReset?.Invoke(this, e);
        }

        private void CreateDevice()
        {
            var pp = PreparePresentationParameters();
            CreateDevice(pp);
        }

        private void CreateDevice(PresentationParameters pp)
        {
            if (graphicsDevice != null) return;
            graphicsDevice = game.Platform.CreateGraphicsDevice(pp);
            OnDeviceCreated(EventArgs.Empty);
        }

        void IGraphicsDeviceManager.CreateDevice()
        {
            CreateDevice();
        }
        ~GraphicsDeviceManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    graphicsDevice?.Dispose();
                    graphicsDevice = null;
                }
                disposed = true;
                Disposed?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool BeginDraw()
        {
            if (graphicsDevice == null)
                return false;

            drawBegun = true;
            return true;
        }

        public void EndDraw()
        {
            if (graphicsDevice != null && drawBegun)
            {
                drawBegun = false;
                graphicsDevice.Present();
            }
        }

        public void ApplyChanges()
        {
            if (graphicsDevice == null) { CreateDevice(); }
            if (!shouldApplyChanges) return;
            shouldApplyChanges = false;

        }

        private PresentationParameters PreparePresentationParameters()
        {
            var pp = new PresentationParameters();
            pp.BackBufferWidth = preferredBackBufferWidth;
            pp.BackBufferHeight = preferredBackBufferHeight;
            pp.HardwareModeSwitch = hardwareModeSwitch;
            pp.IsFullScreen = wantFullScreen;
            return pp;
        }
    }
}
