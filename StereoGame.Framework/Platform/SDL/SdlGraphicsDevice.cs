namespace StereoGame.Framework.Platform.SDL
{
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class SdlGraphicsDevice : GraphicsDevice
    {
        private readonly Game game;
        private readonly SdlGameWindow window;
        private IntPtr handle;
        private readonly StringBuilder stringBuffer = new(512);
        private readonly Dictionary<int, TextCache> textCache = new();
        //private readonly Dictionary<int, IconCache> iconCache = new();
        private readonly List<int> textCacheKeys = new();
        //private readonly List<int> iconCacheKeys = new();
        private int textCacheLimit = 100;


        public SdlGraphicsDevice(Game game, SdlGameWindow window, PresentationParameters pp)
        {
            this.game = game;
            this.window = window;
            var flags = Sdl.Renderer.SDL_RendererFlags.SDL_RENDERER_ACCELERATED;
            switch (pp.PresentInterval)
            {
                case PresentInterval.Default:
                case PresentInterval.One:
                    flags |= Sdl.Renderer.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC;
                    break;
            }
            handle = Sdl.Renderer.Create(window.Handle, 0, flags);
        }

        public override void Clear()
        {
            Sdl.Renderer.Clear(handle);
        }
        public override void Present()
        {
            Sdl.Renderer.Present(handle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearTextCache();
                Sdl.Renderer.Destroy(handle);
                handle = IntPtr.Zero;
            }
            base.Dispose(disposing);
        }
        public override Texture? CreateTexture(int width, int height)
        {
            IntPtr tex = Sdl.Renderer.CreateTexture(handle, Sdl.Renderer.SDL_PIXELFORMAT_ARGB8888, Sdl.Renderer.SDL_TEXTUREACCESS_TARGET, width, height);
            if (tex != IntPtr.Zero)
            {
                SdlTexture texture = new SdlTexture(this, width, height, tex);
                texture.Name = $"Texture {width}x{height}";
                OnResourceCreated(texture);
                return texture;
            }
            return null;
        }
        public override Texture? LoadTexture(string path)
        {
            IntPtr tex = SDL2Image.IMG_LoadTexture(handle, path);
            if (tex != IntPtr.Zero)
            {
                Sdl.Renderer.QueryTexture(tex, out _, out _, out int w, out int h);
                SdlTexture texture = new SdlTexture(this, w, h, tex);
                texture.Name = path;
                OnResourceCreated(texture);
                return texture;
            }
            return null;
        }

        public override  Texture? LoadTexture(string path, byte[] data)
        {
            IntPtr rw = Sdl.RwFromMem(data, data.Length);
            if (rw != IntPtr.Zero)
            {
                IntPtr tex = SDL2Image.IMG_LoadTexture_RW(handle, rw, 1);
                if (tex != IntPtr.Zero)
                {
                    Sdl.Renderer.QueryTexture(tex, out _, out _, out int w, out int h);
                    SdlTexture texture = new SdlTexture(this, w, h, tex);
                    texture.Name = path;
                    OnResourceCreated(texture);
                    return texture;
                }
            }
            return null;
        }

        protected override void DrawTexture(Texture? texture, ref Rectangle src, ref Rectangle dst)
        {
            if (CheckTexture(texture, out IntPtr tex))
            {
                Sdl.Renderer.RenderCopy(handle, tex, ref src, ref dst);
            }
        }

        public override void DrawLine(int x1, int y1, int x2, int y2)
        {
            Sdl.Renderer.DrawLine(handle, x1, y1, x2, y2);
        }


        protected override void DrawRect(ref Rectangle rect)
        {
            Sdl.Renderer.DrawRect(handle, ref rect);
        }

        protected override void DrawText(TextFont? font, ReadOnlySpan<char> text, float x, float y, float width, float height, Color color, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, float offsetX, float offsetY)
        {
            if (text == null) return;
            if (text.Length == 0) return;
            if (CheckTextFont(font, out IntPtr fnt))
            {
                DrawTextCache(GetTextCache(fnt, text, color), x, y, width, height, horizontalAlignment, verticalAlignment, offsetX, offsetY);
            }
        }

        protected override void SetDrawColor(byte r, byte g, byte b, byte a)
        {
            Sdl.Renderer.SetDrawColor(handle, r, g, b, a);
        }

        protected override void SetDrawBlendMode(BlendMode blendMode)
        {
            Sdl.Renderer.SetDrawBlendMode(handle, (int)blendMode);
        }

        private static bool CheckTexture(Texture? texture, out IntPtr tex)
        {
            tex = IntPtr.Zero;
            if (texture is SdlTexture sdlTexture)
            {
                tex = sdlTexture.Handle;
            }
            return tex != IntPtr.Zero;
        }

        private static bool CheckTextFont(TextFont? textFont, out IntPtr font)
        {
            font = IntPtr.Zero;
            if (textFont is SdlTextFont sdlTextFont)
            {
                font = sdlTextFont.Handle;
            }
            return font != IntPtr.Zero;
        }

        private void CheckTextCache()
        {
            if (textCache.Count >= textCacheLimit)
            {
                int len = textCacheKeys.Count / 2;
                var halfKeys = textCacheKeys.GetRange(0, len);
                textCacheKeys.RemoveRange(0, len);                
                //SDLLog.Verbose(LogCategory.RENDER, $"Text cache limit {textCacheLimit} reached. Cleaning up...");
                ClearTextCache(halfKeys);
            }
        }
        private void ClearTextCache()
        {
            foreach (var kvp in textCache)
            {
                TextCache tc = kvp.Value;
                Sdl.Renderer.DestroyTexture(tc.Texture);
            }
            textCache.Clear();
            textCacheKeys.Clear();
        }
        private void ClearTextCache(IEnumerable<int> keys)
        {
            foreach (var key in keys)
            {
                if (textCache.TryGetValue(key, out var tc))
                {
                    if (textCache.Remove(key))
                    {
                        Sdl.Renderer.DestroyTexture(tc.Texture);
                    }
                }
            }
        }
        private void DrawTextCache(TextCache? textCache, float x, float y, float width, float height, HorizontalAlignment hAlign, VerticalAlignment vAlign, float offsetX, float offsetY)
        {
            if (textCache == null) return;
            int w = textCache.Width;
            int h = textCache.Height;
            switch (hAlign)
            {
                case HorizontalAlignment.Left:
                    //nop
                    break;
                case HorizontalAlignment.Right:
                    x = x + width - w;
                    break;
                case HorizontalAlignment.Center:
                    x = x + width / 2 - w / 2;
                    break;
            }
            switch (vAlign)
            {
                case VerticalAlignment.Top:
                    // nop
                    break;
                case VerticalAlignment.Bottom:
                    y = y + height - h;
                    break;
                case VerticalAlignment.Center:
                    y = y + height / 2 - h / 2;
                    break;
            }
            Rectangle srcRect = new Rectangle(0, 0, w, h);
            RectangleF dstRect = new RectangleF(x + offsetX, y + offsetY, w, h);
            SetBlendMode(BlendMode.Blend);
            Sdl.Renderer.RenderCopyF(handle, textCache.Texture, ref srcRect, ref dstRect);
        }

        private TextCache? GetTextCache(IntPtr font, ReadOnlySpan<char> text, Color color)
        {
            int hash = string.GetHashCode(text);
            int key = HashCode.Combine(font, hash, color);
            if (textCache.TryGetValue(key, out var tc))
            {
                if (tc.Matches(font, hash, color))
                {
                    return tc;
                }
            }
            tc = CreateTextCache(font, text, color);
            if (tc != null)
            {
                textCache[key] = tc;
                textCacheKeys.Add(key);
            }
            return tc;
        }

        private TextCache? CreateTextCache(IntPtr font, ReadOnlySpan<char> text, Color color)
        {
            TextCache? textCache = null;
            if (font != IntPtr.Zero)
            {
                stringBuffer.Clear();
                stringBuffer.Append(text);
                int hash = string.GetHashCode(text);
                IntPtr surface = SDL2TTF.TTF_RenderUTF8_Blended(font, stringBuffer, ToSDLColor(color));
                if (surface != IntPtr.Zero)
                {
                    IntPtr texHandle = Sdl.Renderer.CreateTextureFromSurface(handle, surface);
                    if (texHandle != IntPtr.Zero)
                    {
                        _ = Sdl.Renderer.QueryTexture(texHandle, out _, out _, out int w, out int h);
                        _ = Sdl.Renderer.SetTextureAlphaMod(texHandle, color.A);
                        textCache = new TextCache(font, hash, color, w, h, texHandle);
                    }
                    Sdl.FreeSurface(surface);
                }
            }
            return textCache;
        }

        private class TextCache
        {
            internal TextCache(IntPtr font, int textHash, Color color, int width, int height, IntPtr texture)
            {
                Font = font;
                TextHash = textHash;
                Color = color;
                Width = width;
                Height = height;
                Texture = texture;
            }
            public IntPtr Font;
            public int TextHash;
            public int Width;
            public int Height;
            public Color Color;
            public IntPtr Texture;

            public bool Matches(IntPtr font, int textHash, Color color)
            {
                return (textHash == TextHash) && (font == Font) && (color == Color);
            }
        }

        private static int ToSDLColor(Color c)
        {
            int i = c.A;
            i <<= 8;
            i |= c.B;
            i <<= 8;
            i |= c.G;
            i <<= 8;
            i |= c.R;
            return i;
        }
    }
}
