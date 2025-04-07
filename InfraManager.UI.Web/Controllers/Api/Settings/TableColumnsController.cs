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
    public class TableColumnsController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        [HttpGet("{view}")]
        public async Task<TableColumnSettingsModel[]> Get(string view)
        {
            var userId = _httpContextAccessor.HttpContext.GetUserId();

            throw new System.NotImplementedException();
        }

        [HttpPost("{view}")]
        public async Task AddOrUpdate(string view, [FromBody]TableColumnSettingsModel[] model)
        {
            var userId = _httpContextAccessor.HttpContext.GetUserId();

            throw new System.NotImplementedException();
        }
    }
}
