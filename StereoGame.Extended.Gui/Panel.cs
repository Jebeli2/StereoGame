using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Extended.Gui
{
    public class Panel : LayoutControl
    {
        public Panel(Control? parent)
            : base(parent)
        {
            layout = new BoxLayout(Orientation.Horizontal, Alignment.Fill, 0, 10);
        }


    }
}
