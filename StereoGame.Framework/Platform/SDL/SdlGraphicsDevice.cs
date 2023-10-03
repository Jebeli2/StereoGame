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
        private readonly SdlGamePlatform platform;
        private readonly SdlGameWindow window;
        private IntPtr handle;
        private readonly StringBuilder stringBuffer = new(512);
        private readonly Dictionary<int, TextCache> textCache = new();
        private readonly Dictionary<int, IconCache> iconCache = new();
        private readonly List<int> textCacheKeys = new();
        private readonly List<int> iconCacheKeys = new();
        private int textCacheLimit = 100;
        private int iconCacheLimit = 100;
        private readonly Stack<IntPtr> prevTargets = new();
        private readonly Stack<Rectangle> prevClips = new();
        private readonly TextFont? defaultFont;
        private readonly TextFont? iconFont;
        private readonly int[] rectIndices = new int[] { 2, 0, 1, 1, 3, 2 };
        private const int NUM_RECT_INDICES = 6;
        private readonly Sdl.Renderer.SDL_Vertex[] rectVertices = new Sdl.Renderer.SDL_Vertex[4];


        public SdlGraphicsDevice(Game game, SdlGamePlatform platform, SdlGameWindow window, PresentationParameters pp)
        {
            this.game = game;
            this.platform = platform;
            this.window = window;
            defaultFont = platform.LoadFont("default", Properties.Resources.Roboto_Regular, 16);
            iconFont = platform.LoadFont("icon", Properties.Resources.entypo, 16);
            var flags = Sdl.Renderer.SDL_RendererFlags.SDL_RENDERER_ACCELERATED;
            switch (pp.PresentInterval)
            {
                case PresentInterval.Default:
                case PresentInterval.One:
                    flags |= Sdl.Renderer.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC;
                    break;
            }

            handle = Sdl.Renderer.CreateRenderer(window.Handle, 0, flags);
        }

        public override void Clear()
        {
            Sdl.Renderer.RenderClear(handle);
        }
        public override void Present()
        {
            Sdl.Renderer.RenderPresent(handle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearTextCache();
                ClearIconCache();
                defaultFont?.Dispose();
                iconFont?.Dispose();
                Sdl.Renderer.DestroyRenderer(handle);
                handle = IntPtr.Zero;
            }
            base.Dispose(disposing);
        }
        public override Texture? CreateTexture(string? name, int width, int height)
        {
            Sdl.SetHint("SDL_RENDER_SCALE_QUALITY", (int)textureFilter);
            IntPtr tex = Sdl.Renderer.CreateTexture(handle, Sdl.Renderer.SDL_PIXELFORMAT_ARGB8888, Sdl.Renderer.SDL_TEXTUREACCESS_TARGET, width, height);
            if (tex != IntPtr.Zero)
            {
                SdlTexture texture = new(this, width, height, tex) { Name = $"Texture <{name ?? "noname"}> {width}x{height}" };
                OnResourceCreated(texture);
                return texture;
            }
            return null;
        }
        public override Texture? LoadTexture(string path)
        {
            Sdl.SetHint("SDL_RENDER_SCALE_QUALITY", (int)textureFilter);
            IntPtr tex = SDL2Image.LoadTexture(handle, path);
            if (tex != IntPtr.Zero)
            {
                _ = Sdl.Renderer.QueryTexture(tex, out _, out _, out int w, out int h);
                SdlTexture texture = new(this, w, h, tex) { Name = path };
                OnResourceCreated(texture);
                return texture;
            }
            return null;
        }

        public override Texture? LoadTexture(string path, byte[] data)
        {
            IntPtr rw = Sdl.RWFromMem(data, data.Length);
            if (rw != IntPtr.Zero)
            {
                Sdl.SetHint("SDL_RENDER_SCALE_QUALITY", (int)textureFilter);
                IntPtr tex = SDL2Image.LoadTexture_RW(handle, rw, 1);
                if (tex != IntPtr.Zero)
                {
                    _ = Sdl.Renderer.QueryTexture(tex, out _, out _, out int w, out int h);
                    SdlTexture texture = new(this, w, h, tex) { Name = path };
                    OnResourceCreated(texture);
                    return texture;
                }
            }
            return null;
        }

        public override void PushTarget(Texture? texture)
        {
            if (CheckTexture(texture, out IntPtr target))
            {
                IntPtr oldTarget = Sdl.Renderer.GetRenderTarget(handle);
                prevTargets.Push(oldTarget);
                _ = Sdl.Renderer.SetRenderTarget(handle, target);
                _ = Sdl.Renderer.SetRenderDrawBlendMode(handle, (int)blendMode);
                _ = Sdl.Renderer.SetRenderDrawColor(handle, colorR, colorG, colorB, colorA);
            }
        }
        public override void PopTarget()
        {
            if (prevTargets.Count > 0)
            {
                IntPtr oldTarget = prevTargets.Pop();
                _ = Sdl.Renderer.SetRenderTarget(handle, oldTarget);
                _ = Sdl.Renderer.SetRenderDrawBlendMode(handle, (int)blendMode);
                _ = Sdl.Renderer.SetRenderDrawColor(handle, colorR, colorG, colorB, colorA);
            }
        }

        private Rectangle CombineClip(Rectangle clip)
        {
            if (prevClips.Count > 0)
            {
                Rectangle current = prevClips.Peek();
                return Rectangle.Intersect(current, clip);
            }
            return clip;
        }
        public override void PushClip(Rectangle clip)
        {
            clip = CombineClip(clip);
            _ = Sdl.Renderer.RenderSetClipRect(handle, ref clip);
            prevClips.Push(clip);

        }

        public override void PopClip()
        {
            if (prevClips.Count > 0) { _ = prevClips.Pop(); }
            if (prevClips.Count > 0)
            {
                Rectangle clip = prevClips.Peek();
                _ = Sdl.Renderer.RenderSetClipRect(handle, ref clip);
            }
            else
            {
                _ = Sdl.Renderer.RenderSetClipRect(handle, IntPtr.Zero);
            }

        }

        protected override void DrawTexture(Texture? texture, ref Rectangle src, ref Rectangle dst)
        {
            if (CheckTexture(texture, out IntPtr tex))
            {
                _ = Sdl.Renderer.RenderCopy(handle, tex, ref src, ref dst);
            }
        }

        public override void DrawLine(int x1, int y1, int x2, int y2)
        {
            _ = Sdl.Renderer.RenderDrawLine(handle, x1, y1, x2, y2);
        }

        protected override void FillRect(ref Rectangle rect)
        {
            _ = Sdl.Renderer.RenderFillRect(handle, ref rect);
        }

        public override void FillColorRect(ref Rectangle rect, Color colorTopLeft, Color colorTopRight, Color colorBottomLeft, Color colorBottomRight)
        {
            rectVertices[0].color = ToSDLColor(colorTopLeft);
            rectVertices[0].position.X = rect.X;
            rectVertices[0].position.Y = rect.Y;
            rectVertices[1].color = ToSDLColor(colorTopRight);
            rectVertices[1].position.X = rect.Right - 1;
            rectVertices[1].position.Y = rect.Y;
            rectVertices[2].color = ToSDLColor(colorBottomLeft);
            rectVertices[2].position.X = rect.X;
            rectVertices[2].position.Y = rect.Bottom - 1;
            rectVertices[3].color = ToSDLColor(colorBottomRight);
            rectVertices[3].position.X = rect.Right - 1;
            rectVertices[3].position.Y = rect.Bottom - 1;
            _ = Sdl.Renderer.RenderGeometry(handle, IntPtr.Zero, rectVertices, 4, rectIndices, NUM_RECT_INDICES);
        }

        protected override void DrawRect(ref Rectangle rect)
        {
            _ = Sdl.Renderer.RenderDrawRect(handle, ref rect);
        }

        protected override void DrawText(TextFont? font, ReadOnlySpan<char> text, float x, float y, float width, float height, Color color, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, float offsetX, float offsetY)
        {
            if (text == null) return;
            if (text.Length == 0) return;
            if (font == null) { font = defaultFont; }
            if (CheckTextFont(font, out IntPtr fnt))
            {
                DrawTextCache(GetTextCache(fnt, text, color), x, y, width, height, horizontalAlignment, verticalAlignment, offsetX, offsetY);
            }
        }

        protected override void DrawIcon(Icons icon, float x, float y, float width, float height, Color color, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, float offsetX, float offsetY)
        {
            if (icon == Icons.NONE) return;
            TextFont? font = iconFont;
            if (CheckTextFont(font, out IntPtr fnt))
            {
                DrawIconCache(GetIconCache(fnt, icon, color), x, y, width, height, horizontalAlignment, verticalAlignment, offsetX, offsetY);
            }
        }


        protected override void SetDrawColor(byte r, byte g, byte b, byte a)
        {
            Sdl.Renderer.SetRenderDrawColor(handle, r, g, b, a);
        }

        protected override void SetDrawBlendMode(BlendMode blendMode)
        {
            Sdl.Renderer.SetRenderDrawBlendMode(handle, (int)blendMode);
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
                Log($"Text cache limit {textCacheLimit} reached. Cleaning up...");
                ClearTextCache(halfKeys);
            }
        }

        private void CheckIconCache()
        {
            if (iconCache.Count > iconCacheLimit)
            {
                int len = iconCacheKeys.Count / 2;
                var halfKeys = iconCacheKeys.GetRange(0, len);
                iconCacheKeys.RemoveRange(0, len);
                Log($"Icon cache limit {iconCacheLimit} reached. Cleaning up...");
                ClearIconCache(halfKeys);
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

        private void ClearIconCache()
        {
            foreach (var kvp in iconCache)
            {
                IconCache tc = kvp.Value;
                Sdl.Renderer.DestroyTexture(tc.Texture);
            }
            iconCache.Clear();
        }

        private void ClearIconCache(IEnumerable<int> keys)
        {
            foreach (var key in keys)
            {
                if (iconCache.TryGetValue(key, out var tc))
                {
                    if (iconCache.Remove(key))
                    {
                        Sdl.Renderer.DestroyTexture(tc.Texture);
                    }
                }
            }
        }
        private void DrawTextCache(TextCache? textCache, float x, float y, float width, float height, HorizontalAlignment hAlign, VerticalAlignment vAlign, float offsetX, float offsetY)
        {
            if (textCache == null) return;
            if (textCache.Texture == IntPtr.Zero) return;
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
            SetBlendMode(BlendMode.Blend);
            Sdl.Renderer.RenderCopyF(handle, textCache.Texture, x + offsetX, y + offsetY, w, h);
        }

        private void DrawIconCache(IconCache? iconCache, float x, float y, float width, float height, HorizontalAlignment hAlign, VerticalAlignment vAlign, float offsetX, float offsetY)
        {
            if (iconCache == null) return;
            if (iconCache.Texture == IntPtr.Zero) return;
            int w = iconCache.Width;
            int h = iconCache.Height;
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
            //RectangleF dstRect = new RectangleF(x + offsetX, y + offsetY, w, h);
            SetBlendMode(BlendMode.Blend);
            Sdl.Renderer.RenderCopyF(handle, iconCache.Texture, x + offsetX, y + offsetY, w, h);
        }


        private TextCache? GetTextCache(IntPtr font, ReadOnlySpan<char> text, Color color)
        {
            int hash = string.GetHashCode(text);
            int key = HashCode.Combine(font, hash, color);
            CheckTextCache();
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

        private IconCache? GetIconCache(IntPtr font, Icons icon, Color color)
        {
            int key = HashCode.Combine(icon, color);
            CheckIconCache();
            if (iconCache.TryGetValue(key, out var ic))
            {
                if (ic.Matches(icon, color)) { return ic; }
            }
            ic = CreateIconCache(font, icon, color);
            if (ic != null)
            {
                iconCache[key] = ic;
                iconCacheKeys.Add(key);
            }
            return ic;
        }

        private TextCache? CreateTextCache(IntPtr font, ReadOnlySpan<char> text, Color color)
        {
            TextCache? textCache = null;
            if (font != IntPtr.Zero)
            {
                stringBuffer.Clear();
                stringBuffer.Append(text);
                int hash = string.GetHashCode(text);
                IntPtr surface = SDL2TTF.RenderUTF8_Blended(font, stringBuffer, ToSDLColor(color));
                if (surface != IntPtr.Zero)
                {
                    IntPtr texHandle = Sdl.Renderer.CreateTextureFromSurface(handle, surface);
                    if (texHandle != IntPtr.Zero)
                    {
                        _ = Sdl.Renderer.QueryTexture(texHandle, out _, out _, out int w, out int h);
                        _ = Sdl.Renderer.SetTextureBlendMode(texHandle, Sdl.Renderer.SDL_BlendMode.SDL_BLENDMODE_BLEND);
                        _ = Sdl.Renderer.SetTextureAlphaMod(texHandle, color.A);
                        _ = Sdl.Renderer.SetTextureScaleMode(texHandle, Sdl.Renderer.SDL_ScaleMode.SDL_ScaleModeBest);
                        textCache = new TextCache(font, hash, color, w, h, texHandle);
                    }
                    Sdl.FreeSurface(surface);
                }
            }
            return textCache;
        }

        private IconCache? CreateIconCache(IntPtr font, Icons icon, Color color)
        {
            IconCache? iconCache = null;
            if (font != IntPtr.Zero)
            {
                IntPtr surface = SDL2TTF.RenderGlyph32_Blended(font, (uint)icon, ToSDLColor(color));
                if (surface != IntPtr.Zero)
                {
                    IntPtr texHandle = Sdl.Renderer.CreateTextureFromSurface(handle, surface);
                    if (texHandle != IntPtr.Zero)
                    {
                        if (texHandle != IntPtr.Zero)
                        {
                            _ = Sdl.Renderer.QueryTexture(texHandle, out _, out _, out int w, out int h);
                            _ = Sdl.Renderer.SetTextureBlendMode(texHandle, Sdl.Renderer.SDL_BlendMode.SDL_BLENDMODE_BLEND);
                            _ = Sdl.Renderer.SetTextureAlphaMod(texHandle, color.A);
                            _ = Sdl.Renderer.SetTextureScaleMode(texHandle, Sdl.Renderer.SDL_ScaleMode.SDL_ScaleModeBest);
                            iconCache = new IconCache(icon, color, w, h, texHandle);
                        }
                    }
                    Sdl.FreeSurface(surface);
                }
            }
            return iconCache;
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

        private class IconCache
        {

            internal IconCache(Icons icon, Color color, int width, int height, IntPtr texture)
            {
                Icon = icon;
                Color = color;
                Width = width;
                Height = height;
                Texture = texture;
            }

            public Icons Icon;
            public Color Color;
            public int Width;
            public int Height;
            public IntPtr Texture;

            public bool Matches(Icons icon, Color color)
            {
                return (icon == Icon) && (color == Color);
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
