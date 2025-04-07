using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.BLL.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF.SupportSettings;

    [Route("api/support-options/requests")]
    [ApiController]
[Authorize]
public class SupportSettingsRequestsController : ControllerBase
    {
        private readonly ISupportSettingsBll _supportSettingsBll;

        public SupportSettingsRequestsController(ISupportSettingsBll supportSettingsBll)
        {
            _supportSettingsBll = supportSettingsBll;
        }

        [HttpGet]
        public async Task<SupportSettingsRequestsModel> GetRequestsAsync(CancellationToken cancellationToken) =>
            await _supportSettingsBll.GetRequestsSettingsAsync(cancellationToken);
        
        [HttpPut]
        public async Task UpdateRequestsAsync(SupportSettingsRequestsModel settings, CancellationToken cancellationToken) =>
            await _supportSettingsBll.UpdateRequestsSettingsAsync(settings, cancellationToken);
}