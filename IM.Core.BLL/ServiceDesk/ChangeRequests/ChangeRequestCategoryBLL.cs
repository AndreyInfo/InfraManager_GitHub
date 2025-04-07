using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestCategoryBLL : IChangeRequestCategoryBLL, ISelfRegisteredService<IChangeRequestCategoryBLL>
    {
        private readonly IGetEntityArrayBLL<Guid, ChangeRequestCategory, ChangeRequestCategoryDetails, LookupListFilter> _entityArrayBLL;

        public ChangeRequestCategoryBLL(
            IGetEntityArrayBLL<Guid, ChangeRequestCategory, ChangeRequestCategoryDetails, LookupListFilter> entityArrayBLL)
        {
            _entityArrayBLL = entityArrayBLL;
        }

        public async Task<ChangeRequestCategoryDetails[]> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _entityArrayBLL.ArrayAsync(new LookupListFilter(), cancellationToken);         
        }

    }
}
