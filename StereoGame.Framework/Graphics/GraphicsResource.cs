namespace StereoGame.Framework.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class GraphicsResource : IDisposable
    {
        private bool disposed;
        private GraphicsDevice? graphicsDevice;
        private WeakReference? selfReference;

        protected GraphicsResource(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }
        ~GraphicsResource()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public GraphicsDevice? GraphicsDevice
        {
            get
            {
                return graphicsDevice;
            }

            internal set
            {
                Debug.Assert(value != null);

                if (graphicsDevice == value)
                    return;

                if (graphicsDevice != null && selfReference != null)
                {
                    graphicsDevice.RemoveResourceReference(selfReference);
                    selfReference = null;
                }

                graphicsDevice = value;

                selfReference = new WeakReference(this);
                graphicsDevice.AddResourceReference(selfReference);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Disposing?.Invoke(this,EventArgs.Empty);    
                }

                if (graphicsDevice != null && selfReference != null)
                {
                    graphicsDevice.RemoveResourceReference(selfReference);
                    graphicsDevice.OnResourceDestroyed(this, Name);
                }

                selfReference = null;
                graphicsDevice = null;
                disposed = true;
            }
        }

        public bool IsDisposed => disposed;

        public event EventHandler<EventArgs>? Disposing;

        public string Name { get; set; }

        public object Tag { get; set; }

        public override string? ToString()
        {
            if (!string.IsNullOrEmpty(Name)) return Name;
            return base.ToString();
        }
    }
}
