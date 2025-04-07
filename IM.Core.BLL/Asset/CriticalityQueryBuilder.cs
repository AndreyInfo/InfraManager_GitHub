using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System;

namespace InfraManager.BLL.Asset
{
    internal class CriticalityQueryBuilder : 
        IBuildEntityQuery<Criticality, LookupDetails<Guid>, LookupListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<Criticality, LookupDetails<Guid>, LookupListFilter>>
    {
        private readonly IReadonlyRepository<Criticality> _repository;

        public CriticalityQueryBuilder(IReadonlyRepository<Criticality> repository)
        {
            _repository = repository;
        }

        public IExecutableQuery<Criticality> Query(LookupListFilter filterBy)
        {
            var query = _repository.Query();

            if (!string.IsNullOrWhiteSpace(filterBy.SearchName))
            {
                query = query.Where(x => x.Name.ToLower().Contains(filterBy.SearchName));
            }

            return query;
        }
    }
}
