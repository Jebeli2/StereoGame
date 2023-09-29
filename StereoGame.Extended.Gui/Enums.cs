using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Extended.Gui
{
    public enum Alignment
    {
        Minimum,
        Middle,
        Maximum,
        Fill
    }
    public enum Orientation
    {
        Horizontal,
        Vertical,
    }

    public enum HitTestResult
    {
        None,
        Control,
        DragArea,
        SizeRight,
        SizeLeft,
        SizeTop,
        SizeBottom,       
    }
}
