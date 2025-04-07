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
    public class ModuleViewsController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        [HttpPost("{module}")]
        public async Task<CurrentFilterModel> AddOrUpdate(Module module, [FromQuery]string view)
        {
            var userId = _httpContextAccessor.HttpContext.GetUserId();

            throw new System.NotImplementedException();
        }
    }
}
