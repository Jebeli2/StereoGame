namespace StereoGame.Extended.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IGuiRenderer
    {

        void DrawControl(Control control, int offsetX = 0, int offsetY = 0);

        void FillRectangle(Rectangle rect, Color color, Rectangle? clippingRect = null);
        void DrawRectangle(Rectangle rect, Color color, float thickness = 1.0f, Rectangle? clippingRect = null);
    }
}
