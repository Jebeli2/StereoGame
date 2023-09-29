namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static System.Net.Mime.MediaTypeNames;

    internal class DefaultGuiRenderer : IGuiRenderer
    {
        private readonly Game game;
        private GraphicsDevice? gd;

        public DefaultGuiRenderer(Game game)
        {
            this.game = game;
        }

        [MemberNotNullWhen(true, nameof(gd))]
        private bool CheckGraphicsDevice()
        {
            gd ??= game.GraphicsDevice;
            return gd != null;
        }

        public void DrawRegion(TextureRegion? region, Rectangle dest)
        {
            if (CheckGraphicsDevice() && region != null)
            {
                gd.BlendMode = BlendMode.Blend;
                gd.DrawTextureRegion(region, dest);
            }
        }

        public void DrawText(TextFont? font, string? text, Rectangle rect, Color color, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center, VerticalAlignment verticalAlignment = VerticalAlignment.Center, Rectangle? clippingRect = null)
        {
            if (CheckGraphicsDevice() && font != null && !string.IsNullOrEmpty(text))
            {
                gd.BlendMode = BlendMode.Blend;
                gd.DrawText(font, text, rect, color, horizontalAlignment, verticalAlignment);
            }
        }
        public void DrawIcon(Icons icon, Rectangle rect, Color color, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center, VerticalAlignment verticalAlignment = VerticalAlignment.Center, Rectangle? clippingRect = null)
        {
            if (CheckGraphicsDevice() && icon != Icons.NONE)
            {
                gd.BlendMode = BlendMode.Blend;
                gd.DrawIcon(icon, rect, color, horizontalAlignment, verticalAlignment);
            }
        }

        public void FillRectangle(Rectangle rect, Color color, Rectangle? clippingRect = null)
        {
            if (CheckGraphicsDevice())
            {
                gd.BlendMode = BlendMode.Blend;
                gd.Color = color;
                gd.FillRect(rect);
            }
        }

        public void DrawRectangle(Rectangle rect, Color color, float thickness = 1.0f, Rectangle? clippingRect = null)
        {
            if (CheckGraphicsDevice())
            {
                gd.BlendMode = BlendMode.Blend;
                gd.Color = color;
                gd.DrawRect(rect);
            }
        }

        public void DrawWindowBorder(Rectangle rect, Color color, Color shine, Color shadow, Padding border, Rectangle? clippingRect = null)
        {
            if (CheckGraphicsDevice())
            {
                Rectangle topRect = new Rectangle(rect.X, rect.Top, rect.Width, border.Top);
                Rectangle botRect = new Rectangle(rect.X, rect.Bottom - border.Bottom, rect.Width, border.Bottom);
                Rectangle leftRect = new Rectangle(rect.X, rect.Top + border.Top, border.Left, rect.Height - border.Vertical);
                Rectangle rightRect = new Rectangle(rect.Right - border.Right, rect.Top + border.Top, border.Right, rect.Height - border.Vertical);
                Rectangle inner = new Rectangle(rect.X + border.Left, rect.Top + border.Top, rect.Width - border.Horizontal, rect.Height - border.Vertical);
                gd.BlendMode = BlendMode.Blend;
                gd.Color = color;
                gd.FillRect(topRect);
                gd.FillRect(botRect);
                gd.FillRect(leftRect);
                gd.FillRect(rightRect);
                DrawBorder(rect, shine, shadow);
                DrawInnerBorder(inner, shadow, shine);
            }
        }

        private void DrawInnerBorder(Rectangle rect, Color shine, Color shadow, Rectangle? clippingRect = null)
        {
            if (CheckGraphicsDevice())
            {
                gd.BlendMode = BlendMode.Blend;
                gd.Color = shine;
                gd.DrawLine(rect.Left, rect.Top + 1, rect.Left, rect.Bottom - 1);
                gd.DrawLine(rect.Left, rect.Top, rect.Right - 1, rect.Top);
                gd.Color = shadow;
                gd.DrawLine(rect.Right - 1, rect.Top + 1, rect.Right - 1, rect.Bottom - 2);
                gd.DrawLine(rect.Left, rect.Bottom - 1, rect.Right - 1, rect.Bottom - 1);

            }
        }


        public void DrawBorder(Rectangle rect, Color shine, Color shadow, float thickness = 1.0f, Rectangle? clippingRect = null)
        {
            if (CheckGraphicsDevice())
            {
                gd.BlendMode = BlendMode.Blend;
                gd.Color = shine;
                gd.DrawLine(rect.Left, rect.Top + 1, rect.Left, rect.Bottom - 1);
                gd.DrawLine(rect.Left, rect.Top, rect.Right - 1, rect.Top);
                gd.Color = shadow;
                gd.DrawLine(rect.Right - 1, rect.Top + 1, rect.Right - 1, rect.Bottom - 2);
                gd.DrawLine(rect.Left, rect.Bottom - 1, rect.Right - 1, rect.Bottom - 1);

            }
        }


        public void DrawHorizontalLine(int x1, int x2, int y, Color color, float thickness = 1.0f, Rectangle? clippingRect = null)
        {
            if (CheckGraphicsDevice())
            {
                gd.BlendMode = BlendMode.Blend;
                gd.Color = color;
                gd.DrawLine(x1, y, x2, y);
            }
        }
        public void DrawVerticalLine(int x, int y1, int y2, Color color, float thickness = 1.0f, Rectangle? clippingRect = null)
        {
            if (CheckGraphicsDevice())
            {
                gd.BlendMode = BlendMode.Blend;
                gd.Color = color;
                gd.DrawLine(x, y1, x, y2);
            }
        }


    }
}
