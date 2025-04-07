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
    public class MassIncidentInformationChannelsController : ControllerBase
    {
        private readonly IMassIncidentInformationChannelBLL _service;

        public MassIncidentInformationChannelsController(IMassIncidentInformationChannelBLL service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<LookupListItem<short>[]> GetAsync(CancellationToken cancellationToken = default)
        {
            return _service.AllAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public Task<LookupListItem<short>> GetAsync(short id, CancellationToken cancellationToken = default)
        {
            return _service.FindAsync(id, cancellationToken);
        }
    }
}
