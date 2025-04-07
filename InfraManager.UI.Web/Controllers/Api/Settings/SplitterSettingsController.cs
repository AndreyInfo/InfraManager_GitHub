using InfraManager.BLL.Settings;
using InfraManager.UI.Web.Models.Settings;
using InfraManager.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IM.Core.HttpInfrastructure;

namespace InfraManager.UI.Web.Controllers.Api.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SplitterSettingsController : ControllerBase
    {
        private readonly ISplitterSettingsBLL _bllService;

        public SplitterSettingsController(ISplitterSettingsBLL bllService)
        {
            _bllService = bllService;
        }

        [HttpGet("{name}")]
        public async Task<SplitterSettingsModel> Get(string name)
        {
            var userId = HttpContext.GetUserId();
            var distance = await _bllService.GetAsync(userId, name);
            return new SplitterSettingsModel(userId, name)
            {
                Distance = distance
            };
        }

        [HttpPost("{name}")]
        public Task AddOrUpdate(string name, [FromQuery] int distance)
        {
            return _bllService.SetAsync(HttpContext.GetUserId(), name, distance);
        }

    }
}
