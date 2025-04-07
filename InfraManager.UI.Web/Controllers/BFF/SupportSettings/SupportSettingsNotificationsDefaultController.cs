using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using InfraManager.DAL.Notification;
using System;
using InfraManager.BLL.Settings.DefaultNotifications;

namespace InfraManager.UI.Web.Controllers.BFF.SupportSettings;

[Authorize]
[ApiController]
[Route("api/support-options/notifications/default")]
public class SupportSettingsNotificationsDefaultController : ControllerBase
{
    private readonly ISupportSettingsNotificationsDefaultBLL _supportSettingsNotificationsDefaultBLL;

    public SupportSettingsNotificationsDefaultController(ISupportSettingsNotificationsDefaultBLL supportSettingsNotificationsDefaultBLL)
    {
        _supportSettingsNotificationsDefaultBLL = supportSettingsNotificationsDefaultBLL;
    }

    [HttpGet("nodes/classes")]
    public async Task<NodeNotificationDefaultDetails<ObjectClass>[]> GetClassesAsync(CancellationToken cancellationToken)
    {
        return await _supportSettingsNotificationsDefaultBLL.GetClassNodesAsync(cancellationToken);
    }

    [HttpGet("nodes/businessrole")]
    public async Task<NodeNotificationDefaultDetails<BusinessRole>[]> GetBissnesRoleAsync([FromQuery] ObjectClass classID, CancellationToken cancellationToken)
    {
        return await _supportSettingsNotificationsDefaultBLL.GetBusinessRoleNodesAsync(classID, cancellationToken);
    }

    [HttpGet("nodes/notifications")]
    public async Task<NodeNotificationDefaultDetails<Guid>[]> GetNotificationAsync([FromQuery] ObjectClass classID, [FromQuery] BusinessRole bisnuesRole, CancellationToken cancellationToken)
    {
        return await _supportSettingsNotificationsDefaultBLL.GetNotificationNodesAsync(classID, bisnuesRole, cancellationToken);
    }

    [HttpPut("checked/{notificationID}/{businessRole}")]
    public async Task ChekedDefaultNotification([FromRoute] Guid notificationID, [FromRoute] BusinessRole businessRole, CancellationToken cancellationToken)
    {
        await _supportSettingsNotificationsDefaultBLL.ToDefaultBuisnessRoleAsync(notificationID, businessRole, cancellationToken);
    }

    [HttpPut("nocheked/{notificationID}/{businessRole}")]
    public async Task NoChekedDefaultNotification([FromRoute] Guid notificationID, [FromRoute] BusinessRole businessRole, CancellationToken cancellationToken)
    {
        await _supportSettingsNotificationsDefaultBLL.ToNoDefaultBuisnessRoleAsync(notificationID, businessRole, cancellationToken);
    }
}
