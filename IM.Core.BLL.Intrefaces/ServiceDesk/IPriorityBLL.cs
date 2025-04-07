using InfraManager.BLL.Asset.dto;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    public interface IPriorityBLL
    {
        Task<PriorityDetailsModel[]> ListAsync(
            LookupListFilterModel filterBy,
            CancellationToken cancellationToken = default);

        Task<PriorityDetailsModel> FindAsync(
            Guid id, 
            CancellationToken cancellationToken = default);

        Task<PriorityDetailsModel> AddAsync(
            PriorityModel model, 
            CancellationToken cancellationToken = default);

        Task<PriorityDetailsModel> UpdateAsync(
            Guid id,
            PriorityModel model, 
            CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> SaveOrUpdateAsync(PriorityDetailsModel priority, CancellationToken cancellationToken = default);
    }

}
