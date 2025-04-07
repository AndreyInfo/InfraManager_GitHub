using Inframanager.BLL;
using InfraManager.BLL.Snmp;
using InfraManager.DAL.Snmp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Snmp
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SnmpDeviceModelsController : ControllerBase
    {
        private readonly ISnmpDeviceModelBLL _snmpDeviceModelBLL;

        public SnmpDeviceModelsController(ISnmpDeviceModelBLL snmpDeviceModelBLL)
        {
            _snmpDeviceModelBLL = snmpDeviceModelBLL;
        }

        [HttpGet("{id}")]
        public async Task<SnmpDeviceModelDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default) =>
            await _snmpDeviceModelBLL.DetailsAsync(id, cancellationToken);

        [HttpPost]
        public async Task<SnmpDeviceModelDetails> PostAsync([FromBody] SnmpDeviceModelData snmpDeviceModel, CancellationToken cancellationToken = default)
            => await _snmpDeviceModelBLL.AddAsync(snmpDeviceModel, cancellationToken);

        [HttpPut("{id}")]
        public async Task PutAsync([FromRoute] Guid id, [FromBody] SnmpDeviceModelData snmpDeviceModel, CancellationToken cancellationToken = default) =>
            await _snmpDeviceModelBLL.UpdateAsync(id, snmpDeviceModel, cancellationToken);

        [HttpDelete("{id}")]
        public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default) =>
            await _snmpDeviceModelBLL.DeleteAsync(id, cancellationToken);

        [HttpGet("list")]
        public async Task<SnmpDeviceModelDetails[]> PageAsync([FromQuery] SnmpDeviceModelFilter filterBy, [FromQuery] ClientPageFilter<SnmpDeviceModel> pageBy, CancellationToken cancellationToken = default) =>
            await _snmpDeviceModelBLL.GetDetailsPageAsync(filterBy, pageBy,cancellationToken);
    }
}
