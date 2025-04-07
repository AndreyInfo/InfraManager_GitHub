using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Import
{
    internal class ConfigurationCSVBLL : IConfigurationCSVBLL, ISelfRegisteredService<IConfigurationCSVBLL>
    {
        private readonly IImportApi _api;
        public ConfigurationCSVBLL(IImportApi api)
        {
            _api = api;
        }
        public async Task<ConfigurationCSVDetails> GetConfigurationAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _api.GetConfigurationAsync(id, cancellationToken);
        }

        public async Task SetConfigurationAsync(ConfigurationCSVData configurationCSVDetails, CancellationToken cancellationToken)
        {
            await _api.SetConfigurationAsync(configurationCSVDetails, cancellationToken);
        }

        public async Task UpdateConfigurationAsync(Guid id, ConfigurationCSVData configurationCSVDetails, CancellationToken cancellationToken)
        {
            await _api.UpdateConfigurationAsync(id, configurationCSVDetails, cancellationToken);
        }

        public async Task DeleteConfigurationAsync(Guid id, CancellationToken cancellationToken)
        {
            await _api.DeleteConfigurationAsync(id, cancellationToken);
        }
    }
}
