using InfraManager.BLL.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Priorities
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrioritiesController : ControllerBase
    {
        private readonly IPriorityBLL _priorityBLL;

        public PrioritiesController(IPriorityBLL priorityBLL)
        {
            _priorityBLL = priorityBLL;
        }

        [HttpGet("{id}")]
        public Task<PriorityDetailsModel> GetAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return _priorityBLL.FindAsync(id, cancellationToken);
        }

        private const int DefaultListCapacity = 10;

        [HttpGet]
        public Task<PriorityDetailsModel[]> ListAsync(
            [FromQuery] string searchName,
            [FromQuery] int take,
            [FromQuery] int skip)
        {
            return _priorityBLL.ListAsync(
                new LookupListFilterModel
                {
                    SearchName = searchName,
                    Take = take == 0 ? DefaultListCapacity : take,
                    Skip = skip
                });
        }


        public sealed class RemoveModelIn
        {
            public Guid PriorityId { get; set; }
        }

        [HttpDelete("{id}")]
        public Task RemovePriorityByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _priorityBLL.DeleteAsync(id, cancellationToken); //return true <==> HttpStatus = 200
        }

        [HttpPost]
        public Task<PriorityDetailsModel> PostAsync(
            [FromBody] PriorityModel model,
            CancellationToken cancellationToken = default)
        {
            return _priorityBLL.AddAsync(model, cancellationToken);
        }

        [HttpPut("{id}")]
        public Task<PriorityDetailsModel> PutAsync(
            Guid id,
            [FromBody] PriorityModel model,
            CancellationToken cancellationToken = default)
        {
            return _priorityBLL.UpdateAsync(id, model, cancellationToken);
        }
    }
}
