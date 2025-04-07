using InfraManager.ServiceBase.SearchService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Services.SearchService
{
    [Route("bff/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchServiceController : ControllerBase
    {
        private readonly ISearchServiceApi _searchService;

        public SearchServiceController(ISearchServiceApi searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("ensure")]
        public Task<bool> EnsureAsync(CancellationToken cancellationToken = default)
        {
            return _searchService.EnsureAsync(cancellationToken);
        }
    }
}
