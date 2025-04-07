using InfraManager.BLL.OrganizationStructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class OrganizationsClient : ClientWithAuthorization
    {
        internal static string _url = "organizations/";
        public OrganizationsClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<OrganizationDetails> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<OrganizationDetails>($"{_url}{guid}", userId, cancellationToken);
        }
    }
}
