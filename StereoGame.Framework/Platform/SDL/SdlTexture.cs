namespace StereoGame.Framework.Platform.SDL
{
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class SdlTexture : Texture
    {
        private readonly IntPtr handle;
        internal SdlTexture(SdlGraphicsDevice graphicsDevice, int width, int height, IntPtr handle)
            : base(graphicsDevice, width, height)
        {
            this.handle = handle;
            _ = Sdl.Renderer.GetTextureScaleMode(handle, out var sm);
            _ = Sdl.Renderer.GetTextureBlendMode(handle, out var bm);
            _ = Sdl.Renderer.GetTextureColorMod(handle, out var r, out var g, out var b);
            _ = Sdl.Renderer.GetTextureAlphaMod(handle, out var a);
            textureFilter = (TextureFilter)sm;
            blendMode = (BlendMode)bm;
            colorMod = Color.FromArgb(r, g, b);
            alphaMod = a;
        }

        public IntPtr Handle => handle;

        protected override void SetTextureFilter(TextureFilter value)
        {
            base.SetTextureFilter(value);
            Sdl.Renderer.SetTextureScaleMode(handle, (Sdl.Renderer.SDL_ScaleMode)value);
        }

        protected override void SetColorMod(Color value)
        {
            base.SetColorMod(value);
            Sdl.Renderer.SetTextureColorMod(handle, value.R, value.G, value.B);
        }

        protected override void SetAlphaMod(byte value)
        {
            base.SetAlphaMod(value);
            Sdl.Renderer.SetTextureAlphaMod(handle, value);
        }

        protected override void SetBlendMode(BlendMode value)
        {
            base.SetBlendMode(value);
            Sdl.Renderer.SetTextureBlendMode(handle, (Sdl.Renderer.SDL_BlendMode)value);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Sdl.Renderer.DestroyTexture(handle);
        }
    }
}
