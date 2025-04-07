using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.BLL.Import;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.Import.BLL.Ldap.Import
{
    internal class UIADConfigurationsBLL :
        IUIADConfigurationsBLL, ISelfRegisteredService<IUIADConfigurationsBLL>
    {
        private readonly IImportApi _api;
        public UIADConfigurationsBLL(IImportApi api)
        {
            _api = api;
        }

        public async Task<UIADConfigurationsOutputDetails> AddAsync(UIADConfigurationsDetails data, CancellationToken cancellationToken = default)
        {
            return await _api.AddAsync(data, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _api.DeleteUIADConfigurationsDetailsAsync(id, cancellationToken);
        }

        public async Task<UIADConfigurationsOutputDetails> DetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _api.DetailsUIADConfigurationsOutputDetailsAsync(id, cancellationToken);
        }

        public async Task<UIADConfigurationsOutputDetails[]> GetDetailsArrayAsync(UIADConfigurationsFilter filter, CancellationToken cancellationToken)
        {
            return await _api.GetDetailsArrayAsync(filter, cancellationToken);
        }

        public async Task<UIADConfigurationsOutputDetails> UpdateAsync(Guid id, UIADConfigurationsDetails data, CancellationToken cancellationToken = default)
        {
            return await _api.UpdateAsync(id, data, cancellationToken);
        }
    }
}