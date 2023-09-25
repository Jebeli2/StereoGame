namespace StereoGame.Framework.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MouseListener
    {
        private TimeSpan currentTime;
        private MouseState currentState;
        private MouseState previousState;
        private MouseEventArgs? mouseDownArgs;
        private MouseEventArgs? previousClickArgs;
        private int doubleClickMilliseconds = 500;
        private int dragThreshold = 2;
        private bool hasDoubleClicked;

        public event EventHandler<MouseEventArgs>? MouseDown;
        public event EventHandler<MouseEventArgs>? MouseUp;
        public event EventHandler<MouseEventArgs>? MouseClicked;
        public event EventHandler<MouseEventArgs>? MouseDoubleClicked;
        public event EventHandler<MouseEventArgs>? MouseMoved;
        public event EventHandler<MouseEventArgs>? MouseWheelMoved;
        public event EventHandler<MouseEventArgs>? MouseDragStart;
        public event EventHandler<MouseEventArgs>? MouseDrag;
        public event EventHandler<MouseEventArgs>? MouseDragEnd;
        public bool HasMouseMoved => (previousState.X != currentState.X) || (previousState.Y != currentState.Y);

        public void Update(GameTime gameTime)
        {
            currentState = Mouse.GetState();
            currentTime = gameTime.TotalGameTime;
            CheckButtonPressed(s => s.LeftButton, MouseButton.Left);
            CheckButtonPressed(s => s.MiddleButton, MouseButton.Middle);
            CheckButtonPressed(s => s.RightButton, MouseButton.Right);
            CheckButtonPressed(s => s.XButton1, MouseButton.XButton1);
            CheckButtonPressed(s => s.XButton2, MouseButton.XButton2);
            CheckButtonReleased(s => s.LeftButton, MouseButton.Left);
            CheckButtonReleased(s => s.MiddleButton, MouseButton.Middle);
            CheckButtonReleased(s => s.RightButton, MouseButton.Right);
            CheckButtonReleased(s => s.XButton1, MouseButton.XButton1);
            CheckButtonReleased(s => s.XButton2, MouseButton.XButton2);

            if (HasMouseMoved)
            {
                MouseMoved?.Invoke(this, new MouseEventArgs(currentState, previousState, MouseButton.None, currentTime));
            }

            previousState = currentState;
        }

        private void CheckButtonPressed(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if ((getButtonState(currentState) == ButtonState.Pressed) &&
                (getButtonState(previousState) == ButtonState.Released))
            {
                var args = new MouseEventArgs(currentState, previousState, button, currentTime);

                MouseDown?.Invoke(this, args);
                mouseDownArgs = args;
                if (previousClickArgs != null)
                {
                    var clickMilliseconds = (currentTime - previousClickArgs.Time).TotalMilliseconds;
                    if (clickMilliseconds <= doubleClickMilliseconds)
                    {
                        MouseDoubleClicked?.Invoke(this, args);
                        hasDoubleClicked = true;
                    }
                    previousClickArgs = null;
                }
            }
        }

        private void CheckButtonReleased(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if ((getButtonState(currentState) == ButtonState.Released) &&
                (getButtonState(previousState) == ButtonState.Pressed))
            {
                var args = new MouseEventArgs(currentState, previousState, button, currentTime);
                if (mouseDownArgs != null && mouseDownArgs.Button == args.Button)
                {
                    var clickMovement = Distance(args.X, args.Y, mouseDownArgs.X, mouseDownArgs.Y);
                    if (clickMovement < dragThreshold)
                    {
                        if (!hasDoubleClicked)
                        {
                            MouseClicked?.Invoke(this, args);
                        }
                        else
                        {

                        }
                    }
                }
                MouseUp?.Invoke(this, args);
                hasDoubleClicked = false;
                previousClickArgs = args;
            }
        }

        private static int Distance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
    }
}
