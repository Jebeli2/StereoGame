using StereoGame.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Extended.Gui
{
    public abstract class LayoutControl : Control
    {
        private bool isLayoutValid;
        protected LayoutControl(Control? parent) : base(parent)
        {
            isLayoutValid = false;
        }

        public override void InvalidateLayout()
        {
            isLayoutValid = false;
        }

        public override void Update(IGuiSystem gui, GameTime gameTime)
        {
            base.Update(gui, gameTime);
            if (!isLayoutValid)
            {
                Layout(gui, new Rectangle(0, 0, Width, Height));
                isLayoutValid = true;
            }
        }

        protected abstract void Layout(IGuiSystem gui, Rectangle rect);
    }
}
