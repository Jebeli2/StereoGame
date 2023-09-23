namespace StereoGame.Framework.Audio
{
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Resources;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class AudioDevice : IDisposable
    {
        private bool isDisposed;
        private readonly object resourcesLock = new();
        private readonly List<WeakReference> resources = new();
        protected PointF lastPos;

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

        public abstract Sound? LoadSound(string path);
        public abstract Sound? LoadSound(string path, byte[] data);

        public abstract void PlayMusic(Music? music, int loops = -1);
        public abstract void StopMusic();

        public void PlaySound(Sound? sound)
        {
            PlaySound(sound, null, PointF.Empty, false);
        }
        public void PlaySound(Sound? sound, PointF pos, bool loop = false)
        {
            PlaySound(sound, null, pos, loop);
        }
        public abstract void PlaySound(Sound? sound, string? channel, PointF pos, bool loop = false);

        public void UpdateSounds()
        {
            UpdateSounds(lastPos.X, lastPos.Y);
        }
        public abstract void UpdateSounds(float x, float y);

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
