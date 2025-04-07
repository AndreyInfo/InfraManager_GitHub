using InfraManager.BLL.Asset.Adapters;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.Assets
{
    public class AdaptersClient : ClientWithAuthorization
    {
        internal static string _url = "adapters/";
        public AdaptersClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<AdapterDetails> GetAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<AdapterDetails>($"{_url}{id}", userId, cancellationToken);
        }
    }
}
