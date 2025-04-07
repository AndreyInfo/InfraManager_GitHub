using InfraManager.BLL;
using InfraManager.BLL.ConfigurationUnit.DTO;
using InfraManager.BLL.DataEntities.DTO;
using InfraManager.BLL.ServiceDesk.Calls;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.ServiceDesk
{
    public class ConfigurationUnitsClient : ClientWithAuthorization
    {
        internal static string _url = "ConfigurationUnit/";
        public ConfigurationUnitsClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<ConfigurationUnitDetails> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<ConfigurationUnitDetails>($"{_url}{guid}", userId, cancellationToken);
        }
    }
}
