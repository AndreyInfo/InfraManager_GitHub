using Inframanager.BLL;
using InfraManager.BLL;
using InfraManager.BLL.Finance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierBLL _service;

        public SuppliersController(ISupplierBLL service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<SupplierDetails[]> GetDetailsArrayAsync(
            [FromQuery]LookupListFilter filter,
            [FromQuery]ClientPageFilter pageFilter,
            CancellationToken cancellationToken = default)
        {
            return !string.IsNullOrWhiteSpace(pageFilter?.OrderByProperty)
                ? _service.GetDetailsPageAsync(filter, pageFilter, cancellationToken)
                : _service.GetDetailsArrayAsync(filter, cancellationToken);
        }
    }
}
