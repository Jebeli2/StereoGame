using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Input
{
    public struct MouseState
    {
        private const byte LeftButtonFlag = 1;
        private const byte RightButtonFlag = 2;
        private const byte MiddleButtonFlag = 4;
        private const byte XButton1Flag = 8;
        private const byte XButton2Flag = 16;

        private int x;
        private int y;
        private int scrollWheelValue;
        private int horizontalScrollWheelValue;
        private byte buttons;

        public MouseState(
                   int x,
                   int y,
                   int scrollWheel,
                   ButtonState leftButton,
                   ButtonState middleButton,
                   ButtonState rightButton,
                   ButtonState xButton1,
                   ButtonState xButton2)
        {
            this.x = x;
            this.y = y;
            scrollWheelValue = scrollWheel;
            buttons = (byte)(
                (leftButton == ButtonState.Pressed ? LeftButtonFlag : 0) |
                (rightButton == ButtonState.Pressed ? RightButtonFlag : 0) |
                (middleButton == ButtonState.Pressed ? MiddleButtonFlag : 0) |
                (xButton1 == ButtonState.Pressed ? XButton1Flag : 0) |
                (xButton2 == ButtonState.Pressed ? XButton2Flag : 0)
            );
            horizontalScrollWheelValue = 0;
        }

        public MouseState(
                   int x,
                   int y,
                   int scrollWheel,
                   ButtonState leftButton,
                   ButtonState middleButton,
                   ButtonState rightButton,
                   ButtonState xButton1,
                   ButtonState xButton2,
                   int horizontalScrollWheel)
        {
            this.x = x;
            this.y = y;
            scrollWheelValue = scrollWheel;
            buttons = (byte)(
                (leftButton == ButtonState.Pressed ? LeftButtonFlag : 0) |
                (rightButton == ButtonState.Pressed ? RightButtonFlag : 0) |
                (middleButton == ButtonState.Pressed ? MiddleButtonFlag : 0) |
                (xButton1 == ButtonState.Pressed ? XButton1Flag : 0) |
                (xButton2 == ButtonState.Pressed ? XButton2Flag : 0)
            );
            horizontalScrollWheelValue = horizontalScrollWheel;
        }

        public static bool operator ==(MouseState left, MouseState right)
        {
            return left.x == right.x &&
                   left.y == right.y &&
                   left.buttons == right.buttons &&
                   left.scrollWheelValue == right.scrollWheelValue &&
                   left.horizontalScrollWheelValue == right.horizontalScrollWheelValue;
        }

        public static bool operator !=(MouseState left, MouseState right)
        {
            return !(left == right);
        }

        public override readonly bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is MouseState ms)
            {
                return this == ms;
            }
            return false;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(x, y, scrollWheelValue, horizontalScrollWheelValue, buttons);
        }

        public override readonly string ToString()
        {
            string buttons;
            if (this.buttons == 0)
                buttons = "None";
            else
            {
                buttons = string.Empty;
                if ((this.buttons & LeftButtonFlag) == LeftButtonFlag)
                {
                    if (buttons.Length > 0)
                        buttons += " Left";
                    else
                        buttons += "Left";
                }
                if ((this.buttons & RightButtonFlag) == RightButtonFlag)
                {
                    if (buttons.Length > 0)
                        buttons += " Right";
                    else
                        buttons += "Right";
                }
                if ((this.buttons & MiddleButtonFlag) == MiddleButtonFlag)
                {
                    if (buttons.Length > 0)
                        buttons += " Middle";
                    else
                        buttons += "Middle";
                }
                if ((this.buttons & XButton1Flag) == XButton1Flag)
                {
                    if (buttons.Length > 0)
                        buttons += " XButton1";
                    else
                        buttons += "XButton1";
                }
                if ((this.buttons & XButton2Flag) == XButton2Flag)
                {
                    if (buttons.Length > 0)
                        buttons += " XButton2";
                    else
                        buttons += "XButton2";
                }
            }

            return "[MouseState X=" + x +
                    ", Y=" + y +
                    ", Buttons=" + buttons +
                    ", Wheel=" + scrollWheelValue +
                    ", HWheel=" + horizontalScrollWheelValue +
                    "]";
        }

        public int X
        {
            readonly get => x;
            internal set => x = value;
        }

        public int Y
        {
            readonly get => y;
            internal set => y = value;
        }

        public readonly Point Position => new(x, y);

        public ButtonState LeftButton
        {
            readonly get
            {
                return ((buttons & LeftButtonFlag) > 0) ? ButtonState.Pressed : ButtonState.Released;
            }
            internal set
            {
                if (value == ButtonState.Pressed)
                {
                    buttons = (byte)(buttons | LeftButtonFlag);
                }
                else
                {
                    buttons = (byte)(buttons & (~LeftButtonFlag));
                }
            }
        }

        public ButtonState MiddleButton
        {
            readonly get
            {
                return ((buttons & MiddleButtonFlag) > 0) ? ButtonState.Pressed : ButtonState.Released;
            }
            internal set
            {
                if (value == ButtonState.Pressed)
                {
                    buttons = (byte)(buttons | MiddleButtonFlag);
                }
                else
                {
                    buttons = (byte)(buttons & (~MiddleButtonFlag));
                }
            }
        }

        public ButtonState RightButton
        {
            readonly get
            {
                return ((buttons & RightButtonFlag) > 0) ? ButtonState.Pressed : ButtonState.Released;
            }
            internal set
            {
                if (value == ButtonState.Pressed)
                {
                    buttons = (byte)(buttons | RightButtonFlag);
                }
                else
                {
                    buttons = (byte)(buttons & (~RightButtonFlag));
                }
            }
        }

        public int ScrollWheelValue
        {
            readonly get => scrollWheelValue;
            internal set => scrollWheelValue = value;
        }
        public int HorizontalScrollWheelValue
        {
            readonly get => horizontalScrollWheelValue;
            internal set => horizontalScrollWheelValue = value;
        }

        public ButtonState XButton1
        {
            readonly get
            {
                return ((buttons & XButton1Flag) > 0) ? ButtonState.Pressed : ButtonState.Released;
            }
            internal set
            {
                if (value == ButtonState.Pressed)
                {
                    buttons = (byte)(buttons | XButton1Flag);
                }
                else
                {
                    buttons = (byte)(buttons & (~XButton1Flag));
                }
            }
        }

        public ButtonState XButton2
        {
            readonly get
            {
                return ((buttons & XButton2Flag) > 0) ? ButtonState.Pressed : ButtonState.Released;
            }
            internal set
            {
                if (value == ButtonState.Pressed)
                {
                    buttons = (byte)(buttons | XButton2Flag);
                }
                else
                {
                    buttons = (byte)(buttons & (~XButton2Flag));
                }
            }
        }
    }
}
