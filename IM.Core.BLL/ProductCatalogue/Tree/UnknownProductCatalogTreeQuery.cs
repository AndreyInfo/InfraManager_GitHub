using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Localization;
using InfraManager.DAL.ProductCatalogue.Tree;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ProductCatalogue.Tree;

public class UnknownProductCatalogTreeQuery : IProductCatalogTreeQuery, ISelfRegisteredService<IProductCatalogTreeQuery>
{

    private readonly ILocalizeText _localizeText;
    public UnknownProductCatalogTreeQuery(ILocalizeText localizeText)
    {
        _localizeText = localizeText;
    }

    //TODO refactor tree
    private async Task<ProductCatalogNode[]> GetNodeAsync(CancellationToken cancellationToken)
    {
        return new ProductCatalogNode[] {
            new()
            {
                Name = await _localizeText.LocalizeAsync(nameof(Resources.ProductCatalogueTreeCaption), cancellationToken),
                ClassID = ObjectClass.ProductCatalogue,
                ID = Guid.Empty,
            }
        };
    }

    public Task<ProductCatalogNode[]> GetTreeNodesByParentIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return GetNodeAsync(cancellationToken);
    }

    public Task<ProductCatalogNode[]> GetTreeNodesAsync(ProductCatalogTreeFilter filter,
        IEnumerable<ObjectClass> objectClasses, CancellationToken cancellationToken = default)
    {
        return GetNodeAsync(cancellationToken);
    }

    public Task<TreeParentsDetails[]> GetTreeParentsDetailsAsync(Guid id, ObjectClass classID,
        CancellationToken token)
    {
        return Task.FromResult(Array.Empty<TreeParentsDetails>());
    }
}
