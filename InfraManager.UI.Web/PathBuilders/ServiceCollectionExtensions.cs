using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.UI.Web.PathBuilders
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPathBuilders(this IServiceCollection services)
        {
            return services.AddObjectClassServiceMapperScoped<IBuildResourcePath>(
                typeof(ServiceCollectionExtensions).Assembly);           
        }
    }
}
