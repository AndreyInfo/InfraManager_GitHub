using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.WorkOrderTemplates;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public interface IWorkOrderTemplateBLL
    {
        Task<WorkOrderTemplateDetails[]> GetDetailsArrayAsync(
            WorkOrderTemplateLookupListFilter filter,
            CancellationToken cancellationToken = default);

        Task<WorkOrderTemplateDetails[]> GetDetailsPageAsync(
            WorkOrderTemplateLookupListFilter filter,
            ClientPageFilter<WorkOrderTemplate> pageFilter,
            CancellationToken cancellationToken = default);

        Task<WorkOrderTemplateDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
