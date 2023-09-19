using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Graphics
{
    public class PresentationParameters
    {
        public const int DefaultPresentRate = 60;
        private DepthFormat depthStencilFormat;
        private SurfaceFormat backBufferFormat;
        private int backBufferHeight = GraphicsDeviceManager.DefaultBackBufferHeight;
        private int backBufferWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
        private IntPtr deviceWindowHandle;
        private int multiSampleCount;
        private bool disposed;
        private bool isFullScreen;
        private bool hardwareModeSwitch = true;

        public PresentationParameters()
        {
            Clear();
        }

        public SurfaceFormat BackBufferFormat
        {
            get { return backBufferFormat; }
            set { backBufferFormat = value; }
        }
        public int BackBufferHeight
        {
            get { return backBufferHeight; }
            set { backBufferHeight = value; }
        }

        public int BackBufferWidth
        {
            get { return backBufferWidth; }
            set { backBufferWidth = value; }
        }
        public Rectangle Bounds
        {
            get { return new Rectangle(0, 0, backBufferWidth, backBufferHeight); }
        }

        public IntPtr DeviceWindowHandle
        {
            get { return deviceWindowHandle; }
            set { deviceWindowHandle = value; }
        }

        public DepthFormat DepthStencilFormat
        {
            get { return depthStencilFormat; }
            set { depthStencilFormat = value; }
        }

        public bool IsFullScreen
        {
            get
            {
                return isFullScreen;
            }
            set
            {
                isFullScreen = value;
            }
        }

        public bool HardwareModeSwitch
        {
            get { return hardwareModeSwitch; }
            set { hardwareModeSwitch = value; }
        }

        public int MultiSampleCount
        {
            get { return multiSampleCount; }
            set { multiSampleCount = value; }
        }

        public PresentInterval PresentationInterval { get; set; }

        public DisplayOrientation DisplayOrientation
        {
            get;
            set;
        }

        public RenderTargetUsage RenderTargetUsage { get; set; }

        public void Clear()
        {
            backBufferFormat = SurfaceFormat.Color;
            backBufferWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
            backBufferHeight = GraphicsDeviceManager.DefaultBackBufferHeight;
            deviceWindowHandle = IntPtr.Zero;
            depthStencilFormat = DepthFormat.None;
            multiSampleCount = 0;
            PresentationInterval = PresentInterval.Default;
            DisplayOrientation = DisplayOrientation.Default;
        }

        public PresentationParameters Clone()
        {
            PresentationParameters clone = new PresentationParameters();
            clone.backBufferFormat = backBufferFormat;
            clone.backBufferHeight = backBufferHeight;
            clone.backBufferWidth = backBufferWidth;
            clone.deviceWindowHandle = deviceWindowHandle;
            clone.depthStencilFormat = depthStencilFormat;
            clone.IsFullScreen = IsFullScreen;
            clone.HardwareModeSwitch = HardwareModeSwitch;
            clone.multiSampleCount = multiSampleCount;
            clone.PresentationInterval = PresentationInterval;
            clone.DisplayOrientation = DisplayOrientation;
            clone.RenderTargetUsage = RenderTargetUsage;
            return clone;
        }
    }
}
