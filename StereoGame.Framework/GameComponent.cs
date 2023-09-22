namespace StereoGame.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GameComponent : IGameComponent, IUpdateable, IDisposable
    {
        private bool enabled = true;
        private int updateOrder;
        private readonly Game game;

        public GameComponent(Game game)
        {
            this.game = game;
        }

        ~GameComponent()
        {
            Dispose(false);
        }
        public Game Game => game;

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    OnEnabledChanged(this, EventArgs.Empty);
                }
            }
        }

        public int UpdateOrder
        {
            get { return updateOrder; }
            set
            {
                if (updateOrder != value)
                {
                    updateOrder = value;
                    OnUpdateOrderChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler<EventArgs>? EnabledChanged;

        public event EventHandler<EventArgs>? UpdateOrderChanged;

        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }

        protected virtual void OnUpdateOrderChanged(object sender, EventArgs args)
        {
            EventHelpers.Raise(sender, UpdateOrderChanged, args);
        }
        protected virtual void OnEnabledChanged(object sender, EventArgs args)
        {
            EventHelpers.Raise(sender, EnabledChanged, args);
        }
        protected virtual void Dispose(bool disposing) { }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
