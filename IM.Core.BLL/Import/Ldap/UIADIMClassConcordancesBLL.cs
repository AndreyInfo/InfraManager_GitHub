using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.BLL.Import;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.Import.BLL.Ldap.Import
{
    internal class UIADIMClassConcordancesBLL :
        IUIADIMClassConcordancesBLL, ISelfRegisteredService<IUIADIMClassConcordancesBLL>
    {
        private readonly IImportApi _api;
        public UIADIMClassConcordancesBLL(IImportApi api)
        {
            _api = api;
        }

        public async Task<UIADIMClassConcordancesOutputDetails> AddAsync(UIADIMClassConcordancesDetails data, CancellationToken cancellationToken = default)
        {
            return await _api.AddAsync(data, cancellationToken);
        }

        public async Task DeleteAsync(UIADIMClassConcordancesKey id, CancellationToken cancellationToken = default)
        {
            await _api.DeleteAsync(id, cancellationToken);
        }

        public async Task<UIADIMClassConcordancesOutputDetails> DetailsAsync(UIADIMClassConcordancesKey id, CancellationToken cancellationToken)
        {
            return await _api.DetailsAsync(id, cancellationToken);
        }

        public async Task<UIADIMClassConcordancesOutputDetails[]> GetDetailsArrayAsync(UIADIMClassConcordancesFilter filter, CancellationToken cancellationToken)
        {
            return await _api.GetDetailsArrayAsync(filter, cancellationToken);
        }

        public async Task<UIADIMClassConcordancesOutputDetails> UpdateAsync(UIADIMClassConcordancesKey id, UIADIMClassConcordancesDetails data, CancellationToken cancellationToken = default)
        {
            return await _api.UpdateAsync(id, data, cancellationToken);
        }
    }
}