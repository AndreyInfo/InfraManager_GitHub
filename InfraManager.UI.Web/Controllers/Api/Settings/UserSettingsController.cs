using InfraManager.BLL.Settings;
using InfraManager.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using IM.Core.HttpInfrastructure;

namespace InfraManager.UI.Web.Controllers.Api.Settings
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserSettingsController : ControllerBase
    {
        private readonly IWebUserSettingsBLL _service;

        public UserSettingsController(IWebUserSettingsBLL service)
        {
            _service = service;
        }

        [HttpGet]
        public ValueTask<WebUserSettings> Get(CancellationToken cancellationToken = default)
        {
            return _service.GetAsync(HttpContext.GetUserId(), cancellationToken);
        }

        [HttpPost]
        public Task Set([FromBody]WebUserSettings data, CancellationToken cancellationToken = default)
        {
            return _service.SetAsync(HttpContext.GetUserId(), data, cancellationToken);
        }

        [HttpPost("reset")]
        public Task Reset(CancellationToken cancellationToken = default)
        {
            return _service.ResetAsync(HttpContext.GetUserId(), cancellationToken);
        }
    }
}
