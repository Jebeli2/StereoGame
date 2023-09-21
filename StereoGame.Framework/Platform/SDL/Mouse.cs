using StereoGame.Framework.Platform.SDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Input
{
    partial class Mouse
    {
        internal static int ScrollX;
        internal static int ScrollY;
        private static MouseState PlatformGetState(GameWindow window)
        {
            //int x, y;
            //var winFlags = Sdl.Window.GetWindowFlags(window.Handle);
            var state = Sdl.Mouse.GetGlobalState(out int x, out int y);
            var clientBounds = window.ClientBounds;

            window.MouseState.LeftButton = (state & Sdl.Mouse.Button.Left) != 0 ? ButtonState.Pressed : ButtonState.Released;
            window.MouseState.MiddleButton = (state & Sdl.Mouse.Button.Middle) != 0 ? ButtonState.Pressed : ButtonState.Released;
            window.MouseState.RightButton = (state & Sdl.Mouse.Button.Right) != 0 ? ButtonState.Pressed : ButtonState.Released;
            window.MouseState.XButton1 = (state & Sdl.Mouse.Button.X1Mask) != 0 ? ButtonState.Pressed : ButtonState.Released;
            window.MouseState.XButton2 = (state & Sdl.Mouse.Button.X2Mask) != 0 ? ButtonState.Pressed : ButtonState.Released;

            window.MouseState.HorizontalScrollWheelValue = ScrollX;
            window.MouseState.ScrollWheelValue = ScrollY;

            window.MouseState.X = x - clientBounds.X;
            window.MouseState.Y = y - clientBounds.Y;

            return window.MouseState;
        }
    }
}
