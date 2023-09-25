namespace StereoGame.Framework.Platform.SDL
{
    using StereoGame.Framework.Audio;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class SdlSound : Sound
    {
        private readonly IntPtr handle;
        public SdlSound(AudioDevice audioDevice, IntPtr handle)
            : base(audioDevice)
        {
            this.handle = handle;
        }

        public IntPtr Handle => handle;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            SDL2Mixer.FreeChunk(handle);
        }
    }
}
