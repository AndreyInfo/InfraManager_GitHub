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
public class UserSessionHistoriesController: ControllerBase
{
    private readonly ISessionHistoryBLL _service;

    public UserSessionHistoriesController(ISessionHistoryBLL service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<UserSessionHistoryDetails[]> ListAsync([FromQuery] BaseFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await _service.ListAsync(filter, cancellationToken);
    }
}