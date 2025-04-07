using InfraManager.BLL.AccessManagement.Operations;
using InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations;
using InfraManager.DAL.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Operations;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LifeCycleOperationsController : ControllerBase
{
    private readonly ILifeCycleStateOperationBLL _lifeCycleStateOprationBLL;

    public LifeCycleOperationsController(ILifeCycleStateOperationBLL lifeCycleStateOprationBLL)
    {
        _lifeCycleStateOprationBLL = lifeCycleStateOprationBLL;
    }

    [HttpGet]
    public async Task<GroupedLifeCycleListItem[]> GetAsync([FromQuery] LifeCycleStateOperationFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await _lifeCycleStateOprationBLL.GetOperationsAsync(filter, cancellationToken);
    }

    [HttpPut("{roleID}")]
    public async Task SaveAsync([FromRoute] Guid roleID, LifeCycleOperationsData[] data,
        CancellationToken cancellationToken = default)
    {
        await _lifeCycleStateOprationBLL.SaveOperationsAsync(roleID, data, cancellationToken);
    }
}