using InfraManager.BLL.Software.SoftwareModelTabs;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using InfraManager.BLL.Software.SoftwareModelTabs.Updates;

namespace InfraManager.UI.Web.Controllers.Api.Software.SoftwareModelTabs
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SoftwareModelUpdatesController : BaseApiController
    {
        private readonly ISoftwareModelUpdateBLL _softwareModelUpdateBLL;

        public SoftwareModelUpdatesController(ISoftwareModelUpdateBLL softwareModelUpdateBLL)
        {
            _softwareModelUpdateBLL = softwareModelUpdateBLL;
        }

        [HttpGet]
        public async Task<SoftwareModelUpdateListItemDetails[]> GetUpdatesForSoftwareModelAsync([FromQuery] SoftwareModelTabFilter filter, 
            CancellationToken cancellationToken)
            => await _softwareModelUpdateBLL.GetUpdatesForSoftwareModelAsync(filter, cancellationToken);
    }
}
