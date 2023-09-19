namespace StereoGame.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GameComponentCollectionEventArgs : EventArgs
    {
        private readonly IGameComponent gameComponent;

        public GameComponentCollectionEventArgs(IGameComponent gameComponent)
        {
            this.gameComponent = gameComponent;
        }

        public IGameComponent GameComponent => gameComponent;
    }
}
