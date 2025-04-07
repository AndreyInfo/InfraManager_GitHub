using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    public interface IUrgencyBLL
    {
        Task<UrgencyListItemModel[]> ListAsync(CancellationToken cancellationToken);
        Task<Urgency> GetAsync(Guid id, CancellationToken cancellationToken);


        Task<Guid> SaveAsync(UrgencyDTO model, CancellationToken cancellationToken);

        Task RemoveAsync(Guid id, CancellationToken cancellationToken);

        Task<UrgencyDTO[]> GetListForTableAsync(string searchName, int take, int skip,
            CancellationToken cancellationToken);

    }
}
