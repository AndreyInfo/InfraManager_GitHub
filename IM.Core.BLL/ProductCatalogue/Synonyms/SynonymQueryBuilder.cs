using Inframanager.BLL;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Synonyms;

namespace InfraManager.BLL.ProductCatalogue.Synonyms;

internal class SynonymQueryBuilder : IBuildEntityQuery<Synonym, SynonymOutputDetails, SynonymFilter>,
    ISelfRegisteredService<IBuildEntityQuery<Synonym, SynonymOutputDetails, SynonymFilter>>
{
    private readonly IReadonlyRepository<Synonym> _filterQuery;

    public SynonymQueryBuilder(IReadonlyRepository<Synonym> filterQuery)
    {
        _filterQuery = filterQuery;
    }

    public IExecutableQuery<Synonym> Query(SynonymFilter filter)
    {
        var query = _filterQuery.Query();
        
        if (filter.ModelID.HasValue)
            query = query.Where(x => x.ModelID == filter.ModelID);

        if (filter.ClassID.HasValue)
            query = query.Where(x => x.ClassID == filter.ClassID);
        
        
        if (filter.AdapterProductCatalogTypeID.HasValue)
            query = query.Where(x => x.AdapterType.ProductCatalogTypeID == filter.AdapterProductCatalogTypeID.Value);

        if (filter.PeripheralProductCatalogTypeID.HasValue)
            query = query.Where(x => x.PeripheralType.ProductCatalogTypeID == filter.PeripheralProductCatalogTypeID.Value);

        
        if (filter.WithoutModelProducer != null && filter.WithoutModelName != null)
            query = query.Where(x => x.ModelProducer != filter.WithoutModelProducer
                || x.ModelName != filter.WithoutModelName);
        
        if (!string.IsNullOrEmpty(filter.ModelName))
            query = query.Where(x => x.ModelName.Contains(filter.ModelName));

        if (!string.IsNullOrEmpty(filter.ModelProducer))
            query = query.Where(x => x.ModelProducer.Contains(filter.ModelProducer));


        return query;
    }
}