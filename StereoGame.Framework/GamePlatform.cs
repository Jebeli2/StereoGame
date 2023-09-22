namespace StereoGame.Framework
{
    using StereoGame.Framework.Graphics;
    using StereoGame.Framework.Input;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract partial class GamePlatform : IDisposable
    {
        private bool isDisposed;
        private bool isActive;
        private readonly Game game;
        private GameWindow window;

        public GamePlatform(Game game)
        {
            this.game = game;
            window = null!;
        }

        ~GamePlatform()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
            }
        }

        protected bool IsDisposed => isDisposed;
        public Game Game => game;
        public GameWindow Window
        {
            get { return window; }
            protected set
            {
                if (window == null)
                {
                    Mouse.PrimaryWindow = value;
                }
                window = value;
            }
        }

        public abstract GraphicsDevice CreateGraphicsDevice(PresentationParameters pp);

        public abstract GameRunBehavior DefaultRunBehavior { get; }
        public bool IsActive
        {
            get { return isActive; }
            internal set
            {
                if (isActive != value)
                {
                    isActive = value;
                    if (isActive) { Activated?.Invoke(this, EventArgs.Empty); } else { Deactivated?.Invoke(this, EventArgs.Empty); }
                }
            }
        }

        public event EventHandler<EventArgs>? AsyncRunLoopEnded;
        public event EventHandler<EventArgs>? Activated;
        public event EventHandler<EventArgs>? Deactivated;

        protected void RaiseAsyncRunLoopEnded()
        {
            AsyncRunLoopEnded?.Invoke(this, EventArgs.Empty);
        }
        public virtual void BeforeInitialize()
        {
            IsActive = true;
        }
        public virtual bool BeforeRun()
        {
            return true;
        }
        public abstract void Exit();

        public abstract void RunLoop();

        public abstract void StartRunLoop();
        public abstract bool BeforeUpdate(GameTime gameTime);
        public abstract bool BeforeDraw(GameTime gameTime);
        public virtual TimeSpan TargetElapsedTimeChanging(TimeSpan value)
        {
            return value;
        }

        public virtual void TargetElapsedTimeChanged() { }
        public virtual void Present() { }

        public abstract void BeginScreenDeviceChange(bool willBeFullScreen);
        public abstract void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight);

        public abstract TextFont? LoadFont(string path, int ySize);

        [Conditional("DEBUG")]
        public virtual void Log(string message) { }
    }
}
