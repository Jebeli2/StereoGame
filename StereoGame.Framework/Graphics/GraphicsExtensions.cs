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
        public static void DrawTextureRegion(this GraphicsDevice device, TextureRegion region, Rectangle dest)
        {
            if (region is NinePatchRegion patch)
            {
                DrawNinePatch(device, patch, dest);
            }
            else
            {
                device.DrawTexture(region.Texture, region.Bounds, dest);
            }
        }

        public static void DrawNinePatch(this GraphicsDevice device, NinePatchRegion patch, Rectangle dest)
        {
            var dstPatches = patch.CreatePatches(dest);
            var srcPatches = patch.SourcePatches;
            for (int i = 0; i < srcPatches.Length; i++)
            {
                var srcPatch = srcPatches[i];
                var dstPatch = dstPatches[i];
                if (dstPatch.Width > 0 && dstPatch.Height > 0)
                {
                    device.DrawTexture(patch.Texture, srcPatch, dstPatch);
                }
            }
        }

        public static void FillVertGradient(this GraphicsDevice device, Rectangle rect, Color top, Color bottom)
        {
            device.FillColorRect(ref rect, top, top, bottom, bottom);
        }
    }
}
