using InfraManager.BLL;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.BLL.Software.SoftwareTypes;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Software.ModelCatalog;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SoftwareTypesController : BaseApiController
{
    private readonly ISoftwareTypeBLL _softwareTypeBLL;

	public SoftwareTypesController(ISoftwareTypeBLL softwareTypeBLL)
	{
		_softwareTypeBLL= softwareTypeBLL;
	}

	[HttpGet]
	public async Task<SoftwareTypeDetails[]> GetListAsync([FromQuery] LookupListFilter filter, CancellationToken cancellationToken = default)
	{
		return await _softwareTypeBLL.GetListAsync(filter, cancellationToken);
	}
}
