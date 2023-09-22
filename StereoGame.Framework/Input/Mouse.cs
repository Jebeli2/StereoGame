using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Input
{
    public static partial class Mouse
    {
        private static GameWindow? primaryWindow;
        private static readonly MouseState defaultState = new();

        internal static GameWindow? PrimaryWindow
        {
            get => primaryWindow;
            set => primaryWindow = value;
        }

        public static MouseState GetState(GameWindow window)
        {
            return window.MouseState;
        }

        public static MouseState GetState()
        {
            if (primaryWindow != null) return GetState(primaryWindow);
            return defaultState;
        }
    }
}
