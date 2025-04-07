using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IM.Core.WF.BLL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWfBLL(this IServiceCollection services)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();

            services.AddSelfRegisteredServices(thisAssembly);

            return services;
        }
    }
}
