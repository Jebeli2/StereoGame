namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Window : LayoutControl
    {
        public Window()
            : this(null)

        {

        }
        public Window(Screen? screen)
            : base(screen)
        {
            superBitmap = true;
            layout = new BoxLayout(Orientation.Vertical, Alignment.Fill, 10, 10);
        }

        public string? Title
        {
            get => Text;
            set => Text = value;
        }



        public override Size GetContentSize(IGuiSystem context)
        {
            return new Size(Width, Height);
        }

        public override HitTestResult GetHitTestResult(int x, int y)
        {
            Rectangle bounds = GetBounds();
            if (bounds.Contains(x, y))
            {
                if (y >= bounds.Bottom - Padding.Bottom && x <= bounds.Left + Padding.Left)
                {
                    return HitTestResult.SizeBottomLeft;
                }
                else if (y >= bounds.Bottom - Padding.Bottom && x >= bounds.Right - Padding.Right)
                {
                    return HitTestResult.SizeBottomRight;
                }
                else if (x <= bounds.Left + Padding.Left && y <= bounds.Top + Padding.Bottom)
                {
                    return HitTestResult.SizeTopLeft;
                }
                else if (x >= bounds.Right - Padding.Right && y <= bounds.Top + Padding.Bottom)
                {
                    return HitTestResult.SizeTopRight;
                }
                else if (y >= bounds.Bottom - Padding.Bottom)
                {
                    return HitTestResult.SizeBottom;
                }
                else if (y <= bounds.Top + Padding.Bottom)
                {
                    return HitTestResult.SizeTop;
                }
                else if (x <= bounds.Left + Padding.Left)
                {
                    return HitTestResult.SizeLeft;
                }
                else if (x >= bounds.Right - Padding.Right)
                {
                    return HitTestResult.SizeRight;
                }
                else if (y > bounds.Top + 2 && y < bounds.Top + Padding.Top)
                {
                    return HitTestResult.DragArea;
                }
                return HitTestResult.Control;
            }
            return HitTestResult.None;
        }

        public override void Draw(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, int offsetX = 0, int offsetY = 0)
        {
            base.Draw(gui, renderer, gameTime, offsetX, offsetY);
            if (!string.IsNullOrEmpty(Title))
            {
                Rectangle bounds = GetBounds();
                bounds.Offset(offsetX, offsetY);
                bounds.Height = Padding.Top;
                bounds.Inflate(-1, -1);
                Rectangle textRect = bounds;
                textRect.X += 20;
                textRect.Width -= 20;
                renderer.FillRectangle(bounds, BorderColor);
                renderer.DrawText(Font, Title, textRect, TextColor, Framework.Graphics.HorizontalAlignment.Left);
                renderer.DrawHorizontalLine(bounds.Left, bounds.Right - 1, bounds.Bottom, BorderShadowColor);
            }
        }
    }
}
