using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.BLL.ServiceCatalogue;

namespace IM.Core.HttpClient.ServiceCatalog
{
    public class ServiceLevelAgreementClient : ClientWithAuthorization
    {
        internal static string _url = "sla/";

        public ServiceLevelAgreementClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<ServiceLevelAgreementDetails> GetAsync(Guid guid, Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<ServiceLevelAgreementDetails>($"{_url}{guid}", userId, cancellationToken);
        }

        public async Task<OrganizationItemGroupData[]> GetOrganizationItemGroupsAsync(Guid guid, Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<OrganizationItemGroupData[]>($"{_url}{guid}/organization-item-groups", userId, cancellationToken);
        }

        public async Task<SLAReferenceDetails[]> GetServiceReferencesAsync(Guid guid, Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<SLAReferenceDetails[]>($"{_url}{guid}/service-references", userId, cancellationToken);
        }
    }
}
