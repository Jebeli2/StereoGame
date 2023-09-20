using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Graphics
{
    public abstract class GraphicsDevice : IDisposable
    {
        private bool isDisposed;
        private readonly object resourcesLock = new();
        private readonly List<WeakReference> resources = new();

        internal GraphicsDevice()
        {

        }
        ~GraphicsDevice()
        {
            Dispose(false);
        }
        public event EventHandler<EventArgs>? Disposing;

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
                    lock (resourcesLock)
                    {
                        foreach (var resource in resources.ToArray())
                        {
                            var target = resource.Target as IDisposable;
                            target?.Dispose();
                        }
                        resources.Clear();
                    }
                }
                isDisposed = true;
                EventHelpers.Raise(this, Disposing, EventArgs.Empty);
            }
        }

        public abstract void Clear();
        public abstract void Present();

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

        public abstract Texture? CreateTexture(int width, int height);
        public abstract Texture? LoadTexture(string path);

        public void DrawTexture(Texture? texture)
        {
            if (texture != null)
            {
                Rectangle src = new Rectangle(0, 0, texture.Width, texture.Height);
                Rectangle dst = Rectangle.Empty;
                DrawTexture(texture, ref src, ref dst);
            }
        }
        protected abstract void DrawTexture(Texture? texture, ref Rectangle src, ref Rectangle dst);

    }
}
