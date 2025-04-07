using InfraManager.BLL.Software;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using InfraManager.BLL.Software.SoftwareModels;
using InfraManager.BLL.Software.SoftwareModelTabs.Dependencies;

namespace InfraManager.UI.Web.Controllers.Api.Software.SoftwareModelTabs
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SoftwareModelDependenciesController : BaseApiController
    {
        private readonly ISoftwareModelDependencyBLL _softwareModelDependencyBLL;

        public SoftwareModelDependenciesController(ISoftwareModelDependencyBLL softwareModelDependencyBLL)
        {
            _softwareModelDependencyBLL = softwareModelDependencyBLL;
        }

        [HttpGet]
        public async Task<SoftwareModelListItemDetails[]> GetDependenciesForSoftwareModelAsync([FromQuery] SoftwareModelTabDependencyFilter filter, 
            CancellationToken cancellationToken) 
            => await _softwareModelDependencyBLL.GetDependenciesForSoftwareModelAsync(filter, cancellationToken);
    }
}
