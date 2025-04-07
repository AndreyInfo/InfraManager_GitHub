using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.LifeCycles;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.ProductCatalog;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LifeCyclesController : ControllerBase
{
    private readonly ILifeCycleBLL _lifeCycleBLL;
    private readonly IEnumBLL<LifeCycleType> _enumBLL;

    public LifeCyclesController(ILifeCycleBLL lifeCycleBLL
        , IEnumBLL<LifeCycleType> enumBLL)
    {
        _lifeCycleBLL = lifeCycleBLL;
        _enumBLL = enumBLL;
    }

    [HttpGet]
    public async Task<LifeCycleDetails[]> GetTable([FromQuery] LifeCycleFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await _lifeCycleBLL.GetDetailsArrayAsync(filter, cancellationToken);
    }


    [HttpGet("{id::guid}")]
    public async Task<LifeCycleDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return await _lifeCycleBLL.DetailsAsync(id, cancellationToken);
    }

    [HttpPost]
    public async Task<LifeCycleDetails> PostAsync([FromBody] LifeCycleData data,
        CancellationToken cancellationToken = default)
    {
        return await _lifeCycleBLL.AddAsync(data, cancellationToken);
    }

    [HttpPut("{id::guid}")]
    public async Task<LifeCycleDetails> PutAsync([FromRoute] Guid id
        , [FromBody] LifeCycleData data
        , CancellationToken cancellationToken = default)
    {
        return await _lifeCycleBLL.UpdateAsync(id, data, cancellationToken);
    }

    [HttpPost("analogy")]
    public async Task<LifeCycleDetails> InsertAsAsync([FromBody] LifeCycleData data,
        CancellationToken cancellationToken = default)
    {
        return await _lifeCycleBLL.InsertAsAsync(data, cancellationToken);
    }


    [HttpDelete("{id::guid}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        await _lifeCycleBLL.DeleteAsync(id, cancellationToken);
    }

    [HttpGet("types")]
    public async Task<LookupItem<LifeCycleType>[]> GetTypesAsync(CancellationToken cancellationToken = default)
    { 
        var result = await _enumBLL.GetAllAsync(cancellationToken);

        return result.OrderBy(c=> c.Name).ToArray();
    }
}
