using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class ServicesClient : ClientWithAuthorization
    {
        internal static string _url = "Service/";
        public ServicesClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<ServiceDetails> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<ServiceDetails>($"{_url}item", $"id={guid}", (x) => PreProcessRequestHeaders(x, userId), cancellationToken);
        }
    }
}