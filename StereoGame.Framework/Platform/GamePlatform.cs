namespace StereoGame.Framework
{
    using StereoGame.Framework.Platform.SDL;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    partial class GamePlatform
    {
        internal static GamePlatform PlatformCreate(Game game)
        {
            return new SdlGamePlatform(game);
        }
    }
}
