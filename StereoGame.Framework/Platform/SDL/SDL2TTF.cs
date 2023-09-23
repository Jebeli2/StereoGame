using StereoGame.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Platform.SDL
{
    internal static class SDL2TTF
    {
        public static IntPtr NativeLibrary = GetNativeLibrary();

        private static IntPtr GetNativeLibrary()
        {
            if (CurrentPlatform.OS == OS.Windows)
                return FuncLoader.LoadLibraryExt("SDL2_ttf.dll");
            //else if (CurrentPlatform.OS == OS.Linux)
            //    return FuncLoader.LoadLibraryExt("libSDL2_2.0.so.0");
            else if (CurrentPlatform.OS == OS.MacOSX)
                return FuncLoader.LoadLibraryExt("SDL2_ttf.dylib");
            else
                return FuncLoader.LoadLibraryExt("SDL2_ttf");
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_ttf_init();
        public static readonly d_sdl_ttf_init TTF_Init = FuncLoader.LoadFunction<d_sdl_ttf_init>(NativeLibrary, "TTF_Init");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void d_sdl_ttf_quit();
        public static readonly d_sdl_ttf_quit TTF_Quit = FuncLoader.LoadFunction<d_sdl_ttf_quit>(NativeLibrary, "TTF_Quit");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_ttf_wasinit();
        public static readonly d_sdl_ttf_wasinit TTF_WasInit = FuncLoader.LoadFunction<d_sdl_ttf_wasinit>(NativeLibrary, "TTF_WasInit");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void d_sdl_ttf_closefont(IntPtr font);
        public static readonly d_sdl_ttf_closefont TTF_CloseFont = FuncLoader.LoadFunction<d_sdl_ttf_closefont>(NativeLibrary, "TTF_CloseFont");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr d_sdl_ttf_openfont(string file, int ptsize);
        public static readonly d_sdl_ttf_openfont TTF_OpenFont = FuncLoader.LoadFunction<d_sdl_ttf_openfont>(NativeLibrary, "TTF_OpenFont");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr d_sdl_ttf_openfontrw(IntPtr src, int freesrc, int ptsize);
        public static readonly d_sdl_ttf_openfontrw TTF_OpenFontRW = FuncLoader.LoadFunction<d_sdl_ttf_openfontrw>(NativeLibrary, "TTF_OpenFontRW");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_ttf_getfontstyle(IntPtr font);
        public static readonly d_sdl_ttf_getfontstyle TTF_GetFontStyle = FuncLoader.LoadFunction<d_sdl_ttf_getfontstyle>(NativeLibrary, "TTF_GetFontStyle");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_ttf_getfontoutline(IntPtr font);
        public static readonly d_sdl_ttf_getfontoutline TTF_GetFontOutline = FuncLoader.LoadFunction<d_sdl_ttf_getfontoutline>(NativeLibrary, "TTF_GetFontOutline");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_ttf_getfonthinting(IntPtr font);
        public static readonly d_sdl_ttf_getfonthinting TTF_GetFontHinting = FuncLoader.LoadFunction<d_sdl_ttf_getfonthinting>(NativeLibrary, "TTF_GetFontHinting");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_ttf_getfontheight(IntPtr font);
        public static readonly d_sdl_ttf_getfontheight TTF_FontHeight = FuncLoader.LoadFunction<d_sdl_ttf_getfontheight>(NativeLibrary, "TTF_FontHeight");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_ttf_getfontascent(IntPtr font);
        public static readonly d_sdl_ttf_getfontascent TTF_FontAscent = FuncLoader.LoadFunction<d_sdl_ttf_getfontascent>(NativeLibrary, "TTF_FontAscent");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_ttf_getfontdescent(IntPtr font);
        public static readonly d_sdl_ttf_getfontdescent TTF_FontDescent = FuncLoader.LoadFunction<d_sdl_ttf_getfontdescent>(NativeLibrary, "TTF_FontDescent");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_ttf_getfontlineskip(IntPtr font);
        public static readonly d_sdl_ttf_getfontlineskip TTF_FontLineSkip = FuncLoader.LoadFunction<d_sdl_ttf_getfontlineskip>(NativeLibrary, "TTF_FontLineSkip");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int d_sdl_ttf_getfontkerning(IntPtr font);
        public static readonly d_sdl_ttf_getfontkerning TTF_GetFontKerning = FuncLoader.LoadFunction<d_sdl_ttf_getfontkerning>(NativeLibrary, "TTF_GetFontKerning");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr d_sdl_ttf_getfontfamilyname(IntPtr font);
        private static readonly d_sdl_ttf_getfontfamilyname GetFontFaceFamilyName = FuncLoader.LoadFunction<d_sdl_ttf_getfontfamilyname>(NativeLibrary, "TTF_FontFaceFamilyName");

        public static string? TTF_FontFaceFamilyName(IntPtr font)
        {
            return InteropHelpers.Utf8ToString(GetFontFaceFamilyName(font));
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr d_sdl_ttf_getfontfacestylename(IntPtr font);
        private static readonly d_sdl_ttf_getfontfacestylename GetFontFaceStyleName = FuncLoader.LoadFunction<d_sdl_ttf_getfontfacestylename>(NativeLibrary, "TTF_FontFaceStyleName");

        public static string? TTF_FontFaceStyleName(IntPtr font)
        {
            return InteropHelpers.Utf8ToString(GetFontFaceStyleName(font));
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr d_sdl_ttf_renderutf8blended(IntPtr font, StringBuilder text, int fg);
        public static readonly d_sdl_ttf_renderutf8blended TTF_RenderUTF8_Blended = FuncLoader.LoadFunction<d_sdl_ttf_renderutf8blended>(NativeLibrary, "TTF_RenderUTF8_Blended");

    }
}
