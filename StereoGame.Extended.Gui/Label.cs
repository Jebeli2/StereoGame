using StereoGame.Framework;
using StereoGame.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Extended.Gui
{
    public class Label : Control
    {
        public Label(string? text = null)
            : this(null, text)
        {

        }

        public Label(Control? parent, string? text = null)
            : base(parent)
        {
            Text = text;
            HorizontalTextAlignment = HorizontalAlignment.Left;
            VerticalTextAlignment = VerticalAlignment.Top;
        }

        public override Size GetContentSize(IGuiSystem context)
        {
            Size? size = Font?.MeasureText(Text);
            return size ?? Size.Empty;
        }

        public override Size GetPreferredSize(IGuiSystem context)
        {
            Size ct = GetContentSize(context);
            return ct + Padding.Size;
        }

        public override void Draw(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, int offsetX = 0, int offsetY = 0)
        {
            base.Draw(gui, renderer, gameTime, offsetX, offsetY);
            if (!string.IsNullOrEmpty(Text))
            {
                Rectangle bounds = GetBounds();
                bounds.Offset(offsetX, offsetY);
                renderer.DrawText(Font, Text, bounds, TextColor, HorizontalTextAlignment, VerticalTextAlignment);
            }
        }

    }
}
