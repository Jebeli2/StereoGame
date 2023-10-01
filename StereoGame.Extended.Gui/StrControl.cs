namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
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
                NormSelection();
            }
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

        protected bool WrapAtChar(int x, char s)
        {
            return false;
        }

        protected bool MapPosition(int mx, int my, out int pos)
        {
            pos = -1;
            int nLines = 0;
            int x = 0;
            int y = 0;
            if (my < y) return false;
            if (mx < x) return false;
            for (int i = 0; i < buffer.Length; i++)
            {
                char ch = buffer[i];
                if (WrapAtChar(x, ch))
                {
                    x = 0;
                    nLines++;
                }
                if (ch == '\n')
                {
                    x = 0;
                    nLines++;
                }
                else if (ch == '\t')
                {
                    x += textTabWidth;
                }
                else
                {
                    Size gs = Font?.MeasureText("" + ch) ?? new Size(lineSkip, lineSkip);
                    int gx = gs.Width;
                    if ((my >= y && my <= (y + lineSkip)) && (mx >= x && mx <= x + gx))
                    {
                        pos = i;
                        return true;
                    }
                    x += gx;
                }
            }
            if ((my >= y && my <= (y + lineSkip)) && (mx >= x))
            {
                pos = buffer.Length;
                return true;
            }
            return false;
        }

        public override bool OnPointerUp(PointerEventArgs args)
        {
            if (MapPosition(args.X, args.Y, out int pos))
            {
                SetBufferSel(pos);
            }
            return base.OnPointerUp(args);
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
            base.DrawControl(gui, renderer, gameTime, ref bounds);
            string buff = buffer + " ";
            Rectangle cFrame = bounds;
            cFrame.Inflate(-2, -2);
            int x = cFrame.X;
            int y = cFrame.Y;
            bool selected = false;
            Rectangle charRect = cFrame;
            var ha = Framework.Graphics.HorizontalAlignment.Left;
            var va = Framework.Graphics.VerticalAlignment.Top;
            for (int i = 0; i < buffer.Length; i++)
            {
                var c = buffer[i];

                selected = (i >= bufferSelStart && i < bufferSelEnd);
                if (selected)
                {
                    renderer.DrawText(Font, "" + c, charRect, TextColor, ha, va);
                }
                else
                {
                    renderer.DrawText(Font, "" + c, charRect, TextColor, ha, va);
                }
                if (i == bufferPos) // drawCursor
                {
                    if (Focused)
                    {
                        renderer.DrawVerticalLine(x, y, y + lineSkip - 1, TextColor);
                    }
                }
                int ax = Font?.MeasureText("" + c).Width ?? lineSkip;
                charRect.X += ax;
            }
        }
    }
}
