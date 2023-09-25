namespace StereoGame.Extended.Gui
{
    using StereoGame.Framework;
    using StereoGame.Framework.Input;

    public class GuiSystem : DrawableGameComponent, IGuiSystem
    {
        private Screen? activeScreen;
        private IGuiRenderer renderer;
        private int screenWidth;
        private int screenHeight;
        private MouseListener mouseListener;
        private Control? preFocusedControl;
        private Control? focusedControl;
        private Control? hoveredControl;
        public GuiSystem(Game game) : base(game)
        {
            renderer = new DefaultGuiRenderer(game);
            mouseListener = new MouseListener();
            mouseListener.MouseMoved += MouseListener_MouseMoved;
            mouseListener.MouseDown += MouseListener_MouseDown;
            mouseListener.MouseUp += MouseListener_MouseUp;
        }


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
                foreach (Control child in control.Children)
                {
                    UpdateControl(child, gameTime);
                }
            }
        }

        private void DrawControl(Control control, GameTime gameTime)
        {
            if (control.Visible)
            {
                control.Draw(this, renderer, gameTime);
                foreach (Control child in control.Children)
                {
                    DrawControl(child, gameTime);
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
            screen.Width = screenWidth;
            screen.Height = screenHeight;
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

        private void MouseListener_MouseDown(object? sender, MouseEventArgs e)
        {
            if (activeScreen == null || !activeScreen.Visible) return;
            preFocusedControl = FindControlAt(e.X, e.Y);
            var pea = PointerEventArgs.FromMouseEventArgs(e);
            PropagateDown(hoveredControl, x => x.OnPointerDown(pea));
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
            var pea = PointerEventArgs.FromMouseEventArgs(e);
            PropagateDown(hoveredControl,x => x.OnPointerUp(pea));
        }

        private void MouseListener_MouseMoved(object? sender, MouseEventArgs e)
        {
            if (activeScreen == null || !activeScreen.Visible) return;
            var hc = FindControlAt(e.X, e.Y);
            var pea = PointerEventArgs.FromMouseEventArgs(e);
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
                PropagateDown(hoveredControl,x => x.OnPointerMove(pea));
            }
        }

        //private void SetHovered(Control? control)
        //{
        //    if (hoveredControl != control)
        //    {
        //        if (hoveredControl != null)
        //        {
        //            hoveredControl.Hovered = false;
        //        }
        //        hoveredControl = control;
        //        if (hoveredControl != null)
        //        {
        //            hoveredControl.Hovered = true;
        //        }
        //    }
        //}

        private static void PropagateDown(Control? control, Func<Control, bool> predicate)
        {
            while (control != null && predicate(control))
            {
                control = control.Parent;
            }
        }


    }
}