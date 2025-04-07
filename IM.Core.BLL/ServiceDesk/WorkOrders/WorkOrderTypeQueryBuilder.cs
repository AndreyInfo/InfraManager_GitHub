using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class WorkOrderTypeQueryBuilder :
        IBuildEntityQuery<WorkOrderType, WorkOrderTypeDetails, LookupListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<WorkOrderType, WorkOrderTypeDetails, LookupListFilter>>
    {
        private readonly IReadonlyRepository<WorkOrderType> _repository;

        public WorkOrderTypeQueryBuilder(IReadonlyRepository<WorkOrderType> repository)
        {
            _repository = repository;
        }

        public IExecutableQuery<WorkOrderType> Query(LookupListFilter filterBy)
        {
            var query = _repository.Query();

            if (!string.IsNullOrWhiteSpace(filterBy.SearchName))
            {
                query = query.Where(x => x.Name.Contains(filterBy.SearchName));
            }

            return query; // TODO: Устранить копипасту (ааналогичный код есть в WorkOrderTemplateQueryBuilder
        }
    }
}
