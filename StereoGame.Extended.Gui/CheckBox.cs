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

    public class CheckBox : ToggleButton
    {

        public CheckBox(Control? parent, string? text = null)
            : base(parent)
        {
            alwaysUseIconSpace = true;
            Text = text;
            HorizontalTextAlignment = HorizontalAlignment.Left;
        }


        protected override void OnCheckedChanged(EventArgs e)
        {
            if (Checked)
            {
                Icon = Icons.CHECK;
            }
            else
            {
                Icon = Icons.NONE;
            }
            base.OnCheckedChanged(e);
        }


        protected override void DrawControl(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, ref Rectangle bounds)
        {
            base.DrawControl(gui, renderer, gameTime, ref bounds);
            //Rectangle iconBounds = bounds;
            //iconBounds.Width = (ICONWIDTH * 3) / 2;
            //renderer.DrawBorder(iconBounds, BorderShineColor, BorderShadowColor, 1);
        }

        protected override Rectangle AdjustBorderBounds(ref Rectangle bounds)
        {
            Rectangle adjustedRect = bounds;
            adjustedRect.Width = (ICONWIDTH * 3) / 2;
            return adjustedRect;
        }

    }
}
