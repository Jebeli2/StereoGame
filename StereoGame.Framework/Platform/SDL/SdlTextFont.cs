using StereoGame.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Platform.SDL
{
    internal class SdlTextFont : TextFont
    {
        private readonly IntPtr handle;

        public SdlTextFont(IntPtr handle, int ptSize)
            : base(ptSize)
        {
            this.handle = handle;
            fontStyle = (FontStyle)SDL2TTF.TTF_GetFontStyle(handle);
            fontOutline = SDL2TTF.TTF_GetFontOutline(handle);
            fontHinting = (FontHinting)SDL2TTF.TTF_GetFontHinting(handle);
            fontHeight = SDL2TTF.TTF_FontHeight(handle);
            fontAscent = SDL2TTF.TTF_FontAscent(handle);
            fontDescent = SDL2TTF.TTF_FontDescent(handle);
            fontLineSkip = SDL2TTF.TTF_FontLineSkip(handle);
            fontKerning = SDL2TTF.TTF_GetFontKerning(handle);
            familyName = SDL2TTF.TTF_FontFaceFamilyName(handle) ?? "unknown";
            styleName = SDL2TTF.TTF_FontFaceStyleName(handle) ?? "unkown";
        }

        public IntPtr Handle => handle;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            SDL2TTF.TTF_CloseFont(handle);
        }
    }
}
