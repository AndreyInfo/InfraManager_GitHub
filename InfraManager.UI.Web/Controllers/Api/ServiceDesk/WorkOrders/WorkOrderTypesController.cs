using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using System.Threading;
using Inframanager.BLL;
using InfraManager.BLL;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.WorkOrders
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WorkOrderTypesController : ControllerBase
    {
        private readonly IWorkOrderTypeBLL _workOrderTypeBLL;
        private readonly IEnumBLL<WorkOrderTypeClass> _enumBLL;

        public WorkOrderTypesController(IWorkOrderTypeBLL workOrderTypeBLL,
            IEnumBLL<WorkOrderTypeClass> enumBLL)
        {
            _workOrderTypeBLL = workOrderTypeBLL;
            _enumBLL = enumBLL;
        }
        
        [HttpGet]
        public Task<WorkOrderTypeDetails[]> DetailsArrayAsync(
            [FromQuery]LookupListFilter filter,
            [FromQuery]ClientPageFilter pageFilter,
            CancellationToken cancellationToken = default)
        {
            return string.IsNullOrWhiteSpace(pageFilter?.OrderByProperty)
                ? _workOrderTypeBLL.GetDetailsArrayAsync(filter, cancellationToken)
                : _workOrderTypeBLL.GetDetailsPageAsync(filter, pageFilter, cancellationToken);
        }
        
        [HttpDelete("{id}")]
        public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            await _workOrderTypeBLL.DeleteAsync(id, cancellationToken);
        } 

        [HttpPost]
        public async Task<WorkOrderTypeDetails> AddAsync([FromBody] WorkOrderTypeData data, CancellationToken cancellationToken = default)
        {
            return await _workOrderTypeBLL.AddAsync(data, cancellationToken);
        } 
        
        [HttpPut("{id}")]
        public async Task<WorkOrderTypeDetails> UpdateAsync([FromRoute] Guid id, [FromBody] WorkOrderTypeData data, CancellationToken cancellationToken = default)
        {
            return await _workOrderTypeBLL.UpdateAsync(id, data, cancellationToken);
        } 
        
        [HttpGet("{id}")]
        public async Task<WorkOrderTypeDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
           return await _workOrderTypeBLL.DetailsAsync(id, cancellationToken);
        } 

        #region Non-REST legacy methods
        
        [HttpGet("classes")]
        public Task<LookupItem<WorkOrderTypeClass>[]> GetTypeClasses(CancellationToken cancellationToken = default)
        {
            return _enumBLL.GetAllAsync(cancellationToken);
        }

        #endregion
    }
}
