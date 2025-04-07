using InfraManager.BLL.ServiceDesk.ChangeRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Catalogs.RFC
{

    [Route("api/{controller}")]
    [ApiController]
    [Authorize]
    public class RfcCategoryController : ControllerBase
    {
        private readonly IChangeRequestCategoryBLL _service;

        public RfcCategoryController(IChangeRequestCategoryBLL service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<ChangeRequestCategoryDetails[]> GetListAsync(CancellationToken cancellationToken = default)
        {
            return _service.GetListAsync(cancellationToken);
        }

    }
}
