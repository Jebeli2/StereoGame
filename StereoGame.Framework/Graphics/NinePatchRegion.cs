using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Graphics
{
    public class NinePatchRegion : TextureRegion
    {
        public const int TopLeft = 0;
        public const int TopMiddle = 1;
        public const int TopRight = 2;
        public const int MiddleLeft = 3;
        public const int Middle = 4;
        public const int MiddleRight = 5;
        public const int BottomLeft = 6;
        public const int BottomMiddle = 7;
        public const int BottomRight = 8;

        private readonly Padding padding;
        private readonly Rectangle[] sourcePatches = new Rectangle[9];
        private readonly Rectangle[] destinationPatches = new Rectangle[9];

        public NinePatchRegion(TextureRegion textureRegion, Padding padding)
            : base(textureRegion.Texture, textureRegion.X, textureRegion.Y, textureRegion.Width, textureRegion.Height)
        {
            this.padding = padding;
            CachePatches(textureRegion.Bounds, sourcePatches);
        }

        public NinePatchRegion(Texture texture, Padding padding)
            : this(new TextureRegion(texture), padding)
        {

        }

        public Rectangle[] SourcePatches => sourcePatches;

        public Rectangle[] CreatePatches(Rectangle rectangle)
        {
            CachePatches(rectangle, destinationPatches);
            return destinationPatches;
        }

        private int LeftPadding => padding.Left;
        private int TopPadding => padding.Top;
        private int RightPadding => padding.Right;
        private int BottomPadding => padding.Bottom;
        private void CachePatches(Rectangle sourceRectangle, Rectangle[] patchCache)
        {
            var x = sourceRectangle.X;
            var y = sourceRectangle.Y;
            var w = sourceRectangle.Width;
            var h = sourceRectangle.Height;
            var middleWidth = w - LeftPadding - RightPadding;
            var middleHeight = h - TopPadding - BottomPadding;
            var bottomY = y + h - BottomPadding;
            var rightX = x + w - RightPadding;
            var leftX = x + LeftPadding;
            var topY = y + TopPadding;

            patchCache[TopLeft] = new Rectangle(x, y, LeftPadding, TopPadding);
            patchCache[TopMiddle] = new Rectangle(leftX, y, middleWidth, TopPadding);
            patchCache[TopRight] = new Rectangle(rightX, y, RightPadding, TopPadding);
            patchCache[MiddleLeft] = new Rectangle(x, topY, LeftPadding, middleHeight);
            patchCache[Middle] = new Rectangle(leftX, topY, middleWidth, middleHeight);
            patchCache[MiddleRight] = new Rectangle(rightX, topY, RightPadding, middleHeight);
            patchCache[BottomLeft] = new Rectangle(x, bottomY, LeftPadding, BottomPadding);
            patchCache[BottomMiddle] = new Rectangle(leftX, bottomY, middleWidth, BottomPadding);
            patchCache[BottomRight] = new Rectangle(rightX, bottomY, RightPadding, BottomPadding);
        }
    }
}
