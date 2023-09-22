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
        private readonly Dictionary<string, object> loadedAssets = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        private bool disposed;
        public ContentManager(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) { throw new ArgumentNullException(nameof(serviceProvider)); }
            this.serviceProvider = serviceProvider;
        }

        ~ContentManager()
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
            if (!disposed)
            {
                if (disposing)
                {
                    //Unload();
                }

                disposed = true;
            }
        }

        public virtual T? Load<T>(string assetName)
        {
            var key = assetName.Replace('\\', '/');
            if (loadedAssets.TryGetValue(key, out object? asset))
            {
                if (asset is T result)
                {
                    return result;
                }
            }
            T? newResult = ReadAsset<T>(assetName);
            if (newResult != null)
            {
                loadedAssets[key] = newResult;
                return newResult;
            }
            return default;
        }

        protected T? ReadAsset<T>(string assetName)
        {
            return default;
        }
    }
}
