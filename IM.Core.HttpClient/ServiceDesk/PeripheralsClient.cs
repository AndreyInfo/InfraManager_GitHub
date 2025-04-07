using InfraManager.BLL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class PeripheralsClient : ClientWithAuthorization
    {
        internal static string _url = "peripherals/";
        public PeripheralsClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<PeripheralDetails> GetAsync(Guid id, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<PeripheralDetails>($"{_url}{id}", userId, cancellationToken);
        }
    }
}
