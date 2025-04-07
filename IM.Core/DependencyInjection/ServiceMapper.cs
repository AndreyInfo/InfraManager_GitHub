using System;
using System.Collections.Generic;

namespace InfraManager.DependencyInjection
{
    internal class ServiceMapper<TKey, TService> : IServiceMapper<TKey, TService> where TService : class
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly Dictionary<TKey, Type> _implementations = 
            new Dictionary<TKey, Type>();

        public ServiceMapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public bool HasKey(TKey key)
        {
            return _implementations.ContainsKey(key);
        }

        public TService Map(TKey key)
        {
            if (!_implementations.ContainsKey(key))
            {
                throw new Exception($"{typeof(TService)} for {key} is not registered.");
            }

            return _serviceProvider.GetService(_implementations[key]) as TService;
        }

        public ServiceMapper<TKey, TService> WithImplementation<TImplementation>(TKey key)
        {
            AddImplementation(key, typeof(TImplementation));
            return this;
        }

        public void AddImplementation(TKey key, Type implementation)
        {
            if (!implementation.IsAssignableTo(typeof(TService)))
            {
                throw new Exception($"Type {implementation} is not assignable to {typeof(TService)}.");
            }

            if (_implementations.ContainsKey(key))
            {
                throw new Exception($"{typeof(TService)} for {key} is already registered");
            }

            _implementations.Add(key, implementation);
        }
    }
}
