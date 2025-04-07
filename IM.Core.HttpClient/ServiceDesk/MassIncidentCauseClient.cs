using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.Core.Extensions;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.MassIncidents;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class MassIncidentCauseClient : ClientWithAuthorization
    {
        internal static string _url = "MassIncidentCauses/";
        public MassIncidentCauseClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<MassIncidentCauseDetails> GetAsync(Guid guid, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await GetAsync<MassIncidentCauseDetails[]>($"{_url}?GlobalIdentifiers={guid}", userID, cancellationToken)).FirstOrDefault();
        }

        public async Task<MassIncidentCauseDetails> GetAsync(int number, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<MassIncidentCauseDetails>($"{_url}{number}", userID, cancellationToken);
        }
    }
}
