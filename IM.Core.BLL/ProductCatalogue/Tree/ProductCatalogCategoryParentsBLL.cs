using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue.ProductCatalogCategories;
using InfraManager.DAL.ProductCatalogue.Tree;

namespace InfraManager.BLL.ProductCatalogue.Tree;

public class ProductCatalogCategoryParentBLL:IProductCatalogTreeParentBLL,ISelfRegisteredService<IProductCatalogTreeParentBLL>
{
    private readonly IProductCatalogCategoryParentIDQuery _categoryParentQuery;

    public ProductCatalogCategoryParentBLL(IProductCatalogCategoryParentIDQuery categoryParentQuery)
    {
        _categoryParentQuery = categoryParentQuery;
    }

    private async Task<List<Guid>> RecursiveParentGetAsync(List<Guid> identifiers, Guid id, CancellationToken token)
    {
        while (true)
        {
            var parentID = await _categoryParentQuery.ExecuteAsync(id, token);

            var actualParentID = parentID ?? default; // Guid.Empty;
            
            if (identifiers.Contains(actualParentID))
                throw new InvalidObjectException("В дереве каталогов обнаружен цикл");
            
            identifiers.Add(actualParentID);
            
            if (!parentID.HasValue) return identifiers;
            
            id = parentID.Value;
        }
    }

    public async Task<List<TreeParentsDetails>> GetProductCatalogCategoryParentsData(Guid id, CancellationToken token)
    {
        var identifiers = new List<Guid>();
        var filledIdentifiers = await RecursiveParentGetAsync(identifiers, id, token);

        filledIdentifiers.Reverse();

        int i = 0;

        var result = filledIdentifiers.Select(x => new TreeParentsDetails
        {
            CountFromRoot = i++,
            ID = x
        }).ToList();

        return result;
    }
}