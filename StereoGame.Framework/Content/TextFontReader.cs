using StereoGame.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Content
{
    internal class TextFontReader : AssetReader<TextFont>
    {
        public TextFontReader()
            : base("TextFont Reader")
        {

        }

        public override TextFont? ReadAsset(string name, byte[]? data, object? parameter = null)
        {
            var plat = GetPlatform();
            if (plat != null && parameter is int ySize)
            {
                if (data != null)
                {
                    return plat.LoadFont(name, data, ySize);
                }
                else
                {
                    return plat.LoadFont(name, ySize);
                }
            }
            return null;
        }
    }
}
