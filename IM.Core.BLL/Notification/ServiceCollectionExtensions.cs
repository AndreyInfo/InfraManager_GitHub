using System.Linq;
using InfraManager.BLL.Messages;
using InfraManager.BLL.Notification.Templates;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace InfraManager.BLL.Notification
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNotificationTemplate(this IServiceCollection services)
        {
            var serviceMapping = new ServiceMapping<ObjectClass, INotificationTemplateBLL>();

            var typeDefinitions = typeof(NotificationBLL).Assembly.GetTypes()
                .Where(type => type.HasAttribute<ObjectClassMappingAttribute>() && type.HasInterface(typeof(ITemplate<>)))
                .Select(type => new
                {
                    TemplateType = type,
                    Attribute = type.GetAttribute<ObjectClassMappingAttribute>(),
                    EntityType = type.GetInterfaces()
                        .First(xx => xx.IsGenericType && xx.GetGenericTypeDefinition() == typeof(ITemplate<>))
                        .GetGenericArguments()[0],
                });
            
            foreach (var type in typeDefinitions)
            {
                services.TryAddScoped(
                    typeof(IBuildEntityTemplate<,>).MakeGenericType(type.EntityType, type.TemplateType),
                    typeof(DefaultTemplateBuilder<,>).MakeGenericType(type.EntityType, type.TemplateType));

                serviceMapping
                    .Map(typeof(NotificationTemplateBLL<,>)
                        .MakeGenericType(type.EntityType, type.TemplateType))
                    .To(type.Attribute.ObjectClass);
            }

            return services.AddMappingScoped(serviceMapping);
        }
    }
}
