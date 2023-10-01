namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StrControl : Control
    {
        private string buffer;
        private int bufferPos;
        private int textTabWidth;
        private int lineSkip;
        private int bufferSelStart;
        private int bufferSelEnd;
        private int intValue;
        private int dispPos;
        private bool showCaret;
        private double doubleValue;
        private StrFlags flags;
        public StrControl(Control? parent, string buffer = "")
            : base(parent)
        {
            textIsTitle = true;
            this.buffer = buffer;
            textTabWidth = 4 * 24;
            lineSkip = 24;

        }

        public string Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        public int BufferPos
        {
            get { return bufferPos; }
            set
            {
                if (value == -1) value = buffer.Length;
                if (value > buffer.Length) value = buffer.Length;
                if (value < 0) value = 0;
                bufferSelStart = 0;
                bufferSelEnd = 0;
                bufferPos = value;
                AdjustDispPos();
                Invalidate();
            }
        }

        public int DispPos
        {
            get { return dispPos; }
        }

        public bool ShowCaret
        {
            get { return showCaret; }
        }

        public int IntValue
        {
            get
            {
                return intValue;
            }
            set
            {
                intValue = value;
                buffer = value.ToString();
                flags |= StrFlags.Integer;
                flags &= ~StrFlags.Double;
            }
        }

        public double DoubleValue
        {
            get { return doubleValue; }
            set
            {
                doubleValue = value;
                buffer = value.ToString("F");
                flags |= StrFlags.Double;
                flags &= ~StrFlags.Integer;
            }
        }

        public int BufferSelStart
        {
            get { return bufferSelStart; }
        }

        public int BufferSelEnd
        {
            get { return bufferSelEnd; }
        }

        private void NormSelection()
        {
            if (bufferSelStart > bufferSelEnd)
            {
                int temp = bufferSelEnd;
                bufferSelEnd = bufferSelStart;
                bufferSelStart = temp;
            }
        }

        private void SetBufferSel(int start, int end)
        {
            bufferSelStart = start;
            bufferSelEnd = end;
            NormSelection();
            Invalidate();
        }

        private void SetBufferSel(int pos)
        {
            if (pos < bufferPos)
            {
                SetBufferSel(pos, bufferPos);
            }
            else if (pos > bufferPos)
            {
                SetBufferSel(bufferSelStart, pos);
            }
            else
            {
                SetBufferSel(pos, pos);
            }
        }

        private void AdjustDispPos()
        {
            int cd = CalcBufferPosDispPos(Font);
            if (cd != 0)
            {
                Rectangle bounds = GetBounds();
                if (cd > bounds.Width)
                {
                    while (cd > bounds.Width)
                    {
                        dispPos++;
                        cd = CalcBufferPosDispPos(Font);
                    }
                }
                else if (cd < bounds.Width)
                {
                    while (cd < bounds.Width && dispPos > 0)
                    {
                        dispPos--;
                        cd = CalcBufferPosDispPos(Font);
                    }
                }
            }
            else
            {
                dispPos = 0;
            }
        }
        private int CalcBufferPosDispPos(TextFont? font)
        {
            int x = 0;
            if (font != null)
            {
                for (int i = dispPos; i < bufferPos + 1; i++)
                {
                    char ch = ' ';
                    if (i < buffer.Length) { ch = buffer[i]; }
                    font.GetGlyphMetrics(ch, out _, out _, out _, out _, out int advance);
                    x += advance;
                }
            }
            return x;
        }
        private bool MapPosition(TextFont font, int mx, int my, out int pos)
        {
            pos = 0;
            int x = 0;
            int y = 0;
            int lineSkip = font.FontLineSkip;
            if (mx < x) return false;
            if (my < y) return false;
            for (int i = dispPos; i < buffer.Length; i++)
            {
                char ch = buffer[i];
                font.GetGlyphMetrics(ch, out _, out _, out _, out _, out int advance);
                int gx = advance;
                if ((my >= y && my <= (y + lineSkip)) && (mx >= x && mx <= x + gx))
                {
                    pos = i;
                    return true;
                }
                x += gx;
            }
            if ((my >= y && my <= (y + lineSkip)) && (mx >= x))
            {
                pos = buffer.Length;
                return true;
            }
            return false;
        }

        private bool GetPos(int x, int y, out int pos)
        {
            if (Font != null)
            {
                if (MapPosition(Font, x, y, out pos))
                {
                    return true;
                }
            }
            pos = -1;
            return false;
        }

        public override bool OnPointerDown(PointerEventArgs args)
        {
            showCaret = true;
            Pressed = true;
            ScreenToControl(args.X, args.Y, out int x, out int y);
            if (GetPos(x, y, out int pos))
            {

                BufferPos = pos;
                return true;
            }
            return base.OnPointerDown(args);
        }
        public override bool OnPointerUp(PointerEventArgs args)
        {
            showCaret = false;
            if (Pressed)
            {
                Pressed = false;
            }
            ScreenToControl(args.X, args.Y, out int x, out int y);
            if (GetPos(x, y, out int pos))
            {
                SetBufferSel(pos);
                return true;
            }
            return base.OnPointerUp(args);
        }

        public override bool OnPointerMove(PointerEventArgs args)
        {
            if (Active && Pressed)
            {
                ScreenToControl(args.X, args.Y, out int x, out int y);
                if (GetPos(x, y, out int pos))
                {
                    SetBufferSel(pos);
                    return true;
                }
            }
            return base.OnPointerMove(args);
        }

        public override Size GetPreferredSize(IGuiSystem context)
        {
            Size size = new Size(4, 4);
            Size? textSize = Font?.MeasureText(buffer);
            if (textSize != null)
            {
                size.Width += textSize.Value.Width;
                size.Height += textSize.Value.Height;
            }
            return size;
        }

        protected override void DrawControl(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, ref Rectangle bounds)
        {
            Theme?.DrawStrControl(gui, renderer, gameTime, this, ref bounds);
        }
    }
}
