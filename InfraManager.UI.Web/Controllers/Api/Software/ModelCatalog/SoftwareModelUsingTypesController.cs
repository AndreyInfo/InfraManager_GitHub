using InfraManager.BLL.Software.SoftwareLicenses;
using InfraManager.BLL;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using InfraManager.BLL.Software.SoftwareModelUsingTypes;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;

namespace InfraManager.UI.Web.Controllers.Api.Software.ModelCatalog;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SoftwareModelUsingTypesController : BaseApiController
{
    private readonly ISoftwareModelUsingTypeBLL _softwareModelUsingTypeBLL;

    public SoftwareModelUsingTypesController(ISoftwareModelUsingTypeBLL softwareModelUsingTypeBLL)
    {
        _softwareModelUsingTypeBLL= softwareModelUsingTypeBLL;
    }

    [HttpGet]
    public async Task<SoftwareModelUsingTypeDetails[]> GetListAsync([FromQuery] LookupListFilter filter, CancellationToken cancellationToken)
    {
        return await _softwareModelUsingTypeBLL.GetListAsync(filter, cancellationToken);
    }
}
