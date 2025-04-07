using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;

namespace IM.Core.HttpClient.ServiceCatalog
{
    public class ServiceItemClient : ClientWithAuthorization, ISupportLineResponsibleClient
    {
        internal static string _url = "ServiceItems/";

        public ServiceItemClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<ServiceItemDetailsModel> GetAsync(Guid guid, Guid? userId = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<ServiceItemDetailsModel>($"{_url}{guid}", userId, cancellationToken);
        }

        public async Task<SupportLineResponsibleDetails[]> GetResponsibleAsync(Guid guid, Guid? userId = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<SupportLineResponsibleDetails[]>($"{_url}{guid}/responsible", userId, cancellationToken);
        }
    }
}