using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.Software;
using System;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Software.SoftwareModels;

namespace InfraManager.UI.Web.Controllers.Api.Software.ModelCatalog;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SoftwareModelsController : ControllerBase
{
    private readonly ISoftwareModelBLL _softwareModelBLL;

    public SoftwareModelsController(
        ISoftwareModelBLL softwareModelBLL)
    {
        _softwareModelBLL = softwareModelBLL;
    }

    [HttpGet]
    public async Task<SoftwareModelListItemDetails[]> GetListAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken)
    {
        return await _softwareModelBLL.GetListAsync(filter, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<SoftwareModelDetailsBase> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _softwareModelBLL.GetAsync(id, cancellationToken);
    }

    [HttpPost]
    public async Task<SoftwareModelDetailsBase> AddAsync([FromBody] SoftwareModelData model, CancellationToken cancellationToken)
    {
        return await _softwareModelBLL.AddAsync(model, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<SoftwareModelDetailsBase> UpdateAsync([FromRoute] Guid id, [FromBody] SoftwareModelData model, CancellationToken cancellationToken)
    {
        return await _softwareModelBLL.UpdateAsync(id, model, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _softwareModelBLL.DeleteAsync(id, cancellationToken);
    }
}
