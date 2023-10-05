using StereoGame.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Platform.SDL
{
    internal static class SDL2TTF
    {
        private const string nativeLibName = "SDL2_ttf";

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_Init();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TTF_Quit();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_WasInit();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TTF_CloseFont(IntPtr font);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_OpenFont(string file, int ptsize);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_OpenFontRW(IntPtr src, int freesrc, int ptsize);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_GetFontStyle(IntPtr font);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_GetFontOutline(IntPtr font);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_GetFontHinting(IntPtr font);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_FontHeight(IntPtr font);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_FontAscent(IntPtr font);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_FontDescent(IntPtr font);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_FontLineSkip(IntPtr font);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_GetFontKerning(IntPtr font);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_FontFaceFamilyName(IntPtr font);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_FontFaceStyleName(IntPtr font);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderUTF8_Blended(IntPtr font, [In()][MarshalAs(UnmanagedType.LPUTF8Str)] StringBuilder text, int fg);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_SizeUTF8(IntPtr font, [In()][MarshalAs(UnmanagedType.LPUTF8Str)] StringBuilder text, out int w, out int h);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderGlyph32_Blended(IntPtr font, uint ch, int fg);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_GlyphMetrics32(IntPtr font, uint ch, out int minx, out int maxx, out int miny, out int maxy, out int advance);

        public static int Init() => TTF_Init();
        public static void Quit() => TTF_Quit();
        public static int WasInit() => TTF_WasInit();
        public static void CloseFont(IntPtr font) => TTF_CloseFont(font);
        public static IntPtr OpenFont(string file, int ptsize) => TTF_OpenFont(file, ptsize);
        public static IntPtr OpenFontRW(IntPtr src, int freesrc, int ptsize) => TTF_OpenFontRW(src, freesrc, ptsize);
        public static int GetFontStyle(IntPtr font) => TTF_GetFontStyle(font);
        public static int GetFontOutline(IntPtr font) => TTF_GetFontOutline(font);
        public static int GetFontHinting(IntPtr font) => TTF_GetFontHinting(font);
        public static int FontHeight(IntPtr font) => TTF_FontHeight(font);
        public static int FontAscent(IntPtr font) => TTF_FontAscent(font);
        public static int FontDescent(IntPtr font) => TTF_FontDescent(font);
        public static int FontLineSkip(IntPtr font) => TTF_FontLineSkip(font);
        public static int GetFontKerning(IntPtr font) => TTF_GetFontKerning(font);
        public static string? FontFaceFamilyName(IntPtr font)
        {
            return InteropHelpers.Utf8ToString(TTF_FontFaceFamilyName(font));
        }
        public static string? FontFaceStyleName(IntPtr font)
        {
            return InteropHelpers.Utf8ToString(TTF_FontFaceStyleName(font));
        }
        public static IntPtr RenderUTF8_Blended(IntPtr font, StringBuilder text, int fg) => TTF_RenderUTF8_Blended(font, text, fg);

        public static int SizeUTF8(IntPtr font, StringBuilder text, out int w, out int h) => TTF_SizeUTF8(font, text, out w, out h);
        //public static int SizeUTF8(IntPtr font, ReadOnlySpan<char> text, out int w, out int h) => TTF_SizeUTF8(font, text, out w, out h);
        public static IntPtr RenderGlyph32_Blended(IntPtr font, uint ch, int fg) => TTF_RenderGlyph32_Blended(font, ch, fg);
        public static int GlyphMetrics32(IntPtr font, uint ch, out int minx, out int maxx, out int miny, out int maxy, out int advance) => TTF_GlyphMetrics32(font, ch, out minx, out maxx, out miny, out maxy, out advance);
    }
}
