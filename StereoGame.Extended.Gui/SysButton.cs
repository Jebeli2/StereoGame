using StereoGame.Framework;
using StereoGame.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        protected override void DrawControl(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, ref Rectangle bounds)
        {
            Theme.DrawSysButton(gui, renderer, gameTime, this, ref bounds);
        }
    }
}
