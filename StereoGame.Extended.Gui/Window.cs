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
                if (y > bounds.Top && y < bounds.Top + Padding.Top)
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
                renderer.DrawText(Font, Title, bounds, TextColor);
            }
        }
    }
}
