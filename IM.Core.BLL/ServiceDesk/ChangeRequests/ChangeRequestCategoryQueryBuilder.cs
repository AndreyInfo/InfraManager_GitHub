using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestCategoryQueryBuilder : 
        IBuildEntityQuery<ChangeRequestCategory, ChangeRequestCategoryDetails, LookupListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<ChangeRequestCategory, ChangeRequestCategoryDetails, LookupListFilter>>
    {
        private readonly IReadonlyRepository<ChangeRequestCategory> _repository;

        public ChangeRequestCategoryQueryBuilder(IReadonlyRepository<ChangeRequestCategory> repository)
        {
            _repository = repository;
        }

        public IExecutableQuery<ChangeRequestCategory> Query(LookupListFilter filterBy)
        {
            var query = _repository.Query()
                .Where(x => !x.Removed);

            return query;
        }
    }
}
