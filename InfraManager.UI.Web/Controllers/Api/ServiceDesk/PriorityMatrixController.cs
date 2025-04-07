using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PriorityMatrixController : ControllerBase
    {
        private readonly IPriorityMatrixBLL _priorityMatrixBLL;

        public PriorityMatrixController(IPriorityMatrixBLL priorityMatrixBLL)
        {
            _priorityMatrixBLL = priorityMatrixBLL;
        }

        [HttpGet]
        [Route("table")]
        public async Task<ConcordanceDetails[]> GetTablePrioritiesAsync(CancellationToken cancellationToken)
        {
            return await _priorityMatrixBLL.GetTableAsync(cancellationToken);
        }

        [HttpPost]
        [Route("table/save/cell")]
        public async Task<bool> SaveCellPrioritiesAsync([FromBody] ConcordanceDetails model, CancellationToken cancellationToken)
        {
            return await _priorityMatrixBLL.SaveCellAsync(model, cancellationToken);
        }

        [HttpDelete]
        [Route("table/remove/cell")]
        public async Task<bool> RemoveCellPriorities(Guid urgencyId, Guid influencyId)
        {
            return await _priorityMatrixBLL.RemoveCellAsync(urgencyId, influencyId);
        }
    }
}
