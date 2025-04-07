using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IM.Core.DM.BLL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDMBLL(this IServiceCollection services)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();

            services.AddSelfRegisteredServices(thisAssembly);

            return services;
        }
    }
}
