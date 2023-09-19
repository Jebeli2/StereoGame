namespace StereoGame.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GameServiceContainer : IServiceProvider
    {
        private readonly Dictionary<Type, object> services = new();

        public GameServiceContainer()
        {
        }

        public void AddService(Type type, object? provider)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (!type.IsAssignableFrom(provider.GetType()))
                throw new ArgumentException("The provider does not match the specified service type!");

            services.Add(type, provider);
        }

        public object? GetService(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (services.TryGetValue(type, out object? service)) return service;
            return null;
        }

        public void RemoveService(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            services.Remove(type);
        }

        public void AddService<T>(T provider)
        {
            AddService(typeof(T), provider);
        }

        public T? GetService<T>() where T : class
        {
            var service = GetService(typeof(T));
            if (service == null) return null;
            return (T)service;
        }
    }
}
