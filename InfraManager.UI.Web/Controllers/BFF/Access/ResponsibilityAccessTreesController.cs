using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.Location;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ServiceCatalogue.SLA;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;
using InfraManager.DAL.ProductCatalogue.Tree;

namespace InfraManager.UI.Web.Controllers.BFF.Access;

[Authorize]
[ApiController]
[Route("bff/[controller]")]
public class ResponsibilityAccessTreesController : ControllerBase
{
    private readonly IResponsibilityAccessTreeBLL _responsibilityAccessTreeBLL;

    public ResponsibilityAccessTreesController(IResponsibilityAccessTreeBLL responsibilityAccessTreeBLL)
    {
        _responsibilityAccessTreeBLL = responsibilityAccessTreeBLL;
    }

    [HttpGet("location")]
    public async Task<LocationTreeNodeDetails[]> GetTreeLocationTTZAsync([FromQuery] LocationTreeFilter model
                                                                      , [FromQuery] AccessFilter accessFilter
                                                                      , [FromQuery] AccessTypes accessType
                                                                      , CancellationToken cancellationToken)
        => await _responsibilityAccessTreeBLL.GetLocationTreeAsync(model, accessFilter, accessType, cancellationToken);


    [HttpGet("orgsturcture/TOZ")]
    public async Task<OrganizationStructureNodeModelDetails[]> GetTreeLocationTTZAsync([FromQuery] OrganizationStructureNodeRequestModelDetails model
                                                                      , [FromQuery] AccessFilter accessFilter
                                                                      , CancellationToken cancellationToken)
        => await _responsibilityAccessTreeBLL.GetOrgstructureAsync(model, accessFilter, cancellationToken);
    


    [HttpGet("portfolioservice/ServiceCatalogue")]
    public async Task<PortfolioServicesItem[]> GetServiceCatalogueAsync([FromQuery] PortfolioServiceFilter model
                                                                      , [FromQuery] AccessFilter accessFilter
                                                                      , CancellationToken cancellationToken)
        => await _responsibilityAccessTreeBLL.GetTreePortfolioServiceAsync(model, accessFilter, cancellationToken);
    
    
    [HttpGet("ProductCatalog/DevaiceCatalogue")]
    public async Task<ProductCatalogNode[]> GetServiceCatalogueAsync([FromQuery] ProductCatalogTreeFilter model
                                                                      , [FromQuery] AccessFilter accessFilter
                                                                      , CancellationToken cancellationToken)
        => await _responsibilityAccessTreeBLL.GetTreeProductCatalogAsync(model, accessFilter, cancellationToken);
}
