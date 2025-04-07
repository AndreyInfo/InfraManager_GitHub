using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.BLL.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF.SupportSettings;

[Route("api/support-options/general")]
[ApiController]
[Authorize]
public class SupportSettingsGeneralController : ControllerBase
{
    private readonly ISupportSettingsBll _supportSettingsBll;

    public SupportSettingsGeneralController(ISupportSettingsBll supportSettingsBll)
    {
        _supportSettingsBll = supportSettingsBll;
    }

    [HttpGet]
    public SupportSettingsGeneralModel GetGeneral() =>
        _supportSettingsBll.GetGeneralSettings();

    [HttpPut]
    public async Task UpdateGeneral(SupportSettingsGeneralModel supportSettingsGeneralModel, CancellationToken cancellationToken = default) =>
        await _supportSettingsBll.UpdateGeneralSettings(supportSettingsGeneralModel, cancellationToken);

}