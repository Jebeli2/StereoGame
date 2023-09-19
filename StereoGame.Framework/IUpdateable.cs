namespace StereoGame.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IUpdateable
    {
        void Update(GameTime gameTime);


        /// <summary>
        /// Raised when <see cref="Enabled"/> changed.
        /// </summary>
        event EventHandler<EventArgs>? EnabledChanged;

        /// <summary>
        /// Raised when <see cref="UpdateOrder"/> changed.
        /// </summary>
        event EventHandler<EventArgs>? UpdateOrderChanged;

        /// <summary>
        /// Indicates if <see cref="Update"/> will be called.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// The update order of this <see cref="GameComponent"/> relative
        /// to other <see cref="GameComponent"/> instances.
        /// </summary>
        int UpdateOrder { get; }
    }
}
