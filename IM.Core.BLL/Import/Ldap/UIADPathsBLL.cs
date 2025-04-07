using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.BLL.Import;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.Import.BLL.Ldap.Import
{
    internal class UIADPathsBLL :
        IUIADPathsBLL, ISelfRegisteredService<IUIADPathsBLL>
    {
        private readonly IImportApi _api;
        public UIADPathsBLL(IImportApi api)
        {
            _api = api;
        }

        public async Task<UIADPathsOutputDetails> AddAsync(UIADPathsDetails data, CancellationToken cancellationToken = default)
        {
            return await _api.AddAsync(data, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
             await _api.DeleteUIADPathsOutputDetailsAsync(id, cancellationToken);
        }

        public async Task<UIADPathsOutputDetails> DetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _api.DetailsUIADPathsOutputDetailsAsync(id, cancellationToken);
        }

        public async Task<UIADPathsOutputDetails[]> GetDetailsArrayAsync(UIADPathsFilter filter, CancellationToken cancellationToken)
        {
            return await _api.GetDetailsArrayAsync(filter, cancellationToken);
        }

        public async Task<UIADPathsOutputDetails> UpdateAsync(Guid id, UIADPathsDetails data, CancellationToken cancellationToken = default)
        {
            return await _api.UpdateAsync(id, data, cancellationToken);
        }
    }
}