namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Channels;
    using System.Threading.Tasks;

    public class PropControl : Control
    {
        public const int PROP_SIZE = 20;
        public const int KNOBHMIN = 6;
        public const int KNOBVMIN = 4;
        public const int MAXBODY = 0xFFFF;
        public const int MAXPOT = 0xFFFF;

        private int horizBody;
        private int vertBody;
        private int horizPot;
        private int vertPot;
        private int hpotRes;
        private int vpotRes;
        private bool freeHoriz;
        private bool freeVert;
        private int cWidth;
        private int cHeight;

        private Rectangle knob;
        private int knobStartX;
        private int knobStartY;
        private bool knobHit;
        private bool containerHit;
        private int timerDelay;
        public PropControl(Control? parent)
            : base(parent)
        {
            freeHoriz = true;
        }

        public event EventHandler<EventArgs>? PropChanged;
        public int HorizPot
        {
            get { return horizPot; }
            set { horizPot = value; }
        }

        public int VertPot
        {
            get { return vertPot; }
            set { vertPot = value; }
        }

        public int HorizBody
        {
            get { return horizBody; }
            set { horizBody = value; }
        }

        public int VertBody
        {
            get { return vertBody; }
            set { vertBody = value; }
        }

        public int HPotRes
        {
            get { return hpotRes; }
            set { hpotRes = value; }
        }

        public int VPotRes
        {
            get { return vpotRes; }
            set { vpotRes = value; }
        }

        public bool FreeHoriz
        {
            get { return freeHoriz; }
            set { freeHoriz = value; }
        }

        public bool FreeVert
        {
            get { return freeVert; }
            set { freeVert = value; }
        }

        public void ModifyProp(bool freeHoriz, bool freeVert, int horizPot, int vertPot, int horizBody, int vertBody)
        {
            this.freeHoriz = freeHoriz;
            this.freeVert = freeVert;
            this.horizPot = horizPot;
            this.vertPot = vertPot;
            this.horizBody = horizBody;
            this.vertBody = vertBody;
            CalcKnobSize();
            Invalidate();
            OnPropChanged();
        }

        protected virtual void OnPropChanged()
        {
            PropChanged?.Invoke(this, EventArgs.Empty);
        }

        public override Size GetPreferredSize(IGuiSystem context)
        {
            Size size = new Size(PROP_SIZE, PROP_SIZE);
            if (freeHoriz) size.Width += 75;
            if (freeVert) size.Height += 75;
            return size;
        }

        public override void PerformLayout(IGuiSystem context)
        {
            base.PerformLayout(context);
            CalcKnobSize();
        }


        private void CalcKnobSize()
        {
            Rectangle rect = GetBounds();
            knob = CalcKnobSize(rect);
        }

        private Rectangle CalcKnobSize(Rectangle container)
        {
            Rectangle knob = container;
            knob.X = container.X + 2 + BorderThickness;
            knob.Y = container.Y + 2 + BorderThickness;
            knob.Width = container.Width - 4 - 2 * BorderThickness;
            knob.Height = container.Height - 4 - 2 * BorderThickness;
            cWidth = knob.Width;
            cHeight = knob.Height;
            //int leftBorder = container.Left;
            //int topBorder = container.Top;
            if (freeHoriz)
            {
                knob.Width = cWidth * horizBody / MAXBODY;
                if (knob.Width < KNOBHMIN) knob.Width = KNOBHMIN;
                knob.X += (cWidth - knob.Width) * HorizPot / MAXPOT;
                if (horizBody > 0)
                {
                    if (horizBody < MAXBODY / 2)
                    {
                        hpotRes = MAXPOT * 32768 / ((MAXBODY * 32768 / horizBody) - 32768);
                    }
                    else
                    {
                        hpotRes = MAXPOT;
                    }
                }
                else
                {
                    hpotRes = 1;
                }
            }
            if (freeVert)
            {
                knob.Height = cHeight * vertBody / MAXBODY;
                if (knob.Height < KNOBVMIN) knob.Height = KNOBVMIN;
                knob.Y += (cHeight - knob.Height) * VertPot / MAXPOT;
                if (vertBody > 0)
                {
                    if (vertBody < MAXBODY / 2)
                    {
                        vpotRes = MAXPOT * 32768 / ((MAXBODY * 32768 / vertBody) - 32768);
                    }
                    else
                    {
                        vpotRes = MAXPOT;
                    }
                }
                else
                {
                    vpotRes = 1;
                }
            }
            return knob;
        }

        protected override void DrawControl(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, ref Rectangle bounds)
        {
            base.DrawControl(gui, renderer, gameTime, ref bounds);
            Rectangle knob = CalcKnobSize(bounds);
            renderer.FillRectangle(knob, KnobColor);
        }

        public override bool OnPointerDown(PointerEventArgs args)
        {
            Rectangle bounds = GetBounds();
            Rectangle knob = CalcKnobSize(bounds);
            if (knob.Contains(args.X, args.Y))
            {
                knobStartX = args.X - (knob.X - bounds.X);
                knobStartY = args.Y - (knob.Y - bounds.Y);
                Console.WriteLine($"Knob Hit: {args.X}/{args.Y} KS: {knobStartX}/{knobStartY}");
                knobHit = true;
                containerHit = false;
            }
            else
            {
                knobHit = false;
                containerHit = true;
                Console.WriteLine($"Knob Miss: {args.X}/{args.Y}");
                HandleContainerHit(knob, args.X, args.Y);
                timerDelay = 4;
            }
            return base.OnPointerDown(args);
        }

        public override bool OnPointerUp(PointerEventArgs args)
        {
            knobHit = false;
            containerHit = false;
            return base.OnPointerUp(args);
        }

        public override bool OnPointerMove(PointerEventArgs args)
        {
            if (knobHit)
            {
                HandleKnobMove(args.X, args.Y);
            }
            return base.OnPointerMove(args);
        }

        public override bool OnPointerTimerTick(PointerEventArgs args)
        {
            timerDelay--;
            if (timerDelay <= 0 && containerHit)
            {
                Rectangle container = GetBounds();
                Rectangle knob = CalcKnobSize(container);
                HandleContainerHit(knob, args.X, args.Y);
                timerDelay = 2;
            }
            return base.OnPointerTimerTick(args);
        }

        private void HandleKnobMove(int x, int y)
        {
            int dx = x - knobStartX;
            int dy = y - knobStartY;
            if (freeHoriz && cWidth != knob.Width)
            {
                dx = (dx * MAXPOT) / (cWidth - knob.Width);
                if (dx < 0) dx = 0;
                if (dx > MAXPOT) dx = MAXPOT;
            }
            if (freeVert && cHeight != knob.Height)
            {
                dy = (dy * MAXPOT) / (cHeight - knob.Height);
                if (dy < 0) dy = 0;
                if (dy > MAXPOT) dy = MAXPOT;
            }
            if ((freeHoriz && dx != horizPot) || (freeVert && dy != vertPot))
            {
                ModifyProp(freeHoriz, freeVert, dx, dy, horizBody, vertBody);
            }
        }

        private void HandleContainerHit(Rectangle knob, int x, int y)
        {
            int dx = horizPot;
            int dy = vertPot;
            if (freeHoriz)
            {
                if (x < knob.X)
                {
                    if (dx > hpotRes) { dx -= hpotRes; }
                    else { dx = 0; }
                }
                else if (x > knob.Right)
                {
                    if (dx < MAXPOT - hpotRes) { dx += hpotRes; }
                    else { dx = MAXPOT; }
                }
            }
            if (freeVert)
            {
                if (y < knob.Y)
                {
                    if (dy > vpotRes) { dy -= vpotRes; }
                    else { dy = 0; }
                }
                else if (y > knob.Bottom)
                {
                    if (dy < MAXPOT - vpotRes) { dy += vpotRes; }
                    else { dy = MAXPOT; }
                }
            }
            ModifyProp(freeHoriz, freeVert, dx, dy, horizBody, vertBody);
        }

        internal static void FindScrollerValues(int total, int visible, int top, int overlap, out int body, out int pot)
        {
            int hidden = total > visible ? total - visible : 0;
            if (top > hidden) top = hidden;
            body = (hidden > 0) ? ((visible - overlap) * MAXBODY) / (total - overlap) : MAXBODY;
            pot = (hidden > 0) ? (top * MAXPOT) / hidden : 0;
        }

        internal static int FindScrollerTop(int total, int visible, int pot)
        {
            int hidden = total > visible ? total - visible : 0;
            return ((hidden * pot) + (MAXPOT / 2)) / MAXPOT;
        }
    }
}
