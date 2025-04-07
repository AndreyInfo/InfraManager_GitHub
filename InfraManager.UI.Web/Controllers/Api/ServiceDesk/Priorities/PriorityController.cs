using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.ServiceDesk;
using InfraManager.DAL.ServiceDesk;
using InfraManager.Web.Controllers;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.BLL.Asset.dto;
using System.Threading;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.UI.Web.Controllers.Api.Priorities
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PriorityController : ControllerBase
    {
        private readonly IPriorityBLL _priorityBLL;
        private readonly IPriorityMatrixBLL _priorityMatrixBLL;

        public PriorityController(IPriorityBLL priorityBLL, IPriorityMatrixBLL priorityMatrixBLL)
        {
            _priorityBLL = priorityBLL ?? throw new ArgumentNullException(nameof(priorityBLL));
            _priorityMatrixBLL = priorityMatrixBLL ?? throw new ArgumentNullException(nameof(priorityMatrixBLL));
        }


        [HttpGet("item")]
        public async Task<PriorityDetailsModel> GetPriorityByIdAsync(Guid priorityID, CancellationToken cancellationToken = default)
        {
            return await _priorityBLL.FindAsync(priorityID, cancellationToken);
        }

        [HttpGet("list")]
        public async Task<PriorityDetailsModel[]> GetPrioritiesAsync(string searchName = "", int take = 0, int skip = 0, CancellationToken cancellationToken = default)
        {
            return await _priorityBLL.ListAsync(new LookupListFilterModel() { SearchName = searchName, Skip = skip, Take = take }, cancellationToken);
        }


        public sealed class RemoveModelIn
        {
            public Guid PriorityId { get; set; }
        }

        [HttpPost("remove/item")]
        public async Task<bool> RemovePriorityByIdAsync([FromBody] RemoveModelIn modelIn, CancellationToken cancellationToken = default)
        {
            await _priorityBLL.DeleteAsync(modelIn.PriorityId, cancellationToken);

            return true;
        }

        [HttpPost("save/item")]
        public async Task<bool> SavePriorityAsync([FromBody] PriorityDetailsModel priority, CancellationToken cancellationToken = default)
        {
            return await _priorityBLL.SaveOrUpdateAsync(priority, cancellationToken);
        }

        [HttpGet("table")]
        public async Task<ConcordanceDetails[]> GetTablePriorities(CancellationToken cancellationToken = default)
        {
            return await _priorityMatrixBLL.GetTableAsync(cancellationToken);
        }

        [HttpPost("table/save/cell")]
        public async Task<bool> SaveCellPriorities([FromBody] ConcordanceDetails model)
        {
            return await _priorityMatrixBLL.SaveCellAsync(model);
        }

        [HttpDelete("table/remove/cell")]
        public async Task<bool> RemoveCellPriorities(Guid urgencyId, Guid influencyId)
        {
            return await _priorityMatrixBLL.RemoveCellAsync(urgencyId, influencyId);
        }
    }
}
