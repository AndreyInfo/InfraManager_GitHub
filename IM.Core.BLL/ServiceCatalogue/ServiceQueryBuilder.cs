using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using System.Linq;

namespace InfraManager.BLL.ServiceCatalogue
{
    internal class ServiceQueryBuilder : 
        IBuildEntityQuery<Service, ServiceDetails, ServiceListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<Service, ServiceDetails, ServiceListFilter>>
    {
        private readonly IReadonlyRepository<Service> _repository;

        public ServiceQueryBuilder(IReadonlyRepository<Service> repository)
        {
            _repository = repository;
        }

        public IExecutableQuery<Service> Query(ServiceListFilter filterBy)
        {
            var query = _repository.Query();
            if(filterBy.StateList != null) 
                query = query.Where(x => filterBy.StateList.Contains(x.State));

            if (filterBy.CategoryID.HasValue)
            {
                query = filterBy.NullCategory 
                    ? query.Where(x => x.CategoryID == filterBy.CategoryID || x.CategoryID == null)
                    : query.Where(x => x.CategoryID == filterBy.CategoryID);
            }
            else if (filterBy.NullCategory)
            {
                query = query.Where(x => x.CategoryID == null);
            }

            return query;
        }
    }
}
