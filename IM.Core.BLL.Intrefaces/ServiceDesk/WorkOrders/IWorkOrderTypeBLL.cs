using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfraManager.BLL.ServiceDesk.DTOs;
using Inframanager.BLL;
using System.Threading;
using InfraManager.DAL.ServiceDesk;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public interface IWorkOrderTypeBLL
    {
        Task<WorkOrderTypeDetails[]> GetDetailsArrayAsync(
            LookupListFilter filterBy,
            CancellationToken cancellationToken = default);

        Task<WorkOrderTypeDetails[]> GetDetailsPageAsync(
            LookupListFilter filterBy,
            ClientPageFilter<WorkOrderType> pageFilter,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        
        Task<WorkOrderTypeDetails> AddAsync(WorkOrderTypeData data, CancellationToken cancellationToken = default);
        
        Task<WorkOrderTypeDetails> UpdateAsync(Guid id, WorkOrderTypeData data, CancellationToken cancellationToken = default);

        Task<WorkOrderTypeDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
