using InfraManager.BLL.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FormSettingsController : ControllerBase
    {
        private readonly IFormSettingsBLL _service;

        public FormSettingsController(IFormSettingsBLL service)
        {
            _service = service;
        }

        [HttpGet("{name}")]
        public Task<WebUserFormSettingsModel> GetAsync(string name, CancellationToken cancellationToken = default)
        {
            return _service.GetAsync(name, cancellationToken);
        }

        [HttpPost("{name}")]
        public Task<WebUserFormSettingsModel> SetAsync(
            string name, 
            [FromBody]WebUserFormSettingsModel model, 
            CancellationToken cancellationToken = default)
        {
            return _service.SetAsync(name, model, cancellationToken);
        }
    }
}
