namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ITheme
    {

        Color ShinePen { get; }
        Color ShadowPen { get; }
        void DrawControl(IGuiSystem gui, IGuiRenderer renderer, GameTime time, Control control, ref Rectangle bounds);
        void DrawLayoutControl(IGuiSystem gui, IGuiRenderer renderer, GameTime time, LayoutControl layoutControl, ref Rectangle bounds);
        void DrawWindow(IGuiSystem gui, IGuiRenderer renderer, GameTime time, Window window, ref Rectangle bounds);
        void DrawButton(IGuiSystem gui, IGuiRenderer renderer, GameTime time, Button button, ref Rectangle bounds);
        void DrawSysButton(IGuiSystem gui, IGuiRenderer renderer, GameTime time, SysButton sysButton, ref Rectangle bounds);
        void DrawCheckBox(IGuiSystem gui, IGuiRenderer renderer, GameTime time, CheckBox checkBox, ref Rectangle bounds);
        void DrawLabel(IGuiSystem gui, IGuiRenderer renderer, GameTime time, Label label, ref Rectangle bounds);
        void DrawPropControl(IGuiSystem gui, IGuiRenderer renderer, GameTime time, PropControl prop, ref Rectangle bounds, ref Rectangle knob);
        void DrawScrollBar(IGuiSystem gui, IGuiRenderer renderer, GameTime time, ScrollBar scrollBar, ref Rectangle bounds);

    }
}
