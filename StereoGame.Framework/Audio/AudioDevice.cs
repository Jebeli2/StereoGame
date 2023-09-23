namespace StereoGame.Framework.Audio
{
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Resources;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class AudioDevice : IDisposable
    {
        private bool isDisposed;
        private readonly object resourcesLock = new();
        private readonly List<WeakReference> resources = new();

        protected AudioDevice() { }

        ~AudioDevice()
        {
            Dispose(false);
        }

        public event EventHandler<ResourceCreatedEventArgs>? ResourceCreated;
        public event EventHandler<ResourceDestroyedEventArgs>? ResourceDestroyed;
        public event EventHandler<EventArgs>? Disposing;

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
                Disposing?.Invoke(this, EventArgs.Empty);
            }
        }

        public abstract Music? LoadMusic(string path);
        public abstract Music? LoadMusic(string path, byte[] data);

        public abstract void PlayMusic(Music? music);
        public abstract void StopMusic();

        internal void AddResourceReference(WeakReference resourceReference)
        {
            lock (resourcesLock)
            {
                resources.Add(resourceReference);
            }
        }

        internal void RemoveResourceReference(WeakReference resourceReference)
        {
            lock (resourcesLock)
            {
                resources.Remove(resourceReference);
            }
        }

        internal void OnResourceCreated(object resource)
        {
            Log($"{resource} created");
            ResourceCreated?.Invoke(this, new ResourceCreatedEventArgs(resource));
        }

        internal void OnResourceDestroyed(object resource, string name)
        {
            Log($"{resource} destroyed");
            ResourceDestroyed?.Invoke(this, new ResourceDestroyedEventArgs(resource, name));
        }

        [Conditional("DEBUG")]
        public void Log(string message)
        {
            Game.Instance.Platform.Log(message);
        }

    }
}
