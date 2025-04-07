using InfraManager.BLL;
using InfraManager.UI.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchRequestsController : ControllerBase
    {
        private readonly IServiceMapper<string, IObjectSearcher> _searchers;

        public SearchRequestsController(IServiceMapper<string, IObjectSearcher> searchers)
        {
            _searchers = searchers;
        }

        [HttpPost("{searcher}")]
        public async Task<ActionResult<ObjectSearchResult[]>> SearchAsync(
            string searcher, 
            [FromBody] SearchCriteria searchCriteria, 
            CancellationToken cancellationToken = default)
        {
            if (!_searchers.HasKey(searcher))
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            var particularSearcher = _searchers.Map(searcher);
            return await particularSearcher.SearchAsync(searchCriteria.Content, cancellationToken);
        }
    }
}
