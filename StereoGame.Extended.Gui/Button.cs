namespace StereoGame.Extended.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Button : Control
    {
        private bool pointerDown;
        public Button()
            : base(null)
        {

        }
        public Button(Control? parent)
            : base(parent)
        {
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
    }
}
