using InfraManager.BLL.Asset;
using InfraManager.BLL.Asset.Adapters;
using InfraManager.DAL.Asset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using AdapterDetails = InfraManager.BLL.Asset.Adapters.AdapterDetails;

namespace InfraManager.UI.Web.Controllers.Api.Asset;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AdaptersController : ControllerBase
{
    private readonly IAdapterBLL _adapters;
    private readonly IEquipmentBaseBLL<Guid, Adapter, AdapterData, AdapterDetails> _equipmentBaseBLL;

    public AdaptersController(IAdapterBLL adapters
        , IEquipmentBaseBLL<Guid, Adapter, AdapterData, AdapterDetails> equipmentBaseBLL)
    {
        _adapters = adapters;
        _equipmentBaseBLL = equipmentBaseBLL;
    }

    [HttpGet("{id}")]
    public async Task<AdapterDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => await _equipmentBaseBLL.DetailsAsync(id, cancellationToken);

    [HttpPost]
    public async Task<AdapterDetails> AddAsync([FromBody] AdapterData data, CancellationToken cancellationToken)
       => await _equipmentBaseBLL.AddAsync(data, cancellationToken);

    [HttpPut("{id}")]
    public async Task<AdapterDetails> UpdateAsync([FromRoute] Guid id, AdapterData data, CancellationToken cancellationToken)
        => await _equipmentBaseBLL.UpdateAsync(id, data, cancellationToken);

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        => await _equipmentBaseBLL.DeleteAsync(id, cancellationToken);

    [HttpGet("adapters/{networkDeviceID}")]
    public async Task<AdapterListItemDetails[]> GetAdaptersForNetworkDeviceAsync([FromRoute] int networkDeviceID, [FromQuery] BaseFilter filter, CancellationToken cancellationToken)
        => await _adapters.GetAdaptersForNetworkDeviceAsync(networkDeviceID, filter, cancellationToken);
}