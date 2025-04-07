using System;
using System.Threading.Tasks;
using InfraManager.BLL.Asset.dto;
using System.Threading;

namespace InfraManager.BLL.ServiceDesk
{
    public interface IInfluenceBLL
    {
        Task<InfluenceDetails[]> GetAllInfluenceAsync(CancellationToken cancellationToken = default);

        Task<bool> SaveAsync(InfluenceDetails influence, CancellationToken cancellationToken = default);

        Task RemoveByIdAsync(Guid influencyId, CancellationToken cancellationToken);

        Task<InfluenceListItemModel[]> ListAsync(CancellationToken cancellationToken);
    }
}
