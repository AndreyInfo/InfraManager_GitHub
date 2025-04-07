using System.Linq;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallQueryBuilder :
        IBuildEntityQuery<Call, CallDetails, CallListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<Call, CallDetails, CallListFilter>>
    {
        private readonly IReadonlyRepository<Call> _repository;

        public CallQueryBuilder(IReadonlyRepository<Call> repository)
        {
            _repository = repository;
        }

        public IExecutableQuery<Call> Query(CallListFilter filterBy)
        {
            var query = _repository
                .With(x => x.Client)
                .With(x => x.CallService)
                .ThenWith(x => x.Service)
                .ThenWith(x => x.Category)
                .With(x => x.CallService)
                .ThenWith(x => x.ServiceItem)
                .ThenWith(x => x.Service)
                .With(x => x.CallService)
                .ThenWith(x => x.ServiceAttendance)
                .ThenWith(x => x.Service)
                .With(x => x.CallType)
                .With(x => x.IncidentResult)
                .With(x => x.RequestForServiceResult)
                .With(x => x.Priority)
                .With(x => x.Queue)
                .With(x => x.Urgency)
                .Query();

            if (filterBy.Number.HasValue)
            {
                query = query.Where(x => x.Number == filterBy.Number);
            }

            if (filterBy.ShouldSearchFinished.HasValue && !filterBy.ShouldSearchFinished.Value)
            {
                query = query.Where(x => x.EntityStateID != null || x.WorkflowSchemeID != null || x.WorkflowSchemeVersion == null);
            }

            if (filterBy.IDs != null && filterBy.IDs.Any())
            {
                query = query.Where(x => filterBy.IDs.Contains(x.IMObjID));
            }

            if (!filterBy.ShouldSearchAccomplished.GetValueOrDefault(true))
            {
                query = query.Where(x =>
                    x.UtcDateOpened == null || x.UtcDateAccomplished == null || x.UtcDateClosed == null ||
                    x.UtcDateOpened > x.UtcDateAccomplished);
            }

            if (filterBy.ExecutorID.HasValue)
            {
                query = query.Where(x => x.ExecutorID == filterBy.ExecutorID);
            }

            if (filterBy.OwnerID.HasValue)
            {
                query = query.Where(x => x.OwnerID == filterBy.OwnerID);
            }

            return query;
        }
    }
}
