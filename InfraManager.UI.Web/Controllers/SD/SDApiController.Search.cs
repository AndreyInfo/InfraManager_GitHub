using InfraManager.Core.Logging;
using InfraManager.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdvancedSearchMode = InfraManager.Services.SearchService.SearchHelper.AdvancedSearchMode;
using SearchMode = InfraManager.Services.SearchService.SearchHelper.SearchMode;
using SearchResult = InfraManager.Web.BLL.Search.SearchResult;

namespace InfraManager.Web.Controllers.SD
{
    partial class SDApiController
    {
        public sealed class SearchParams
        {
            public string FormID { get; set; }

            //
            public string SearchText { get; set; }
            public int Mode { get; set; }
            public List<int> Classes { get; set; }
            public string Tags { get; set; }
            public bool SearchFinished { get; set; }
            public int AdvancedSearchMode { get; set; }

            public string ViewName { get; set; }

            //
            public bool LoadOnClient { get; set; }
            public int ClassID { get; set; } //ИД класса для подгрузки результатов поиска на клиент

            public int StartIdx { get; set; } //индекс для подгрузки результатов поиска на клиент

            //
            public Guid CallClientID { get; set; } //ИД клиента заявки

            //
            public bool FindNotBound { get; set; }
        }

        [HttpPost]
        [Route("sdApi/GetFoundObjects", Name = "GetFoundObjects")]
        public async Task<IReadOnlyList<SearchResult>> GetFoundObjects(SearchParams searchParams, CancellationToken cancellationToken = default)
        {
            var user = CurrentUser;

            Logger.Trace(
                "SDApiController.GetFoundObjects SearchText={0}, Mode={1}, Classes={2}, Tags={3}, SearchFinishedOnly={4}, AdvancedSearchMode={5}, ViewName={6}, LoadOnClient={7}, ClassID={8}, StartIdx={9}, CallClientID={10}, UserID={11}, UserName={12}",
                searchParams.SearchText, searchParams.Mode, searchParams.Classes, searchParams.Tags,
                searchParams.SearchFinished,
                searchParams.AdvancedSearchMode, searchParams.ViewName, searchParams.LoadOnClient, searchParams.ClassID,
                searchParams.StartIdx, searchParams.CallClientID, user.User.ID, user.User.Name);

            if (string.IsNullOrEmpty(searchParams.SearchText)
                && string.IsNullOrEmpty(searchParams.Tags)
                && !searchParams.LoadOnClient
                && !searchParams.FindNotBound)
                return new List<SearchResult>();

            try
            {
                var searchText = GetSearchText(searchParams.SearchText);
                if (searchText == "null") searchText = string.Empty;
                var searchMode = GetSearchMode(searchText, searchParams.Mode);
                var advancedSearchMode = (AdvancedSearchMode)searchParams.AdvancedSearchMode;
                if (advancedSearchMode == AdvancedSearchMode.SearchInCurrentList &&
                    string.IsNullOrEmpty(searchParams.ViewName))
                    throw new InvalidOperationException("viewName");

                var tags = GetTags(searchParams.Tags, searchText);
                var classes = ApplicationUser.GetClasses(searchParams.Classes, searchParams.ViewName);
                return searchParams.LoadOnClient
                    ? await _serviceDeskSearchService.LoadSearchResultAsync(searchParams.FormID, searchText, searchMode,
                            classes,
                            tags, advancedSearchMode,
                            searchParams.SearchFinished, 
                            searchParams.FindNotBound,
                            searchParams.ViewName, user.User.ID, cancellationToken)
                        .ConfigureAwait(false)
                    : await _serviceDeskSearchService.SearchAsync(searchParams.FormID, searchText, searchMode, classes,
                            tags, advancedSearchMode,
                            searchParams.SearchFinished, 
                            searchParams.FindNotBound,
                            searchParams.ViewName,  user.User.ID, cancellationToken)
                        .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        #region private method GetSearchText

        private string GetSearchText(string text)
        {
            return text != null ? System.Web.HttpUtility.UrlDecode(text) : string.Empty;
        }

        #endregion

        #region private method GetTags

        private string[] GetTags(string tags, string searchText)
        {
            var tagList = !string.IsNullOrEmpty(tags) ? tags.Split(',').ToList() : new List<string>();
            //
            var textTags = searchText.Split(' ').Where(x => x.Length != 0 && x[0] == '#').Select(x => x.Remove(0, 1))
                .ToList();
            //
            if (textTags.Count != 0)
                tagList.AddRange(textTags);
            return tagList.ToArray();
        }

        #endregion

        #region private method GetSearchMode

        private SearchMode GetSearchMode(string searchText, int searchMode)
        {
            SearchMode mode;
            if (searchMode == 0)
            {
                int number;
                mode = int.TryParse(searchText, out number) ? SearchMode.Number : SearchMode.Context;
            }
            else
                mode = (SearchMode)searchMode;

            //
            return mode;
        }

        #endregion
    }
}