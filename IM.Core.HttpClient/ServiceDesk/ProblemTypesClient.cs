using InfraManager.WebApi.Contracts.Models.ServiceDesk.Problems;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class ProblemTypesClient : ClientWithAuthorization
    {
        private static readonly string _url = "ProblemTypes/";

        public ProblemTypesClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<ProblemTypeDetailsModel> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<ProblemTypeDetailsModel>($"{_url}{guid}", userId, cancellationToken);
        }

        public async Task<ProblemTypeDetailsModel[]> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await GetListAsync<ProblemTypeDetailsModel[]>(_url, null, cancellationToken);
        }
    }
}
