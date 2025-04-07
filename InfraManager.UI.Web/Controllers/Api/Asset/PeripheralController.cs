using InfraManager.BLL.Asset;
using InfraManager.BLL.Asset.Peripherals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.Asset;

namespace InfraManager.UI.Web.Controllers.Api.Asset;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PeripheralController : ControllerBase
{
    private readonly IPeripheralBLL _peripheral;
    private readonly IEquipmentBaseBLL<Guid, Peripheral, PeripheralData, PeripheralDetails> _equipmentBaseBLL;

    public PeripheralController(IPeripheralBLL peripheral
        , IEquipmentBaseBLL<Guid, Peripheral, PeripheralData, PeripheralDetails> equipmentBaseBLL)
    {
        _peripheral = peripheral;
        _equipmentBaseBLL = equipmentBaseBLL;
    }

    [HttpGet("{id}")]
    public async Task<PeripheralDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => await _equipmentBaseBLL.DetailsAsync(id, cancellationToken);

    [HttpPost]
    public async Task<PeripheralDetails> AddAsync([FromBody] PeripheralData data, CancellationToken cancellationToken)
       => await _equipmentBaseBLL.AddAsync(data, cancellationToken);

    [HttpPut("{id}")]
    public async Task<PeripheralDetails> UpdateAsync([FromRoute] Guid id, PeripheralData data, CancellationToken cancellationToken)
        => await _equipmentBaseBLL.UpdateAsync(id, data, cancellationToken);

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        => await _equipmentBaseBLL.DeleteAsync(id, cancellationToken);

    [HttpGet("peripherals/{networkDeviceID}")]
    public async Task<PeripheralListItemDetails[]> GetPeripheralsForNetworkDeviceAsync([FromRoute] int networkDeviceID, [FromQuery] BaseFilter filter, CancellationToken cancellationToken)
        => await _peripheral.GetPeripheralsForNetworkDeviceAsync(networkDeviceID, filter, cancellationToken);
}