using StereoGame.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Content
{
    internal class TextureReader : AssetReader<Texture>
    {
        public TextureReader()
            : base("Texture Reader")
        {

        }

        public override Texture? ReadAsset(string name, byte[]? data, object? parameter = null)
        {
            var gd = GetGraphicsDevice();
            if (gd != null)
            {
                if (data == null)
                {
                    return gd.LoadTexture(name);
                }
                else
                {
                    return gd.LoadTexture(name, data);
                }
            }
            return null;
        }
    }
}
