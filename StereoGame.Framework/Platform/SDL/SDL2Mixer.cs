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
        public static IntPtr NativeLibrary = GetNativeLibrary();

        private static IntPtr GetNativeLibrary()
        {
            if (CurrentPlatform.OS == OS.Windows)
                return FuncLoader.LoadLibraryExt("SDL2_mixer.dll");
            //else if (CurrentPlatform.OS == OS.Linux)
            //    return FuncLoader.LoadLibraryExt("libSDL2_2.0.so.0");
            else if (CurrentPlatform.OS == OS.MacOSX)
                return FuncLoader.LoadLibraryExt("libSDL2_mixer.dylib");
            else
                return FuncLoader.LoadLibraryExt("SDL2_mixer");
        }

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
        public delegate int d_sdl_mix_init(MIX_InitFlags flags);
        public static readonly d_sdl_mix_init Mix_Init = FuncLoader.LoadFunction<d_sdl_mix_init>(NativeLibrary, "Mix_Init");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void d_sdl_mix_quit();
        public static readonly d_sdl_mix_quit Mix_Quit = FuncLoader.LoadFunction<d_sdl_mix_quit>(NativeLibrary, "Mix_Quit");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_mix_audioopen(int frequency, ushort format, int channels, int chunksize);
        public static readonly d_sdl_mix_audioopen Mix_OpenAudio = FuncLoader.LoadFunction<d_sdl_mix_audioopen>(NativeLibrary, "Mix_OpenAudio");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void d_sdl_mix_closeaudio();
        public static readonly d_sdl_mix_closeaudio Mix_CloseAudio = FuncLoader.LoadFunction<d_sdl_mix_closeaudio>(NativeLibrary, "Mix_CloseAudio");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_mix_allocatechannels(int numchans);
        public static readonly d_sdl_mix_allocatechannels Mix_AllocateChannels = FuncLoader.LoadFunction<d_sdl_mix_allocatechannels>(NativeLibrary, "Mix_AllocateChannels");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr d_sdl_mix_loadmus(string file);
        public static readonly d_sdl_mix_loadmus Mix_LoadMUS = FuncLoader.LoadFunction<d_sdl_mix_loadmus>(NativeLibrary, "Mix_LoadMUS");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr d_sdl_mix_loadmusrw(IntPtr rw, int freesrc);
        public static readonly d_sdl_mix_loadmusrw Mix_LoadMUS_RW = FuncLoader.LoadFunction<d_sdl_mix_loadmusrw>(NativeLibrary, "Mix_LoadMUS_RW");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void d_sdl_mix_freemusic(IntPtr music);
        public static readonly d_sdl_mix_freemusic Mix_FreeMusic = FuncLoader.LoadFunction<d_sdl_mix_freemusic>(NativeLibrary, "Mix_FreeMusic");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_mix_playmusic(IntPtr music, int loops);
        public static readonly d_sdl_mix_playmusic Mix_PlayMusic = FuncLoader.LoadFunction<d_sdl_mix_playmusic>(NativeLibrary, "Mix_PlayMusic");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_mix_volumemusic(int volume);
        public static readonly d_sdl_mix_volumemusic Mix_VolumeMusic = FuncLoader.LoadFunction<d_sdl_mix_volumemusic>(NativeLibrary, "Mix_VolumeMusic");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_mix_haltmusic();
        public static readonly d_sdl_mix_haltmusic Mix_HaltMusic = FuncLoader.LoadFunction<d_sdl_mix_haltmusic>(NativeLibrary, "Mix_HaltMusic");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void d_sdl_mix_pausemusic();
        public static readonly d_sdl_mix_pausemusic Mix_PauseMusic = FuncLoader.LoadFunction<d_sdl_mix_pausemusic>(NativeLibrary, "Mix_PauseMusic");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void d_sdl_mix_resumemusic();
        public static readonly d_sdl_mix_resumemusic Mix_ResumeMusic = FuncLoader.LoadFunction<d_sdl_mix_resumemusic>(NativeLibrary, "Mix_ResumeMusic");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void d_sdl_mix_rewindmusic();
        public static readonly d_sdl_mix_rewindmusic Mix_RewindMusic = FuncLoader.LoadFunction<d_sdl_mix_rewindmusic>(NativeLibrary, "Mix_RewindMusic");

    }
}
