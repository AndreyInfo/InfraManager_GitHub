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
    public class AvailableFilterElementsController : ControllerBase
    {
        private readonly IAvailableTableFilterElementsBLL _service;

        public AvailableFilterElementsController(IAvailableTableFilterElementsBLL service)
        {
            _service = service;
        }

        [HttpGet("{view}")]
        public Task<FilterElementBase[]> GetListAsync(string view, CancellationToken cancellationToken = default)
        {
            return _service.GetAllAsync(view, cancellationToken);
        }

        [HttpGet("{view}/{property}")]
        public Task<FilterElementBase> GetByPropertyNameAsync(string view, string property, CancellationToken cancellationToken = default)
        {
            return _service.GetByPropertyNameAsync(view, property, cancellationToken);
        }
    }
}
