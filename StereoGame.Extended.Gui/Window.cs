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
        private WindowCloseAction defaultCloseAction;
        private readonly int windowId;
        public Window()
            : this(null)

        {

        }
        public Window(Screen? screen)
            : base(screen)
        {
            windowId = ++nextWindowId;
            defaultCloseAction = WindowCloseAction.None;
            superBitmap = true;
            layout = new BoxLayout(Orientation.Vertical, Alignment.Fill, 10, 10);
            closeButton = MakeCloseButton();
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

        private SysButton MakeCloseButton()
        {
            SysButton cb = new SysButton(this, Icons.CROSS);
            cb.ExcludeFromLayout = true;
            cb.X = 2;
            cb.Y = 2;
            cb.FixedWidth = 28;
            cb.FixedHeight = 28;
            cb.Width = 28;
            cb.Height = 28;
            cb.Clicked += Cb_Clicked;
            return cb;
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

        public override Size GetContentSize(IGuiSystem context)
        {
            return new Size(Width, Height);
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

        public override void Draw(IGuiSystem gui, IGuiRenderer renderer, GameTime gameTime, int offsetX = 0, int offsetY = 0)
        {
            base.Draw(gui, renderer, gameTime, offsetX, offsetY);
            if (!string.IsNullOrEmpty(Title))
            {
                Rectangle bounds = GetBounds();
                bounds.Offset(offsetX, offsetY);
                renderer.DrawWindowBorder(bounds, BorderColor, BorderShineColor, BorderShadowColor, Padding);
                bounds.Height = Padding.Top;
                bounds.Inflate(-1, -1);
                Rectangle textRect = bounds;
                textRect.X += 32;
                textRect.Width -= 32;
                //renderer.FillRectangle(bounds, BorderColor);
                renderer.DrawText(Font, Title, textRect, TextColor, Framework.Graphics.HorizontalAlignment.Left);
                //renderer.DrawHorizontalLine(bounds.Left, bounds.Right - 1, bounds.Bottom, BorderShadowColor);
            }
        }

        public override string ToString()
        {
            return $"{GetType().Name} #{windowId}";
        }
    }
}
