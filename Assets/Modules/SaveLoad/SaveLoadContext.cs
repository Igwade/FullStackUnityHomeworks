using System;
using System.Collections.Generic;

namespace SaveLoad
{
    public class SaveLoadContext : ISaveLoadContext
    {
        private readonly Dictionary<Type, object> _services = new();

        public void Register<T>(T service)
        {
            _services[typeof(T)] = service;
        }

        public void Register<T>(T service, params Type[] interfaces)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            
            if (interfaces == null || interfaces.Length == 0)
                throw new ArgumentException("At least one interface must be provided.", nameof(interfaces));
            
            foreach (var iface in interfaces)
            {
                if (!iface.IsAssignableFrom(service.GetType()))
                    throw new ArgumentException($"Service does not implement interface {iface}", nameof(interfaces));

                _services[iface] = service;
            }
        }

        public T Get<T>()
        {
            if (_services.TryGetValue(typeof(T), out var service))
                return (T)service;

            throw new InvalidOperationException($"Service of type {typeof(T)} not registered in SerializationContext.");
        }

        public bool TryGet<T>(out T value)
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                value = (T)service;
                return true;
            }

            value = default;
            return false;
        }
    }
}