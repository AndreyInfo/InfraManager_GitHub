using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk.Search;

namespace InfraManager.UI.Web.Services.Search
{
    internal class SearchByTextStrategy : IServiceDeskSearchStrategy<SearchByTextParameters>
    {
        private readonly ISearchService _searchService;

        public SearchByTextStrategy(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public Task<IReadOnlyList<FoundObject>> SearchAsync(SearchByTextParameters searchParameters, CancellationToken cancellationToken = default)
        {
            return _searchService.SearchAsync(searchParameters.Text, searchParameters.Mode, searchParameters.Classes,
                searchParameters.Tags,
                searchParameters.ShouldSearchFinished, cancellationToken);
        }
    }
}