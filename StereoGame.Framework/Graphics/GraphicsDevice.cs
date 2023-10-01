using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        protected BlendMode blendMode;
        protected Color color;
        protected byte colorR;
        protected byte colorG;
        protected byte colorB;
        protected byte colorA;
        protected TextureFilter textureFilter;
        internal GraphicsDevice()
        {
            color = Color.Empty;
            textureFilter = TextureFilter.Nearest;
        }
        ~GraphicsDevice()
        {
            Dispose(false);
        }

        public event EventHandler<ResourceCreatedEventArgs>? ResourceCreated;
        public event EventHandler<ResourceDestroyedEventArgs>? ResourceDestroyed;
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
                Disposing?.Invoke(this, EventArgs.Empty);
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

        public Color Color
        {
            get { return color; }
            set { SetColor(value); }
        }

        public BlendMode BlendMode
        {
            get { return blendMode; }
            set { SetBlendMode(value); }
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

        public abstract Texture? CreateTexture(string? name, int width, int height);
        public abstract Texture? LoadTexture(string path);
        public abstract Texture? LoadTexture(string path, byte[] data);

        public abstract void PushTarget(Texture? texture);
        public abstract void PopTarget();

        public void DrawTexture(Texture? texture)
        {
            if (texture != null)
            {
                Rectangle src = new(0, 0, texture.Width, texture.Height);
                Rectangle dst = Rectangle.Empty;
                DrawTexture(texture, ref src, ref dst);
            }
        }

        public void DrawTexture(Texture? texture, int x, int y, int w, int h)
        {
            if (texture != null)
            {
                Rectangle src = new(0, 0, texture.Width, texture.Height);
                Rectangle dst = new(x, y, w, h);
                DrawTexture(texture, ref src, ref dst);
            }
        }

        public void DrawTexture(Texture? texture, Rectangle src, Rectangle dst)
        {
            if (texture != null)
            {
                DrawTexture(texture, ref src, ref dst);
            }
        }

        public void DrawTexture(Texture? texture, int srcX, int srcY, int srcW, int srcH, int dstX, int dstY)
        {
            if (texture != null)
            {
                Rectangle src = new(srcX, srcY, srcW, srcH);
                Rectangle dst = new(dstX, dstY, srcW, srcH);
                DrawTexture(texture, ref src, ref dst);
            }
        }

        protected abstract void DrawTexture(Texture? texture, ref Rectangle src, ref Rectangle dst);

        public abstract void DrawLine(int x1, int y1, int x2, int y2);


        public void FillRect(Rectangle rect)
        {
            FillRect(ref rect);
        }

        protected abstract void FillRect(ref Rectangle rect);

        public abstract void FillColorRect(ref Rectangle rect, Color colorTopLeft, Color colorTopRight, Color colorBottomLeft, Color colorBottomRight);

        public void DrawRect(Rectangle rect)
        {
            DrawRect(ref rect);
        }
        public void DrawRect(int x, int y, int w, int h)
        {
            DrawRect(new Rectangle(x, y, w, h));
        }

        protected abstract void DrawRect(ref Rectangle rect);

        public void DrawText(TextFont? font, ReadOnlySpan<char> text, float x, float y)
        {
            DrawText(font, text, x, y, 0, 0, Color.White, HorizontalAlignment.Left, VerticalAlignment.Top, 0, 0);
        }
        public void DrawText(TextFont? font, ReadOnlySpan<char> text, Rectangle rect, Color color, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            DrawText(font, text, rect.X, rect.Y, rect.Width, rect.Height, color, horizontalAlignment, verticalAlignment, 0, 0);
        }
        public void DrawText(TextFont? font, ReadOnlySpan<char> text, float x, float y, float width, float height, Color color, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            DrawText(font, text, x, y, width, height, color, horizontalAlignment, verticalAlignment, 0, 0);
        }
        protected abstract void DrawText(TextFont? font, ReadOnlySpan<char> text, float x, float y, float width, float height, Color color, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, float offsetX, float offsetY);

        public void DrawIcon(Icons icon, Rectangle rect, Color color, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            DrawIcon(icon, rect.X, rect.Y, rect.Width, rect.Height, color, horizontalAlignment, verticalAlignment, 0, 0);
        }
        public void DrawIcon(Icons icon, float x, float y)
        {
            DrawIcon(icon, x, y, 0, 0, Color.White, HorizontalAlignment.Left, VerticalAlignment.Top, 0, 0);
        }

        protected abstract void DrawIcon(Icons icon, float x, float y, float width, float height, Color color, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, float offsetX, float offsetY);

        protected abstract void SetDrawColor(byte r, byte g, byte b, byte a);

        public void SetColor(Color value)
        {
            if (color != value)
            {
                color = value;
                colorR = value.R;
                colorG = value.G;
                colorB = value.B;
                colorA = value.A;
                SetDrawColor(colorR, colorG, colorB, colorA);
            }
        }

        protected abstract void SetDrawBlendMode(BlendMode value);

        public void SetBlendMode(BlendMode value)
        {
            if (blendMode != value)
            {
                blendMode = value;
                SetDrawBlendMode(blendMode);
            }
        }

        [Conditional("DEBUG")]
        public void Log(string message)
        {
            Game.Instance.Platform.Log(message);
        }
    }
}
