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
    public class ColumnSettingsController : ControllerBase
    {
        private readonly IUserColumnSettingsBLL _service;

        public ColumnSettingsController (IUserColumnSettingsBLL service)
        {
            _service = service;
        }

        [HttpGet("{view}")]
        public Task<ColumnSettings[]> GetAsync(string view, CancellationToken cancellationToken = default)
        {
            return _service.GetAsync(view, cancellationToken);
        }

        [HttpPost("{view}")]
        public Task PostAsync(string view, [FromBody] ColumnSettings[] columns, CancellationToken cancellationToken = default)
        {
            return _service.SetAsync(view, columns, cancellationToken);
        }
    }
}
