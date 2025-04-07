using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL;
using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.Calendar.Exclusions;

namespace InfraManager.UI.Web.Controllers.Api.Calendar;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ExclusionsController : ControllerBase
{
    private readonly IExclusionBLL _exclusionBLL;

    public ExclusionsController(IExclusionBLL exclusionBLL)
    {
        _exclusionBLL = exclusionBLL;
    }

    [HttpGet("{id}")]
    public async Task<ExclusionDetails> GetByIDAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _exclusionBLL.GetByIDAsync(id, cancellationToken);
    }

    [HttpGet]
    public async Task<ExclusionDetails[]> GetDataTableAsync([FromQuery] ExclusionFilter filter, CancellationToken cancellationToken)
    {
        return await _exclusionBLL.GetByFilterAsync(filter, cancellationToken);
    }

    [HttpPost]
    public async Task<Guid> CreateExclutionAsync([FromBody] ExclusionDetails model, CancellationToken cancellationToken)
    {
        return await _exclusionBLL.AddAsync(model, cancellationToken);
    }

    [HttpPut]
    public async Task<Guid> UpdateExclusionAsync([FromBody] ExclusionDetails model, CancellationToken cancellationToken)
    {
        return await _exclusionBLL.UpdateAsync(model, cancellationToken);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromBody] DeleteModel<Guid>[] models, CancellationToken cancellationToken)
    {
        await _exclusionBLL.DeleteAsync(models, cancellationToken);
    }
}
