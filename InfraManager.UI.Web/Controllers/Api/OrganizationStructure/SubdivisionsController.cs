using Inframanager.BLL;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.OrganizationStructure;
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
    public class SubdivisionsController : ControllerBase
    {
        private readonly ISubdivisionBLL _service;

        public SubdivisionsController(ISubdivisionBLL service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<SubdivisionDetails[]> GetAsync(
            [FromQuery] SubdivisionListFilter filterBy,
            [FromQuery] ClientPageFilter<Subdivision> pageFilter,
            CancellationToken cancellationToken = default)
        {
            return string.IsNullOrWhiteSpace(pageFilter?.OrderByProperty)
                ? _service.GetDetailsArrayAsync(filterBy, cancellationToken)
                : _service.GetDetailsPageAsync(filterBy, pageFilter, cancellationToken);
        }

        // TODO объединить GetAsync и GetTableAsync идентичны по функцмионалу(различаются сортировкой)
        [HttpGet("Table")]
        public Task<SubdivisionDetails[]> GetTableAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken = default)
        {
            return _service.GetTableAsync(filter, cancellationToken);
        }

        [HttpGet("{id:guid}")]
        public Task<SubdivisionDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _service.GetDetailsAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<Guid> AddAsync([FromBody] SubdivisionData subdivision, CancellationToken cancellationToken) =>
            await _service.AddAsync(subdivision, cancellationToken);

        [HttpPut("{id}")]
        public async Task UpdateSubdivisionAsync([FromRoute] Guid id, [FromBody] SubdivisionData subdivision, CancellationToken cancellationToken) =>
            await _service.UpdateAsync(id, subdivision, cancellationToken);

        [HttpDelete("{id}")]

        public async Task DeleteSubdivisionAsync([FromRoute] Guid id, CancellationToken cancellationToken) =>
            await _service.DeleteByIDAsync(id, cancellationToken);

        [HttpGet("{parentID:guid}/childer")]
        public async Task<SubdivisionDetails[]> GetAllChildrenByParentID([FromRoute] Guid parentID, CancellationToken cancellationToken)
        {
            return await _service.GetAllSubSubdivisionsAsync(parentID, cancellationToken);
        }
    }
}
