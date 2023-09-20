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

        internal GraphicsResource()
        {

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
                    // Release managed objects
                    // ...
                }

                if (disposing)
                {
                    EventHelpers.Raise(this, Disposing, EventArgs.Empty);
                }

                if (graphicsDevice != null && selfReference != null)
                {
                    graphicsDevice.RemoveResourceReference(selfReference);
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
    }
}
