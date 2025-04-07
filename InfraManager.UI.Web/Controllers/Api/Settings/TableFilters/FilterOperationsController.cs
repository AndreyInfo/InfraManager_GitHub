using InfraManager.BLL;
using InfraManager.BLL.Settings.TableFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Filters
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilterOperationsController : ControllerBase
    {
        private readonly ITableFilterOperationsBLL _service;

        public FilterOperationsController(ITableFilterOperationsBLL service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<LookupListItem<byte>[]> ListAsync(
            [FromQuery]FilterTypes filterType, 
            CancellationToken cancellationToken = default)
        {
            return _service.GetByElementTypeAsync(filterType, cancellationToken);
        }
    }
}
