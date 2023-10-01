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
        protected LayoutControl(Control? parent = null, ITheme? theme = null) : base(parent, theme)
        {
        }

        protected override void DrawControl(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, ref Rectangle bounds)
        {
            Theme.DrawLayoutControl(gui, renderer, gameTime, this, ref bounds);
        }
    }
}
