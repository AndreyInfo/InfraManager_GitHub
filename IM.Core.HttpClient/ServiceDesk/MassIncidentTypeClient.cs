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
    public class MassIncidentTypeClient : ClientWithAuthorization
    {
        internal static string _url = "MassIncidentTypes/";
        public MassIncidentTypeClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<MassIncidentTypeDetails> GetAsync(Guid guid, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await GetAsync<MassIncidentTypeDetails[]>($"{_url}?GlobalIdentifiers={guid}", userID, cancellationToken)).FirstOrDefault();
        }

        public async Task<MassIncidentTypeDetails> GetAsync(int number, Guid? userID = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<MassIncidentTypeDetails>($"{_url}{number}", userID, cancellationToken);
        }
    }
}
