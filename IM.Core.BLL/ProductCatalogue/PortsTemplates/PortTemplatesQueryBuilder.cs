using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.PortTemplates;
using InfraManager.DAL;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.PortsTemplates;

public class PortTemplatesQueryBuilder:IBuildEntityQuery<PortTemplate,PortTemplatesDetails, PortTemplatesFilter>,
    ISelfRegisteredService<IBuildEntityQuery<PortTemplate,PortTemplatesDetails, PortTemplatesFilter>>
{
    private readonly IRepository<PortTemplate> _repository;

    public PortTemplatesQueryBuilder(IRepository<PortTemplate> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<PortTemplate> Query(PortTemplatesFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrEmpty(filterBy.SearchString))
            query = query.Where(x => x.PortNumber.ToString().Contains(filterBy.SearchString));

        if (filterBy.PortNumber.HasValue)
            query = query.Where(x => x.PortNumber == filterBy.PortNumber);

        if (filterBy.ClassID.HasValue)
            query = query.Where(x => x.ClassID == filterBy.ClassID);

        if (filterBy.ObjectID.HasValue)
            query = query.Where(x => x.ObjectID == filterBy.ObjectID);

        if (filterBy.TechnologyID.HasValue)
            query = query.Where(x => x.TechnologyID == filterBy.TechnologyID);

        if (filterBy.TechnologyName != null)
            query = query.Where(x => x.TechnologyType.Name.Contains(filterBy.TechnologyName));
        
        if (filterBy.JackTypeID.HasValue)
            query = query.Where(x => x.JackTypeID == filterBy.JackTypeID);

        if (filterBy.JackTypeName != null)
            query = query.Where(x => x.JackType.Name.Contains(filterBy.JackTypeName));

        return query;
    }
}