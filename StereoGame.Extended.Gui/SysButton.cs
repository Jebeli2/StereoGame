using StereoGame.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Extended.Gui
{
    public class SysButton : Button
    {
        public SysButton()
            : this(null, Icons.NONE)
        {

        }

        public SysButton(Control? parent, Icons icon = Icons.NONE)
            : base(parent, null, icon)
        {

        }
    }
}
