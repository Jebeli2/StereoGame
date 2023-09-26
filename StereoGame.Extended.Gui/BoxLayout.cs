
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StereoGame.Extended.Gui
{
    public class BoxLayout : ILayout
    {
        private Orientation orientation;
        private Alignment alignment;
        private int margin;
        private int spacing;

        public BoxLayout(Orientation orientation, Alignment alignment = Alignment.Middle, int margin = 0, int spacing = 0)
        {
            this.orientation = orientation;
            this.alignment = alignment;
            this.margin = margin;
            this.spacing = spacing;
        }

        public Size GetPreferredSize(IGuiSystem context, Control control)
        {
            int sizeW = margin * 2;
            int sizeH = margin * 2;
            bool first = true;
            foreach (Control child in control.Children)
            {
                if (!child.Visible) continue;
                if (first)
                {
                    first = false;
                }
                else
                {
                    switch (orientation)
                    {
                        case Orientation.Horizontal:
                            sizeW += spacing;
                            break;
                        case Orientation.Vertical:
                            sizeH += spacing;
                            break;
                    }
                }
                Size ps = child.GetPreferredSize(context);
                Size fs = child.GetFixedSize();
                int targetW = fs.Width != 0 ? fs.Width : ps.Width;
                int targetH = fs.Height != 0 ? fs.Height : ps.Height;
                switch (orientation)
                {
                    case Orientation.Horizontal:
                        sizeW += targetW;
                        sizeH = Math.Max(sizeH, targetH + 2 * margin);
                        break;
                    case Orientation.Vertical:
                        sizeH += targetH;
                        sizeW = Math.Max(sizeW, targetW + 2 * margin);
                        break;
                }
            }
            return new Size(sizeW, sizeH);
        }

        public void PerformLayout(IGuiSystem context, Control control)
        {
            Size fsw = control.GetFixedSize();
            int cw = fsw.Width != 0 ? fsw.Width : control.Width;
            int ch = fsw.Height != 0 ? fsw.Height : control.Height;
            int position = margin;
            bool first = true;
            foreach (Control child in control.Children)
            {
                if (!child.Visible) continue;
                if (first)
                {
                    first = false;
                }
                else
                {
                    position += spacing;
                }
                Size ps = child.GetPreferredSize(context);
                Size fs = child.GetFixedSize();
                int targetW = fs.Width != 0 ? fs.Width : ps.Width;
                int targetH = fs.Height != 0 ? fs.Height : ps.Height;
                Point pos = new Point();
                switch (orientation)
                {
                    case Orientation.Horizontal:
                        pos.X = position;
                        switch (alignment)
                        {
                            case Alignment.Minimum:
                                pos.Y += margin;
                                break;
                            case Alignment.Middle:
                                pos.Y += (ch - targetH) / 2;
                                break;
                            case Alignment.Maximum:
                                pos.Y += ch - targetH - margin * 2;
                                break;
                            case Alignment.Fill:
                                pos.Y += margin;
                                targetH = fsw.Height != 0 ? fsw.Height : (ch - margin * 2);
                                break;
                        }

                        break;
                    case Orientation.Vertical:
                        pos.Y = position;
                        switch (alignment)
                        {
                            case Alignment.Minimum:
                                pos.X += margin;
                                break;
                            case Alignment.Middle:
                                pos.X += (cw - targetW) / 2;
                                break;
                            case Alignment.Maximum:
                                pos.X += cw - targetW - margin * 2;
                                break;
                            case Alignment.Fill:
                                pos.X += margin;
                                targetW = fsw.Width != 0 ? fsw.Width : (cw - margin * 2);
                                break;
                        }
                        break;
                }
                child.X = pos.X;
                child.Y = pos.Y;
                child.Width = targetW;
                child.Height = targetH;
                child.PerformLayout(context);
                switch(orientation)
                {
                    case Orientation.Horizontal:
                        position += targetW;
                        break;
                    case Orientation.Vertical:
                        position += targetH;
                        break;
                }
            }
        }
    }
}
