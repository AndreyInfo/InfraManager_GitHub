using InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue.LifeCycles;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LifeCycleStatesController : ControllerBase
{
    private readonly ILifeCycleStateBLL _lifeCycleStateBLL;

    public LifeCycleStatesController(ILifeCycleStateBLL lifeCycleStateBLL)
    {
        _lifeCycleStateBLL = lifeCycleStateBLL;
    }

    [HttpGet]
    public async Task<LifeCycleStateDetails[]> GetListAsync([FromQuery] LifeCycleStateFilter filter, CancellationToken cancellationToken = default)
        => await _lifeCycleStateBLL.GetByLifeCycleIDAsync(filter, cancellationToken);

    [HttpGet("{id::guid}")]
    public async Task<LifeCycleStateDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        => await _lifeCycleStateBLL.DetailsAsync(id, cancellationToken);


    [HttpPost]
    public async Task<LifeCycleStateDetails> PostAsync([FromBody] LifeCycleStateData data, CancellationToken cancellationToken = default)
        => await _lifeCycleStateBLL.AddAsync(data, cancellationToken);

    [HttpPut("{id::guid}")]
    public async Task<LifeCycleStateDetails> PostAsync([FromRoute] Guid id
        , [FromBody] LifeCycleStateData data
        , CancellationToken cancellationToken = default)
        => await _lifeCycleStateBLL.UpdateAsync(id, data, cancellationToken);


    [HttpDelete("{id::guid}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        => await _lifeCycleStateBLL.DeleteAsync(id, cancellationToken);

}
