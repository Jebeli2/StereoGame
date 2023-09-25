namespace StereoGame.Framework.Platform.SDL
{
    using StereoGame.Framework.Audio;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class SdlMusic : Music
    {
        private readonly IntPtr handle;
        private readonly string? tempFile;

        public SdlMusic(AudioDevice audioDevice, IntPtr handle, string? tempFile = null)
            : base(audioDevice)
        {
            this.handle = handle;
            this.tempFile = tempFile;
        }

        public IntPtr Handle => handle;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            SDL2Mixer.FreeMusic(handle);
        }
    }
}
