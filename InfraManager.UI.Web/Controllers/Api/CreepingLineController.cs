using InfraManager.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System;
using System.Threading.Tasks;
using InfraManager.BLL.CreepingLines;

namespace InfraManager.UI.Web.Controllers.Api
{
    [Authorize]
    [ApiController]
    [Route("/api/creepinglines/")]
    public class CreepingLineController : ControllerBase
    {
        private readonly ICreepingLineBLL _service;
        public CreepingLineController(ICreepingLineBLL service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public Task<CreepingLineDetails[]> GetAsync([FromQuery] CreepingLineFilter filter,
            CancellationToken cancellationToken = default)
        {
            return _service.ListAsync(filter, cancellationToken);
        }

        [HttpGet("{id}")]
        public Task<CreepingLineDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            return _service.GetAsync(id, cancellationToken);
        }

        [HttpPost]
        public Task<Guid> PostAsync(CreepingLineData data, CancellationToken cancellationToken = default)
        { 
            return _service.InsertAsync(data, cancellationToken);
        }

        [HttpPut("{id}")]
        public Task<Guid> PutAsync([FromRoute] Guid id, CreepingLineData data,
            CancellationToken cancellationToken = default)
        {
            return _service.UpdateAsync(id, data, cancellationToken);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            return _service.DeleteAsync(id, cancellationToken);
        }
    }
}
