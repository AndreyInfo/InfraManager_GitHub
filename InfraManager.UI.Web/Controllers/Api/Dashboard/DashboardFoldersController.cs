using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using InfraManager.BLL.Dashboards;
using Inframanager.BLL;
using InfraManager.DAL.Dashboards;

namespace InfraManager.UI.Web.Controllers.Api.Dashboard
{
    [Authorize]
    [ApiController]
    [Route("/api/[Controller]/")]
    public class DashboardFoldersController : ControllerBase
    {
        private readonly IDashboardFolderBLL _service;
        public DashboardFoldersController(IDashboardFolderBLL service)
        {
            _service = service;
        }
        [HttpGet("{id}")]
        public async Task<DashboardFolderDetails> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _service.DetailsAsync(id, cancellationToken);
        }

        [HttpGet]
        public async Task<DashboardFolderDetails[]> ListAsync([FromQuery] DashboardFolderListFilter filterBy, [FromQuery] ClientPageFilter<DashboardFolder> pageBy, CancellationToken cancellationToken = default) =>
            !string.IsNullOrWhiteSpace(pageBy.OrderByProperty)
                ? await _service.GetDetailsPageAsync(filterBy, pageBy, cancellationToken)
                : await _service.GetDetailsArrayAsync(filterBy, cancellationToken);

        [HttpPost]
        public async Task<DashboardFolderDetails> PostAsync([FromBody] DashboardFolderData data, CancellationToken cancellationToken = default) =>
            await _service.AddAsync(data, cancellationToken);

        [HttpPut("{id}")]
        public async Task<DashboardFolderDetails> PutAsync([FromRoute] Guid id, [FromBody] DashboardFolderData data, CancellationToken cancellationToken = default) =>
            await _service.UpdateAsync(id, data, cancellationToken);

        [HttpDelete("{id}")]
        public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default) => await _service.DeleteAsync(id, cancellationToken);

    }
}
