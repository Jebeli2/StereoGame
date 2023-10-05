using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework
{
    public interface IGraphicsDeviceManager
    {
        bool BeginDraw();
        void CreateDevice();
        void EndDraw();
        void ToggleFullScreen();

        bool IsFullScreen { get; }
    }
}
