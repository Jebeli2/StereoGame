namespace StereoGame.Extended.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Window : Control
    {
        public Window()
            : base(null)

        {

        }
        public Window(Screen? screen)
            : base(screen)
        {

        }
    }
}
