using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.Location.AdminLocation;
using InfraManager.BLL.Location.Floors;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace InfraManager.UI.Web.Controllers.Api.Location
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FloorsController: ControllerBase
    {
        private readonly IFloorBLL _service;

        public FloorsController(IFloorBLL service)
        {
            _service = service;
        }
        
        [HttpGet("list")]
        public Task<FloorDetails[]> IndexAsync([FromQuery] FloorListFilter filterBy, [FromQuery] ClientPageFilter<Floor> pageBy, CancellationToken cancellationToken = default) =>
            !string.IsNullOrWhiteSpace(pageBy.OrderByProperty)
                ? _service.GetDetailsPageAsync(filterBy, pageBy, cancellationToken)
                : _service.GetDetailsArrayAsync(filterBy, cancellationToken);

        [HttpGet]
        [Obsolete("Метод добавлен для совместимости с админкой. Удалить, после обновления админки")]
        public async Task<FloorDetails[]> GetListAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken)
        {
            var filterBy = new FloorListFilter
            {
                Name = filter.SearchString
            };
            var pageBy = new ClientPageFilter<Floor>
            {
                Skip = filter.StartRecordIndex,
                Take = filter.CountRecords
            };
            return await _service.GetDetailsPageAsync(filterBy, pageBy, cancellationToken);
        }
        
        [HttpGet("{id}")]
        public Task<FloorDetails> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default) =>
            _service.DetailsAsync(id, cancellationToken);

        [HttpPost]
        public Task<FloorDetails> PostAsync([FromBody] FloorData data, CancellationToken cancellationToken = default) =>
            _service.AddAsync(data, cancellationToken);

        [HttpPut]
        [Obsolete("Метод добавлен для совместимости с админкой. Удалить, после обновления админки")]
        public Task<FloorDetails> PutAsync([FromBody] AdminFloorData data, CancellationToken cancellationToken = default) =>
            _service.UpdateAsync(data.ID, data, cancellationToken);
        
        [HttpPut("{id}")]
        public Task<FloorDetails> PutAsync([FromRoute] int id, [FromBody] FloorData data, CancellationToken cancellationToken = default) =>
            _service.UpdateAsync(id, data, cancellationToken);

        [HttpDelete("{id}")]
        public Task DeleteAsync([FromRoute] int id, CancellationToken cancellationToken = default) => _service.RemoveAsync(id, cancellationToken);
    }
}