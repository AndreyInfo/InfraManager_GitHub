using Inframanager.BLL;
using InfraManager.BLL;
using InfraManager.BLL.Asset;
using InfraManager.DAL.Asset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Asset
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CriticalitiesController : ControllerBase
    {
        private readonly ICriticalityBLL _service;

        public CriticalitiesController(ICriticalityBLL service)
        {
            _service = service;
        }

        // GET api/criticalities[?searchName=...&orderByProperty=...&take=...&skip=...&ascending=...]
        [HttpGet]
        public Task<LookupDetails<Guid>[]> GetAsync(
            [FromQuery] LookupListFilter filterCriteria,
            [FromQuery] ClientPageFilter<Criticality> pagingCriteria,
            CancellationToken cancellationToken = default) => _service.GetDetailsPageAsync(
                filterCriteria,
                string.IsNullOrWhiteSpace(pagingCriteria.OrderByProperty)
                    ? new ClientPageFilter<Criticality> { OrderByProperty = nameof(Criticality.ID) } // если take, skip или ascending используются без указания сортировки - игнор
                    : pagingCriteria,
                cancellationToken);

        // GET api/criticalities/12345678-90AB-CDEF-1234-567890ABCDEF
        [HttpGet("{id}")]
        public Task<LookupDetails<Guid>> GetAsync(Guid id, CancellationToken cancellationToken = default) => _service.DetailsAsync(id, cancellationToken);

        // POST api/criticalities
        [HttpPost]
        public Task<LookupDetails<Guid>> PostAsync([FromBody] LookupData data, CancellationToken cancellationToken = default) => _service.AddAsync(data, cancellationToken);

        // PUT api/criticalities/12345678-90AB-CDEF-1234-567890ABCDEF
        [HttpPut("{id}")]
        public Task<LookupDetails<Guid>> PutAsync(Guid id, [FromBody] LookupData data, CancellationToken cancellationToken = default) =>
            _service.UpdateAsync(id, data, cancellationToken);

        // DELETE api/criticalities/12345678-90AB-CDEF-1234-567890ABCDEF
        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) => _service.DeleteAsync(id, cancellationToken);
    }
}
