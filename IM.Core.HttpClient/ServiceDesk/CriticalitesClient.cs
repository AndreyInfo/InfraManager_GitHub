using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class CriticalitesClient : ClientWithAuthorization
    {
        internal static string _url = "criticalities/";
        public CriticalitesClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<LookupDetails<Guid>> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<LookupDetails<Guid>>($"{_url}{guid}", userId, cancellationToken);
        }
    }
}
