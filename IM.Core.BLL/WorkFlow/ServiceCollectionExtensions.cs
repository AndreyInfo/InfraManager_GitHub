using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace InfraManager.BLL.Workflow
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflow(this IServiceCollection services)
        {
            var serviceMapping = new ServiceMapping<ObjectClass, IFindWorkflowEntity>();
            foreach (var workflowEntityType in typeof(IWorkflowEntity)
                .Assembly
                .GetTypes()
                .Where(
                    tp => tp.HasAttribute<ObjectClassMappingAttribute>()
                        && tp.HasInterface<IWorkflowEntity>()))
            {
                var objectClassMapping = 
                    workflowEntityType.GetAttribute<ObjectClassMappingAttribute>();
                serviceMapping
                     .Map(typeof(WorkflowEntityFinder<>).MakeGenericType(workflowEntityType))
                     .To(objectClassMapping.ObjectClass);
            }

            return services.AddMappingScoped(serviceMapping);
        }
    }
}
