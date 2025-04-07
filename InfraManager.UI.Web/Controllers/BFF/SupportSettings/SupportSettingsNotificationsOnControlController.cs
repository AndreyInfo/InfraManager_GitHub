using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.BLL.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF.SupportSettings;

[Route("api/support-options/notifications/control")]
[ApiController]
[Authorize]
public class SupportSettingsNotificationsOnControlController : ControllerBase
{
    private readonly ISupportSettingsBll _supportSettingsBll;

    public SupportSettingsNotificationsOnControlController(ISupportSettingsBll supportSettingsBll)
    {
        _supportSettingsBll = supportSettingsBll;
    }
    [HttpGet]
    public SpecialNotificationOnControl GetOnControl() => 
        _supportSettingsBll.GetNotificationOnControl();

    [HttpPut]
    public async Task UpdateOnControlAsync(SpecialNotificationOnControl specialNotification, CancellationToken cancellationToken) =>
        await _supportSettingsBll.UpdateNotificationOnControlAsync(specialNotification, cancellationToken);
}