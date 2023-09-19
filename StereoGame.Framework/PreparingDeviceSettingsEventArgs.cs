using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework
{
    public class PreparingDeviceSettingsEventArgs : EventArgs
    {
        private readonly GraphicsDeviceInformation graphicsDeviceInformation;
        public PreparingDeviceSettingsEventArgs(GraphicsDeviceInformation graphicsDeviceInformation)
        {
            this.graphicsDeviceInformation = graphicsDeviceInformation;
        }
        public GraphicsDeviceInformation GraphicsDeviceInformation => graphicsDeviceInformation;
    }
}
