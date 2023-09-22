namespace StereoGame.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DrawableGameComponent : GameComponent, IDrawable
    {
        private bool initialized;
        private bool disposed;
        private int drawOrder;
        private bool visible = true;
        public DrawableGameComponent(Game game)
          : base(game)
        {
        }

        public Graphics.GraphicsDevice GraphicsDevice
        {
            get { return Game.GraphicsDevice; }
        }

        public int DrawOrder
        {
            get { return drawOrder; }
            set
            {
                if (drawOrder != value)
                {
                    drawOrder = value;
                    OnDrawOrderChanged(this, EventArgs.Empty);
                }
            }
        }

        public bool Visible
        {
            get { return visible; }
            set
            {
                if (visible != value)
                {
                    visible = value;
                    OnVisibleChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler<EventArgs>? DrawOrderChanged;

        public event EventHandler<EventArgs>? VisibleChanged;

        public override void Initialize()
        {
            if (!initialized)
            {
                initialized = true;
                LoadContent();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                UnloadContent();
            }
        }

        protected virtual void LoadContent() { }

        protected virtual void UnloadContent() { }

        public virtual void Draw(GameTime gameTime) { }

        protected virtual void OnVisibleChanged(object sender, EventArgs args)
        {
            EventHelpers.Raise(sender, VisibleChanged, args);
        }
        protected virtual void OnDrawOrderChanged(object sender, EventArgs args)
        {
            EventHelpers.Raise(sender, DrawOrderChanged, args);
        }
    }
}
