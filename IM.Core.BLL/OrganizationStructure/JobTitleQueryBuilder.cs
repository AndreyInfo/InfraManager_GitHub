using Inframanager.BLL;
using InfraManager.BLL.OrganizationStructure.JobTitles;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure
{
    internal class JobTitleQueryBuilder : 
        IBuildEntityQuery<JobTitle, JobTitleDetails, JobTitleListFilter>, 
        ISelfRegisteredService<IBuildEntityQuery<JobTitle, JobTitleDetails, JobTitleListFilter>>
    {
        private readonly IReadonlyRepository<JobTitle> _repository;

        public JobTitleQueryBuilder(IReadonlyRepository<JobTitle> repository)
        {
            _repository = repository;
        }

        public IExecutableQuery<JobTitle> Query(JobTitleListFilter filterBy)
        {
            var query = _repository.Query();

            if (!string.IsNullOrWhiteSpace(filterBy.Name))
            {
                query = query.Where(jobTitle => jobTitle.Name.ToLower().Contains(filterBy.Name.ToLower()));
            }

            if (filterBy.IMObjID.HasValue)
            {
                query = query.Where(jobTitle => jobTitle.IMObjID == filterBy.IMObjID);
            }

            return query;
        }
    }
}
