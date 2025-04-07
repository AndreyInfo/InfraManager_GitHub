using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.Asset.dto;
using InfraManager.BLL.ServiceDesk;
using System.Threading;

namespace InfraManager.UI.Web.Controllers.Api
{
    [Authorize]
    [ApiController]
    [Route("api/influence/")]
    public class InfluenceController : ControllerBase
    {
        private readonly IInfluenceBLL _influenceBLL;

        public InfluenceController(IInfluenceBLL influenceBLL)
        {
            _influenceBLL = influenceBLL;
        }


        [HttpGet("all")]
        public async Task<InfluenceDetails[]> GetAllAsync()
        {
            return await _influenceBLL.GetAllInfluenceAsync();
        }


        [HttpDelete("{id}")]
        public async Task RemoveByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _influenceBLL.RemoveByIdAsync(id, cancellationToken);
        }


        [HttpPost("save/item")]
        public async Task<bool> SaveAsync([FromBody] InfluenceDetails details)
        {
            return await _influenceBLL.SaveAsync(details);

        }
    }
}
