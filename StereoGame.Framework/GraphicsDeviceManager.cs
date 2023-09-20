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
        public GraphicsDevice? GraphicsDevice => graphicsDevice;

        public event EventHandler<EventArgs>? DeviceCreated;

        public event EventHandler<EventArgs>? DeviceDisposing;

        public event EventHandler<EventArgs>? DeviceResetting;

        public event EventHandler<EventArgs>? DeviceReset;

        public event EventHandler<EventArgs>? Disposed;


        protected void OnDeviceCreated(EventArgs e)
        {
            EventHelpers.Raise(this, DeviceCreated, e);
        }

        protected void OnDeviceDisposing(EventArgs e)
        {
            EventHelpers.Raise(this, DeviceDisposing, e);
        }

        protected void OnDeviceResetting(EventArgs e)
        {
            EventHelpers.Raise(this, DeviceResetting, e);
        }

        protected void OnDeviceReset(EventArgs e)
        {
            EventHelpers.Raise(this, DeviceReset, e);
        }

        private void CreateDevice()
        {
            if (graphicsDevice != null) return;
            graphicsDevice = game.Platform.CreateGraphicsDevice();
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
                EventHelpers.Raise(this, Disposed, EventArgs.Empty);
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
    }
}
