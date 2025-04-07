using Inframanager.BLL;
using InfraManager.BLL.OrganizationStructure.JobTitles;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.OrganizationStructure
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobTitlesController : ControllerBase
    {
        private readonly IJobTitleBLL _service;

        public JobTitlesController(IJobTitleBLL service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<JobTitleDetails[]> IndexAsync([FromQuery] JobTitleListFilter filterBy, [FromQuery] ClientPageFilter<JobTitle> pageBy, CancellationToken cancellationToken = default) =>
            !string.IsNullOrWhiteSpace(pageBy.OrderByProperty)
                ? _service.GetDetailsPageAsync(filterBy, pageBy, cancellationToken)
                : _service.GetDetailsArrayAsync(filterBy, cancellationToken);

        [HttpGet("list")]
        public async Task<JobTitleDetails[]> IndexAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken = default) 
            => await _service.GetPaggingAsync(filter, cancellationToken);

        [HttpGet("{id}")]
        public Task<JobTitleDetails> GetAsync(int id, CancellationToken cancellationToken = default) =>
            _service.DetailsAsync(id, cancellationToken);

        [HttpPost]
        public Task<JobTitleDetails> PostAsync([FromBody] JobTitleData data, CancellationToken cancellationToken = default) =>
            _service.AddAsync(data, cancellationToken);

        [HttpPut("{id}")]
        public Task<JobTitleDetails> PutAsync(int id, [FromBody] JobTitleData data, CancellationToken cancellationToken = default) =>
            _service.UpdateAsync(id, data, cancellationToken);

        [HttpDelete("{id}")]
        public Task DeleteAsync(int id, CancellationToken cancellationToken = default) => _service.DeleteAsync(id, cancellationToken);
    }
}
