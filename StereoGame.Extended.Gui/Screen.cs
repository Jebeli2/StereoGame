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

        public override Size GetContentSize(IGuiSystem context)
        {
            return new Size(context.ScreenWidth, context.ScreenHeight);
        }
    }
}
