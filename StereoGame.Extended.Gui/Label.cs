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
            Size ct = GetContentSize(context);
            return ct + Padding.Size;
        }


    }
}
