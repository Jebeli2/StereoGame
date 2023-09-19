using StereoGame.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework
{
    public class GraphicsDeviceInformation
    {
        public GraphicsAdapter Adapter { get; set; }

        public GraphicsProfile GraphicsProfile { get; set; }

        public PresentationParameters PresentationParameters { get; set; }
    }
}
