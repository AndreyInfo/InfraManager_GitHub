using InfraManager.BLL.Asset.ConnectorTypes;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Asset;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ConnectorTypesController : ControllerBase
{
    private readonly IConnectorTypeBLL _connectorTypeBLL;

    public ConnectorTypesController(IConnectorTypeBLL connectorTypeBLL)
    {
        _connectorTypeBLL = connectorTypeBLL;
    }

    [HttpGet]
    public async Task<ConnectorTypeDetails[]> GetDetailsAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken = default)
        => await _connectorTypeBLL.GetListAsync(filter, cancellationToken);

    [HttpGet("{id}")]
    public async Task<ConnectorTypeDetails> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => await _connectorTypeBLL.DetailsAsync(id, cancellationToken);

    [HttpPost]
    public async Task<ConnectorTypeDetails> AddAsync([FromBody] ConnectorTypeData data, CancellationToken cancellationToken = default)
        => await _connectorTypeBLL.AddAsync(data, cancellationToken);

    [HttpPut("{id}")]
    public async Task<ConnectorTypeDetails> UpdateAsync([FromRoute] int id
        , [FromBody] ConnectorTypeData data
        , CancellationToken cancellationToken = default)
        => await _connectorTypeBLL.UpdateAsync(id, data, cancellationToken);

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => await _connectorTypeBLL.DeleteAsync(id, cancellationToken);
}
