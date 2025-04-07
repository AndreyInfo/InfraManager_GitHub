using System.Threading.Tasks;
using Inframanager.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InfraManager.BLL.Asset.Filters;
using InfraManager.BLL.Location;
using InfraManager.DAL.Asset;
using InfraManager.BLL.Location.Racks;

namespace InfraManager.UI.Web.Controllers.Api.Location
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RackController : ControllerBase
    {
        private readonly IRackBLL _rackBll;

        public RackController(IRackBLL rackBll) => _rackBll = rackBll;

        [HttpGet("{id:int}")]
        public Task<RackDetails> GetByIntIDAsync(int id) => _rackBll.DetailsAsync(id, HttpContext.RequestAborted);

        [HttpGet("list")]
        public Task<RackDetails[]> GetListAsync(
            [FromQuery] RackListFilter filter,
            [FromQuery] ClientPageFilter<Rack> paging) =>
            paging.OrderByProperty is null or ""
                ? _rackBll.GetDetailsArrayAsync(filter, HttpContext.RequestAborted)
                : _rackBll.GetDetailsPageAsync(filter, paging, HttpContext.RequestAborted);
    }
}
