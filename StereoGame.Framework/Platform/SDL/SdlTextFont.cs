using StereoGame.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Platform.SDL
{
    internal class SdlTextFont : TextFont
    {
        private readonly IntPtr handle;
        private readonly IntPtr mem;

        public SdlTextFont(IntPtr handle, int ptSize)
            : this(handle, ptSize, IntPtr.Zero)
        {

        }
        public SdlTextFont(IntPtr handle, int ptSize, IntPtr mem)
            : base(ptSize)
        {
            this.handle = handle;
            this.mem = mem;
            fontStyle = (FontStyle)SDL2TTF.GetFontStyle(handle);
            fontOutline = SDL2TTF.GetFontOutline(handle);
            fontHinting = (FontHinting)SDL2TTF.GetFontHinting(handle);
            fontHeight = SDL2TTF.FontHeight(handle);
            fontAscent = SDL2TTF.FontAscent(handle);
            fontDescent = SDL2TTF.FontDescent(handle);
            fontLineSkip = SDL2TTF.FontLineSkip(handle);
            fontKerning = SDL2TTF.GetFontKerning(handle);
            familyName = SDL2TTF.FontFaceFamilyName(handle) ?? "unknown";
            styleName = SDL2TTF.FontFaceStyleName(handle) ?? "unkown";
        }

        public IntPtr Handle => handle;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            SDL2TTF.CloseFont(handle);
            if (mem != IntPtr.Zero) { Marshal.FreeHGlobal(mem); }
        }

        public override Size MeasureText(string? text)
        {
            int w = 0;
            int h = 0;
            if (!string.IsNullOrEmpty(text))
            {
                _ = SDL2TTF.SizeUTF8(handle, text, out w, out h);
            }
            return new Size(w, h);
        }
    }
}
