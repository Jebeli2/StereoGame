namespace StereoGame.Framework.Audio
{
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class AudioResource : IDisposable
    {
        private bool disposed;
        private AudioDevice? audioDevice;
        private WeakReference? selfReference;

        internal AudioResource()
        {

        }
        ~AudioResource()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public AudioDevice? AudioDevice
        {
            get
            {
                return audioDevice;
            }

            internal set
            {
                Debug.Assert(value != null);

                if (audioDevice == value)
                    return;

                if (audioDevice != null && selfReference != null)
                {
                    audioDevice.RemoveResourceReference(selfReference);
                    selfReference = null;
                }

                audioDevice = value;

                selfReference = new WeakReference(this);
                audioDevice.AddResourceReference(selfReference);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Disposing?.Invoke(this, EventArgs.Empty);
                }

                if (audioDevice != null && selfReference != null)
                {
                    audioDevice.RemoveResourceReference(selfReference);
                    audioDevice.OnResourceDestroyed(this, Name);
                }

                selfReference = null;
                audioDevice = null;
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
