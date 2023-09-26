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

    public class Button : Control
    {
        private bool pointerDown;
        public Button(string? text = null)
            : this(null, text)
        {

        }
        public Button(Control? parent, string? text = null)
            : base(parent)
        {
            Text = text;
        }

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

        public override void Draw(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime)
        {
            base.Draw(gui, renderer, gameTime);
            if (!string.IsNullOrEmpty(Text))
            {
                renderer.DrawText(Font, Text, GetBounds(), TextColor, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
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
    }
}
