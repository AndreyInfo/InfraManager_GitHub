using InfraManager.BLL.ProductCatalogue.Tree;
using InfraManager.DAL.ProductCatalogue.Tree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue;

[Authorize]
[ApiController]
[Route("bff/ProductCatalog")]
public class BffProductCatalogTreeNodesController : ControllerBase
{
    private readonly IProductCatalogTree _productCatalogTree;
    
    public BffProductCatalogTreeNodesController(IProductCatalogTree productCatalogTree)
    {
        _productCatalogTree = productCatalogTree;
    }

    [HttpGet]
    public async Task<ProductCatalogNode[]> GetTreeNodeAsync(
        [FromQuery] ProductCatalogTreeFilter filter, 
        CancellationToken cancellationToken = default)
        => await _productCatalogTree.ExecuteAsync(filter, cancellationToken);
}
