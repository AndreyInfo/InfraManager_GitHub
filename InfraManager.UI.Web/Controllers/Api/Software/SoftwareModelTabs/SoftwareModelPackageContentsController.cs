using InfraManager.BLL.Software;
using InfraManager.BLL.Software.SoftwareModelTabs;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using InfraManager.BLL.Software.SoftwareModelTabs.PackageContents;

namespace InfraManager.UI.Web.Controllers.Api.Software.SoftwareModelTabs
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SoftwareModelPackageContentsController : BaseApiController
    {
        private readonly ISoftwareModelPackageContentsBLL _softwareModelPackageContentsBLL;

        public SoftwareModelPackageContentsController(ISoftwareModelPackageContentsBLL softwareModelPackageContentsBLL)
        {
            _softwareModelPackageContentsBLL = softwareModelPackageContentsBLL;
        }

        [HttpGet]
        public async Task<SoftwareModelPackageContentsListItemDetails[]> GetPackageContentsForSoftwareModelAsync([FromQuery] SoftwareModelTabFilter filter, 
            CancellationToken cancellationToken)
            => await _softwareModelPackageContentsBLL.GetPackageContentsForSoftwareModelAsync(filter, cancellationToken);
    }
}
