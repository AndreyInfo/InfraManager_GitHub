using System.Collections.Generic;
using InfraManager.Services.SearchService;

namespace InfraManager.BLL.ServiceDesk.Search
{
    public class SearchByTextParameters : ServiceDeskSearchParameters
    {
        public string Text { get; }
        public SearchHelper.SearchMode Mode { get; }
        public IReadOnlyList<string> Tags { get; }
        public bool ShouldSearchFinished { get; }

        public SearchByTextParameters(string text, SearchHelper.SearchMode mode, IReadOnlyList<ObjectClass> classes,
            IReadOnlyList<string> tags,
            bool shouldSearchFinished) : base(classes)
        {
            Text = text;
            Mode = mode;
            Tags = tags;
            ShouldSearchFinished = shouldSearchFinished;
        }
    }
}