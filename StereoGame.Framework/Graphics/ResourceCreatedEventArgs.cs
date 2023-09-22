namespace StereoGame.Framework.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class ResourceCreatedEventArgs : EventArgs
    {
        private readonly object resource;
        public ResourceCreatedEventArgs(object resource)
        {
            this.resource = resource;
        }

        public object Resource => resource;
    }
}
