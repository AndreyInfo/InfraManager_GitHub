using System.Linq;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    internal class ProblemQueryBuilder : 
        IBuildEntityQuery<Problem, ProblemDetails, ProblemListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<Problem, ProblemDetails, ProblemListFilter>>
    {
        private readonly IReadonlyRepository<Problem> _repository;

        public ProblemQueryBuilder(IReadonlyRepository<Problem> repository)
        {
            _repository = repository;
        }

        public IExecutableQuery<Problem> Query(ProblemListFilter filterBy)
        {
            var query = _repository
                .With(x => x.Urgency)
                .With(x => x.Influence)
                .With(x => x.Type)
                .With(x => x.Priority)
                .With(x => x.ProblemCause)
                .With(x => x.Dependencies)
                .With(x => x.Negotiations)
                .With(x => x.CallReferences)
                .With(x => x.WorkOrderReferences)
                .With(x => x.Notes)
                .Query();

            if (filterBy.Number.HasValue)
            {
                query = query.Where(x => x.Number == filterBy.Number);
            }

            if (filterBy.IDs != null && filterBy.IDs.Any())
            {
                query = query.Where(x => filterBy.IDs.Contains(x.IMObjID));
            }

            if (filterBy.ShouldSearchFinished.HasValue && !filterBy.ShouldSearchFinished.Value)
            {
                query = query.Where(x => x.EntityStateID != null || x.WorkflowSchemeID != null || x.WorkflowSchemeVersion == null);
            }

            if (filterBy.ExecutorID.HasValue)
            {
                query = query.Where(x => x.ExecutorID == filterBy.ExecutorID.Value);
            }

            if (!filterBy.ShouldSearchAccomplished.GetValueOrDefault(true))
            {
                query = query.Where(x => x.UtcDateSolved == null || x.UtcDateClosed == null);
            }

            return query;
        }
    }
}
