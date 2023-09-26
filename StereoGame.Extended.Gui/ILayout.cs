using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Extended.Gui
{
    public interface ILayout
    {
        void PerformLayout(IGuiSystem context, Control control);
        Size GetPreferredSize(IGuiSystem context, Control control);
    }
}
