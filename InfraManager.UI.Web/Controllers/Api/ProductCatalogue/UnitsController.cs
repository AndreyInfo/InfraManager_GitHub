using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue.Units;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitBLL _bll;

        public UnitsController(IUnitBLL bll)
        {
            _bll = bll;
        }

        [HttpGet("{id}")]
        public Task<UnitDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
            => _bll.DetailsAsync(id, cancellationToken);

        [HttpGet]
        public Task<UnitDetails[]> GetListAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken = default)
            => _bll.GetListAsync(filter, cancellationToken);

        [HttpPost]
        public Task<UnitDetails> AddAsync([FromBody] UnitData data, CancellationToken cancellationToken = default)
            => _bll.AddAsync(data, cancellationToken);

        [HttpPut("{id}")]
        public Task<UnitDetails> UpdateAsync([FromRoute] Guid id, [FromBody] UnitData data, CancellationToken cancellationToken = default)
            => _bll.UpdateAsync(id, data, cancellationToken);

        [HttpDelete("{id}")]
        public Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
            => _bll.DeleteAsync(id, cancellationToken);
    }
}