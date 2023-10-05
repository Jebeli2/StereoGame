namespace StereoGame.Framework.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class KeyboardListener
    {
        private Array keysValues = Enum.GetValues(typeof(Keys));
        private bool isInitial;
        private TimeSpan lastPressTime;
        private bool repeatPress;
        private int initialDelay;
        private int repeatDelay;

        private Keys previousKey;
        private KeyboardState previousState;

        public KeyboardListener()
        {
            repeatPress = true;
            initialDelay = 800;
            repeatDelay = 50;
            Game.Instance.Window.TextInput += Window_TextInput;
        }

        private void Window_TextInput(object? sender, TextInputEventArgs e)
        {
            KeyTyped?.Invoke(this, new KeyboardEventArgs(e));
        }

        public event EventHandler<KeyboardEventArgs>? KeyTyped;
        public event EventHandler<KeyboardEventArgs>? KeyPressed;
        public event EventHandler<KeyboardEventArgs>? KeyReleased;

        public void Update(GameTime gameTime)
        {
            var currentState = Keyboard.GetState();

            RaisePressedEvents(gameTime, currentState);
            RaiseReleasedEvents(currentState);

            if (repeatPress) RaiseRepeatEvents(gameTime, currentState);

            previousState = currentState;
        }

        private void RaisePressedEvents(GameTime gameTime, KeyboardState currentState)
        {
            if (!currentState.IsKeyDown(Keys.LeftAlt) && !currentState.IsKeyDown(Keys.RightAlt))
            {
                var pressedKeys = keysValues
                    .Cast<Keys>()
                    .Where(key => currentState.IsKeyDown(key) && previousState.IsKeyUp(key));

                foreach (var key in pressedKeys)
                {
                    var args = new KeyboardEventArgs(key, currentState);

                    KeyPressed?.Invoke(this, args);

                    //if (args.Character.HasValue)
                    //    KeyTyped?.Invoke(this, args);

                    previousKey = key;
                    lastPressTime = gameTime.TotalGameTime;
                    isInitial = true;
                }
            }
        }

        private void RaiseReleasedEvents(KeyboardState currentState)
        {
            var releasedKeys = keysValues
                .Cast<Keys>()
                .Where(key => currentState.IsKeyUp(key) && previousState.IsKeyDown(key));

            foreach (var key in releasedKeys)
                KeyReleased?.Invoke(this, new KeyboardEventArgs(key, currentState));
        }

        private void RaiseRepeatEvents(GameTime gameTime, KeyboardState currentState)
        {
            var elapsedTime = (gameTime.TotalGameTime - lastPressTime).TotalMilliseconds;

            if (currentState.IsKeyDown(previousKey) &&
                (isInitial && elapsedTime > initialDelay || !isInitial && elapsedTime > repeatDelay))
            {
                var args = new KeyboardEventArgs(previousKey, currentState);

                KeyPressed?.Invoke(this, args);

                //if (args.Character.HasValue)
                //    KeyTyped?.Invoke(this, args);

                lastPressTime = gameTime.TotalGameTime;
                isInitial = false;
            }
        }


    }
}
