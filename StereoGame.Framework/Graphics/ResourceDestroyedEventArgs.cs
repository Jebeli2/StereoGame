namespace StereoGame.Framework.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class ResourceDestroyedEventArgs : EventArgs
    {
        private readonly object resource;
        private readonly string name;

        public ResourceDestroyedEventArgs(object resource, string name)
        {
            this.resource = resource;
            this.name = name;
        }

        public string Name => name;
        public object Resource => resource;
    }
}
