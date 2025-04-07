using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.BLL.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF.SupportSettings;

[Route("api/support-options/notifications/agreement")]
[ApiController]
[Authorize]
public class SupportSettingsNotificationsOnAgreementController : ControllerBase
{
    private readonly ISupportSettingsBll _supportSettingsBll;

    public SupportSettingsNotificationsOnAgreementController(ISupportSettingsBll supportSettingsBll)
    {
        _supportSettingsBll = supportSettingsBll;
    }
    [HttpGet]
    public SpecialNotificationOnAgreement GetOnAgreement() =>
        _supportSettingsBll.GetNotificationOnAgreement();
    
    [HttpPut]
    public async Task UpdateOnAgreementAsync(SpecialNotificationOnAgreement specialNotification, CancellationToken cancellationToken) =>
        await _supportSettingsBll.UpdateNotificationOnAgreementAsync(specialNotification, cancellationToken);
    
}