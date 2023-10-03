namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using StereoGame.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection.Emit;
    using System.Text;
    using System.Threading.Tasks;

    public class BaseTheme : ITheme
    {
        public const int ICONWIDTH = 20;
        public const int ICONHEIGHT = 20;

        private static readonly string spaceText = " ";
        private static BaseTheme? instance;

        public Color ShinePen { get; set; }
        public Color ShadowPen { get; set; }
        public Color SelectedTextBackPen { get; set; }
        public Color SelectedTextPen { get; set; }
        public Color TextPen { get; set; }
        public Color TextHoverPen { get; set; }
        public Color SysTextPen { get; set; }
        public Color SysHoverTextPen { get; set; }

        public Color WindowActiveBackPen { get; set; }
        public Color WindowInactiveActiveBackPen { get; set; }
        public Color WindowActiveTopBorderPen { get; set; }
        public Color WindowActiveBottomBorderPen { get; set; }
        public Color WindowInactiveTopBorderPen { get; set; }
        public Color WindowInactiveBottomBorderPen { get; set; }
        public Color WindowActiveTitlePen { get; set; }
        public Color WindowInactiveTitlePen { get; set; }

        public Color ButtonTopBackPen { get; set; }
        public Color ButtonBottomBackPen { get; set; }
        public Color ButtonTopHoverPen { get; set; }
        public Color ButtonBottomHoverPen { get; set; }
        public Color ButtonTopSelectedPen { get; set; }
        public Color ButtonBottomSelectedPen { get; set; }

        public Color PropTopBackPen { get; set; }
        public Color PropBottomBackPen { get; set; }
        public Color KnobTopPen { get; set; }
        public Color KnobBottomPen { get; set; }
        public Color KnobTopHoverPen { get; set; }
        public Color KnobBottomHoverPen { get; set; }

        public Color StrTopBackPen { get; set; }
        public Color StrBottomBackPen { get; set; }

        public BaseTheme()
        {
            ShinePen = Color.FromArgb(250, 92, 92, 92);
            ShadowPen = Color.FromArgb(250, 29, 29, 29);
            SelectedTextBackPen = Color.FromArgb(230, 62, 92, 154);
            SelectedTextPen = Color.FromArgb(255, 255, 255, 255);
            TextPen = Color.FromArgb(238, 238, 238);
            TextHoverPen = Color.FromArgb(240, 240, 240);
            SysTextPen = Color.FromArgb(160, 220, 220, 220);
            SysHoverTextPen = Color.FromArgb(222, 245, 245, 245);

            WindowActiveBackPen = Color.FromArgb(230, 45, 45, 45);
            WindowInactiveActiveBackPen = Color.FromArgb(230, 43, 43, 43);
            WindowActiveTopBorderPen = Color.FromArgb(200, 62 + 10, 92 + 10, 154 + 10);
            WindowActiveBottomBorderPen = Color.FromArgb(130, 62, 92, 154);
            WindowInactiveTopBorderPen = Color.FromArgb(74, 74, 74);
            WindowInactiveBottomBorderPen = Color.FromArgb(48, 48, 48);
            WindowActiveTitlePen = Color.FromArgb(222, 245, 245, 245);
            WindowInactiveTitlePen = Color.FromArgb(160, 220, 220, 220);

            ButtonTopBackPen = Color.FromArgb(64, 64, 64);
            ButtonBottomBackPen = Color.FromArgb(48, 48, 48);
            ButtonTopHoverPen = Color.FromArgb(84, 84, 84);
            ButtonBottomHoverPen = Color.FromArgb(68, 68, 68);
            ButtonTopSelectedPen = Color.FromArgb(41, 41, 41);
            ButtonBottomSelectedPen = Color.FromArgb(28, 28, 28);

            PropTopBackPen = Color.FromArgb(32, 0, 0, 0);
            PropBottomBackPen = Color.FromArgb(92, 0, 0, 0);
            KnobTopPen = Color.FromArgb(100, 100, 100, 100);
            KnobBottomPen = Color.FromArgb(100, 128, 128, 128);
            KnobTopHoverPen = Color.FromArgb(100, 220, 220, 220);
            KnobBottomHoverPen = Color.FromArgb(100, 128, 128, 128);

            StrTopBackPen = Color.FromArgb(32, 0, 0, 0);
            StrBottomBackPen = Color.FromArgb(92, 0, 0, 0);

        }

        public static ITheme Instance
        {
            get
            {
                instance ??= new BaseTheme();
                return instance;
            }
        }

        public void DrawControl(IGuiSystem gui, IGuiRenderer renderer, GameTime time, Control control, ref Rectangle bounds)
        {
            renderer.DrawBorder(bounds, ShinePen, ShadowPen);
        }
        public void DrawLayoutControl(IGuiSystem gui, IGuiRenderer renderer, GameTime time, LayoutControl layoutControl, ref Rectangle bounds)
        {

        }

        public void DrawWindow(IGuiSystem gui, IGuiRenderer renderer, GameTime time, Window window, ref Rectangle bounds)
        {
            Color bg = window.Active ? WindowActiveBackPen : WindowInactiveActiveBackPen;
            renderer.FillRectangle(bounds, bg);
            Color btop = window.Active ? WindowActiveTopBorderPen : WindowInactiveTopBorderPen;
            Color bbot = window.Active ? WindowActiveBottomBorderPen : WindowInactiveBottomBorderPen;
            DrawWindowBorder(renderer, bounds, btop, bbot, window.Padding);
            DrawWindowTitle(renderer, window, bounds);
        }

        private void DrawWindowTitle(IGuiRenderer renderer, Window window, Rectangle rect)
        {
            Rectangle titleRect = rect;
            titleRect.Height = window.Padding.Top;
            titleRect.Inflate(-1, -1);
            titleRect.X += 32;
            titleRect.Width -= 32;
            Color tc = window.Active ? WindowActiveTitlePen : WindowInactiveTitlePen;
            renderer.DrawText(window.Font, window.Title, titleRect, tc, HorizontalAlignment.Left);

        }
        private void DrawWindowBorder(IGuiRenderer renderer, Rectangle rect, Color top, Color bottom, Padding border)
        {
            Rectangle topRect = new Rectangle(rect.X, rect.Top, rect.Width, border.Top);
            Rectangle botRect = new Rectangle(rect.X, rect.Bottom - border.Bottom, rect.Width, border.Bottom);
            Rectangle leftRect = new Rectangle(rect.X, rect.Top + border.Top, border.Left, rect.Height - border.Vertical);
            Rectangle rightRect = new Rectangle(rect.Right - border.Right, rect.Top + border.Top, border.Right, rect.Height - border.Vertical);
            Rectangle inner = new Rectangle(rect.X + border.Left, rect.Top + border.Top, rect.Width - border.Horizontal, rect.Height - border.Vertical);
            renderer.FillVertGradient(topRect, top, bottom);
            renderer.FillRectangle(leftRect, bottom);
            renderer.FillRectangle(rightRect, bottom);
            renderer.FillRectangle(botRect, bottom);
            renderer.DrawBorder(rect, ShinePen, ShadowPen);
            renderer.DrawBorder(inner, ShadowPen, ShinePen);
        }

        public void DrawButton(IGuiSystem gui, IGuiRenderer renderer, GameTime time, Button button, ref Rectangle bounds)
        {
            DrawControlBorder(gui, renderer, button, ref bounds);
            DrawControlText(gui, renderer, button, TextPen, TextHoverPen, ref bounds);
        }

        public void DrawSysButton(IGuiSystem gui, IGuiRenderer renderer, GameTime time, SysButton sysButton, ref Rectangle bounds)
        {
            if (sysButton.Pressed && !sysButton.Borderless)
            {
                renderer.DrawBorder(bounds, ShadowPen, ShinePen);
            }
            else if (sysButton.Hovered && !sysButton.Borderless)
            {
                renderer.DrawBorder(bounds, ShinePen, ShadowPen);
            }
            DrawControlText(gui, renderer, sysButton, SysTextPen, SysHoverTextPen, ref bounds);
        }

        public void DrawCheckBox(IGuiSystem gui, IGuiRenderer renderer, GameTime time, CheckBox checkBox, ref Rectangle bounds)
        {
            Rectangle border = AdjustCheckBoxBounds(ref bounds);
            DrawControlBorder(gui, renderer, checkBox, ref border);
            DrawControlText(gui, renderer, checkBox, TextPen, TextHoverPen, ref bounds);
        }


        public void DrawLabel(IGuiSystem gui, IGuiRenderer renderer, GameTime time, Label label, ref Rectangle bounds)
        {
            DrawControlText(gui, renderer, label, TextPen, TextHoverPen, ref bounds);
        }

        public void DrawPropControl(IGuiSystem gui, IGuiRenderer renderer, GameTime time, PropControl prop, ref Rectangle bounds, ref Rectangle knob)
        {
            renderer.FillVertGradient(bounds, PropTopBackPen, PropBottomBackPen);
            if (!prop.Borderless)
            {
                renderer.DrawBorder(bounds, ShinePen, ShadowPen);
            }
            Color kt = KnobTopPen;
            Color kb = KnobBottomPen;
            if (prop.Hovered)
            {
                kt = KnobTopHoverPen;
                kb = KnobBottomHoverPen;
            }
            renderer.FillVertGradient(knob, kt, kb);
        }

        public void DrawScrollBar(IGuiSystem gui, IGuiRenderer renderer, GameTime time, ScrollBar scrollBar, ref Rectangle bounds)
        {
            if (!scrollBar.Borderless)
            {
                renderer.DrawBorder(bounds, ShinePen, ShadowPen);
            }
        }

        public void DrawStrControl(IGuiSystem gui, IGuiRenderer renderer, GameTime time, StrControl str, ref Rectangle bounds)
        {
            renderer.FillVertGradient(bounds, StrTopBackPen, StrBottomBackPen);
            if (!str.Borderless) { renderer.DrawBorder(bounds, ShadowPen, ShinePen); }
            Rectangle inner = bounds;
            inner.Inflate(-1, -1);
            string buffer = str.Buffer;
            int x = inner.X;
            int y = inner.Y;
            int last = buffer.Length;
            int dispPos = str.DispPos;
            TextFont? font = str.Font;
            if (font != null)
            {
                renderer.PushClip(inner);
                for (int i = dispPos; i < last + 1; i++)
                {
                    char c = ' ';
                    ReadOnlySpan<char> txt;
                    if (i < last)
                    {
                        c = buffer[i];
                        txt = buffer.AsSpan(i, 1);
                    }
                    else
                    {
                        txt = spaceText;
                    }
                    bool selected = (i >= str.BufferSelStart && i < str.BufferSelEnd);
                    font.GetGlyphMetrics(c, out _, out _, out _, out _, out int advance);
                    if (selected)
                    {
                        renderer.FillRectangle(new Rectangle(x, y, advance, inner.Height), SelectedTextBackPen);
                        renderer.DrawText(font, txt, x, y, 0, 0, SelectedTextPen, HorizontalAlignment.Left, VerticalAlignment.Top);
                    }
                    else
                    {
                        renderer.DrawText(font, txt, x, y, 0, 0, TextPen, HorizontalAlignment.Left, VerticalAlignment.Top);

                    }
                    if (i == str.BufferPos && str.Active && str.ShowCaret)
                    {
                        renderer.DrawVerticalLine(x, y, y + font.FontHeight, TextPen);
                    }
                    x += advance;
                }
                renderer.PopClip();
            }
        }

        private Rectangle AdjustCheckBoxBounds(ref Rectangle bounds)
        {
            Rectangle adjustedRect = bounds;
            adjustedRect.Width = (ICONWIDTH * 3) / 2;
            return adjustedRect;
        }
        private void DrawControlBorder(IGuiSystem gui, IGuiRenderer renderer, Control control, ref Rectangle bounds)
        {
            Color bgtop = ButtonTopBackPen;
            Color bgbot = ButtonBottomBackPen;
            if (control.Pressed || control.Checked)
            {
                bgtop = ButtonTopSelectedPen;
                bgbot = ButtonBottomSelectedPen;
            }
            else if (control.Hovered)
            {
                bgtop = ButtonTopHoverPen;
                bgbot = ButtonBottomHoverPen;
            }
            renderer.FillVertGradient(bounds, bgtop, bgbot);
            if (!control.Borderless)
            {
                if (control.Pressed)
                {
                    renderer.DrawBorder(bounds, ShadowPen, ShinePen);
                }
                else
                {
                    renderer.DrawBorder(bounds, ShinePen, ShadowPen);
                }
            }
        }

        private void DrawControlText(IGuiSystem gui, IGuiRenderer renderer, Control control, Color textPen, Color hoverPen, ref Rectangle bounds)
        {
            if (control.TextIsTitle) return;
            Color tc = control.Hovered ? hoverPen : textPen;
            if (!string.IsNullOrEmpty(control.Text) && (control.Icon != Icons.NONE || control.AlwaysUseIconSpace))
            {
                Rectangle textBounds = bounds;
                Rectangle iconBounds = bounds;
                iconBounds.Width = (ICONWIDTH * 3) / 2;
                switch (control.HorizontalTextAlignment)
                {
                    case HorizontalAlignment.Left:
                        textBounds.X += ICONWIDTH * 2;
                        textBounds.Width -= ICONWIDTH * 2;
                        break;
                    case HorizontalAlignment.Right:
                        break;
                }
                renderer.DrawText(control.Font, control.Text, textBounds, tc, control.HorizontalTextAlignment, control.VerticalTextAlignment);
                renderer.DrawIcon(control.Icon, iconBounds, tc, HorizontalAlignment.Center, control.VerticalTextAlignment);
            }
            else if (!string.IsNullOrEmpty(control.Text))
            {
                renderer.DrawText(control.Font, control.Text, bounds, tc, control.HorizontalTextAlignment, control.VerticalTextAlignment);
            }
            else if (control.Icon != Icons.NONE)
            {
                renderer.DrawIcon(control.Icon, bounds, tc, control.HorizontalTextAlignment, control.VerticalTextAlignment);
            }
        }

    }
}
