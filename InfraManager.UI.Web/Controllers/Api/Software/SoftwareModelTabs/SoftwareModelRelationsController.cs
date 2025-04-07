using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using InfraManager.BLL.Software.SoftwareModelTabs.Relations;
using InfraManager.BLL.Software.SoftwareModelTabs.Dependencies;

namespace InfraManager.UI.Web.Controllers.Api.Software.SoftwareModelTabs
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SoftwareModelRelationsController : BaseApiController
    {
        private readonly ISoftwareModelRelationBLL _softwareModelRelationBLL;

        public SoftwareModelRelationsController(ISoftwareModelRelationBLL softwareModelRelationBLL)
        {
            _softwareModelRelationBLL = softwareModelRelationBLL;
        }

        [HttpGet]
        public async Task<SoftwareModelRelatedListItemDetails[]> GetRelationsForSoftwareModelAsync([FromQuery] SoftwareModelTabDependencyFilter filter,
            CancellationToken cancellationToken)
            => await _softwareModelRelationBLL.GetRelationsForSoftwareModelAsync(filter, cancellationToken);
    }
}
