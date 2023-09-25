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
            : base(null)

        {

        }
        public Window(Screen? screen)
            : base(screen)
        {

        }

        protected override void Layout(IGuiSystem gui, Rectangle rect)
        {
            
        }
    }
}
