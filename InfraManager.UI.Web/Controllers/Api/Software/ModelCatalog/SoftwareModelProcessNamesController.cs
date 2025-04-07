using InfraManager.BLL.Software.SoftwareModelProcessNames;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Software.ModelCatalog;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SoftwareModelProcessNamesController : BaseApiController
{
    private readonly ISoftwareModelProcessNameBLL _softwareModelProcessNameBLL;

    public SoftwareModelProcessNamesController(ISoftwareModelProcessNameBLL softwareModelProcessNameBLL)
    {
        _softwareModelProcessNameBLL = softwareModelProcessNameBLL;
    }

    [HttpGet]
    public async Task<SoftwareModelProcessNameDetails[]> GetListAsync([FromQuery] SoftwareModelProcessNameFilter filter, CancellationToken cancellationToken = default)
    {
        return await _softwareModelProcessNameBLL.GetListAsync(filter, cancellationToken);
    }

    [HttpPost]
    public async Task AddAsync([FromBody] SoftwareModelProcessNameData processNameData, CancellationToken cancellationToken = default)
    {
        await _softwareModelProcessNameBLL.AddProcessNameToSoftwareModelAsync(processNameData, cancellationToken);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromBody] SoftwareModelProcessNameData processNameData, CancellationToken cancellation = default)
    {
        await _softwareModelProcessNameBLL.DeleteProcessNameFromSoftwareModelAsync(processNameData, cancellation);
    }
}
