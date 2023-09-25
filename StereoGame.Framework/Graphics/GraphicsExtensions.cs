using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Graphics
{
    public static class GraphicsExtensions
    {
        public static void DrawTextureRegion(this GraphicsDevice device, TextureRegion region, int x, int y)
        {
            device.DrawTexture(region.Texture, region.X, region.Y, region.Width, region.Height, x, y);
        }
    }
}
