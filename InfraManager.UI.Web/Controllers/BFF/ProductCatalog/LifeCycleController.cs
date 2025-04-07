using InfraManager.BLL.ProductCatalogue.LifeCycles;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.ProductCatalog;

[Authorize]
[ApiController]
[Route("api/[controller]/")]
public class LifeCycleController : ControllerBase
{
    private readonly ILifeCycleCatalogBLL _lifeCycleCatalogBLL;

    public LifeCycleController(ILifeCycleCatalogBLL lifeCycleCatalogBLL)
    {
        _lifeCycleCatalogBLL = lifeCycleCatalogBLL;
    }
    
    [HttpGet("tree")]
    public async Task<LifeCycleTreeNode[]> GetNodes([FromQuery] LifeCycleTreeFilter filter, CancellationToken cancellationToken)
        => await _lifeCycleCatalogBLL.GetNodesAsync(filter, cancellationToken);
}
