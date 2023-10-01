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

    public class Window : LayoutControl
    {
        private static int nextWindowId;
        private readonly SysButton closeButton;
        private readonly SysButton minmaxButton;
        private WindowCloseAction defaultCloseAction;
        private readonly int windowId;
        private bool maximized;
        private int oldX;
        private int oldY;
        private int oldWidth;
        private int oldHeight;
        public Window()
            : this(null)

        {

        }
        public Window(Screen? screen)
            : base(screen)
        {
            windowId = ++nextWindowId;
            Padding = new Padding(4, 30, 4, 4);
            SetPosition(33, 33);
            defaultCloseAction = WindowCloseAction.None;
            superBitmap = true;
            textIsTitle = true;
            layout = new BoxLayout(Orientation.Vertical, Alignment.Fill, 10, 10);
            closeButton = MakeCloseButton();
            minmaxButton = MakeMinMaxButton();
        }

        public event EventHandler<WindowClosingEventArgs>? WindowClosing;
        public event EventHandler<EventArgs>? WindowClosed;

        public WindowCloseAction DefaultWindowCloseAction
        {
            get { return defaultCloseAction; }
            set { defaultCloseAction = value; }
        }

        public string? Title
        {
            get => Text;
            set => Text = value;
        }

        public bool Maximized => maximized;

        public void Maximize()
        {
            Screen? screen = Screen;
            if (screen != null)
            {
                if (!maximized)
                {
                    oldX = X;
                    oldY = Y;
                    oldWidth = Width;
                    oldHeight = Height;
                    maximized = true;
                    SetFixedBounds(0, 0, screen.Width, screen.Height);
                }
            }
        }

        public void Restore()
        {
            if (maximized)
            {
                SetFixedBounds(oldX, oldY, oldWidth, oldHeight);
                maximized = false;
            }
        }

        public override void Invalidate()
        {
            base.Invalidate();
            AdjustSysButtons();
        }

        public override void InvalidateLayout()
        {
            base.InvalidateLayout();
            AdjustSysButtons();
        }

        private void AdjustSysButtons()
        {
            if (minmaxButton != null)
            {
                minmaxButton.SetFixedBounds(Width - 30, 2, 28, 28);
                if (maximized)
                {
                    minmaxButton.Icon = Icons.RESIZE_100_PERCENT;
                }
                else
                {
                    minmaxButton.Icon = Icons.RESIZE_FULL_SCREEN;
                }
            }
        }

        private SysButton MakeCloseButton()
        {
            SysButton cb = new SysButton(this, Icons.CROSS);
            cb.ExcludeFromLayout = true;
            cb.SetFixedBounds(2, 2, 28, 28);
            cb.Clicked += Cb_Clicked;
            return cb;
        }

        private SysButton MakeMinMaxButton()
        {
            SysButton mmb = new SysButton(this, Icons.RESIZE_FULL_SCREEN);
            mmb.ExcludeFromLayout = true;
            mmb.SetFixedBounds(Width - 30, 2, 28, 28);
            mmb.Clicked += Mmb_Clicked;
            return mmb;
        }

        private void Mmb_Clicked(object? sender, EventArgs e)
        {
            if (maximized)
            {
                Restore();
            }
            else
            {
                Maximize();
            }
        }

        private void Cb_Clicked(object? sender, EventArgs e)
        {
            WindowCloseAction action = OnWindowClosing(defaultCloseAction);
            switch (action)
            {
                case WindowCloseAction.None:
                    WindowClosed?.Invoke(this, EventArgs.Empty);
                    break;
                case WindowCloseAction.Hide:
                    Visible = false;
                    WindowClosed?.Invoke(this, EventArgs.Empty);
                    break;
                case WindowCloseAction.Remove:
                    Visible = false;
                    Parent?.Remove(this);
                    WindowClosed?.Invoke(this, EventArgs.Empty);
                    break;
                case WindowCloseAction.Dispose:
                    Visible = false;
                    Parent?.Remove(this);
                    WindowClosed?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        protected virtual WindowCloseAction OnWindowClosing(WindowCloseAction defaultAction)
        {
            if (WindowClosing != null)
            {
                WindowClosingEventArgs args = new WindowClosingEventArgs(this, defaultAction);
                WindowClosing(this, args);
                defaultAction = args.WindowCloseAction;
            }
            return defaultAction;
        }

        public override HitTestResult GetHitTestResult(int x, int y)
        {
            Rectangle bounds = GetBounds();
            if (bounds.Contains(x, y))
            {
                if (y >= bounds.Bottom - Padding.Bottom && x <= bounds.Left + Padding.Left)
                {
                    return HitTestResult.SizeBottomLeft;
                }
                else if (y >= bounds.Bottom - Padding.Bottom && x >= bounds.Right - Padding.Right)
                {
                    return HitTestResult.SizeBottomRight;
                }
                else if (x <= bounds.Left + Padding.Left && y <= bounds.Top + Padding.Bottom)
                {
                    return HitTestResult.SizeTopLeft;
                }
                else if (x >= bounds.Right - Padding.Right && y <= bounds.Top + Padding.Bottom)
                {
                    return HitTestResult.SizeTopRight;
                }
                else if (y >= bounds.Bottom - Padding.Bottom)
                {
                    return HitTestResult.SizeBottom;
                }
                else if (y <= bounds.Top + Padding.Bottom)
                {
                    return HitTestResult.SizeTop;
                }
                else if (x <= bounds.Left + Padding.Left)
                {
                    return HitTestResult.SizeLeft;
                }
                else if (x >= bounds.Right - Padding.Right)
                {
                    return HitTestResult.SizeRight;
                }
                else if (y > bounds.Top + 2 && y < bounds.Top + Padding.Top)
                {
                    return HitTestResult.DragArea;
                }
                return HitTestResult.Control;
            }
            return HitTestResult.None;
        }



        protected override void DrawControl(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, ref Rectangle bounds)
        {
            Theme.DrawWindow(gui, renderer, gameTime, this, ref bounds);
            //base.DrawControl(gui, renderer, gameTime, ref bounds);
            //renderer.DrawWindowBorder(bounds, BorderColor, BorderShineColor, BorderShadowColor, Padding);
            //Rectangle titleRect = bounds;
            //titleRect.Height = Padding.Top;
            //titleRect.Inflate(-1, -1);
            //titleRect.X += 32;
            //titleRect.Width -= 32;
            //renderer.DrawText(Font, Title, titleRect, TextColor, HorizontalAlignment.Left);
        }

        public override string ToString()
        {
            return $"{GetType().Name} #{windowId}";
        }
    }
}
