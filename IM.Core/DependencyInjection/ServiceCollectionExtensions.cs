using Inframanager;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InfraManager.DependencyInjection
{
    /// <summary>
    /// Этот класс содержит общие методы расширения IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Этот метод расширения регистрирует в коллекции сервисов все типы сборки, реализующие интерфейс ISelfRegisteredService
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="assembly">Сборка с сервисами</param>
        /// <returns>Коллекция сервисов</returns>
        public static IServiceCollection AddSelfRegisteredServices(this IServiceCollection services, Assembly assembly)
        {
            foreach(var dependency 
                in assembly.EnumerateGenericImplementationsOf(typeof(ISelfRegisteredService<>)))
            {
                services.AddScoped(dependency.TypeParameters[0], dependency.Implementation);
            }

            return services;
        }

        public static IServiceCollection AddServiceMapperScoped<TKey, TService>(
            this IServiceCollection services, 
            IDictionary<TKey, Type> implementations) where TService : class
        {
            foreach (var implementation in implementations
                .Where(t => services.All(x => x.ServiceType != t.Value)))
            {
                services.AddScoped(implementation.Value);
            }

            services.AddScoped<IServiceMapper<TKey, TService>>(
                provider =>
                {
                    var resolver = new ServiceMapper<TKey, TService>(provider);
                    foreach(var implementation in implementations)
                    {
                        resolver.AddImplementation(implementation.Key, implementation.Value);
                    }

                    return resolver;
                });

            return services;
        }

        public static IServiceCollection AddObjectClassServiceMapperScoped<TService>(this IServiceCollection services, Assembly assembly)
            where TService : class
        {
            return services.AddServiceMapperScoped<ObjectClass, TService>(
                assembly.GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(TService)) 
                        && t.GetCustomAttribute<ObjectClassMappingAttribute>() != null)
                    .ToDictionary(t => t.GetCustomAttribute<ObjectClassMappingAttribute>().ObjectClass));
        }

        public static IServiceCollection AddMappingScoped<TKey, TService>(this IServiceCollection services, ServiceMapping<TKey, TService> mapping)
            where TService : class
        {
            return services.AddServiceMapperScoped<TKey, TService>(mapping.Mapping);
        }

        public static IServiceCollection AddDefaultGenericImplementationScoped(
            this IServiceCollection services,
            IEnumerable<Type> types,
            Type service,
            Type implementation)
        {
            foreach(var type in types)
            {
                services.AddScoped(
                    service.MakeGenericType(type),
                    implementation.MakeGenericType(type));
            }

            return services;
        }
    }
}
