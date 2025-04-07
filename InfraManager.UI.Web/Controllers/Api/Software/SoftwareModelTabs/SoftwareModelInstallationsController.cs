using InfraManager.BLL.Software.SoftwareModelTabs;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using InfraManager.BLL.Software.SoftwareModelTabs.Installations;

namespace InfraManager.UI.Web.Controllers.Api.Software.SoftwareModelTabs
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SoftwareModelInstallationsController : BaseApiController
    {
        private readonly ISoftwareModelInstallationBLL _softwareModelInstallationBLL;

        public SoftwareModelInstallationsController(ISoftwareModelInstallationBLL softwareModelInstallationBLL)
        {
            _softwareModelInstallationBLL = softwareModelInstallationBLL;
        }

        [HttpGet]
        public async Task<SoftwareModelInstallationListItemDetails[]> GetInstallationsForSoftwareModelAsync([FromQuery] SoftwareModelTabFilter filter, 
            CancellationToken cancellationToken)
            => await _softwareModelInstallationBLL.GetInstallationsForSoftwareModelAsync(filter, cancellationToken);
    }
}
