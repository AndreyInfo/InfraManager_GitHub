using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.MassIncidents.TechnicalFailureCategories;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class TechnicalFailureCategoryLoader :
        ILoadEntity<int, TechnicalFailureCategory, TechnicalFailureCategoryDetails>,
        IBuildEntityQuery<TechnicalFailureCategory, TechnicalFailureCategoryDetails, TechnicalFailureCategoryFilter>,
        ISelfRegisteredService<ILoadEntity<int, TechnicalFailureCategory, TechnicalFailureCategoryDetails>>,
        ISelfRegisteredService<IBuildEntityQuery<TechnicalFailureCategory, TechnicalFailureCategoryDetails, TechnicalFailureCategoryFilter>>
    {
        private IReadonlyRepository<TechnicalFailureCategory> _repository;

        public TechnicalFailureCategoryLoader(IReadonlyRepository<TechnicalFailureCategory> repository)
        {
            _repository = repository;
        }

        public async Task<TechnicalFailureCategory> LoadAsync(int id, CancellationToken cancellationToken = default)
        {
            return await Include(_repository).SingleOrDefaultAsync(x => x.ID == id, cancellationToken);
        }

        public IExecutableQuery<TechnicalFailureCategory> Query(TechnicalFailureCategoryFilter filterBy)
        {
            var query = Include(_repository).Query();

            if (filterBy.ServiceReferenceID.HasValue)
            {
                query = query.Where(x => x.Services.Any(x => x.IMObjID == filterBy.ServiceReferenceID));
            }
            if(filterBy.GlobalIdentifiers.HasValue)
            {
                query = query.Where(x=>x.IMObjID==filterBy.GlobalIdentifiers.Value);
            }

            return filterBy.ServiceID.HasValue
                ? query.Where(TechnicalFailureCategory.AvailableForService.Build(filterBy.ServiceID.Value))
                : query;
        }

        private static IReadonlyRepository<TechnicalFailureCategory> Include(
            IReadonlyRepository<TechnicalFailureCategory> repository)
        {
            return repository.WithMany(x => x.Services).ThenWith(x => x.Reference);
        }
    }
}
