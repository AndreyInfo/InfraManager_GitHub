using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk
{

    [Route("api/{controller}")]
    [ApiController]
    [Authorize]
    public class UrgencyController : ControllerBase
    {
        private readonly IUrgencyBLL _urgencyBLL;
        public UrgencyController(IUrgencyBLL urgencyBLL)
        {
            _urgencyBLL = urgencyBLL;
        }

        public sealed class GetModelIn
        {
            public Guid Id { get; set; }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<Urgency> GetByIDAsync([FromQuery] GetModelIn model, CancellationToken cancellationToken = default)
        {
            return await _urgencyBLL.GetAsync(model.Id, cancellationToken);
        }


        [HttpGet]
        [Route("GetListForTable")]
        public async Task<UrgencyDTO[]> GetListForTableAsync([FromQuery] BaseFilter model, CancellationToken cancellationToken = default)
        {
            return await _urgencyBLL.GetListForTableAsync(model.SearchString, model.CountRecords, model.StartRecordIndex, cancellationToken);
        }


        [HttpPost]
        [Route("Save")]
        public async Task<Guid> SaveAsync([FromBody] UrgencyDTO model, CancellationToken cancellationToken = default)
        {
            return await _urgencyBLL.SaveAsync(model, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task RemoveAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _urgencyBLL.RemoveAsync(id, cancellationToken);
        }
    }
}
