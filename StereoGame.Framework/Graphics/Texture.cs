namespace StereoGame.Framework.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class Texture : GraphicsResource
    {
        private readonly int width;
        private readonly int height;
        protected Texture(GraphicsDevice graphicsDevice, int width, int height)
        {
            GraphicsDevice = graphicsDevice;
            this.width = width;
            this.height = height;
        }

        public int Width => width;
        public int Height => height;
    }
}
