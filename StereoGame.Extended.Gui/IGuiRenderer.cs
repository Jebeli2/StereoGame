namespace StereoGame.Extended.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IGuiRenderer
    {

        void DrawControl(Control control, int offsetX = 0, int offsetY = 0);
    }
}
