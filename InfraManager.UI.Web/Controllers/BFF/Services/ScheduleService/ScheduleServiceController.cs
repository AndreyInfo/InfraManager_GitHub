using InfraManager.ServiceBase.MailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.ServiceBase.ScheduleService;
using InfraManager.Services.ScheduleService;

namespace InfraManager.UI.Web.Controllers.BFF.Services.ScheduleService;

[Route("bff/[controller]")]
[ApiController]
[Authorize]
public class ScheduleServiceController : ControllerBase
{
    private readonly IScheduleServiceWebApi _scheduleService;

    public ScheduleServiceController(IScheduleServiceWebApi mailService)
    {
        _scheduleService = mailService;
    }

    [HttpGet("ensure")]
    public Task<bool> EnsureAsync(CancellationToken cancellationToken = default)
    {
        return _scheduleService.EnsureAsync(cancellationToken);
    }
}