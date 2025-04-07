using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.BLL.Import;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.Import.BLL.Ldap.Import
{
    internal class UIADSettingsBLL :
        IUIADSettingsBLL, ISelfRegisteredService<IUIADSettingsBLL>
    {
        private readonly IImportApi _api;
        public UIADSettingsBLL(IImportApi api)
        {
            _api = api;
        }

        public async Task<UIADSettingsOutputDetails> AddAsync(UIADSettingsDetails data, CancellationToken cancellationToken = default)
        {
            return await _api.AddAsync(data, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _api.DeleteUIADSettingsOutputDetailsAsync(id, cancellationToken);
        }

        public async Task<UIADSettingsOutputDetails> DetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _api.DetailsUIADSettingsOutputDetailsAsync(id, cancellationToken);
        }

        public async Task<UIADSettingsOutputDetails[]> GetDetailsArrayAsync(UIADSettingsFilter filter, CancellationToken cancellationToken)
        {
            return await _api.GetDetailsArrayAsync(filter, cancellationToken);
        }

        public async Task<UIADSettingsOutputDetails> UpdateAsync(Guid id, UIADSettingsDetails data, CancellationToken cancellationToken = default)
        {
            return await _api.UpdateAsync(id, data, cancellationToken);
        }
    }
}