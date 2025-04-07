using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk.WorkOrders.Priorities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Priorities
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WorkOrderPriorityController : ControllerBase
    {
        private readonly IWorkOrderPriorityBLL _workOrderPriorityBLL;

        public WorkOrderPriorityController(IWorkOrderPriorityBLL workOrderPriorityBLL)
        {
            _workOrderPriorityBLL = workOrderPriorityBLL;
        }

        [HttpGet]
        public async Task<WorkOrderPriorityDetails[]> DetailsArrayAsync(
           [FromQuery] LookupListFilter filter,
           CancellationToken cancellationToken = default)
        {
            return await _workOrderPriorityBLL.GetDetailsArrayAsync(filter, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            await _workOrderPriorityBLL.DeleteAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<WorkOrderPriorityDetails> AddAsync([FromBody] WorkOrderPriorityData data, CancellationToken cancellationToken = default)
        {
            return await _workOrderPriorityBLL.AddAsync(data, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<WorkOrderPriorityDetails> UpdateAsync([FromRoute] Guid id, [FromBody] WorkOrderPriorityData data, CancellationToken cancellationToken = default)
        {
            return await _workOrderPriorityBLL.UpdateAsync(id, data, cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<WorkOrderPriorityDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            return await _workOrderPriorityBLL.DetailsAsync(id, cancellationToken);
        }
    }
}
