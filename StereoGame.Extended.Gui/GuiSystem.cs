namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using StereoGame.Framework.Input;
    using System;
    using System.Drawing;

    public class GuiSystem : DrawableGameComponent, IGuiSystem
    {
        private Screen? activeScreen;
        private IGuiRenderer renderer;
        private int screenWidth;
        private int screenHeight;
        private readonly MouseListener mouseListener;
        private Window? activeWindow;
        private Control? preFocusedControl;
        private Control? focusedControl;
        private Control? hoveredControl;
        private Control? preDragControl;
        private Control? preSizeControl;
        private HitTestResult dragSizeHitTest;
        private Rectangle sizeStartRect;
        private int sizingMod = 3;
        private int sizingCounter;
        private int dragSizeStartX;
        private int dragSizeStartY;
        private bool moveWindowToFrontOnActivate = true;
        private readonly Queue<Window> activationWindows = new();
        private TimeSpan lastTime;
        private static readonly TimeSpan tickDuration = TimeSpan.FromSeconds(1.0 / 60);
        public GuiSystem(Game game) : base(game)
        {
            DrawOrder = 1000;
            renderer = new DefaultGuiRenderer(game);
            mouseListener = new MouseListener();
            mouseListener.MouseMoved += MouseListener_MouseMoved;
            mouseListener.MouseDown += MouseListener_MouseDown;
            mouseListener.MouseUp += MouseListener_MouseUp;
        }

        public int ScreenWidth => screenWidth;
        public int ScreenHeight => screenHeight;

        public Screen? ActiveScreen
        {
            get { return activeScreen; }
            set
            {
                if (activeScreen != value)
                {
                    activationWindows.Clear();
                    activeWindow = null;
                    activeScreen = value;
                    if (activeScreen != null) { InitScreen(activeScreen); }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (activeScreen != null && activeScreen.Visible)
            {
                CheckScreenSize();
                mouseListener.Update(gameTime);
                CheckWindowActivationQueue();
                CheckTimer(mouseListener, gameTime);
                UpdateControl(activeScreen, gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (activeScreen != null && activeScreen.Visible)
            {
                DrawControl(activeScreen, gameTime);
            }
        }

        public void ActivateScreenAndWindow(Screen? screen, Window? window)
        {
            ActiveScreen = screen;
            ActivateWindow(window);
        }

        public void ActivateWindow(Window? window)
        {
            if (window != null) { activationWindows.Enqueue(window); }
        }

        public void SetFocus(Control? control)
        {
            if (focusedControl != control)
            {
                if (focusedControl != null)
                {
                    focusedControl.Focused = false;
                }
                focusedControl = control;
                if (focusedControl != null)
                {
                    focusedControl.Focused = true;
                }
            }
        }

        private void UpdateControl(Control control, GameTime gameTime)
        {
            if (control.Visible)
            {
                control.Update(this, gameTime);
                foreach (Control child in control.Children) { UpdateControl(child, gameTime); }
            }
        }

        private void DrawControl(Control control, GameTime gameTime, int offsetX = 0, int offsetY = 0)
        {
            if (control.Visible)
            {
                Rectangle bounds = control.GetBounds();
                if (control.SuperBitmap)
                {
                    if (control.NeedsNewBitmap)
                    {
                        control.PushBitmap(this);
                        offsetX -= bounds.Left;
                        offsetY -= bounds.Top;
                        control.Draw(this, renderer, gameTime, offsetX, offsetY);
                        foreach (Control child in control.Children) { DrawControl(child, gameTime, offsetX, offsetY); }
                        control.PopBitmap(this);
                    }
                    GraphicsDevice.BlendMode = Framework.Graphics.BlendMode.Blend;
                    GraphicsDevice.DrawTexture(control.Bitmap, 0, 0, bounds.Width, bounds.Height, bounds.X, bounds.Y);
                }
                else
                {
                    control.Draw(this, renderer, gameTime, offsetX, offsetY);
                    foreach (Control child in control.Children) { DrawControl(child, gameTime, offsetX, offsetY); }
                }
            }
        }

        private void CheckScreenSize()
        {
            if (activeScreen != null && activeScreen.Visible && (screenWidth != Width || screenHeight != Height))
            {
                InitScreen(activeScreen);
            }
        }

        private void CheckTimer(MouseListener mouseListener, GameTime gameTime)
        {
            TimeSpan timeDiff = gameTime.Since(lastTime);
            if (timeDiff >= tickDuration)
            {
                lastTime = gameTime.TotalGameTime;
                MouseTimerTick(mouseListener.LastMouseMoveEventArgs);
            }
        }

        private void InitScreen(Screen screen)
        {
            screenWidth = Width;
            screenHeight = Height;
            screen.SetFixedBounds(0, 0, screenWidth, screenHeight);
            screen.InvalidateWindows();
        }

        private Control? FindControlAt(int x, int y)
        {
            if (activeScreen == null || !activeScreen.Visible) return null;
            return FindControlAt(activeScreen, x, y);
        }
        private Control? FindControlAt(Control control, int x, int y)
        {
            foreach (var child in control.Children.Reverse())
            {
                var c = FindControlAt(child, x, y);
                if (c != null) return c;
            }
            if (control.Visible && control.Contains(x, y)) return control;
            return null;
        }

        private bool CheckWindowFocus(PointerEventArgs pea)
        {
            Window? window = preFocusedControl?.Window;
            if (window != null)
            {
                SetActiveWindow(window);
            }
            return false;
        }

        private bool CheckSizeMouse(Control? hc, PointerEventArgs pea)
        {
            if (hc != null && preSizeControl == null)
            {
                HitTestResult hitTest = hc.GetHitTestResult(pea.X, pea.Y);
                switch (hitTest)
                {
                    case HitTestResult.SizeBottom:
                    case HitTestResult.SizeTop:
                        Mouse.SetSystemCursor(SystemCursor.SizeNS);
                        return true;
                    case HitTestResult.SizeLeft:
                    case HitTestResult.SizeRight:
                        Mouse.SetSystemCursor(SystemCursor.SizeWE);
                        return true;
                    case HitTestResult.SizeBottomLeft:
                    case HitTestResult.SizeTopRight:
                        Mouse.SetSystemCursor(SystemCursor.SizeNESW);
                        return true;
                    case HitTestResult.SizeBottomRight:
                    case HitTestResult.SizeTopLeft:
                        Mouse.SetSystemCursor(SystemCursor.SizeNWSE);
                        return true;
                }
            }
            return false;
        }
        private bool CheckDragOrSizeStart(PointerEventArgs pea)
        {
            if (preFocusedControl != null && pea.Button == MouseButton.Left)
            {
                HitTestResult hitTest = preFocusedControl.GetHitTestResult(pea.X, pea.Y);
                if (hitTest == HitTestResult.DragArea)
                {
                    Mouse.ClearSystemCursor();
                    preDragControl = preFocusedControl;
                    dragSizeStartX = pea.X;
                    dragSizeStartY = pea.Y;
                    return true;
                }
                else if (hitTest == HitTestResult.SizeBottom ||
                        hitTest == HitTestResult.SizeTop ||
                        hitTest == HitTestResult.SizeLeft ||
                        hitTest == HitTestResult.SizeRight ||
                        hitTest == HitTestResult.SizeBottomRight ||
                        hitTest == HitTestResult.SizeBottomLeft ||
                        hitTest == HitTestResult.SizeTopRight ||
                        hitTest == HitTestResult.SizeTopLeft)
                {
                    preSizeControl = preFocusedControl;
                    dragSizeHitTest = hitTest;
                    sizingCounter = 0;
                    sizeStartRect = preSizeControl.BoundingRectangle;
                    dragSizeStartX = pea.X;
                    dragSizeStartY = pea.Y;
                    return true;
                }
            }
            return false;
        }

        private bool CheckDragging(PointerEventArgs pea)
        {
            if (preDragControl != null && (pea.X != dragSizeStartX || pea.Y != dragSizeStartY))
            {
                int dX = pea.X - dragSizeStartX;
                int dY = pea.Y - dragSizeStartY;
                if (preDragControl.Move(dX, dY))
                {
                    dragSizeStartX = pea.X;
                    dragSizeStartY = pea.Y;
                    return true;
                }
            }
            return false;
        }

        private bool CheckSizing(PointerEventArgs pea)
        {
            if (preSizeControl != null && (pea.X != dragSizeStartX && pea.Y != dragSizeStartY))
            {
                int dX = pea.X - dragSizeStartX;
                int dY = pea.Y - dragSizeStartY;
                if (preSizeControl.Resize(sizeStartRect, dX, dY, dragSizeHitTest))
                {
                    sizingCounter++;
                    if (sizingCounter > sizingMod)
                    {
                        sizingCounter = 0;
                        preSizeControl.InvalidateLayout();
                        //CheckSizingLayout();
                    }
                    return true;
                }
            }
            return preSizeControl != null;
        }

        private void MouseTimerTick(MouseEventArgs e)
        {
            if (activeScreen == null || !activeScreen.Visible) return;
            Control? c = preFocusedControl ?? focusedControl;
            if (c != null)
            {
                var pea = PointerEventArgs.FromMouseEventArgs(e);
                PropagateDown(c, x => x.OnPointerTimerTick(pea));
            }
        }


        private void MouseListener_MouseDown(object? sender, MouseEventArgs e)
        {
            if (activeScreen == null || !activeScreen.Visible) return;
            preFocusedControl = FindControlAt(e.X, e.Y);
            var pea = PointerEventArgs.FromMouseEventArgs(e);
            CheckWindowFocus(pea);
            if (!CheckDragOrSizeStart(pea))
            {
                PropagateDown(hoveredControl, x => x.OnPointerDown(pea));
            }
        }
        private void MouseListener_MouseUp(object? sender, MouseEventArgs e)
        {
            if (activeScreen == null || !activeScreen.Visible) return;
            var postFocusedControl = FindControlAt(e.X, e.Y);
            var pea = PointerEventArgs.FromMouseEventArgs(e);
            //if (CheckEndSizing(pea))
            //{

            Mouse.ClearSystemCursor();
            //}
            if (preFocusedControl == postFocusedControl)
            {
                SetFocus(postFocusedControl);
            }
            Control? prevPreFocusedControl = preFocusedControl;
            preFocusedControl = null;
            preDragControl = null;
            preSizeControl = null;
            PropagateDown(hoveredControl, x => x.OnPointerUp(pea));
            if (prevPreFocusedControl != null && prevPreFocusedControl != hoveredControl)
            {
                PropagateDown(prevPreFocusedControl, x => x.OnPointerUp(pea));
            }
        }

        private void MouseListener_MouseMoved(object? sender, MouseEventArgs e)
        {
            if (activeScreen == null || !activeScreen.Visible) return;
            var hc = FindControlAt(e.X, e.Y);
            var pea = PointerEventArgs.FromMouseEventArgs(e);
            if (!CheckDragging(pea) && !CheckSizing(pea))
            {
                if (!CheckSizeMouse(hc, pea)) { Mouse.ClearSystemCursor(); }
                if (hc != hoveredControl)
                {
                    if (hoveredControl != null && (hc == null || !hc.HasParent(hoveredControl)))
                    {
                        PropagateDown(hoveredControl, x => x.OnPointerLeave(pea));
                    }
                    hoveredControl = hc;
                    PropagateDown(hoveredControl, x => x.OnPointerEnter(pea));
                }
                else
                {
                    PropagateDown(hoveredControl, x => x.OnPointerMove(pea));
                }
                if (preFocusedControl != null && preFocusedControl != hoveredControl)
                {
                    PropagateDown(preFocusedControl, x => x.OnPointerMove(pea));
                }
            }
        }


        private static void PropagateDown(Control? control, Func<Control, bool> predicate)
        {
            while (control != null && predicate(control))
            {
                control = control.Parent;
            }
        }

        private void CheckWindowActivationQueue()
        {
            if (activationWindows.Count > 0)
            {
                Window win = activationWindows.Dequeue();
                SetActiveWindow(win);
            }
        }
        private void SetActiveWindow(Window? window)
        {
            if (activeWindow != window)
            {
                if (activeWindow != null) activeWindow.Active = false;
                activeWindow = window;
                if (activeWindow != null)
                {
                    activeWindow.Active = true;
                    if (moveWindowToFrontOnActivate) { activeScreen?.WindowToFront(activeWindow); }
                }
            }
        }
    }
}