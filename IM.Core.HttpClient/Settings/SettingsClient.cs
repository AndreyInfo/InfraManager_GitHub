using InfraManager;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Calls;
using InfraManager.WebApi.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.Settings
{
    public class SettingsClient : ClientWithAuthorization
    {
        internal static string _url = "Settings/";
        public SettingsClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<SettingDetails> GetAsync(SystemSettings key, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<SettingDetails>($"{_url}{key}", userId, cancellationToken);
        }

    }
}
