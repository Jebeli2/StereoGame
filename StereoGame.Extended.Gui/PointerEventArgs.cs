namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework.Input;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PointerEventArgs:EventArgs
    {
        private readonly int x;
        private readonly int y;
        private readonly MouseButton button;
        private readonly int scrollWheelDelta;
        private readonly int scrollWheelValue;
        private readonly TimeSpan time;

        public PointerEventArgs(int x, int y, MouseButton button, int scrollWheelDelta, int scrollWheelValue, TimeSpan time)
        {
            this.x = x;
            this.y = y;
            this.button = button;
            this.scrollWheelDelta = scrollWheelDelta;
            this.scrollWheelValue = scrollWheelValue;
            this.time = time;
        }

        public int X => x;
        public int Y => y;
        public MouseButton Button => button;
        public int ScrollWheelDelta => scrollWheelDelta;
        public int ScrollWheelValue => scrollWheelValue;
        public TimeSpan Time => time;

        public static PointerEventArgs FromMouseEventArgs(MouseEventArgs args)
        {
            return new PointerEventArgs(args.X, args.Y, args.Button, 0, 0, args.Time);
        }
    }
}
