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

    public class ScrollBar : LayoutControl
    {
        private ArrowPlace arrowPlace;
        private Orientation orientation;
        private int min;
        private int max;
        private int value;
        private int visibleAmount;
        private int overlap;
        private int increment;
        private readonly PropControl prop;
        private readonly SysButton arrowInc;
        private readonly SysButton arrowDec;

        public ScrollBar(Control? parent, Orientation orientation, int min = 1, int max = 100, int visibleAmount = 100)
            : base(parent)
        {
            arrowPlace = ArrowPlace.Split;
            this.orientation = orientation;
            overlap = 1;
            increment = 1;
            this.min = min;
            this.max = max;
            this.visibleAmount = visibleAmount;
            value = min;
            bool vert = orientation == Orientation.Vertical;
            prop = new PropControl(this)
            {
                FreeHoriz = !vert,
                FreeVert = vert,
                Borderless = true
            };
            arrowInc = new SysButton(this, vert ? Icons.TRIANGLE_DOWN : Icons.TRIANGLE_RIGHT)
            {
                Repeat = true,
                Borderless = true,
            };
            arrowDec = new SysButton(this, vert ? Icons.TRIANGLE_UP : Icons.TRIANGLE_LEFT)
            {
                Repeat = true,
                Borderless = true,
            };

            prop.PropChanged += Prop_PropChanged;
            arrowInc.Clicked += ArrowInc_Clicked;
            arrowDec.Clicked += ArrowDec_Clicked;
            AdjustProp();
        }


        public int Increment
        {
            get { return increment; }
            set { increment = value; }
        }
        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                    if (orientation == Orientation.Horizontal)
                    {
                        arrowInc.Icon = Icons.TRIANGLE_RIGHT;
                        arrowDec.Icon = Icons.TRIANGLE_LEFT;
                        prop.FreeHoriz = true;
                        prop.FreeVert = false;
                    }
                    else
                    {
                        arrowInc.Icon = Icons.TRIANGLE_DOWN;
                        arrowDec.Icon = Icons.TRIANGLE_UP;
                        prop.FreeHoriz = false;
                        prop.FreeVert = true;
                    }
                }
            }
        }
        public int Min
        {
            get { return min; }
            set
            {
                if (min != value)
                {
                    min = value;
                    AdjustProp();
                }
            }
        }

        public int Max
        {
            get { return max; }
            set
            {
                if (max != value)
                {
                    max = value;
                    AdjustProp();
                }
            }
        }

        public int Value
        {
            get { return value; }
            set
            {
                if (value > max) value = max;
                if (value < min) value = min;
                if (this.value != value)
                {
                    this.value = value;
                    AdjustProp();
                }
            }
        }

        public int VisibleAmount
        {
            get { return visibleAmount; }
            set
            {
                if (visibleAmount != value)
                {
                    visibleAmount = value;
                    AdjustProp();
                }
            }
        }

        public int Overlap
        {
            get { return overlap; }
            set
            {
                if (overlap != value)
                {
                    overlap = value;
                    AdjustProp();
                }
            }
        }

        private void ArrowDec_Clicked(object? sender, EventArgs e)
        {
            Value -= increment;
        }

        private void ArrowInc_Clicked(object? sender, EventArgs e)
        {
            Value += increment;
        }

        private void Prop_PropChanged(object? sender, EventArgs e)
        {
            int total = max - min;
            int pot = orientation == Orientation.Horizontal ? prop.HorizPot : prop.VertPot;
            Value = PropControl.FindScrollerTop(total, visibleAmount, pot);
        }

        private void AdjustProp()
        {

            int total = max - min;
            PropControl.FindScrollerValues(total, visibleAmount, value, overlap, out int body, out int pot);
            prop.ModifyProp(prop.FreeHoriz, prop.FreeVert, pot, pot, body, body);

        }
        private Control[] GetControls()
        {
            switch (arrowPlace)
            {
                case ArrowPlace.Split:
                    return new Control[] { arrowDec, prop, arrowInc };
                case ArrowPlace.LeftTop:
                    return new Control[] { arrowDec, arrowInc, prop };
                case ArrowPlace.RightBottom:
                    return new Control[] { prop, arrowDec, arrowInc };
                case ArrowPlace.None:
                default:
                    return new Control[] { prop };
            }
        }

        public override Size GetPreferredSize(IGuiSystem context)
        {
            Size size = prop.GetPreferredSize(context);
            if (arrowPlace != ArrowPlace.None)
            {
                if (orientation == Orientation.Horizontal)
                {
                    size.Width += 2 * PropControl.PROP_SIZE;
                }
                else
                {
                    size.Height += 2 * PropControl.PROP_SIZE;
                }
            }
            return size;
        }

        public override void PerformLayout(IGuiSystem context)
        {
            int x = 1;
            int y = 1;
            int size = PropControl.PROP_SIZE - 2;
            int propSize = orientation == Orientation.Horizontal ? Width - 2 : Height - 2;
            if (arrowPlace != ArrowPlace.None)
            {
                propSize -= 2 * size;
            }
            arrowDec.Visible = arrowPlace != ArrowPlace.None;
            arrowInc.Visible = arrowPlace != ArrowPlace.None;
            Control[] gadgets = GetControls();
            foreach (var gad in gadgets)
            {
                if (orientation == Orientation.Horizontal)
                {
                    if (gad is PropControl)
                    {
                        gad.SetFixedBounds(x, y, propSize, size);
                    }
                    else
                    {
                        gad.SetFixedBounds(x, y, size, size);
                    }
                    x += gad.Width;
                }
                else
                {
                    if (gad is PropControl)
                    {
                        gad.SetFixedBounds(x, y, size, propSize);
                    }
                    else
                    {
                        gad.SetFixedBounds(x, y, propSize, size);
                    }
                    y += gad.Height;
                }
            }
            foreach (var c in Children)
            {
                if (c.Visible)
                {
                    c.PerformLayout(context);
                }
            }
        }

        protected override void DrawControl(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, ref Rectangle bounds)
        {
            Theme.DrawScrollBar(gui, renderer, gameTime, this, ref bounds);
        }
    }
}
