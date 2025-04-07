using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Manhours;

namespace InfraManager.BLL.ServiceDesk.Manhours
{
    internal class ManhoursWorkQueryBuilder : IBuildEntityQuery<ManhoursWork, ManhoursWorkDetails, ManhoursListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<ManhoursWork, ManhoursWorkDetails, ManhoursListFilter>>
    {
        private readonly IRepository<ManhoursWork> _repository;

        public ManhoursWorkQueryBuilder(IRepository<ManhoursWork> repository)
        {
            _repository = repository;
        }

        public IExecutableQuery<ManhoursWork> Query(ManhoursListFilter filterBy)
        {
            var query = _repository
                .WithMany(x => x.Entries)
                .Query();

            if (filterBy.Parent.HasValue)
            {
                query = query
                    .Where(
                        x => x.ObjectID == filterBy.Parent.Value.Id
                             && x.ObjectClassID == filterBy.Parent.Value.ClassId);
            }

            if (filterBy.ExecutorID.HasValue)
            {
                query = query
                    .Where(x => x.ExecutorID == filterBy.ExecutorID.Value);
            }

            return query;
        }
    }
}
