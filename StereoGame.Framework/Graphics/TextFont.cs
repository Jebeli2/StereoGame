using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Framework.Graphics
{
    public abstract class TextFont : IDisposable
    {
        private int ySize;
        protected FontStyle fontStyle;
        protected int fontOutline;
        protected FontHinting fontHinting;
        protected int fontHeight;
        protected int fontAscent;
        protected int fontDescent;
        protected int fontLineSkip;
        protected int fontKerning;
        protected string familyName;
        protected string styleName;
        private bool disposed;

        protected TextFont(int ySize)
        {
            this.ySize = ySize;
            familyName = "unknown";
            styleName = "regular";
        }

        ~TextFont()
        {
            Dispose(false);
        }

        public int YSize => ySize;

        public FontStyle FontStyle => fontStyle;
        public FontHinting FontHinting => fontHinting;
        public int FontHeight => fontHeight;
        public int FontOutline => fontOutline;
        public int FontAscent => fontAscent;
        public int FontDescent => fontDescent;
        public int FontLineSkip => fontLineSkip;
        public int FontKerning => fontKerning;
        public string FontName => familyName;
        public string StyleName => styleName;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
            }
        }

        public abstract Size MeasureText(ReadOnlySpan<char> text);
        public abstract Size MeasureText(StringBuilder text);
        public abstract int GetGlyphMetrics(char c, out int minx, out int maxx, out int miny, out int maxy, out int advance);
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (familyName != null) { sb.Append(familyName); }
            sb.Append(' ');
            if (styleName != null) { sb.Append(styleName); }
            sb.Append(' ');
            sb.Append(ySize);
            return sb.ToString();
        }
    }
}
