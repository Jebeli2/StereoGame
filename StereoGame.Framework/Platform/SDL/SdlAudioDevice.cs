namespace StereoGame.Framework.Platform.SDL
{
    using StereoGame.Framework.Audio;
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Text;
    using System.Threading.Tasks;

    internal class SdlAudioDevice : AudioDevice
    {
        private readonly Game game;

        public SdlAudioDevice(Game game)
        {
            this.game = game;
            if (SDL2Mixer.Mix_OpenAudio(22050, SDL2Mixer.MIX_DEFAULT_FORMAT, 2, 1024) == 0)
            {

            }
        }

        public override Music? LoadMusic(string path)
        {
            IntPtr mus = SDL2Mixer.Mix_LoadMUS(path);
            if (mus != IntPtr.Zero)
            {
                SdlMusic music = new(this, mus) { Name = path };
                OnResourceCreated(music);
                return music;
            }
            return null;
        }

        public override Music? LoadMusic(string path, byte[] data)
        {
            IntPtr rw = Sdl.RwFromMem(data, data.Length);
            if (rw != IntPtr.Zero)
            {
                IntPtr mus = SDL2Mixer.Mix_LoadMUS_RW(rw, 1);
                if (mus != IntPtr.Zero)
                {

                    SdlMusic music = new(this, mus) { Name = path };
                    OnResourceCreated(music);
                    return music;
                }
            }
            return null;
        }

        public override void PlayMusic(Music? music)
        {
            if (CheckMusic(music, out IntPtr mus))
            {
                SDL2Mixer.Mix_PlayMusic(mus, -1);
            }
        }

        public override void StopMusic()
        {
            SDL2Mixer.Mix_HaltMusic();
        }

        private static bool CheckMusic(Music? music, out IntPtr tex)
        {
            tex = IntPtr.Zero;
            if (music is SdlMusic sdlMusic)
            {
                tex = sdlMusic.Handle;
            }
            return tex != IntPtr.Zero;
        }

        protected override void Dispose(bool disposing)
        {
            SDL2Mixer.Mix_CloseAudio();
            base.Dispose(disposing);
        }

    }
}
