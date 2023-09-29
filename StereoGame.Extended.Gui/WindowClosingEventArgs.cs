using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Extended.Gui
{
    public class WindowClosingEventArgs : EventArgs
    {
        private readonly Window window;
        private WindowCloseAction windowCloseAction;

        public WindowClosingEventArgs(Window window, WindowCloseAction windowCloseAction)
        {
            this.window = window;
            this.windowCloseAction = windowCloseAction;
        }
        public Window Window => window;
        public WindowCloseAction WindowCloseAction
        {
            get { return windowCloseAction; }
            set { windowCloseAction = value; }
        }
    }
}
