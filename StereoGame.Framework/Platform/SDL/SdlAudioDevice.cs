namespace StereoGame.Framework.Platform.SDL
{
    using StereoGame.Framework.Audio;
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Text;
    using System.Threading.Tasks;
    using static StereoGame.Framework.Platform.SDL.SDL2Mixer;

    internal class SdlAudioDevice : AudioDevice
    {
        private readonly Game game;

        public const string GlobalChannel = "_global_";
        private const int NumChannels = 128;
        private int soundFallOff = 15;
        private int musicVolume = MIX_MAX_VOLUME;
        private int soundVolume = MIX_MAX_VOLUME;
        private readonly Dictionary<int, Playback> playback = new();
        private readonly Dictionary<string, int> channels = new();
        private readonly ChannelFinishedDelegate channelFinished;

        public SdlAudioDevice(Game game)
        {
            channelFinished = OnChannelFinished;
            this.game = game;
            if (OpenAudio(22050, MIX_DEFAULT_FORMAT, 2, 1024) == 0)
            {
                AllocateChannels(NumChannels);
                ChannelFinished(channelFinished);
            }
        }

        public override Music? LoadMusic(string path)
        {
            IntPtr mus = LoadMUS(path);
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
            IntPtr rw = Sdl.RWFromMem(data, data.Length);
            if (rw != IntPtr.Zero)
            {
                IntPtr mus = LoadMUS_RW(rw, 1);
                if (mus != IntPtr.Zero)
                {

                    SdlMusic music = new(this, mus) { Name = path };
                    OnResourceCreated(music);
                    return music;
                }
            }
            return null;
        }

        public override Sound? LoadSound(string path)
        {
            IntPtr chunk = LoadWAV(path);
            if (chunk != IntPtr.Zero)
            {
                SdlSound sound = new(this, chunk) { Name = path };
                OnResourceCreated(sound);
                return sound;
            }
            return null;
        }

        public override Sound? LoadSound(string path, byte[] data)
        {
            IntPtr rw = Sdl.RWFromMem(data, data.Length);
            if (rw != IntPtr.Zero)
            {
                IntPtr chunk = LoadWAV_RW(rw, 1);
                if (chunk != IntPtr.Zero)
                {

                    SdlSound sound = new(this, chunk) { Name = path };
                    OnResourceCreated(sound);
                    return sound;
                }
            }
            return null;
        }

        public override void PlayMusic(Music? music, int loops = -1)
        {
            if (CheckMusic(music, out IntPtr mus))
            {
                SDL2Mixer.PlayMusic(mus, loops);
            }
        }

        public override void StopMusic()
        {
            HaltMusic();
        }

        public override void PlaySound(Sound? sound, string? channel, PointF pos, bool loop = false)
        {
            if (CheckSound(sound, out IntPtr snd))
            {
                Play(new Playback(snd, channel ?? GlobalChannel, pos, loop));
            }
        }

        public override void UpdateSounds(float x, float y)
        {
            lastPos.X = x;
            lastPos.Y = y;
            List<int> cleanup = new();
            foreach (var it in playback)
            {
                int channel = it.Key;
                Playback play = it.Value;
                if (play.Finished)
                {
                    cleanup.Add(channel);
                    continue;
                }
                if (play.Location.X == 0 && play.Location.Y == 0)
                {
                    continue;
                }
                float v = Distance(x, y, play.Location.X, play.Location.Y) / soundFallOff;
                if (play.Loop)
                {
                    if (v < 1.0f && play.Paused)
                    {
                        Resume(channel);
                        play.Paused = false;
                    }
                    else if (v > 1.0f && !play.Paused)
                    {
                        Pause(channel);
                        play.Paused = true;
                        continue;
                    }
                }
                v = Math.Min(Math.Max(v, 0.0f), 1.0f);
                byte dist = (byte)(255.0f * v);
                _ = SetPosition(channel, 0, dist);
            }
            while (cleanup.Count > 0)
            {
                int channel = cleanup[0];
                cleanup.RemoveAt(0);
                if (playback.TryGetValue(channel, out Playback? play))
                {
                    playback.Remove(channel);
                    channels.Remove(play.Channel);
                }
            }
        }

        private static bool CheckMusic(Music? music, out IntPtr handle)
        {
            handle = IntPtr.Zero;
            if (music is SdlMusic sdlMusic)
            {
                handle = sdlMusic.Handle;
            }
            return handle != IntPtr.Zero;
        }
        private static bool CheckSound(Sound? sound, out IntPtr handle)
        {
            handle = IntPtr.Zero;
            if (sound is SdlSound sdlSound)
            {
                handle = sdlSound.Handle;
            }
            return handle != IntPtr.Zero;
        }

        protected override void Dispose(bool disposing)
        {
            ClearChannelFinished();
            CloseAudio();
            base.Dispose(disposing);
        }

        private void Play(Playback pb)
        {
            bool setChannel = false;
            if (!string.Equals(GlobalChannel, pb.Channel))
            {
                if (channels.TryGetValue(pb.Channel, out int vc))
                {
                    _ = HaltChannel(vc);
                    channels.Remove(pb.Channel);
                }
                setChannel = true;
            }
            int channel = PlayChannelTimed(-1, pb.Sound, pb.Loop ? -1 : 0, -1);
            if (channel == -1)
            {
                //SDLLog.Error(LogCategory.AUDIO, $"Failed to play sound '{pb.Sound}', no more channels available");
            }
            else
            {
                _ = Volume(channel, soundVolume);
                //SDLLog.Verbose(LogCategory.AUDIO, $"Playing sound '{pb.Sound}' on channel {channel} ({pb.Channel})");
            }
            byte dist;
            if (!pb.Location.IsEmpty)
            {
                float v = 255.0f * (Distance(lastPos, pb.Location)) / soundFallOff;
                v = MathF.Min(MathF.Max(v, 0.0f), 255.0f);
                dist = (byte)v;
            }
            else
            {
                dist = 0;
            }
            _ = SetDistance(channel, dist);
            if (setChannel) { channels[pb.Channel] = channel; }
            playback[channel] = pb;
        }

        private void OnChannelFinished(int channel)
        {
            if (playback.TryGetValue(channel, out Playback? play))
            {
                if (play != null)
                {
                    play.Finished = true;
                }
            }
            _ = SetDistance(channel,0);
        }
        private static float Distance(PointF x, PointF y)
        {
            return Distance(x.X, x.Y, y.X, y.Y);
        }
        private static float Distance(float x0, float y0, float x1, float y1)
        {
            return MathF.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
        }
        private class Playback
        {
            public Playback(IntPtr sound, string channel, PointF pos, bool loop)
            {
                Sound = sound;
                Channel = channel;
                Location = pos;
                Loop = loop;
            }

            public IntPtr Sound;
            public string Channel;
            public PointF Location;
            public bool Loop;
            public bool Paused;
            public bool Finished;
        }
    }
}
