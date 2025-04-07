using InfraManager.BLL.Software.SoftwareModelDependencies;
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
public class SoftwareModelDependenciesController : BaseApiController
{
    private readonly ISoftwareModelDependencyBLL _softwareModelDependencyBLL;

    public SoftwareModelDependenciesController(ISoftwareModelDependencyBLL softwareModelDependencyBLL)
    {
        _softwareModelDependencyBLL = softwareModelDependencyBLL;
    }

    [HttpPost]
    public async Task<SoftwareModelDependencyDetails> AddAsync([FromBody] SoftwareModelDependencyData model, CancellationToken cancellationToken)
    {
        return await _softwareModelDependencyBLL.AddAsync(model, cancellationToken);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] SoftwareModelDependencyData model, CancellationToken cancellationToken)
    {
        await _softwareModelDependencyBLL.DeleteAsync(model, cancellationToken);
    }
}
