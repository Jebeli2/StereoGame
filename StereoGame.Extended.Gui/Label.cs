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

        private Size CalcContentSize()
        {
            Size size = Size.Empty;
            Size? textSize = Font?.MeasureText(Text);
            if (textSize != null)
            {
                size.Width += textSize.Value.Width;
                size.Height += textSize.Value.Height;
            }
            if (Icon != Icons.NONE || alwaysUseIconSpace)
            {
                size.Width += (ICONWIDTH * 3) / 2;
                size.Height = Math.Max(size.Height, ICONHEIGHT);
            }
            return size;
        }

        public override Size GetPreferredSize(IGuiSystem context)
        {
            Size ct = CalcContentSize();
            return ct + Padding.Size;
        }

        protected override void DrawControl(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, ref Rectangle bounds)
        {
            Theme.DrawLabel(gui, renderer, gameTime, this, ref bounds);
        }


    }
}
