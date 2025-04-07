using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Problems;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class ProblemCausesClient : ClientWithAuthorization
    {
        internal static string _url = "ProblemCauses/";
        public ProblemCausesClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<ProblemCauseDetails> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<ProblemCauseDetails>($"{_url}{guid}", userId, cancellationToken);
        }


    }
}
