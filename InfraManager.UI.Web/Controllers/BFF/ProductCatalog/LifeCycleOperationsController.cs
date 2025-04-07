using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.ProductCatalog;
[Route("bff/[controller]")]
[ApiController]
[Authorize]
public class LifeCycleOperationsController : ControllerBase
{
    private readonly IEnumBLL<LifeCycleOperationCommandType> _enumBLL;
    private readonly ILifeCycleStateOperationBLL _lifeCycleStateOprationBLL;

    public LifeCycleOperationsController(ILifeCycleStateOperationBLL lifeCycleStateOprationBLL
        , IEnumBLL<LifeCycleOperationCommandType> enumBLL)
    {
        _lifeCycleStateOprationBLL = lifeCycleStateOprationBLL;
        _enumBLL = enumBLL;
    }

    [HttpGet("{id::guid}")]
    public async Task<LifeCycleStateOperationDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        => await _lifeCycleStateOprationBLL.DetailsAsync(id, cancellationToken);

    [HttpGet]
    public async Task<LifeCycleStateOperationDetails[]> GetListAsync([FromQuery] LifeCycleStateOperationFilter filter
        , CancellationToken cancellationToken = default)
        => await _lifeCycleStateOprationBLL.GetDetailsArrayAsync(filter, cancellationToken);

    [HttpPost]
    public async Task<LifeCycleStateOperationDetails> PostAsync([FromBody] LifeCycleStateOperationData data, CancellationToken cancellationToken = default)
        => await _lifeCycleStateOprationBLL.AddAsync(data, cancellationToken);

    [HttpPut("{id::guid}")]
    public async Task<LifeCycleStateOperationDetails> PutAsync([FromRoute] Guid id
        , [FromBody] LifeCycleStateOperationData data
        , CancellationToken cancellationToken = default)
        => await _lifeCycleStateOprationBLL.UpdateAsync(id, data, cancellationToken);

    [HttpDelete("{id::guid}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        => await _lifeCycleStateOprationBLL.DeleteAsync(id, cancellationToken);

    [HttpGet("types")]
    public async Task<LookupItem<LifeCycleOperationCommandType>[]> GetTypesAsync(CancellationToken cancellationToken = default)
     => await _enumBLL.GetAllAsync(cancellationToken);
}
