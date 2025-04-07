using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Import;
using InfraManager.ServiceBase.ImportService.LdapModels;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF.Services.ImportService;

[Route("bff/[controller]")]
[ApiController]
[Authorize]
public class ImportServiceController : BaseApiController
{
    private readonly IImportApi _importService;

    public ImportServiceController(IImportApi importService)
    {
        _importService = importService;
    }

    [HttpGet("ensure")]
    public Task<bool> EnsureAsync(CancellationToken cancellationToken = default)
    {
        return _importService.EnsureAsync(cancellationToken);
    }

    [HttpGet("Validate")]
    public async Task<FieldProtocol> ValidateAsync([FromQuery] Guid configurationID, [FromQuery] Guid settingsID,
        CancellationToken cancellationToken = default)
    {
        return await _importService.ValidateSettingsAsync(configurationID, settingsID, cancellationToken);
    }
}