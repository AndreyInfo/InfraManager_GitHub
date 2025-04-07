using System.Linq;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure
{
    internal class SubdivisionQueryBuilder :
        IBuildEntityQuery<Subdivision, SubdivisionDetails, SubdivisionListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<Subdivision, SubdivisionDetails, SubdivisionListFilter>>
    {
        private readonly IReadonlyRepository<Subdivision> _repository;
        private readonly IReadonlyRepository<Organization> _organizationsRepository;

        public SubdivisionQueryBuilder(IReadonlyRepository<Subdivision> repository, IReadonlyRepository<Organization> organizationsRepository)
        {
            _repository = repository;
            _organizationsRepository = organizationsRepository;
        }

        public IExecutableQuery<Subdivision> Query(SubdivisionListFilter filterBy)
        {
            var query = _repository.Query();

            if (filterBy.OrganizationID.HasValue)
            {
                query = query.Where(x => x.OrganizationID == filterBy.OrganizationID.Value);
            }

            if (filterBy.ParentID.HasValue)
            {
                query = query.Where(x => x.SubdivisionID == filterBy.ParentID.Value);
            }
            else if (filterBy.OnlyRoots)
            {
                query = query.Where(x => x.SubdivisionID == null);
            }

            if (! string.IsNullOrWhiteSpace(filterBy.OrganizationNameSearch))
            {
                var organizationIDs = _organizationsRepository.Where(x =>
                    x.Name.ToLower().Contains(filterBy.OrganizationNameSearch.ToLower())).Select(x=>x.ID);
                
                query = query.Where(x => organizationIDs.Contains(x.OrganizationID));
            }

            return query;
        }
    }
}
