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
        public const int ICONWIDTH = 20;
        public const int ICONHEIGHT = 20;
        public Button(string? text = null)
            : this(null, text)
        {

        }
        public Button(Control? parent, string? text = null)
            : base(parent)
        {
            Text = text;
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

        public override void Draw(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, int offsetX = 0, int offsetY = 0)
        {
            base.Draw(gui, renderer, gameTime, offsetX, offsetY);
            if (!string.IsNullOrEmpty(Text) && Icon != Icons.NONE)
            {
                Rectangle bounds = GetBounds();
                bounds.Offset(offsetX, offsetY);
                Rectangle textBounds = bounds;
                Rectangle iconBounds = bounds;
                iconBounds.Width = 2 * ICONWIDTH;
                textBounds.X += 2 * ICONWIDTH;
                textBounds.Width -= 2 * ICONWIDTH;
                renderer.DrawText(Font, Text, textBounds, TextColor, HorizontalTextAlignment, VerticalTextAlignment);
                renderer.DrawIcon(Icon, iconBounds, TextColor, HorizontalTextAlignment, VerticalTextAlignment);
            }
            else if (!string.IsNullOrEmpty(Text))
            {
                Rectangle bounds = GetBounds();
                bounds.Offset(offsetX, offsetY);
                renderer.DrawText(Font, Text, bounds, TextColor, HorizontalTextAlignment, VerticalTextAlignment);
            }
            else if (Icon != Icons.NONE)
            {
                Rectangle bounds = GetBounds();
                bounds.Offset(offsetX, offsetY);
                renderer.DrawIcon(Icon, bounds, TextColor, HorizontalTextAlignment, VerticalTextAlignment);
            }
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
            if (Icon != Icons.NONE)
            {
                size.Width += 2 * ICONWIDTH;
                size.Height = Math.Max(size.Height, ICONHEIGHT);
            }
            return size;
        }

        public override Size GetPreferredSize(IGuiSystem context)
        {
            Size ct = GetContentSize(context);
            return ct + Padding.Size;
        }

        public void Click()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
