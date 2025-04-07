using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.CallSummary
{
    /// <summary>
    /// Обслуживание справочника "Краткое описание заявок"
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CallSummaryController : ControllerBase
    {
        private readonly ICallSummaryBLL _callSummaryBLL;

        public CallSummaryController(
            ICallSummaryBLL callSummaryBLL)
        {
            _callSummaryBLL = callSummaryBLL;
        }

        [HttpPost("byFilter")]
        public async Task<CallSummaryDetails[]> GetCallSummariesAsync([FromBody] CallSummaryFilter filter, CancellationToken cancellationToken = default)
        {
            return await _callSummaryBLL.GetListAsync(filter, cancellationToken);
        }

        [HttpPost("saveOrUpdate")]
        public async Task<Guid> SaveOrUpdateAsync([FromBody] CallSummaryDetails callSummary)
        {
            return await _callSummaryBLL.AddOrUpdateAsync(callSummary, HttpContext.RequestAborted);
        }

        [HttpDelete]
        public async Task<string[]> BulkDeleteAsync([FromBody] List<DeleteModel<Guid>> deleteModels)
        {
            return await _callSummaryBLL.DeleteAsync(deleteModels, HttpContext.RequestAborted);
        }

    }
}
