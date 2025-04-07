using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.BLL.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF.SupportSettings;

[Route("api/support-options/notifications/replacement")]
[ApiController]
[Authorize]
public class SupportSettingsNotificationsOnReplacementController : ControllerBase
{
    private readonly ISupportSettingsBll _supportSettingsBll;

    public SupportSettingsNotificationsOnReplacementController(ISupportSettingsBll supportSettingsBll)
    {
        _supportSettingsBll = supportSettingsBll;
    }
    [HttpGet]
    public SpecialNotificationOnReplacement GetOnReplacement() =>
        _supportSettingsBll.GetNotificationOnReplacement();

    [HttpPut]
    public async Task UpdateOnReplacementAsync(SpecialNotificationOnReplacement specialNotification, CancellationToken cancellationToken) =>
        await _supportSettingsBll.UpdateNotificationOnReplacementAsync(specialNotification, cancellationToken);
}