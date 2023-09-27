namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class DefaultGuiRenderer : IGuiRenderer
    {
        private readonly Game game;

        public DefaultGuiRenderer(Game game)
        {
            this.game = game;
        }

        public void DrawControl(Control control, int offsetX = 0, int offsetY = 0)
        {
            //GraphicsDevice? gd = game.GraphicsDevice;
            //if (gd != null)
            //{
            //    Rectangle bounds = control.GetBounds();
            //    Color fg = Color.LightGray;
            //    if (control.Focused)
            //    {
            //        fg = Color.White;
            //    }
            //    else if (control.Hovered)
            //    {
            //        fg = Color.Gray;
            //    }
            //    gd.Color = fg;
            //    gd.DrawRect(bounds);
            //}
        }

        public void DrawText(TextFont? font, string? text, Rectangle rect, Color color, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center, VerticalAlignment verticalAlignment = VerticalAlignment.Center, Rectangle? clippingRect = null)
        {
            GraphicsDevice? gd = game.GraphicsDevice;
            if (gd != null && font != null && !string.IsNullOrEmpty(text))
            {
                gd.BlendMode = BlendMode.Blend;
                gd.DrawText(font, text, rect, color, horizontalAlignment, verticalAlignment);
            }
        }

        public void FillRectangle(Rectangle rect, Color color, Rectangle? clippingRect = null)
        {
            GraphicsDevice? gd = game.GraphicsDevice;
            if (gd != null)
            {
                gd.BlendMode = BlendMode.Blend;
                gd.Color = color;
                gd.FillRect(rect);
            }
        }

        public void DrawRectangle(Rectangle rect, Color color, float thickness = 1.0f, Rectangle? clippingRect = null)
        {
            GraphicsDevice? gd = game.GraphicsDevice;
            if (gd != null)
            {
                gd.BlendMode = BlendMode.Blend;
                gd.Color = color;
                gd.DrawRect(rect);
            }
        }

    }
}
