using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.Location.AdminLocation;
using InfraManager.BLL.Location.Buildings;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace InfraManager.UI.Web.Controllers.Api.Location
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BuildingsController: ControllerBase
    {
        private readonly IBuildingBLL _service;

        public BuildingsController(IBuildingBLL service)
        {
            _service = service;
        }
        
        [HttpGet("list")]
        public Task<BuildingDetails[]> IndexAsync([FromQuery] BuildingListFilter filterBy, [FromQuery] ClientPageFilter<Building> pageBy, CancellationToken cancellationToken = default) =>
            !string.IsNullOrWhiteSpace(pageBy.OrderByProperty)
                ? _service.GetDetailsPageAsync(filterBy, pageBy, cancellationToken)
                : _service.GetDetailsArrayAsync(filterBy, cancellationToken);

        [HttpGet("{id}")]
        public Task<BuildingDetails> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default) =>
            _service.DetailsAsync(id, cancellationToken);

        [HttpGet]
        [Obsolete("Метод добавлен для совместимости с админкой. Удалить, после обновления админки")]
        public async Task<BuildingDetails[]> GetListAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken)
        {
            var filterBy = new BuildingListFilter
            {
                Name = filter.SearchString
            };
            var pageBy = new ClientPageFilter<Building>
            {
                Skip = filter.StartRecordIndex,
                Take = filter.CountRecords
            };
            return await _service.GetDetailsPageAsync(filterBy, pageBy, cancellationToken);
        }
        
        [HttpPost]
        public Task<BuildingDetails> PostAsync([FromBody] BuildingData data, CancellationToken cancellationToken = default) =>
            _service.AddAsync(data, cancellationToken);

        [HttpPut]
        [Obsolete("Метод добавлен для совместимости с админкой. Удалить, после обновления админки")]
        public Task<BuildingDetails> PutAsync([FromBody] AdminBuildingData data, CancellationToken cancellationToken = default) =>
            _service.UpdateAsync(data.ID, data, cancellationToken);

        [HttpPut("{id}")]
        public Task<BuildingDetails> PutAsync([FromRoute] int id, [FromBody] BuildingData data, CancellationToken cancellationToken = default) =>
            _service.UpdateAsync(id, data, cancellationToken);

        [HttpDelete("{id}")]
        public Task DeleteAsync([FromRoute] int id, CancellationToken cancellationToken = default) => _service.RemoveAsync(id, cancellationToken);
    }
}