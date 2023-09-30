
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
            Size size = new Size(margin * 2, margin * 2);
            int xOffset = control.Padding.Horizontal;
            int yOffset = control.Padding.Vertical;
            bool first = true;
            int axis1 = (int)orientation;
            int axis2 = (((int)orientation) + 1) % 2;
            foreach (Control child in control.Children)
            {
                if (!child.Visible) continue;
                if (child.ExcludeFromLayout) continue;
                if (first)
                {
                    first = false;
                }
                else
                {
                    size.AddAxis(axis1, spacing);
                }
                Size ps = child.GetPreferredSize(context);
                Size fs = child.GetFixedSize();
                Size targetSize = ps.GetValidSize(ref fs);
                size.AddAxis(axis1, targetSize.GetAxis(axis1));
                size.SetAxis(axis2, Math.Max(size.GetAxis(axis2), targetSize.GetAxis(axis2) + 2 * margin));
            }
            size.Width += xOffset;
            size.Height += yOffset;
            return size;
        }

        public void PerformLayout(IGuiSystem context, Control control)
        {
            Size fsw = control.GetFixedSize();
            Size cw = new Size(control.Width, control.Height);
            Size cs = cw.GetValidSize(ref fsw);
            cs.Width -= control.Padding.Horizontal;
            cs.Height -= control.Padding.Vertical;
            int axis1 = (int)orientation;
            int axis2 = (((int)orientation) + 1) % 2;
            int position = margin;
            int xOffset = control.Padding.Left;
            int yOffset = control.Padding.Top;
            if (orientation == Orientation.Vertical)
            {
                position += yOffset;
            }
            else
            {
                position += xOffset;
            }
            bool first = true;
            foreach (Control child in control.Children)
            {
                if (!child.Visible) continue;
                if (child.ExcludeFromLayout) continue;
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
                Size targetSize = ps.GetValidSize(ref fs);
                Point pos = new Point(xOffset, yOffset);
                pos.SetAxis(axis1, position);
                switch (alignment)
                {
                    case Alignment.Minimum:
                        pos.AddAxis(axis2, margin);
                        break;
                    case Alignment.Middle:
                        pos.AddAxis(axis2, (cs.GetAxis(axis2) - targetSize.GetAxis(axis2)) / 2);
                        break;
                    case Alignment.Maximum:
                        pos.AddAxis(axis2, cs.GetAxis(axis2) - targetSize.GetAxis(axis2) - margin * 2);
                        break;
                    case Alignment.Fill:
                        pos.AddAxis(axis2, margin);
                        targetSize.SetAxis(axis2, fs.GetAxis(axis2) != 0 ? fs.GetAxis(axis2) : (cs.GetAxis(axis2) - margin * 2));
                        break;
                }

                child.SetBounds(pos.X, pos.Y, targetSize.Width, targetSize.Height);
                child.PerformLayout(context);
                position += targetSize.GetAxis(axis1);
            }
        }
    }
}
