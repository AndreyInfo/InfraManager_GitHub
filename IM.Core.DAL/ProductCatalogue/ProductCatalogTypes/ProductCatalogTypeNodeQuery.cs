using InfraManager.DAL.ProductCatalogue.Tree;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogTypes;

internal sealed class ProductCatalogTypeNodeQuery :
    IProductCatalogTypeNodeQuery
    , ISelfRegisteredService<IProductCatalogTypeNodeQuery>
{
    private readonly DbSet<ProductCatalogType> _productCatalogTypes;
    public ProductCatalogTypeNodeQuery(DbSet<ProductCatalogType> productCatalogTypes)
    {
        _productCatalogTypes = productCatalogTypes;
    }

    public async Task<ProductCatalogNode[]> ExecuteAsync(ProductCatalogTreeFilter filter, CancellationToken cancellationToken)
    {
        var query = _productCatalogTypes.AsNoTracking();

        if (filter.HasLifeCycle.HasValue)
            query = query.Where(c => c.LifeCycleID.HasValue == filter.HasLifeCycle.Value);

        if (filter.AvailableTemplateClassID.HasValue)
            query = query.Where(x => x.ProductCatalogTemplate.ClassID == filter.AvailableTemplateClassID);

        if (filter.AvailableTemplateID.HasValue)
            query = query.Where(x => x.ProductCatalogTemplateID == filter.AvailableTemplateID);

        if (filter.AvailableTemplateClassArray?.Any() ?? false)
            query = query.Where(x => filter.AvailableTemplateClassArray.Contains(x.ProductCatalogTemplate.ClassID));

        if (filter.ParentID.HasValue)
            query = query.Where(x => x.ProductCatalogCategoryID == filter.ParentID);

        return await query.Select(x => new ProductCatalogNode()
        {
            ID = x.IMObjID,
            Name = x.Name,
            IconName = x.IconName,
            ClassID = ObjectClass.ProductCatalogType,
            TemplateID = x.ProductCatalogTemplateID,
            TemplateClassID = x.ProductCatalogTemplate.ClassID,
            ParentID = x.ProductCatalogCategoryID,
            CanContainsSubNodes = false,
            Location = DbFunctions.GetCategoryFullName(x.ProductCatalogCategoryID) + "\\" + x.Name,
        }).OrderBy(c=> c.Name).ToArrayAsync(cancellationToken);

    }
}