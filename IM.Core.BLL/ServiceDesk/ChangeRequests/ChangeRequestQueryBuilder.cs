using System;
using System.Linq;
using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.ChangeRequests;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    internal class ChangeRequestQueryBuilder :
        IBuildEntityQuery<ChangeRequest, ChangeRequestDetails, ChangeRequestListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<ChangeRequest, ChangeRequestDetails, ChangeRequestListFilter>>
    {
        private readonly IReadonlyRepository<ChangeRequest> _repository;
        private readonly IReadonlyRepository<Problem> _problems;

        public ChangeRequestQueryBuilder(
            IReadonlyRepository<ChangeRequest> repository,
            IReadonlyRepository<Problem> problems)
        {
            _repository = repository;
            _problems = problems;
        }

        public IExecutableQuery<ChangeRequest> Query(ChangeRequestListFilter filterBy)
        {
            var query = _repository
                .With(x => x.Urgency)
                .With(x => x.Influence)
                .With(x => x.Type)
                .With(x => x.Category)
                .With(x => x.Priority)
                .With(x => x.WorkOrderReferences)
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
                query = query.Where(x =>
                    x.EntityStateID != null
                    || x.WorkflowSchemeID != null
                    || x.WorkflowSchemeVersion == null);
            }

            if (filterBy.ProblemID.HasValue)
            {
                // Задан уникальный идентификатор проблемы - ищем связанные с ней Запросы на изменения
                var ids = _problems.WithMany(p => p.ChangeRequests)
                              .SingleOrDefault(x => x.IMObjID == filterBy.ProblemID.Value)
                              ?.ChangeRequests
                              .Select(x => x.Reference.IMObjID)
                              .ToArray()
                          ?? Array.Empty<Guid>();
                query = query.Where(x => ids.Contains(x.IMObjID));
            }

            return query;
        }
    }
}