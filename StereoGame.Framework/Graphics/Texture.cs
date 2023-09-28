namespace StereoGame.Framework.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection.Emit;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class Texture : GraphicsResource
    {
        private readonly int width;
        private readonly int height;

        protected TextureFilter textureFilter;
        protected byte alphaMod;
        protected Color colorMod;
        protected BlendMode blendMode;
        protected Texture(GraphicsDevice graphicsDevice, int width, int height)
            : base(graphicsDevice)
        {
            this.width = width;
            this.height = height;
        }

        public int Width => width;
        public int Height => height;
        public TextureFilter TextureFilter
        {
            get { return textureFilter; }
            set
            {
                if (textureFilter != value)
                {
                    SetTextureFilter(value);
                }
            }
        }

        public Color ColorMod
        {
            get { return colorMod; }
            set
            {
                if (colorMod != value)
                {
                    SetColorMod(value);
                }
            }
        }

        public byte AlphaMod
        {
            get { return alphaMod; }
            set
            {
                if (alphaMod != value)
                {
                    SetAlphaMod(value);
                }
            }
        }

        public BlendMode BlendMode
        {
            get { return blendMode; }
            set
            {
                if (blendMode != value)
                {
                    SetBlendMode(value);
                }
            }
        }

        protected virtual void SetTextureFilter(TextureFilter value)
        {
            textureFilter = value;
        }

        protected virtual void SetColorMod(Color value)
        {
            colorMod = value;
        }

        protected virtual void SetAlphaMod(byte value)
        {
            alphaMod = value;
        }

        protected virtual void SetBlendMode(BlendMode value)
        {
            blendMode = value;
        }
    }
}
