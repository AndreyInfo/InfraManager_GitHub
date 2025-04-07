using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations.Transitions;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.ProductCatalog;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LifeCycleStateOperationTransitionsController : ControllerBase
{
    private readonly ILifeCycleStateOperationTransitionBLL _lifeCycleStateOperationTransitionBLL;
    private readonly IEnumBLL<LifeCycleTransitionMode> _enumBLL;

    public LifeCycleStateOperationTransitionsController(ILifeCycleStateOperationTransitionBLL lifeCycleStateOperationTransitionBLL
        , IEnumBLL<LifeCycleTransitionMode> enumBLL)
    {
        _lifeCycleStateOperationTransitionBLL = lifeCycleStateOperationTransitionBLL;
        _enumBLL = enumBLL;
    }

    [HttpGet]
    public async Task<LifeCycleStateOperationTransitionDetails[]> GetTable([FromQuery] LifeCycleStateOperationTransitionFilter filter, CancellationToken cancellationToken)
           => await _lifeCycleStateOperationTransitionBLL.GetDetailsArrayAsync(filter, cancellationToken);


    [HttpGet("{id::guid}")]
    public async Task<LifeCycleStateOperationTransitionDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        => await _lifeCycleStateOperationTransitionBLL.DetailsAsync(id, cancellationToken);

    [HttpPost]
    public async Task<LifeCycleStateOperationTransitionDetails> PostAsync([FromBody] LifeCycleStateOperationTransitionData data, CancellationToken cancellationToken)
        => await _lifeCycleStateOperationTransitionBLL.AddAsync(data, cancellationToken);

    [HttpPut("{id::guid}")]
    public async Task<LifeCycleStateOperationTransitionDetails> PutAsync([FromRoute] Guid id
        , [FromBody] LifeCycleStateOperationTransitionData data
        , CancellationToken cancellationToken)
        => await _lifeCycleStateOperationTransitionBLL.UpdateAsync(id, data, cancellationToken);

    [HttpDelete("{id::guid}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        => await _lifeCycleStateOperationTransitionBLL.DeleteAsync(id, cancellationToken);

    [HttpGet("modes")]
    public async Task<LookupItem<LifeCycleTransitionMode>[]> GetTypesAsync(CancellationToken cancellationToken = default)
        => await _enumBLL.GetAllAsync(cancellationToken);
}
