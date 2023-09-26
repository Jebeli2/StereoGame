namespace StereoGame.Extended.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Window : LayoutControl
    {
        public Window()
            : this(null)

        {

        }
        public Window(Screen? screen)
            : base(screen)
        {
            layout = new BoxLayout(Orientation.Vertical, Alignment.Fill, 10, 10);
        }

        public override Size GetContentSize(IGuiSystem context)
        {
            return new Size(Width, Height);
        }
    }
}
