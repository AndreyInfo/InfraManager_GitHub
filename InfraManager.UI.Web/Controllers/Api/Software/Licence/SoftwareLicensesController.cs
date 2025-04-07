using InfraManager.BLL;
using InfraManager.BLL.Software.SoftwareLicenses;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Software.Licence;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SoftwareLicensesController : BaseApiController
{
    private readonly ISoftwareLicenseBLL _softwareLicenseBLL;

    public SoftwareLicensesController(ISoftwareLicenseBLL softwareLicenseBLL)
    {
        _softwareLicenseBLL = softwareLicenseBLL;
    }

    [HttpGet]
    public async Task<SoftwareLicenseDetails[]> GetListAsync([FromQuery] LookupListFilter filter, CancellationToken cancellationToken)
    {
        return await _softwareLicenseBLL.GetListAsync(filter, cancellationToken);
    }

}
