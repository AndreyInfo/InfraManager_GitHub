using InfraManager.BLL.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Events
{
    [ApiController]
    [Authorize]
    [Route("bff/[controller]")]
    public class EventsController : ControllerBase
    {
        private IEventBLL _events;

        public EventsController(IEventBLL events)
        {
            _events = events;
        }

        [HttpPost]
        public async Task<EventDetails[]> GetEventSubjectsAsync([FromBody] EventSubjectFilter filter, CancellationToken cancellationToken)
        {
            return await _events.GetEventSubjectsAsync(filter, cancellationToken);
        }
        
        [HttpGet("{id}")]
        public async Task<EventDetails> GetEventSubjectAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _events.GetEventSubjectAsync(id, cancellationToken);
        }
    }
}
