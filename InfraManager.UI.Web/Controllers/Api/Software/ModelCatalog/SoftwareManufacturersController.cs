using InfraManager.BLL.Software.SoftwareManufacturers;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Software.ModelCatalog;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SoftwareManufacturersController : BaseApiController
{
    private readonly ISoftwareManufacturerBLL _softwareManufacturerBLL;

    public SoftwareManufacturersController(ISoftwareManufacturerBLL softwareManufacturerBLL)
    {
        _softwareManufacturerBLL = softwareManufacturerBLL;
    }

    [HttpGet]
    public async Task<SoftwareManufacturerDetails[]> GetListAsync(CancellationToken cancellationToken)
    {
        return await _softwareManufacturerBLL.GetListAsync(cancellationToken);
    }
}
