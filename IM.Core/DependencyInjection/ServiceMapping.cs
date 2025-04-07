using System;
using System.Collections.Generic;

namespace InfraManager.DependencyInjection
{
    public class ServiceMapping<T, TService>
    {
        internal Dictionary<T, Type> Mapping { get; } = new Dictionary<T, Type>();

        public class KeyMapping
        {
            private readonly ServiceMapping<T, TService> _parent;

            private readonly Type _serviceType;

            internal KeyMapping(ServiceMapping<T, TService> parent, Type serviceType)
            {
                _parent = parent;
                _serviceType = serviceType;
            }

            public ServiceMapping<T, TService> To(T key)
            {
                _parent.Mapping.Add(key, _serviceType);

                return _parent;
            }
        }

        public KeyMapping Map<V>() where V : TService
        {
            return new KeyMapping(this, typeof(V));
        }

        public KeyMapping Map(Type service)
        {
            return new KeyMapping(this, service);
        }
    }
}
