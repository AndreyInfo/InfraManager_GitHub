using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DependencyInjection
{
    public class CustomInjector
    {
        private readonly Dictionary<Type, object> _injections =
            new Dictionary<Type, object>();

        private readonly Type[] _genericParams;

        public CustomInjector()
        {
            _genericParams = Array.Empty<Type>();
        }

        public CustomInjector(params Type[] genericParams)
        {
            _genericParams = genericParams;
        }

        public CustomInjector Inject<T>(T value)
        {
            _injections.Add(typeof(T), value);
            return this;
        }

        public object GetService(
            IServiceProvider serviceProvider, 
            Type implementationType)
        {
            var concreteImplementationType = _genericParams.Any()
                ? implementationType.MakeGenericType(_genericParams)
                : implementationType;
            var constructor = concreteImplementationType
                .GetConstructors()
                .First();
            var parameters = constructor
                .GetParameters()
                .Select(
                    p => _injections.ContainsKey(p.ParameterType)
                        ? _injections[p.ParameterType]
                        : serviceProvider.GetService(p.ParameterType))
                .ToArray();

            return constructor.Invoke(parameters);
        }

        public T GetService<T>(IServiceProvider provider) => (T)GetService(provider, typeof(T));
    }
}
