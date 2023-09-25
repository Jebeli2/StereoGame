namespace StereoGame.Framework.Platform.SDL
{
    using StereoGame.Framework.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    internal static class SDL2Mixer
    {
        private const string nativeLibName = "SDL2_mixer";
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Mix_Init(MIX_InitFlags flags);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Mix_Quit();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Mix_OpenAudio(int frequency, ushort format, int channels, int chunksize);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Mix_CloseAudio();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Mix_AllocateChannels(int numchans);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Mix_LoadMUS_RW(IntPtr rwops, int freesrc);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Mix_LoadMUS(string file);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Mix_FreeMusic(IntPtr music);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Mix_PlayMusic(IntPtr music, int loops);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Mix_VolumeMusic(int volume);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Mix_HaltMusic();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Mix_PauseMusic();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Mix_ResumeMusic();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Mix_RewindMusic();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Mix_LoadWAV_RW(IntPtr src, int freesrc);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Mix_LoadWAV(string file);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Mix_FreeChunk(IntPtr chunk);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Mix_PlayChannelTimed(int channel, IntPtr chunk, int loops, int ticks);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Mix_Volume(int channel, int volume);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Mix_VolumeChunk(IntPtr chunk, int volume);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Mix_HaltChannel(int channel);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Mix_SetPosition(int channel, short angle, byte distance);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Mix_SetDistance(int channel, byte distance);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Mix_Pause(int channel);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Mix_Resume(int channel);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Mix_ChannelFinished(ChannelFinishedDelegate channel_finished);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Mix_ChannelFinished(IntPtr channel_finished);


        public static int Init(MIX_InitFlags flags) => Mix_Init(flags);
        public static void Quit() => Mix_Quit();
        public static int OpenAudio(int frequency, ushort format, int channels, int chunksize) => Mix_OpenAudio(frequency, format, channels, chunksize);
        public static void CloseAudio() => Mix_CloseAudio();
        public static int AllocateChannels(int numchans) => Mix_AllocateChannels(numchans);
        public static IntPtr LoadMUS_RW(IntPtr rwops, int freesrc) => Mix_LoadMUS_RW(rwops, freesrc);
        public static IntPtr LoadMUS(string file) => Mix_LoadMUS(file);
        public static void FreeMusic(IntPtr music) => Mix_FreeMusic(music);
        public static int PlayMusic(IntPtr music, int loops) => Mix_PlayMusic(music, loops);
        public static int VolumeMusic(int volume) => Mix_VolumeMusic(volume);
        public static int HaltMusic() => Mix_HaltMusic();
        public static void PauseMusic() => Mix_PauseMusic();
        public static void ResumeMusic() => Mix_ResumeMusic();
        public static void RewindMusic() => Mix_RewindMusic();
        public static IntPtr LoadWAV_RW(IntPtr rwops, int freesrc) => Mix_LoadWAV_RW(rwops, freesrc);
        public static IntPtr LoadWAV(string file) => Mix_LoadWAV(file);
        public static void FreeChunk(IntPtr chunk) => Mix_FreeChunk(chunk);
        public static int PlayChannelTimed(int channel, IntPtr chunk, int loops, int ticks) => Mix_PlayChannelTimed(channel, chunk, loops, ticks);
        public static int Volume(int channel, int volume) => Mix_Volume(channel, volume);
        public static int VolumeChunk(IntPtr chunk, int volume) => Mix_VolumeChunk(chunk, volume);
        public static int HaltChannel(int channel) => Mix_HaltChannel(channel);
        public static int SetPosition(int channel, short angle, byte distance) => Mix_SetPosition(channel, angle, distance);
        public static int SetDistance(int channel, byte distance) => Mix_SetDistance(channel, distance);
        public static void Pause(int channel) => Mix_Pause(channel);
        public static void Resume(int channel) => Mix_Resume(channel);
        public static void ChannelFinished(ChannelFinishedDelegate channelFinished) => Mix_ChannelFinished(channelFinished);
        public static void ClearChannelFinished() => Mix_ChannelFinished(IntPtr.Zero);

        //public static IntPtr NativeLibrary = GetNativeLibrary();

        //private static IntPtr GetNativeLibrary()
        //{
        //    if (CurrentPlatform.OS == OS.Windows)
        //        return FuncLoader.LoadLibraryExt("SDL2_mixer.dll");
        //    //else if (CurrentPlatform.OS == OS.Linux)
        //    //    return FuncLoader.LoadLibraryExt("libSDL2_2.0.so.0");
        //    else if (CurrentPlatform.OS == OS.MacOSX)
        //        return FuncLoader.LoadLibraryExt("libSDL2_mixer.dylib");
        //    else
        //        return FuncLoader.LoadLibraryExt("SDL2_mixer");
        //}



        [Flags]
        public enum MIX_InitFlags
        {
            MIX_INIT_FLAC = 0x00000001,
            MIX_INIT_MOD = 0x00000002,
            MIX_INIT_MP3 = 0x00000008,
            MIX_INIT_OGG = 0x00000010,
            MIX_INIT_MID = 0x00000020,
            MIX_INIT_OPUS = 0x00000040
        }

        public const byte MIX_MAX_VOLUME = 128;


        public const ushort AUDIO_U8 = 0x0008;
        public const ushort AUDIO_S8 = 0x8008;
        public const ushort AUDIO_U16LSB = 0x0010;
        public const ushort AUDIO_S16LSB = 0x8010;
        public const ushort AUDIO_U16MSB = 0x1010;
        public const ushort AUDIO_S16MSB = 0x9010;
        public const ushort AUDIO_U16 = AUDIO_U16LSB;
        public const ushort AUDIO_S16 = AUDIO_S16LSB;
        public const ushort AUDIO_S32LSB = 0x8020;
        public const ushort AUDIO_S32MSB = 0x9020;
        public const ushort AUDIO_S32 = AUDIO_S32LSB;
        public const ushort AUDIO_F32LSB = 0x8120;
        public const ushort AUDIO_F32MSB = 0x9120;
        public const ushort AUDIO_F32 = AUDIO_F32LSB;

        public static readonly ushort MIX_DEFAULT_FORMAT = BitConverter.IsLittleEndian ? AUDIO_S16LSB : AUDIO_S16MSB;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MusicFinishedDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ChannelFinishedDelegate(int channel);


        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_mix_init(MIX_InitFlags flags);
        //public static readonly d_sdl_mix_init Mix_Init = FuncLoader.LoadFunction<d_sdl_mix_init>(NativeLibrary, "Mix_Init");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_mix_quit();
        //public static readonly d_sdl_mix_quit Mix_Quit = FuncLoader.LoadFunction<d_sdl_mix_quit>(NativeLibrary, "Mix_Quit");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_mix_audioopen(int frequency, ushort format, int channels, int chunksize);
        //public static readonly d_sdl_mix_audioopen Mix_OpenAudio = FuncLoader.LoadFunction<d_sdl_mix_audioopen>(NativeLibrary, "Mix_OpenAudio");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_mix_closeaudio();
        //public static readonly d_sdl_mix_closeaudio Mix_CloseAudio = FuncLoader.LoadFunction<d_sdl_mix_closeaudio>(NativeLibrary, "Mix_CloseAudio");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_mix_allocatechannels(int numchans);
        //public static readonly d_sdl_mix_allocatechannels Mix_AllocateChannels = FuncLoader.LoadFunction<d_sdl_mix_allocatechannels>(NativeLibrary, "Mix_AllocateChannels");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate IntPtr d_sdl_mix_loadmus(string file);
        //public static readonly d_sdl_mix_loadmus Mix_LoadMUS = FuncLoader.LoadFunction<d_sdl_mix_loadmus>(NativeLibrary, "Mix_LoadMUS");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate IntPtr d_sdl_mix_loadmusrw(IntPtr rw, int freesrc);
        //public static readonly d_sdl_mix_loadmusrw Mix_LoadMUS_RW = FuncLoader.LoadFunction<d_sdl_mix_loadmusrw>(NativeLibrary, "Mix_LoadMUS_RW");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_mix_freemusic(IntPtr music);
        //public static readonly d_sdl_mix_freemusic Mix_FreeMusic = FuncLoader.LoadFunction<d_sdl_mix_freemusic>(NativeLibrary, "Mix_FreeMusic");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_mix_playmusic(IntPtr music, int loops);
        //public static readonly d_sdl_mix_playmusic Mix_PlayMusic = FuncLoader.LoadFunction<d_sdl_mix_playmusic>(NativeLibrary, "Mix_PlayMusic");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_mix_volumemusic(int volume);
        //public static readonly d_sdl_mix_volumemusic Mix_VolumeMusic = FuncLoader.LoadFunction<d_sdl_mix_volumemusic>(NativeLibrary, "Mix_VolumeMusic");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_mix_haltmusic();
        //public static readonly d_sdl_mix_haltmusic Mix_HaltMusic = FuncLoader.LoadFunction<d_sdl_mix_haltmusic>(NativeLibrary, "Mix_HaltMusic");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_mix_pausemusic();
        //public static readonly d_sdl_mix_pausemusic Mix_PauseMusic = FuncLoader.LoadFunction<d_sdl_mix_pausemusic>(NativeLibrary, "Mix_PauseMusic");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_mix_resumemusic();
        //public static readonly d_sdl_mix_resumemusic Mix_ResumeMusic = FuncLoader.LoadFunction<d_sdl_mix_resumemusic>(NativeLibrary, "Mix_ResumeMusic");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_mix_rewindmusic();
        //public static readonly d_sdl_mix_rewindmusic Mix_RewindMusic = FuncLoader.LoadFunction<d_sdl_mix_rewindmusic>(NativeLibrary, "Mix_RewindMusic");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_mix_freechunk(IntPtr sound);
        //public static readonly d_sdl_mix_freechunk Mix_FreeChunk = FuncLoader.LoadFunction<d_sdl_mix_freechunk>(NativeLibrary, "Mix_FreeChunk");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate IntPtr d_sdl_mix_loadwav(string file);
        //public static readonly d_sdl_mix_loadwav Mix_LoadWAV = FuncLoader.LoadFunction<d_sdl_mix_loadwav>(NativeLibrary, "Mix_LoadWAV");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate IntPtr d_sdl_mix_loadwavrw(IntPtr rw, int freesrc);
        //public static readonly d_sdl_mix_loadwavrw Mix_LoadWAV_RW = FuncLoader.LoadFunction<d_sdl_mix_loadwavrw>(NativeLibrary, "Mix_LoadWAV_RW");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_mix_playchanneltimed(int channel, IntPtr sound, int loops, int ticks);
        //public static readonly d_sdl_mix_playchanneltimed Mix_PlayChannelTimed = FuncLoader.LoadFunction<d_sdl_mix_playchanneltimed>(NativeLibrary, "Mix_PlayChannelTimed");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_mix_volume(int channel, int volume);
        //public static readonly d_sdl_mix_volume Mix_Volume = FuncLoader.LoadFunction<d_sdl_mix_volume>(NativeLibrary, "Mix_Volume");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_mix_volumechunk(IntPtr sound, int volume);
        //public static readonly d_sdl_mix_volumechunk Mix_VolumeChunk = FuncLoader.LoadFunction<d_sdl_mix_volumechunk>(NativeLibrary, "Mix_VolumeChunk");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_mix_haltchannel(int channel);
        //public static readonly d_sdl_mix_haltchannel Mix_HaltChannel = FuncLoader.LoadFunction<d_sdl_mix_haltchannel>(NativeLibrary, "Mix_HaltChannel");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_mix_setposition(int channel, short angle, byte distance);
        //public static readonly d_sdl_mix_setposition Mix_SetPosition = FuncLoader.LoadFunction<d_sdl_mix_setposition>(NativeLibrary, "Mix_SetPosition");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_mix_setdistance(int channel, byte distance);
        //public static readonly d_sdl_mix_setdistance Mix_SetDistance = FuncLoader.LoadFunction<d_sdl_mix_setdistance>(NativeLibrary, "Mix_SetDistance");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_mix_channelfinished(ChannelFinishedDelegate? channelFinished);
        //public static readonly d_sdl_mix_channelfinished Mix_ChannelFinished = FuncLoader.LoadFunction<d_sdl_mix_channelfinished>(NativeLibrary, "Mix_ChannelFinished");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_mix_pause(int channel);
        //public static readonly d_sdl_mix_pause Mix_Pause = FuncLoader.LoadFunction<d_sdl_mix_pause>(NativeLibrary, "Mix_Pause");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_mix_resume(int channel);
        //public static readonly d_sdl_mix_resume Mix_Resume = FuncLoader.LoadFunction<d_sdl_mix_resume>(NativeLibrary, "Mix_Resume");

    }
}
