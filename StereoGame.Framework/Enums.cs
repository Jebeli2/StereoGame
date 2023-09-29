namespace StereoGame.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public enum GameRunBehavior
    {
        Asynchronous,
        Synchronous
    }

    public enum GraphicsProfile
    {
        Reach,
        HiDef
    }

    [Flags]
    public enum DisplayOrientation
    {
        Default = 0,
        LandscapeLeft = 1,
        LandscapeRight = 2,
        Portrait = 4,
        PortraitDown = 8,
        Unknown = 16
    }

  
}
