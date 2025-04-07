using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

internal class WorkOrderTemplateQueryBuilder : 
    IBuildEntityQuery<WorkOrderTemplate, WorkOrderTemplateDetails, WorkOrderTemplateLookupListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<WorkOrderTemplate, WorkOrderTemplateDetails, WorkOrderTemplateLookupListFilter>>
{
    private readonly IReadonlyRepository<WorkOrderTemplate> _repository;

    public WorkOrderTemplateQueryBuilder(IReadonlyRepository<WorkOrderTemplate> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<WorkOrderTemplate> Query(WorkOrderTemplateLookupListFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(filterBy.SearchName))
        {
            query = query.Where(x => x.Name.Contains(filterBy.SearchName));
        } // TODO: Сделать метод расширения

        if (filterBy.WorkOrderTypeID.HasValue)
        {
            query = query.Where(x => x.WorkOrderTypeID.Equals(filterBy.WorkOrderTypeID.Value));
        }

        return query;
    }
}
