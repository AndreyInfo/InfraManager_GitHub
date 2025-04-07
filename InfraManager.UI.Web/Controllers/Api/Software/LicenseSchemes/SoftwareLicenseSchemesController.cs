using InfraManager.BLL;
using InfraManager.BLL.Software.SoftwareLicenseSchemes;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;

namespace InfraManager.UI.Web.Controllers.Api.Software.LicenseSchemes
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SoftwareLicenseSchemesController : BaseApiController
    {
        private readonly ISoftwareLicenseSchemeBLL _softwareLicenseSchemeBLL;

        public SoftwareLicenseSchemesController(ISoftwareLicenseSchemeBLL softwareLicenseSchemeBLL)
        {
            _softwareLicenseSchemeBLL = softwareLicenseSchemeBLL;
        }

        [HttpGet]
        public async Task<SoftwareLicenseSchemeDetails[]> GetListAsync([FromQuery] LookupListFilter filter, CancellationToken cancellationToken = default)
        {
            return await _softwareLicenseSchemeBLL.GetListAsync(filter, cancellationToken);
        }
    }
}
