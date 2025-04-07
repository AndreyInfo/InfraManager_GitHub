using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.BLL.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF.SupportSettings;

    [Route("api/support-options/tasks")]
    [ApiController]
[Authorize]
public class SupportSettingsTasksController : ControllerBase
    {
        private readonly ISupportSettingsBll _supportSettingsBll;

        public SupportSettingsTasksController(ISupportSettingsBll supportSettingsBll)
        {
            _supportSettingsBll = supportSettingsBll;
        }

        [HttpGet]
        public ActionResult<SupportSettingsTasksModel> GetTasks() =>
            Ok(_supportSettingsBll.GetTasksSettings());

        [HttpPut]
        public async Task<ActionResult> UpdateTasks(SupportSettingsTasksModel supportSettingsTasksModel, CancellationToken cancellationToken = default) =>
            Ok(_supportSettingsBll.UpdateTasksSettings(supportSettingsTasksModel, cancellationToken));
    
}