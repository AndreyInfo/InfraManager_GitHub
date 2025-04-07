using InfraManager.BLL.Asset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Asset
{
    [Authorize]
    [ApiController]
    [Route("api/terminalDevices")]
    public class TerminalDevicesController : ControllerBase
    {
        private readonly ITerminalDeviceBLL _terminalDevices;

        public TerminalDevicesController(ITerminalDeviceBLL terminalDevices)
        {
            _terminalDevices = terminalDevices;
        }

        [HttpGet("{id}")]
        public async Task<TerminalDeviceDetails> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            return await _terminalDevices.DetailsAsync(id, cancellationToken);
        }
    }
}
