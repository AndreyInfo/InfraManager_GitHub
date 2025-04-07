using InfraManager.BLL.OrganizationStructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class SubdivisionsClient : ClientWithAuthorization
    {
        internal static string _url = "subdivisions/";
        public SubdivisionsClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<SubdivisionDetails> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<SubdivisionDetails>($"{_url}{guid}", userId, cancellationToken);
        }

        public async Task<SubdivisionDetails[]> GetChildrenAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<SubdivisionDetails[]>($"{_url}{guid}/childer", userId, cancellationToken);
        }
    }
}
