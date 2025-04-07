using InfraManager.BLL.ServiceDesk.Negotiations;
using InfraManager;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Negotiations;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Problems;
using System.Security.Policy;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class NegotiationsClient : ClientWithAuthorization
    {
        internal static string _url = "Negotiations";
        public NegotiationsClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<NegotiationDetailsModel> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<NegotiationDetailsModel>($"{_url}/{guid}", userId, cancellationToken);
        }

        public async Task<NegotiationDetailsModel[]> GetListByObject(int objectCalssId, Guid objectId, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetListAsync<NegotiationDetailsModel[]>($"{_url}?objectClassId={objectCalssId}&objectId={objectId}&userID={userId}", null, userId, cancellationToken);
        }
        public async Task<NegotiationDetailsModel> AddAsync(NegotiationData negotiation, int objectCalssId, Guid objectId, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<NegotiationDetailsModel, NegotiationData>($"{objectCalssId}/{objectId}/{_url}", negotiation, userId, cancellationToken);
        }
        public async Task<NegotiationDetailsModel> SaveAsync(NegotiationData negotiation, Guid negotiationId, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PatchAsync<NegotiationDetailsModel, NegotiationData>($"{_url}/{negotiationId}", negotiation, userId, cancellationToken);
        }
        public async Task<NegotiationUserDetailsModel> PatchNegotiationUserAsync(Guid negotiationId, Guid userId, VoteData data, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PatchAsync<NegotiationUserDetailsModel, VoteData>($"{_url}/{negotiationId}/users/{userId}", data, userId, cancellationToken);
        }
    }
}
