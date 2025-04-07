using InfraManager.BLL.ServiceCatalogue.PortfolioServices;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceCatalog
{
    public class ServiceClient : ClientWithAuthorization, ISupportLineResponsibleClient
    {
        internal static string _url = "services/";
        public ServiceClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<ServiceDetailsModel> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<ServiceDetailsModel>($"{_url}{guid}", userId, cancellationToken);
        }

        public async Task<SupportLineResponsibleDetails[]> GetResponsibleAsync(Guid guid, Guid? userId = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<SupportLineResponsibleDetails[]>($"service/{guid}/responsible", userId, cancellationToken);
        }
    }
}
