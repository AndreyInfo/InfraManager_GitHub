using InfraManager.BLL.Software.SoftwareModelTabs.Components;
using InfraManager.BLL.Software.SoftwareModelTabs;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;

namespace InfraManager.UI.Web.Controllers.Api.Software.SoftwareModelTabs
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SoftwareModelComponentsController : BaseApiController
    {
        private readonly ISoftwareModelComponentBLL _softwareModelComponentBLL;

        public SoftwareModelComponentsController(ISoftwareModelComponentBLL softwareModelComponentBLL)
        {
            _softwareModelComponentBLL = softwareModelComponentBLL;
        }

        [HttpGet]
        public async Task<SoftwareModelComponentListItemDetails[]> GetComponentsForSoftwareModelAsync([FromQuery] SoftwareModelTabFilter filter, 
            CancellationToken cancellationToken) 
            => await _softwareModelComponentBLL.GetComponentsForSoftwareModelAsync(filter, cancellationToken);
    }
}
