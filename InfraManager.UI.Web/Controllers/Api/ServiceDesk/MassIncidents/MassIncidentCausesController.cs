using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.MassIncidents
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MassIncidentCausesController : ControllerBase
    {
        private readonly IMassIncidentCauseBLL _service;

        public MassIncidentCausesController(IMassIncidentCauseBLL service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<MassIncidentCauseDetails[]> GetAsync([FromQuery] MassIncidentCauseListFilter filter, CancellationToken cancellationToken = default)
        {
            return _service.GetDetailsPageAsync(filter, cancellationToken);
        }

        [HttpGet("{id}")]
        public Task<MassIncidentCauseDetails> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return _service.DetailsAsync(id, cancellationToken);
        }

        [HttpPost]
        public Task<MassIncidentCauseDetails> PostAsync([FromBody] LookupData data, CancellationToken cancellationToken = default)
        {
            return _service.AddAsync(data, cancellationToken);
        }

        [HttpPut("{id}")]
        public Task<MassIncidentCauseDetails> PutAsync(int id, [FromBody] LookupData data, CancellationToken cancellationToken = default)
        {
            return _service.UpdateAsync(id, data, cancellationToken);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            return _service.DeleteAsync(id, cancellationToken);
        }
    }
}
