using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using System.Threading;

namespace InfraManager.UI.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InfluencesController : ControllerBase
    {
        private readonly ILookupBLL<InfluenceListItemModel, InfluenceDetailsModel, InfluenceModel, Guid> _influenceBLL;

        public InfluencesController(
            ILookupBLL<InfluenceListItemModel, InfluenceDetailsModel, InfluenceModel, Guid> influenceBLL)
        {
            _influenceBLL = influenceBLL;
        }

        [HttpGet]
        public Task<InfluenceListItemModel[]> GetListAsync(CancellationToken cancellationToken = default)
        {
            return _influenceBLL.ListAsync(cancellationToken);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _influenceBLL.DeleteAsync(id, cancellationToken);
        }
        
        [HttpPost]
        public Task<InfluenceDetailsModel> PostAsync(
            [FromBody]InfluenceModel model, 
            CancellationToken cancellationToken = default)
        {
            return _influenceBLL.AddAsync(model, cancellationToken);
        }

        [HttpPut("id")]
        public Task<InfluenceDetailsModel> PutAsync(
            Guid id,
            [FromBody] InfluenceModel model,
            CancellationToken cancellationToken = default)
        {
            return _influenceBLL.UpdateAsync(id, model, cancellationToken);
        }

        [HttpGet("{id}")]
        public Task<InfluenceDetailsModel> GetAsync(
            [FromRoute]Guid id,
            CancellationToken cancellationToken = default)
        {
            return _influenceBLL.FindAsync(id, cancellationToken);
        }
    }
}
