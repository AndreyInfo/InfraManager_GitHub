using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.Dashboards.ForTable;
using InfraManager.BLL.Dashboards;
using System.IO;
using System.Text;

namespace InfraManager.UI.Web.Controllers.Api.Dashboard
{
    [Authorize]
    [ApiController]
    [Route("/api/[Controller]/")]
    public class DashboardsController : ControllerBase
    {
        private IDashboardBLL _service;
        public DashboardsController(IDashboardBLL service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<DashboardFullDetails> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _service.AllDetailsAsync(id, cancellationToken);
        }

        [HttpGet]
        public async Task<DashboardsForTableDetails[]> ListAsync([FromQuery] DashboardListFilter filterBy, CancellationToken cancellationToken = default) 
        {
            return await _service.GetDetailsArrayAsync(filterBy, cancellationToken);
        }

        [HttpPost]
        public async Task<DashboardFullDetails> PostAsync([FromBody] DashboardFullData data, CancellationToken cancellationToken = default) =>
            await _service.InsertAsync(data, cancellationToken);

        [HttpPut("{id}")]
        public async Task<DashboardFullDetails> PutAsync([FromRoute] Guid id, [FromBody] DashboardFullData data, CancellationToken cancellationToken = default) =>
            await _service.UpdateDashboardAsync(id, data, cancellationToken);

        [HttpDelete("{id}")]
        public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default) 
            => await _service.DeleteAsync(id, cancellationToken);

        [HttpPost("Export/{id}")]
        public async Task<FileResult> ExportAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _service.AllDetailsAsync(id, cancellationToken);

            var response = File(new MemoryStream(Encoding.UTF8.GetBytes(result.Data ?? "")),
                "application/octet-stream");

            Response.Headers.ContentDisposition =
                $"attachment; filename=dashboardScheme.xml";

            return response;
        }
    }
}
