using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Sessions;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.Sessions;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserPersonalLicencesController : ControllerBase
{
    private readonly IUserPersonalLicenceBLL _service;

    public UserPersonalLicencesController(IUserPersonalLicenceBLL service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<UserPersonalLicenceDetails[]> ListAsync([FromQuery] BaseFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await _service.ListAsync(filter, cancellationToken);
    }

    [HttpPost]
    public async Task PostAsync([FromBody] UserPersonalLicenceData request, CancellationToken cancellationToken = default)
    {
        await _service.InsertAsync(request.UserID, cancellationToken);
    }

    [HttpDelete("{userID}")]
    public async Task DeleteAsync([FromRoute] Guid userID, CancellationToken cancellationToken = default)
    {
        await _service.DeleteAsync(userID, cancellationToken);
    }
}