using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.Tree;
internal sealed class ProductCatalogNodeQuery : IProductCatalogNodeQuery
    , ISelfRegisteredService<IProductCatalogNodeQuery>
{
    private readonly DbSet<ProductCatalogCategory> _productCatalogCategories;

    public ProductCatalogNodeQuery(DbSet<ProductCatalogCategory> productCatalogCategories)
    {
        _productCatalogCategories = productCatalogCategories;
    }

    public async Task<ProductCatalogNode[]> ExecuteAsync(ProductCatalogTreeFilter filter, CancellationToken cancellationToken)
    {
        var query = QuerySubCategoriesAsync(filter);

        return await query.OrderBy(c=> c.Name).ToArrayAsync(cancellationToken);
    }

    private IQueryable<ProductCatalogNode> QuerySubCategoriesAsync(ProductCatalogTreeFilter filter)
    {
        var query = _productCatalogCategories.AsNoTracking().Where(c => c.ParentProductCatalogCategoryID == filter.ParentID);

        return query.Select(x => new ProductCatalogNode()
        {
            ID = x.ID,
            Name = x.Name,
            ClassID = ObjectClass.ProductCatalogCategory,
            IconName = x.IconName,
            Location = DbFunctions.GetCategoryFullName(x.ParentProductCatalogCategoryID.Value) + "\\" + x.Name,
            ParentID = x.ParentProductCatalogCategoryID,
            CanContainsSubNodes = x.SubCategories.Any() || x.ProductCatalogTypes.Any(),
        });
    }
}
