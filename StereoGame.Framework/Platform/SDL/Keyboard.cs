namespace StereoGame.Framework.Input
{
    using StereoGame.Framework.Platform.SDL;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static partial class Keyboard
    {
        static List<Keys> _keys = new();

        private static KeyboardState PlatformGetState()
        {
            var modifiers = Sdl.Keyboard.GetModState();
            return new KeyboardState(_keys,
                                     (modifiers & Sdl.Keyboard.Keymod.CapsLock) == Sdl.Keyboard.Keymod.CapsLock,
                                     (modifiers & Sdl.Keyboard.Keymod.NumLock) == Sdl.Keyboard.Keymod.NumLock);
        }

        internal static void SetKeys(List<Keys> keys)
        {
            _keys = keys;
        }
    }
}
