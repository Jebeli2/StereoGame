namespace StereoGame.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal static class EventHelpers
    {
        internal static void Raise<TEventArgs>(object? sender, EventHandler<TEventArgs>? handler, TEventArgs e)
        {
            handler?.Invoke(sender, e);
        }

        internal static void Raise(object? sender, EventHandler? handler, EventArgs e)
        {
            handler?.Invoke(sender, e);
        }
    }
}
