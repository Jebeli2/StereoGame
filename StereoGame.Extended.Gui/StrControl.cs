﻿namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using StereoGame.Framework.Graphics;
    using StereoGame.Framework.Input;
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
        private bool lockDispPos;
        private bool showCaret;
        private double doubleValue;
        private StrFlags flags;
        public StrControl(Control? parent, string buffer = "")
            : base(parent)
        {
            textIsTitle = true;
            showCaret = true;
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
            AdjustDispPos();
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
            if (lockDispPos) { return; }
            int cd = CalcBufferPosDispPos(Font);
            if (cd != 0)
            {
                Rectangle bounds = GetBounds();
                Rectangle inner = bounds;
                inner.Inflate(-1, -1);
                if (cd > inner.Width)
                {
                    while (cd > inner.Width)
                    {
                        dispPos++;
                        cd = CalcBufferPosDispPos(Font);
                    }
                }
                else if (cd < inner.Width)
                {
                    while (cd < inner.Width && dispPos > 0)
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
            Pressed = true;
            ScreenToControl(args.X, args.Y, out int x, out int y);
            if (GetPos(x, y, out int pos))
            {
                lockDispPos = true;
                BufferPos = pos;
                return true;
            }
            return base.OnPointerDown(args);
        }
        public override bool OnPointerUp(PointerEventArgs args)
        {
            if (Pressed)
            {
                Pressed = false;
            }
            ScreenToControl(args.X, args.Y, out int x, out int y);
            if (GetPos(x, y, out int pos))
            {
                SetBufferSel(pos);
                lockDispPos = false;
                return true;
            }
            lockDispPos = false;
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

        public override bool OnKeyPressed(KeyboardEventArgs args)
        {
            switch (args.Key)
            {
                case Keys.Enter: return true;
                case Keys.Delete:
                    RemoveOrDelText(false);
                    break;
                case Keys.Back:
                    RemoveOrDelText(true);
                    break;
                case Keys.Home:
                    BufferPos = 0;
                    break;
                case Keys.End:
                    BufferPos = Buffer.Length;
                    break;
                case Keys.Left:
                    if (BufferPos > 0)
                    {
                        BufferPos--;
                    }
                    break;
                case Keys.Right:
                    if (BufferPos < Buffer.Length)
                    {
                        BufferPos++;
                    }
                    break;
                default:
                    if (args.Character != null)
                    {
                        ReplaceOrAddText("" + args.Character.Value);
                    }
                    break;
            }
            return base.OnKeyPressed(args);
        }

        public override bool OnKeyTyped(KeyboardEventArgs args)
        {
            return base.OnKeyTyped(args);
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

        private void RemoveOrDelText(bool backSpace)
        {
            if (bufferSelStart >= 0 && bufferSelEnd > 0)
            {
                int start = bufferSelStart;
                int len = bufferSelEnd - start;
                string temp = buffer.Remove(start,len);                
                if (MaybeChangeBuffer(temp))
                {
                    bufferSelStart = 0;
                    bufferSelEnd = 0;
                    BufferPos = start;
                }
            }
            else if (backSpace)
            {
                if (bufferPos > 0)
                {
                    string temp = buffer.Remove(bufferPos - 1, 1);
                    if (MaybeChangeBuffer(temp))
                    {
                        BufferPos--;
                    }
                }
            }
            else
            {
                if (bufferPos < buffer.Length)
                {
                    string temp = buffer.Remove(bufferPos, 1);
                    MaybeChangeBuffer(temp);
                    Invalidate();
                }
            }
        }
        private void ReplaceOrAddText(string text)
        {
            if (bufferSelStart >= 0 && bufferSelEnd > 0)
            {
                int start = bufferSelStart;
                int len = bufferSelEnd - start;
                string temp = buffer.Remove(start, len);
                temp = temp.Insert(start, text);
                if (MaybeChangeBuffer(temp))
                {
                    BufferPos = start + text.Length;
                }
            }
            else
            {
                string temp = buffer.Insert(bufferPos, text);
                if (MaybeChangeBuffer(temp))
                {
                    BufferPos += text.Length;
                }
            }
        }

        protected virtual bool MaybeChangeBuffer(string newBuffer)
        {
            if (!string.Equals(buffer, newBuffer))
            {
                if (flags == StrFlags.Integer)
                {
                    if (string.IsNullOrEmpty(newBuffer))
                    {
                        intValue = 0;
                    }
                    else if (!int.TryParse(newBuffer, out int result))
                    {
                        return false;
                    }
                    else
                    {
                        intValue = result;
                    }
                }
                else if (flags == StrFlags.Double)
                {
                    if (string.IsNullOrEmpty(newBuffer))
                    {
                        doubleValue = 0;
                    }
                    else if (!double.TryParse(newBuffer, out double result))
                    {
                        return false;
                    }
                    else
                    {
                        doubleValue = result;
                    }

                }
                buffer = newBuffer;
                return true;
            }
            return false;
        }

    }
}
