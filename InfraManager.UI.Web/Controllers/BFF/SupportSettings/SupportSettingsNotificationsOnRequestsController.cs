using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.BLL.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF.SupportSettings;

[Route("api/support-options/notifications/requests")]
[ApiController]
[Authorize]
public class SupportSettingsNotificationsOnRequestsController : ControllerBase
{
    private readonly ISupportSettingsBll _supportSettingsBll;

    public SupportSettingsNotificationsOnRequestsController(ISupportSettingsBll supportSettingsBll)
    {
        _supportSettingsBll = supportSettingsBll;
    }
    [HttpGet]
    public SpecialNotificationOnRequest GetOnRequest() =>
        _supportSettingsBll.GetNotificationOnRequest();
    
    [HttpPut]
    public async Task UpdateOnRequestAsync(SpecialNotificationOnRequest specialNotification, CancellationToken cancellationToken) =>
        await _supportSettingsBll.UpdateNotificationOnRequestAsync(specialNotification, cancellationToken);
}