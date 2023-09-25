using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Graphics
{
    public class TextureRegion
    {
        private readonly Texture texture;
        private readonly int x;
        private readonly int y;
        private readonly int width;
        private readonly int height;

        public TextureRegion(Texture texture)
            : this(texture, 0, 0, texture.Width, texture.Height)
        {

        }
        public TextureRegion(Texture texture, Rectangle rect)
            : this(texture, rect.X, rect.Y, rect.Width, rect.Height)
        {

        }
        public TextureRegion(Texture texture, int x, int y, int width, int height)
        {
            this.texture = texture;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public Texture Texture => texture;
        public int X => x;
        public int Y => y;
        public int Width => width;
        public int Height => height;

        public Rectangle Bounds => new Rectangle(x, y, width, height);
    }
}
