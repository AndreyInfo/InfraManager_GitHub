using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ISearchService = InfraManager.BLL.ServiceDesk.Search.ISearchService;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchServiceController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchServiceController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost("[action]")]
        public void RebuildIndex()
        {
            _searchService.RebuildIndex();
        }

        [HttpPost("[action]")]
        public void OptimizeIndex()
        {
            _searchService.OptimizeIndex();
        }

        [HttpGet("status")]
        public SearchServiceStatus GetStatus()
        {
            return _searchService.GetStatus();
        }
    }
}