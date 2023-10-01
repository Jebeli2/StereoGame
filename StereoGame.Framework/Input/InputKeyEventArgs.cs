namespace StereoGame.Framework.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class InputKeyEventArgs : EventArgs
    {
        private readonly Keys key;

        /// <summary>
        /// Create a new keyboard input event
        /// </summary>
        /// <param name="key">The key involved in this event</param>
        public InputKeyEventArgs(Keys key = Keys.None)
        {
            this.key = key;
        }

        public Keys Key => key;
    }
}
