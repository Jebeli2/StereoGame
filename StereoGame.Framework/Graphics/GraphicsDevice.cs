using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Graphics
{
    public partial class GraphicsDevice : IDisposable
    {
        private bool isDisposed;

        internal GraphicsDevice() 
        { 
            PresentationParameters = new PresentationParameters();
            PresentationParameters.DepthStencilFormat = DepthFormat.Depth24;
            GraphicsCapabilities = new GraphicsCapabilities();
            GraphicsCapabilities.Initialize(this);

        }
        public PresentationParameters PresentationParameters
        {
            get;
            private set;
        }
        internal GraphicsCapabilities GraphicsCapabilities { get; private set; }
        ~GraphicsDevice()
        {
            Dispose(false);
        }
        
        public bool IsDisposed => isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) 
        { 
            if (!isDisposed)
            {
                if (disposing)
                {

                }
                isDisposed = true;
            }
        }
    }
}
