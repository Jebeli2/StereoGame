namespace StereoGame.Framework.Audio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class Music : AudioResource
    {

        protected Music(AudioDevice audioDevice)
        {
            AudioDevice = audioDevice;
        }
    }
}
