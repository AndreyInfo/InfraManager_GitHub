using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.WorkOrders.Priorities;

internal class WorkOrderPriorityQueryBuilder :
        IBuildEntityQuery<WorkOrderPriority, WorkOrderPriorityDetails, LookupListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<WorkOrderPriority, WorkOrderPriorityDetails, LookupListFilter>>
{
    private readonly IReadonlyRepository<WorkOrderPriority> _repository;

    public WorkOrderPriorityQueryBuilder(IReadonlyRepository<WorkOrderPriority> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<WorkOrderPriority> Query(LookupListFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(filterBy.SearchName))
        {
            query = query.Where(x => x.Name.Contains(filterBy.SearchName));
        }

        return query;
    }
}
