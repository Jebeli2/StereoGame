namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using StereoGame.Framework.Input;
    using System;
    using System.Drawing;
    using System.Security.Cryptography;

    public class GuiSystem : DrawableGameComponent, IGuiSystem
    {
        private Screen? activeScreen;
        private IGuiRenderer renderer;
        private int screenWidth;
        private int screenHeight;
        private readonly MouseListener mouseListener;
        private Control? preFocusedControl;
        private Control? focusedControl;
        private Control? hoveredControl;
        private Control? preDragControl;
        private int dragStartX;
        private int dragStartY;
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
                control.ForEachChild(UpdateControl, gameTime);
                //foreach (Control child in control.Children)
                //{
                //    UpdateControl(child, gameTime);
                //}
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
                        control.ForEachChild(DrawControl, gameTime, offsetX, offsetY);
                        //foreach (Control child in control.Children)
                        //{
                        //    DrawControl(child, gameTime, offsetX, offsetY);
                        //}
                        control.PopBitmap(this);
                    }
                    GraphicsDevice.BlendMode = Framework.Graphics.BlendMode.Blend;
                    GraphicsDevice.DrawTexture(control.Bitmap, 0, 0, bounds.Width, bounds.Height, bounds.X, bounds.Y);
                }
                else
                {
                    control.Draw(this, renderer, gameTime, offsetX, offsetY);
                    control.ForEachChild(DrawControl, gameTime, offsetX, offsetY);
                    //foreach (Control child in control.Children)
                    //{
                    //    DrawControl(child, gameTime, offsetX, offsetY);
                    //}
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

        private void InitScreen(Screen screen)
        {
            screenWidth = Width;
            screenHeight = Height;
            screen.X = 0;
            screen.Y = 0;
            screen.FixedWidth = screenWidth;
            screen.FixedHeight = screenHeight;
            screen.Width = screenWidth;
            screen.Height = screenHeight;
            screen.PerformLayout(this);
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
            if (preFocusedControl is Window window)
            {
                activeScreen?.WindowToFront(window);
            }
            return false;
        }

        private bool CheckDragStart(PointerEventArgs pea)
        {
            if (preFocusedControl != null && pea.Button == MouseButton.Left)
            {
                HitTestResult hitTest = preFocusedControl.GetHitTestResult(pea.X, pea.Y);
                if (hitTest == HitTestResult.DragArea)
                {
                    preDragControl = preFocusedControl;
                    dragStartX = pea.X;
                    dragStartY = pea.Y;
                    return true;
                }
            }
            return false;
        }

        private bool CheckDragging(PointerEventArgs pea)
        {
            if (preDragControl != null && (pea.X != dragStartX || pea.Y != dragStartY))
            {
                int dX = pea.X - dragStartX;
                int dY = pea.Y - dragStartY;
                if (preDragControl.Move(dX, dY))
                {
                    dragStartX = pea.X;
                    dragStartY = pea.Y;
                    return true;
                }
            }
            return false;
        }

        private void MouseListener_MouseDown(object? sender, MouseEventArgs e)
        {
            if (activeScreen == null || !activeScreen.Visible) return;
            preFocusedControl = FindControlAt(e.X, e.Y);
            var pea = PointerEventArgs.FromMouseEventArgs(e);
            CheckWindowFocus(pea);
            if (!CheckDragStart(pea))
            {
                PropagateDown(hoveredControl, x => x.OnPointerDown(pea));
            }
        }
        private void MouseListener_MouseUp(object? sender, MouseEventArgs e)
        {
            if (activeScreen == null || !activeScreen.Visible) return;
            var postFocusedControl = FindControlAt(e.X, e.Y);
            if (preFocusedControl == postFocusedControl)
            {
                SetFocus(postFocusedControl);
            }
            preFocusedControl = null;
            preDragControl = null;
            var pea = PointerEventArgs.FromMouseEventArgs(e);
            PropagateDown(hoveredControl, x => x.OnPointerUp(pea));
        }

        private void MouseListener_MouseMoved(object? sender, MouseEventArgs e)
        {
            if (activeScreen == null || !activeScreen.Visible) return;
            var hc = FindControlAt(e.X, e.Y);
            var pea = PointerEventArgs.FromMouseEventArgs(e);
            if (!CheckDragging(pea))
            {
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
            }
        }


        private static void PropagateDown(Control? control, Func<Control, bool> predicate)
        {
            while (control != null && predicate(control))
            {
                control = control.Parent;
            }
        }


    }
}