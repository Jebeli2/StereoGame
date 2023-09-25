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
            }
        }

        private void CheckButtonReleased(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if ((getButtonState(currentState) == ButtonState.Released) &&
                (getButtonState(previousState) == ButtonState.Pressed))
            {
                var args = new MouseEventArgs(currentState, previousState, button, currentTime);

                MouseUp?.Invoke(this, args);
            }
        }

    }
}
