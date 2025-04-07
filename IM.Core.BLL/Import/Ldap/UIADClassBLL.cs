using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.BLL.Import;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.Import.BLL.Ldap.Import
{
    internal class UIADClassBLL :
        IUIADClassBLL, ISelfRegisteredService<IUIADClassBLL>
    {
        private readonly IImportApi _api;
        public UIADClassBLL(IImportApi api)
        {
            _api = api;
        }

        public async Task<UIADClassOutputDetails> AddAsync(UIADClassDetails data, CancellationToken cancellationToken = default)
        {
            return await _api.AddAsync(data, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
           await _api.DeleteAsync(id, cancellationToken);
        }

        public async Task<UIADClassOutputDetails> DetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _api.DetailsAsync(id, cancellationToken);
        }

        public async Task<UIADClassOutputDetails[]> GetDetailsArrayAsync(UIADClassFilter filter, CancellationToken cancellationToken)
        {
            return await _api.GetDetailsArrayAsync(filter, cancellationToken);
        }

        public async Task<UIADClassOutputDetails> UpdateAsync(Guid id, UIADClassDetails data, CancellationToken cancellationToken = default)
        {
            return await _api.UpdateAsync(id, data, cancellationToken);
        }
    }
}