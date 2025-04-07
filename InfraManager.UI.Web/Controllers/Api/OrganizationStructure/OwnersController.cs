using InfraManager.BLL.OrganizationStructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.OrganizationStructure
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerBLL _service;

        public OwnersController(IOwnerBLL service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<OwnerDetails[]> GetAsync([FromQuery]int? take, CancellationToken cancellationToken= default)
        {
            return _service.AllAsync(take, cancellationToken);
        }

        [HttpGet("{id}")]
        public Task<OwnerDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _service.DetailsAsync(id, cancellationToken);
        }

        [HttpPut("{id}")]
        public Task<OwnerDetails> PutAsync(Guid id, [FromBody]OwnerData data, CancellationToken cancellationToken = default)
        {
            return _service.ModifyAsync(id, data, cancellationToken);
        }
    }
}
