using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    public interface IEditNegotiationBLL
    {
        Task<NegotiationDetails> AddAsync(
            Guid parentObjectID,
            NegotiationData data,
            CancellationToken cancellationToken = default);
        Task<NegotiationDetails> UpdateAsync(
            Guid id,
            NegotiationData data,
            CancellationToken cancellationToken = default);
        Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default);
        Task DeleteNegotiationUserAsync(
            Guid id,
            Guid userId,
            CancellationToken cancellationToken = default);
        Task<NegotiationUserDetails> UpdateNegotiationUserAsync(
            Guid id,
            Guid userID,
            VoteData data,
            CancellationToken cancellationToken = default);
    }

}
