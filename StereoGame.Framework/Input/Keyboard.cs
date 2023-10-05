namespace StereoGame.Framework.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static partial class Keyboard
    {
		public static KeyboardState GetState()
        {
            return PlatformGetState();
        }
    }
}
