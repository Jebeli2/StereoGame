namespace StereoGame.Framework.Platform.SDL
{
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
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
        }

        public IntPtr Handle => handle;
    }
}
