using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceDesk;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class UrgencyClient : ClientWithAuthorization
    {
        internal static string _url = "Urgency/";

        public UrgencyClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<Urgency> GetAsync(Guid guid, Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<Urgency>($"{_url}Get?id={guid}", userId, cancellationToken);
        }
    }
}