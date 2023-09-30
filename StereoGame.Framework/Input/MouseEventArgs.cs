namespace StereoGame.Framework.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MouseEventArgs : EventArgs
    {
        private readonly MouseState currentState;
        private readonly MouseState previousState;
        private readonly MouseButton button;
        private readonly TimeSpan time;
        public MouseEventArgs(MouseState currentState, MouseState previousState, MouseButton button, TimeSpan time)
        {
            this.currentState = currentState;
            this.previousState = previousState;
            this.button = button;
            this.time = time;
        }

        public int X => currentState.X;
        public int Y => currentState.Y;
        public MouseButton Button => button;
        public TimeSpan Time => time;


    }
}
