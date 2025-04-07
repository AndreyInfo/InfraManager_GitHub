using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.BLL.Import;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.Import.BLL.Ldap.Import
{
    internal class UIADIMFieldConcordancesBLL :
        IUIADIMFieldConcordancesBLL, ISelfRegisteredService<IUIADIMFieldConcordancesBLL>
    {
        private readonly IImportApi _api;
        public UIADIMFieldConcordancesBLL(IImportApi api)
        {
            _api = api;
        }

        public async Task<UIADIMFieldConcordancesOutputDetails> AddAsync(UIADIMFieldConcordancesDetails data, CancellationToken cancellationToken = default)
        {
            return await _api.AddAsync(data, cancellationToken);
        }

        public async Task DeleteAsync(UIADIMFieldConcordancesKey id, CancellationToken cancellationToken = default)
        {
            await _api.DeleteAsync(id, cancellationToken);
        }

        public async Task<UIADIMFieldConcordancesOutputDetails> DetailsAsync(UIADIMFieldConcordancesKey id, CancellationToken cancellationToken)
        {
            return await _api.DetailsAsync(id, cancellationToken);
        }

        public async Task<UIADIMFieldConcordancesOutputDetails[]> GetDetailsArrayAsync(UIADIMFieldConcordancesFilter filter, CancellationToken cancellationToken)
        {
            return await _api.GetDetailsArrayAsync(filter, cancellationToken);
        }

        public async Task<UIADIMFieldConcordancesOutputDetails> UpdateAsync(UIADIMFieldConcordancesKey id, UIADIMFieldConcordancesDetails data, CancellationToken cancellationToken = default)
        {
            return await _api.UpdateAsync(id, data, cancellationToken);
        }
    }
}