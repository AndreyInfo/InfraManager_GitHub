using DevExpress.XtraReports.UI;
using InfraManager.BLL.Report;
using InfraManager.UI.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Report
{
    [Authorize]
    [ApiController]
    [Route("/api/[Controller]/")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportBLL _service;
        public ReportsController(IReportBLL service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ReportDetails> GetReportAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _service.GetReportAsync(id, cancellationToken);
        }

        [HttpGet]
        public async Task<ReportForTableDetails[]> GetAllReportsAsync([FromQuery] ReportsFilter filter,CancellationToken cancellationToken)
        {
            return await _service.GetReportsAsync(filter, cancellationToken);
        }

        [HttpPost]
        public async Task CreateAsync([FromBody] ReportData report, CancellationToken cancellationToken)
        {
            using var ms = new MemoryStream();
            using XtraReport devExReport = new DevExReport();
            devExReport.SaveLayoutToXml(ms);
            report.Data = Encoding.Default.GetString(ms.ToArray());

            await _service.InsertAsync(report, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task PutAsync(Guid id, [FromBody]ReportData report, CancellationToken cancellationToken)
        { 
             await _service.UpdateAsync(id, report, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
             await _service.DeleteAsync(id, cancellationToken);
        }

        [HttpGet("Export/{id}")]
        public async Task<FileResult> ExportAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetReportAsync(id, cancellationToken);

            var response = File(new MemoryStream(Encoding.UTF8.GetBytes(result.Data ?? "")),
                "application/octet-stream");

            Response.Headers.ContentDisposition =
                $"attachment; filename=reportScheme.repx";

            return response;
        }
    }
}
