namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class DefaultGuiRenderer : IGuiRenderer
    {
        private readonly Game game;

        public DefaultGuiRenderer(Game game)
        {
            this.game = game;
        }

        public void DrawControl(Control control, int offsetX = 0, int offsetY = 0)
        {
            GraphicsDevice? gd = game.GraphicsDevice;
            if (gd != null)
            {
                Rectangle bounds = control.GetBounds();
                Color fg = Color.LightGray;
                if (control.Focused)
                {
                    fg = Color.White;
                }
                else if (control.Hovered)
                {
                    fg = Color.Gray;
                }
                gd.Color = fg;
                gd.DrawRect(bounds);
            }
        }
    }
}
