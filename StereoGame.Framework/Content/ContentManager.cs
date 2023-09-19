namespace StereoGame.Framework.Content
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public partial class ContentManager : IDisposable
    {
        private readonly IServiceProvider serviceProvider;

        private bool disposed;
        public ContentManager(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) { throw new ArgumentNullException(nameof(serviceProvider)); }
            this.serviceProvider = serviceProvider;
            //AddContentManager(this);
        }

        public ContentManager(IServiceProvider serviceProvider, string rootDirectory)
        {
            if (serviceProvider == null) { throw new ArgumentNullException(nameof(serviceProvider)); }
            if (rootDirectory == null) { throw new ArgumentNullException(nameof(rootDirectory)); }
            //this.RootDirectory = rootDirectory;
            this.serviceProvider = serviceProvider;
            //AddContentManager(this);
        }
        ~ContentManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            //RemoveContentManager(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Unload();
                }

                disposed = true;
            }
        }
    }
}
