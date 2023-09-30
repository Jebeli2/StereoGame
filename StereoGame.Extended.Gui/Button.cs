﻿namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Button : Control
    {
        private bool pointerDown;
        public Button(string? text = null)
            : this(null, text)
        {

        }
        public Button(Icons icon)
            : this(null, null, icon)
        {

        }
        public Button(Control? parent, Icons icon = Icons.NONE)
            : this(parent, null, icon)
        {
        }

        public Button(Control? parent, string? text = null, Icons icon = Icons.NONE)
            : base(parent)
        {
            Text = text;
            Icon = icon;
        }

        public event EventHandler<EventArgs>? Clicked;

        public override bool OnPointerDown(PointerEventArgs args)
        {
            if (Enabled)
            {
                pointerDown = true;
                Pressed = true;
            }
            return base.OnPointerDown(args);
        }

        public override bool OnPointerUp(PointerEventArgs args)
        {
            pointerDown = false;
            if (Pressed)
            {
                Pressed = false;
                if (BoundingRectangle.Contains(args.X, args.Y) && Enabled)
                {
                    Click();
                }
            }
            return base.OnPointerUp(args);
        }

        public override bool OnPointerEnter(PointerEventArgs args)
        {
            if (Enabled && pointerDown)
            {
                Pressed = true;
            }
            return base.OnPointerEnter(args);
        }

        public override bool OnPointerLeave(PointerEventArgs args)
        {
            if (Enabled)
            {
                Pressed = false;
            }
            return base.OnPointerLeave(args);
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
            if (layout != null)
            {
                return layout.GetPreferredSize(context, this);
            }
            Size ct = GetContentSize(context);
            return ct + Padding.Size;
        }

        public void Click()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
