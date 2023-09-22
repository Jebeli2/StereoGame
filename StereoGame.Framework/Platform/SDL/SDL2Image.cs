namespace StereoGame.Framework.Platform.SDL
{
    using StereoGame.Framework.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    internal static class SDL2Image
    {
        public static IntPtr NativeLibrary = GetNativeLibrary();

        private static IntPtr GetNativeLibrary()
        {
            if (CurrentPlatform.OS == OS.Windows)
                return FuncLoader.LoadLibraryExt("SDL2_image.dll");
            //else if (CurrentPlatform.OS == OS.Linux)
            //    return FuncLoader.LoadLibraryExt("libSDL2_2.0.so.0");
            else if (CurrentPlatform.OS == OS.MacOSX)
                return FuncLoader.LoadLibraryExt("libSDL2_image.dylib");
            else
                return FuncLoader.LoadLibraryExt("SDL2_image");
        }

        [Flags]
        public enum IMG_InitFlags
        {
            IMG_INIT_JPG = 0x00000001,
            IMG_INIT_PNG = 0x00000002,
            IMG_INIT_TIF = 0x00000004,
            IMG_INIT_WEBP = 0x00000008
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_image_init(IMG_InitFlags flags);
        public static readonly d_sdl_image_init IMG_Init = FuncLoader.LoadFunction<d_sdl_image_init>(NativeLibrary, "IMG_Init");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void d_sdl_image_quit();
        public static readonly d_sdl_image_quit IMG_Quit = FuncLoader.LoadFunction<d_sdl_image_quit>(NativeLibrary, "IMG_Quit");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr d_sdl_image_load_texture(IntPtr renderer, string file);
        public static readonly d_sdl_image_load_texture IMG_LoadTexture = FuncLoader.LoadFunction<d_sdl_image_load_texture>(NativeLibrary, "IMG_LoadTexture");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr d_sdl_image_load_texture_rw(IntPtr renderer, IntPtr rw, int freesrc);
        public static readonly d_sdl_image_load_texture_rw IMG_LoadTexture_RW = FuncLoader.LoadFunction<d_sdl_image_load_texture_rw>(NativeLibrary, "IMG_LoadTexture_RW");

    }
}
