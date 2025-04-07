using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL;
using InfraManager.BLL.Localization;
using InfraManager.BLL.Location.Workplaces;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.Location;
using InfraManager.ResourcesArea;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace InfraManager.UI.Web.Controllers.Api.Location
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkplacesController: ControllerBase
    {
        private readonly IWorkplaceBLL _service;
        private readonly ILocalizeText _localizeText;

        public WorkplacesController(IWorkplaceBLL service, ILocalizeText localizeText)
        {
            _service = service;
            _localizeText = localizeText;
        }
        
        [HttpGet("list")]
        public Task<WorkplaceDetails[]> IndexAsync([FromQuery] WorkplaceListFilter filterBy, [FromQuery] ClientPageFilter<Workplace> pageBy, CancellationToken cancellationToken = default) =>
            !string.IsNullOrWhiteSpace(pageBy.OrderByProperty)
                ? _service.GetDetailsPageAsync(filterBy, pageBy, cancellationToken)
                : _service.GetDetailsArrayAsync(filterBy, cancellationToken);

        [HttpGet]
        [Obsolete("Метод добавлен для совместимости с админкой. Удалить, после обновления админки")]
        public async Task<WorkplaceDetails[]> GetListAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken)
        {
            var filterBy = new WorkplaceListFilter
            {
                Name = filter.SearchString
            };
            var pageBy = new ClientPageFilter<Workplace>
            {
                Skip = filter.StartRecordIndex,
                Take = filter.CountRecords
            };
            return await _service.GetDetailsPageAsync(filterBy, pageBy, cancellationToken);
        }
        
        [HttpGet("{id:int}")]
        public Task<WorkplaceDetails> GetAsync(int id, CancellationToken cancellationToken = default) =>
            _service.DetailsAsync(id, cancellationToken);

        [HttpPost]
        public Task<WorkplaceDetails> PostAsync([FromBody] WorkplaceData data, CancellationToken cancellationToken = default) =>
            _service.AddAsync(data, cancellationToken);

        
        [HttpPut("{id:int}")]
        public Task<WorkplaceDetails> PutAsync(int id, [FromBody] WorkplaceData data, CancellationToken cancellationToken = default) =>
            _service.UpdateAsync(id, data, cancellationToken);

        [HttpDelete("{id:int}")]
        public async Task DeleteAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            // TODO перевсти проверку на уровне DAL
            try
            {
                await _service.DeleteAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is not null && ex.InnerException.Message.ToLower().Contains("fk"))
                    throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.ErroTextUsedSystem), cancellationToken));
                else
                    throw;
            }
        }
    }
}