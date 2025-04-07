using InfraManager.BLL.Asset;
using InfraManager.BLL.Asset.NetworkDevices;
using InfraManager.DAL.Asset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Asset;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NetworkDevicesController : ControllerBase
{
    private readonly IEquipmentBaseBLL<int, NetworkDevice, NetworkDeviceData, NetworkDeviceDetails> _equipmentBaseBLL;
    
    public NetworkDevicesController(
        IEquipmentBaseBLL<int, NetworkDevice, NetworkDeviceData, NetworkDeviceDetails> equipmentBaseBLL)
    {
        _equipmentBaseBLL = equipmentBaseBLL;
    }

    [HttpGet("{id}")]
    public async Task<NetworkDeviceDetails> GetAsync(int id, CancellationToken cancellationToken = default)
        => await _equipmentBaseBLL.DetailsAsync(id, cancellationToken);

    [HttpPost]
    public async Task<NetworkDeviceDetails> AddAsync([FromBody] NetworkDeviceData data, CancellationToken cancellationToken)
        => await _equipmentBaseBLL.AddAsync(data, cancellationToken);

    [HttpPut("{id}")]
    public async Task<NetworkDeviceDetails> UpdateAsync([FromRoute] int id, NetworkDeviceData data, CancellationToken cancellationToken)
        => await _equipmentBaseBLL.UpdateAsync(id, data, cancellationToken);

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
        => await _equipmentBaseBLL.DeleteAsync(id, cancellationToken);
}
