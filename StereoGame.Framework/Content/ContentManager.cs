namespace StereoGame.Framework.Content
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Resources;
    using System.Text;
    using System.Threading.Tasks;

    public class ContentManager : IDisposable
    {
        private readonly Game game;
        private readonly List<IAssetReader> assetReaders = new();
        private readonly Dictionary<string, object> loadedAssets = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<IDisposable> disposableAssets = new();
        private readonly List<ResourceManager> resourceManagers = new();
        private readonly List<string> knownNames = new();

        private bool disposed;
        public ContentManager(Game game)
        {
            this.game = game;
            RegisterAssetReader(new TextureReader());
            RegisterAssetReader(new TextFontReader());
            RegisterAssetReader(new MusicReader());
            RegisterAssetReader(new SoundReader());
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
                    Unload();
                }
                disposed = true;
            }
        }

        public Game Game => game;

        public void AddResourceManager(ResourceManager resourceManager)
        {
            if (!resourceManagers.Contains(resourceManager))
            {
                resourceManagers.Add(resourceManager);
                knownNames.AddRange(ListResources(resourceManager));
            }
        }
        public void RegisterAssetReader<T>(IAssetReader<T> reader)
        {
            reader.ContentManager = this;
            assetReaders.Add(reader);
        }

        public virtual void UnloadAsset(object asset)
        {
            string? key = FindKey(asset);
            if (key != null) { UnloadAsset(key); }
        }

        public virtual void UnloadAsset(string assetName)
        {
            var key = assetName.Replace('\\', '/');
            if (loadedAssets.TryGetValue(key, out var asset))
            {
                if (asset is IDisposable disposable)
                {
                    disposable.Dispose();
                    disposableAssets.Remove(disposable);
                }
                loadedAssets.Remove(key);
            }
        }

        public virtual void UnloadAssets(IEnumerable<string> assets)
        {
            foreach (var asset in assets) { UnloadAsset(asset); }
        }

        public virtual void UnloadAssets(params string[] assets)
        {
            foreach (var asset in assets) { UnloadAsset(asset); }
        }

        public virtual T? Load<T>(string assetName, object? parameter = null)
        {
            var key = assetName.Replace('\\', '/');
            if (loadedAssets.TryGetValue(key, out object? asset))
            {
                if (asset is T result)
                {
                    return result;
                }
            }
            T? newResult = ReadAsset<T>(assetName, parameter);
            if (newResult != null)
            {
                loadedAssets[key] = newResult;
                RecordDisposable(newResult as IDisposable);
                return newResult;
            }
            return default;
        }

        protected T? ReadAsset<T>(string assetName, object? parameter)
        {
            byte[]? data = FindContent(assetName);
            foreach (var reader in GetAssetReaders<T>())
            {
                T? result = reader.ReadAsset(assetName, data, parameter);
                if (result != null) return result;
            }
            return default;
        }

        public virtual void Unload()
        {
            foreach (var disposable in disposableAssets)
            {
                disposable?.Dispose();
            }
            disposableAssets.Clear();
            loadedAssets.Clear();
        }

        private void RecordDisposable(IDisposable? disposable)
        {
            if (disposable != null)
            {
                if (!disposableAssets.Contains(disposable))
                {
                    disposableAssets.Add(disposable);
                }
            }
        }

        public byte[]? FindContent(string assetName)
        {
            byte[]? data = null;
            if (!string.IsNullOrEmpty(assetName))
            {
                if (data == null) { data = FindInFileSystem(assetName); }
                if (data == null) { data = FindInResManagers(assetName); }
            }
            return data;
        }

        private static byte[]? FindInFileSystem(string name)
        {
            try
            {
                if (File.Exists(name))
                {
                    return File.ReadAllBytes(name);
                }
            }
            catch (IOException ioe)
            {
                //
            }
            return null;
        }

        private byte[]? FindInResManagers(string name)
        {
            name = FindResName(name);
            foreach (ResourceManager rm in resourceManagers)
            {
                object? obj = rm.GetObject(name);
                if (obj != null)
                {
                    if (obj is byte[] data) { return data; }
                    else if (obj is string str)
                    {
                        return Encoding.UTF8.GetBytes(str);
                    }
                    else if (obj is UnmanagedMemoryStream ums1)
                    {
                        byte[] umsData = new byte[ums1.Length];
                        ums1.Read(umsData, 0, umsData.Length);
                        return umsData;
                    }
                }
                UnmanagedMemoryStream? ums = rm.GetStream(name);
                if (ums != null)
                {
                    byte[] umsData = new byte[ums.Length];
                    ums.Read(umsData, 0, umsData.Length);
                    return umsData;
                }
            }
            return null;
        }

        private string FindResName(string name)
        {
            if (knownNames.Contains(name)) return name;
            string testName = name.Replace('_', '-');
            if (knownNames.Contains(testName)) return testName;
            testName = name.Replace('_', ' ').Trim();
            if (knownNames.Contains(testName)) return testName;
            return name;
        }

        private string? FindKey(object asset)
        {
            foreach (var kvp in loadedAssets)
            {
                if (kvp.Value == asset) return kvp.Key;
            }
            return null;
        }

        private IEnumerable<IAssetReader<T>> GetAssetReaders<T>()
        {
            foreach (var reader in assetReaders)
            {
                if (reader is IAssetReader<T> assetReader)
                {
                    yield return assetReader;
                }
            }
        }

        private static IEnumerable<string> ListResources(ResourceManager rm)
        {
            ResourceSet? rs = rm.GetResourceSet(System.Globalization.CultureInfo.InvariantCulture, true, false);
            if (rs != null)
            {
                var enumerator = rs.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Key is string s)
                    {
                        yield return s;
                    }
                }
                //foreach (System.Collections.DictionaryEntry e in rs)
                //{
                //    string? s = e.Key?.ToString();
                //    if (!string.IsNullOrEmpty(s))
                //    {
                //        yield return s;
                //    }
                //}
            }
        }
    }
}
