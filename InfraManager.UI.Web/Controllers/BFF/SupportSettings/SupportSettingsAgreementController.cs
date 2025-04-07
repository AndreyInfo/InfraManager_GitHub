using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.BLL.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF.SupportSettings;

[Route("api/support-options/agreement")]
[ApiController]
[Authorize]
public class SupportSettingsAgreementController : ControllerBase
    { 
        private readonly ISupportSettingsBll _supportSettingsBll;

        public SupportSettingsAgreementController(ISupportSettingsBll supportSettingsBll)
        { 
            _supportSettingsBll = supportSettingsBll; 
        }
        
        [HttpGet]
        public SupportSettingsAgreementsModelDetails GetAgreementSettings() =>
            _supportSettingsBll.GetAgrementSettings();
        
        [HttpPut]
        public async Task UpdateAgreementAsync(SupportSettingsAgreementsModelDetails model, CancellationToken cancellationToken) =>
            await _supportSettingsBll.UpdateAgreementSettingsAsync(model, cancellationToken);
    
}