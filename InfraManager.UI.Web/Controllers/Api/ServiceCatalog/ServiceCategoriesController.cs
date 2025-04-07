using InfraManager.BLL.ServiceCatalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceCatalog
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceCategoriesController : ControllerBase
    {
        private readonly IServiceCategoryBLL _service;

        public ServiceCategoriesController(IServiceCategoryBLL service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ServiceCategoryDetails[]> GetAsync([FromQuery] ServiceCategoryListFilter filterBy, CancellationToken cancellationToken = default) =>
            await _service.GetDetailsPageAsync(filterBy, cancellationToken);

        [HttpGet("{id}")]
        public async Task<ServiceCategoryDetails> GetAsync(Guid id, CancellationToken cancellationToken = default) =>
            await _service.DetailsAsync(id, cancellationToken);
    }
}
