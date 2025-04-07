using InfraManager.BLL.Technologies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Settings.Configuration;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TechnologyTypeCompatibilitiesController : ControllerBase
{
    private readonly ITechnologyTypeCompatibilityBLL _technologyTypeCompatibilities;

    public TechnologyTypeCompatibilitiesController(ITechnologyTypeCompatibilityBLL technologyTypeCompatibilities)
    {
        _technologyTypeCompatibilities = technologyTypeCompatibilities;
    }

    [HttpGet]
    public async Task<TechnologyTypeDetails[]> GetListCompatibilityTechTypeByFromIDAsync([FromQuery] TechnologyTypesFilter filter, CancellationToken cancellationToken = default)
        => await _technologyTypeCompatibilities.GetListCompatibilityTechTypeByIDAsync(filter, cancellationToken);


    [HttpPost]
    public async Task AddListCompatibilityTechTypeByAsync(int fromID, [FromBody] int[] toID, CancellationToken cancellationToken = default)
        => await _technologyTypeCompatibilities.SaveAsync(fromID, toID, cancellationToken);


    [HttpDelete]
    public async Task RemoveCompatibilityTechTypeAsync(int fromID, [FromBody] int[] ids, CancellationToken cancellationToken = default)
        => await _technologyTypeCompatibilities.RemoveAsync(fromID, ids, cancellationToken);


    [HttpGet("not")]
    public async Task<TechnologyTypeDetails[]> GetListNotCompatibilityTechTypeByFromIDAsync([FromQuery] TechnologyTypesFilter filter, CancellationToken cancellationToken = default)
        => await _technologyTypeCompatibilities.GetListNotCompatibilityTechTypeByIDAsync(filter, cancellationToken);
}
