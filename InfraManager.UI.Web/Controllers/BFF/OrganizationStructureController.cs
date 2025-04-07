using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.OrganizationStructure;

namespace InfraManager.UI.Web.Controllers.BFF;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrganizationStructureController : ControllerBase
{
    private readonly IOrganizationStructureBLL _organizationStructureBLL;

    public OrganizationStructureController(IOrganizationStructureBLL organizationStructureBLL)
    {
        _organizationStructureBLL = organizationStructureBLL;
    }

    [HttpPost("tree/node")]
    public async Task<OrganizationStructureNodeModelDetails[]> GetByNodeAsync([FromBody] OrganizationStructureNodeRequestModelDetails nodeRequest, CancellationToken cancellationToken)
    {
        return await _organizationStructureBLL.GetNodesAsync(nodeRequest, cancellationToken);
    }

    [HttpGet("tree/pathToNode")]
    public async Task<OrganizationStructureNodeModelDetails[]> GetPathToNode(
        [FromQuery] OrganizationStructureNodeRequestModelDetails nodeRequest, CancellationToken cancellationToken) =>
        
        await _organizationStructureBLL.GetPathToNodeAsync(nodeRequest, cancellationToken);
}
