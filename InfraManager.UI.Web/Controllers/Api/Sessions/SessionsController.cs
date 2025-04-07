using System;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.DashboardCommon.Native.DashboardRestfulService;
using InfraManager.BLL.Sessions;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.UI.Web.Models.Sessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.Sessions;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SessionsController : ControllerBase
{
    private readonly ISessionBLL _service;

    public SessionsController(ISessionBLL service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<SessionDetails[]> ListAsync([FromQuery] SessionFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await _service.ListAsync(filter, cancellationToken);
    }

    [HttpPost("Deactivate")]
    public async Task DeactivateAsync([FromBody] DeactivateSessionRequest request,
        CancellationToken cancellationToken = default)
    {
        await _service.DeactivateSessionAsync(request.UserID, request.UserAgent, cancellationToken);
    }

    [HttpGet("Statistic")]
    public async Task<SessionsStatisticDetails> StatisticAsync(CancellationToken cancellationToken = default)
    {
        return await _service.SessionStatisticAsync(cancellationToken);
    }
}