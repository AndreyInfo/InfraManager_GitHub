using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue.Classes;
using InfraManager.BLL.ProductCatalogue.ProductTemplates;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue;

[Authorize]
[ApiController]
[Route("api/ProductCatalog/Templates")]
public class ProductCatalogTemplatesController : ControllerBase
{
    private readonly IProductCatalogTemplateBLL _productCatalogTemplateBLL;

    public ProductCatalogTemplatesController(IProductCatalogTemplateBLL productCatalogTemplateBLL)
    {
        _productCatalogTemplateBLL = productCatalogTemplateBLL;
    }

    [HttpGet("{id}")]
    public Task<ProductTemplateInfo> GetNodesAsync([FromRoute] ProductTemplate id,
       CancellationToken cancellationToken)
       => _productCatalogTemplateBLL.GetByID(id, cancellationToken);

    [HttpGet]
    public Task<ProductTemplateInfo[]> GetNodesAsync([FromQuery] ProductTemplateTreeFilter filter,
        CancellationToken cancellationToken)
        => _productCatalogTemplateBLL.GetNodesAsync(filter, cancellationToken);
}
