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
        SizeBottomLeft,
        SizeBottomRight,
        SizeTopLeft,
        SizeTopRight,
    }

    public enum WindowCloseAction
    {
        None,
        Hide,
        Remove,
        Dispose
    }

    public enum ArrowPlace
    {
        None,
        LeftTop,
        RightBottom,
        Split
    }

    [Flags]
    public enum StrFlags
    {
        None = 0,
        Integer = 1,
        Double = 2
    }
}
