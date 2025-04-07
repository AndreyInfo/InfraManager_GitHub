using InfraManager.BLL.Software.SoftwareModelTabs;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using InfraManager.BLL.Software.SoftwareModelTabs.Licenses;

namespace InfraManager.UI.Web.Controllers.Api.Software.SoftwareModelTabs
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SoftwareModelLicensesController : BaseApiController
    {
        private readonly ISoftwareModelLicenseBLL _softwareModelLicenseBLL;

        public SoftwareModelLicensesController(ISoftwareModelLicenseBLL softwareModelLicenseBLL)
        {
            _softwareModelLicenseBLL = softwareModelLicenseBLL;
        }

        [HttpGet]
        public async Task<SoftwareModelLicenseListItemDetails[]> GetLicensesForSoftwareModelAsync([FromQuery] SoftwareModelTabFilter filter, 
            CancellationToken cancellationToken)
            => await _softwareModelLicenseBLL.GetLicensesForSoftwareModelAsync(filter, cancellationToken);
    }
}
