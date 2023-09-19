﻿namespace StereoGame.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract partial class GamePlatform : IDisposable
    {
        private bool isDisposed;
        private bool isActive;
        private readonly Game game;

        public GamePlatform(Game game)
        {
            this.game = game;
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
        public abstract GameRunBehavior DefaultRunBehavior { get; }
        public bool IsActive
        {
            get { return isActive; }
            internal set
            {
                if (isActive != value)
                {
                    isActive = value;
                    EventHelpers.Raise(this, isActive ? Activated : Deactivated, EventArgs.Empty);
                }
            }
        }

        public event EventHandler<EventArgs>? AsyncRunLoopEnded;
        public event EventHandler<EventArgs>? Activated;
        public event EventHandler<EventArgs>? Deactivated;

        protected void RaiseAsyncRunLoopEnded()
        {
            EventHelpers.Raise(this, AsyncRunLoopEnded, EventArgs.Empty);
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
    }
}