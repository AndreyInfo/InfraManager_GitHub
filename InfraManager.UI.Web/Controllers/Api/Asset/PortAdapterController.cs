using InfraManager.BLL.Asset.PortAdapter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Asset;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PortAdapterController : ControllerBase
{
    private readonly IPortAdapterBLL _portAdapterBLL;

    public PortAdapterController(IPortAdapterBLL portAdapterBLL)
    {
        _portAdapterBLL = portAdapterBLL;
    }

    [HttpGet("{id}")]
    public async Task<PortAdapterDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return await _portAdapterBLL.DetailsAsync(id, cancellationToken);
    }

    [HttpGet]
    public async Task<PortAdapterDetails[]> GetDetailsAsync([FromQuery] PortAdapterFilter filter, CancellationToken cancellationToken = default)
        => await _portAdapterBLL.GetListAsync(filter, cancellationToken);

    [HttpPost]
    public async Task<PortAdapterDetails> AddAsync([FromBody] PortAdapterData data, CancellationToken cancellationToken)
        => await _portAdapterBLL.AddAsync(data, cancellationToken);

    [HttpPut("{id}")]
    public async Task<PortAdapterDetails> UpdateAsync([FromRoute] Guid id, 
        [FromBody] PortAdapterData data, CancellationToken cancellationToken)
    {
        return await _portAdapterBLL.UpdateAsync(id, data, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _portAdapterBLL.DeleteAsync(id, cancellationToken);
    }
}