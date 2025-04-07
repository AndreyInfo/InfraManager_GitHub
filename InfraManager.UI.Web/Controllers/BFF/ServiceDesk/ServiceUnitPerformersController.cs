using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using InfraManager.BLL.Asset;
using System.Threading;
using InfraManager.BLL.ServiceDesk.ServiceUnits;

namespace InfraManager.UI.Web.Controllers.BFF.ServiceDesk
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceUnitPerformersController : ControllerBase
    {
        private readonly IServiceUnitPerformersBLL _serviceUnitBLL;

        public ServiceUnitPerformersController(IServiceUnitPerformersBLL serviceUnitBLL)
        {
            _serviceUnitBLL = serviceUnitBLL;
        }

        [HttpGet]
        public async Task<PerformerDetails[]> GetPerformersByServiceUnitIdAsync([FromQuery] Guid serviceUnitId, CancellationToken cancellationToken)
        {
            return await _serviceUnitBLL.GetPerformersByServiceUnitIdAsync(serviceUnitId, cancellationToken);
        }

        [HttpPost]
        public async Task AddAsync([FromBody] PerformerServiceUnitDetails model, CancellationToken cancellationToken)
        {
            await _serviceUnitBLL.AddPerformersAsync(model, cancellationToken);
        }

        [HttpDelete]
        public async Task DeletePerformersAsync([FromBody] PerformerServiceUnitDetails[] models, CancellationToken cancellationToken)
        {
            await _serviceUnitBLL.DeletePerformersAsync(models, cancellationToken);
        }
    }
}
