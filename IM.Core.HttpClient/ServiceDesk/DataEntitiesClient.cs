using InfraManager.BLL;
using InfraManager.BLL.DataEntities.DTO;
using InfraManager.BLL.ServiceDesk.Calls;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class DataEntitiesClient : ClientWithAuthorization
    {
        internal static string _url = "dataentity/";
        public DataEntitiesClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<DataEntityDetails> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<DataEntityDetails>($"{_url}{guid}", userId, cancellationToken);
        }
    }
}
