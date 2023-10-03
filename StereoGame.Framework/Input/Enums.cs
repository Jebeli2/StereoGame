namespace StereoGame.Framework.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Flags]
    public enum MouseButton
    {
        None = 0,
        Left = 1,
        Middle = 2,
        Right = 4,
        XButton1 = 8,
        XButton2 = 16
    }

    public enum KeyState
    {
        Up,
        Down,
    }
    public enum SystemCursor
    {
        Arrow,
        IBeam,
        Wait,
        Crosshair,
        WaitArrow,
        SizeNWSE,
        SizeNESW,
        SizeWE,
        SizeNS,
        SizeAll,
        No,
        Hand
    }

    [Flags]
    public enum KeyboardModifiers
    {
        Control = 1,
        Shift = 2,
        Alt = 4,
        None = 0
    }
}
