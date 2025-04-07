using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceAttendances;

namespace IM.Core.HttpClient.ServiceCatalog
{
    public class ServiceAttendanceClient : ClientWithAuthorization, ISupportLineResponsibleClient
    {
        private const string _url = "ServiceAttendance/";

        public ServiceAttendanceClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<ServiceAttendanceDetails> GetAsync(Guid guid, Guid? userId = null,
            CancellationToken cancellationToken = default) 
            => await GetAsync<ServiceAttendanceDetails>($"{_url}{guid}", userId, cancellationToken);

        public async Task<SupportLineResponsibleDetails[]> GetResponsibleAsync(Guid guid, Guid? userId = null,
            CancellationToken cancellationToken = default)
            => await GetAsync<SupportLineResponsibleDetails[]>($"{_url}{guid}/responsible", userId, cancellationToken);
    }
}