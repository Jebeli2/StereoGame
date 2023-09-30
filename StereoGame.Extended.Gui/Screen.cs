namespace StereoGame.Extended.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Screen : LayoutControl
    {

        public Screen()
            : base(null)
        {

        }

        public override Size GetPreferredSize(IGuiSystem context)
        {
            return new Size(context.ScreenWidth, context.ScreenHeight);
        }
        public void WindowToFront(Window window)
        {
            ToFront(window);
        }

        public void WindowToBack(Window window)
        {
            ToBack(window);
        }

        public void InvalidateWindows()
        {
            foreach (Control c in Children)
            {
                if (c is Window window)
                {
                    if (window.Maximized)
                    {
                        window.SetFixedBounds(0, 0, Width, Height);
                    }
                    window.Invalidate();
                }
            }
        }
    }
}
