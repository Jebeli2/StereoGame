﻿namespace StereoGame.Framework.Platform.SDL
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


        public SdlGraphicsDevice(Game game, SdlGameWindow window)
        {
            this.game = game;
            this.window = window;
            handle = Sdl.Renderer.Create(window.Handle, 0, Sdl.Renderer.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
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
    }
}
