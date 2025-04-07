using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.BLL.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF.SupportSettings;

    [Route("api/support-options/problems")]
    [ApiController]
[Authorize]
public class SupportSettingsProblemsController : ControllerBase
    {
        private readonly ISupportSettingsBll _supportSettingsBll;
    
        public SupportSettingsProblemsController(ISupportSettingsBll supportSettingsBll)
        {
            _supportSettingsBll = supportSettingsBll;
        }

        [HttpGet]
        public ActionResult<SupportSettingsProblemsModel> GetProblems() =>
            Ok(_supportSettingsBll.GetProblemsSettings());

        [HttpPut]
        public async Task<ActionResult> UpdateProblems(SupportSettingsProblemsModel supportSettingsProblemsModel, CancellationToken cancellationToken = default) =>
            Ok(_supportSettingsBll.UpdateProblemsSettings(supportSettingsProblemsModel, cancellationToken));
    
    }