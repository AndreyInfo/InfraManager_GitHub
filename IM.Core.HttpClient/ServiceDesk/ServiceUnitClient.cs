using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk.ServiceUnits;
using InfraManager.DAL.ServiceDesk;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class ServiceUnitClient : ClientWithAuthorization
    {
        internal static string _url = "ServiceUnits/";

        public ServiceUnitClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<ServiceUnitDetails> GetAsync(Guid guid, Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<ServiceUnitDetails>($"{_url}Get?id={guid}", userId, cancellationToken);
        }
    }
}