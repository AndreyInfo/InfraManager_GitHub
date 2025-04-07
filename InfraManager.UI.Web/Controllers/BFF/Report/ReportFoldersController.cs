using InfraManager.BLL.Report;
using InfraManager.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Report
{
    [Authorize]
    [ApiController]
    [Route("/api/[Controller]/")]
    public class ReportFoldersController : ControllerBase
    {
        private readonly IReportFolderBLL _service;
        public ReportFoldersController(IReportFolderBLL service)
        {
            _service = service;
        }
        
        [HttpGet("{id}")]
        public async Task<ReportFolderDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return await _service.GetReportFolderAsync(id, cancellationToken);
        }

        [HttpGet]
        public async Task<ReportFolderDetails[]> GetListAsync([FromQuery] ReportFolderFilter filter,
            CancellationToken cancellationToken)
        {
            return await _service.GetReportFoldersAsync(filter, cancellationToken);
        }

        [HttpPost]
        public async Task<ReportFolderDetails> CreateAsync([FromBody] ReportFolderData reportFolderDetails, CancellationToken cancellationToken)
        {
            return await _service.InsertAsync(reportFolderDetails, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task PutAsync([FromBody] ReportFolderData reportFolderDetails, Guid id, CancellationToken cancellationToken)
        {
            await _service.PutAsync(id, reportFolderDetails, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(id, cancellationToken);
        }


    }
}
