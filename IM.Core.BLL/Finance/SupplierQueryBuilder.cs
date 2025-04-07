using Inframanager.BLL;
using InfraManager.DAL;
using SupplierEntity = InfraManager.DAL.Finance.Supplier;

namespace InfraManager.BLL.Finance
{
    internal class SupplierQueryBuilder : 
        IBuildEntityQuery<SupplierEntity, SupplierDetails, LookupListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<SupplierEntity, SupplierDetails, LookupListFilter>>
    {
        private readonly IReadonlyRepository<SupplierEntity> _repository;

        public SupplierQueryBuilder(IReadonlyRepository<SupplierEntity> repository)
        {
            _repository = repository;
        }

        public IExecutableQuery<SupplierEntity> Query(LookupListFilter filterBy)
        {
            var query = _repository.Query();

            if (!string.IsNullOrWhiteSpace(filterBy.SearchName))
            {
                query = query.Where(x => x.Name.Contains(filterBy.SearchName));
            }

            return query;
        }
    }
}
