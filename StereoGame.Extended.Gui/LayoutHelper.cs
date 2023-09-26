using StereoGame.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Extended.Gui
{
    public static class LayoutHelper
    {
        public static void PlaceControl(IGuiSystem context, Control control, float x, float y, float width, float height)
        {
            var rectangle = new Rectangle((int)x, (int)y, (int)width, (int)height);
            var desiredSize = control.CalculateActualSize(context);
            var alignedRectangle = AlignRectangle(control.HorizontalAlignment, control.VerticalAlignment, desiredSize, rectangle);

            control.X = control.Margin.Left + alignedRectangle.X;
            control.Y = control.Margin.Top + alignedRectangle.Y;
            //control.Position = new Point(control.Margin.Left + alignedRectangle.X, control.Margin.Top + alignedRectangle.Y);

            //control.ActualWidth = alignedRectangle.Width - control.Margin.Horizontal;
            //control.ActualHeight = alignedRectangle.Height - control.Margin.Vertical;
            //control.ActualSize = (Size)alignedRectangle.Size - control.Margin.Size;
            control.InvalidateLayout();
        }

        public static Rectangle AlignRectangle(HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Size size, Rectangle targetRectangle)
        {
            var x = GetHorizontalPosition(horizontalAlignment, size, targetRectangle);
            var y = GetVerticalPosition(verticalAlignment, size, targetRectangle);
            var width = horizontalAlignment == HorizontalAlignment.Stretch ? targetRectangle.Width : size.Width;
            var height = verticalAlignment == VerticalAlignment.Stretch ? targetRectangle.Height : size.Height;

            return new Rectangle(x, y, width, height);
        }

        public static int GetHorizontalPosition(HorizontalAlignment horizontalAlignment, Size size, Rectangle targetRectangle)
        {
            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Stretch:
                case HorizontalAlignment.Left:
                    return targetRectangle.X;
                case HorizontalAlignment.Right:
                    return targetRectangle.Right - size.Width;
                case HorizontalAlignment.Center:
                    return targetRectangle.X + targetRectangle.Width / 2 - size.Width / 2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(horizontalAlignment), horizontalAlignment, $"{horizontalAlignment} is not supported");
            }
        }

        public static int GetVerticalPosition(VerticalAlignment verticalAlignment, Size size, Rectangle targetRectangle)
        {
            switch (verticalAlignment)
            {
                case VerticalAlignment.Stretch:
                case VerticalAlignment.Top:
                    return targetRectangle.Y;
                case VerticalAlignment.Bottom:
                    return targetRectangle.Bottom - size.Height;
                case VerticalAlignment.Center:
                    return targetRectangle.Y + targetRectangle.Height / 2 - size.Height / 2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(verticalAlignment), verticalAlignment, $"{verticalAlignment} is not supported");
            }
        }
    }
}
