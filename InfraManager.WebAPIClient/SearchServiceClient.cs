using InfraManager.Core.Data;
using InfraManager.ServiceBase.SearchService;
using InfraManager.ServiceBase.SearchService.WebAPIModels;
using InfraManager.Services;
using InfraManager.Services.SearchService;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.WebAPIClient
{
    public class SearchServiceClient : WebAPIBaseClient, ISearchService, ISearchServiceApi
    {

        public SearchServiceClient(string baseUrl) 
            : base(baseUrl)
        {
        }

        #region ISearchServiceApi
        public async Task<bool> EnsureAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await GetAsync<OperationResult>(
                                         "/searchservice/Ensure",
                                         preProcessHeader: null,
                                         cancellationToken: cancellationToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        public OperationResult DeleteDocument(DataSourceLink dataSourceLink, Guid documentID)
        {
            var task = PostAsync<OperationResult, SearchServiceIDRequest>("searchservice/delete", new SearchServiceIDRequest() 
            { 
                DataSourceLink = dataSourceLink,
                DocumentID = documentID,
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call SearchService" };
        }

        public OperationResult EnsureAvailability()
        {
            var task = GetAsync<OperationResult>("searchservice/ensure");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public bool IndexIsRebuilding(DataSourceLink dataSourceLink)
        {
            var task = PostAsync<bool, SearchServiceRequest>("searchservice/index-is-rebuilding", new SearchServiceRequest() 
            { 
                DataSourceLink = dataSourceLink,
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return false;
        }

        public bool IndexIsOptimizing(DataSourceLink dataSourceLink)
        {
            var task = PostAsync<bool, SearchServiceRequest>("searchservice/index-is-optimizing", new SearchServiceRequest 
            { 
                DataSourceLink = dataSourceLink,
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return false;
        }

        public DateTime? GetLastIndexRebuild(DataSourceLink dataSourceLink)
        {
            var task = PostAsync<DateTime?, SearchServiceRequest>("searchservice/last-index-rebuild", new SearchServiceRequest 
            { 
                DataSourceLink = dataSourceLink,
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return null;
        }

        public OperationResult InsertDocument(DataSourceLink dataSourceLink, Document document)
        {
            var task = PostAsync<OperationResult, SearchServiceDocumentRequest>("searchservice/insert", new SearchServiceDocumentRequest() 
            { 
                DataSourceLink = dataSourceLink,
                Document = new DocumentModel(document),
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call SearchService" };
        }

        public OperationResult OptimizeIndex(DataSourceLink dataSourceLink)
        {
            var task = PostAsync<OperationResult, SearchServiceRequest>("searchservice/optimize", new SearchServiceRequest
            { 
                DataSourceLink = dataSourceLink,
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call SearchService" };
        }

        public OperationResult RebuildIndex(DataSourceLink dataSourceLink)
        {
            var task = PostAsync<OperationResult, SearchServiceRequest>("searchservice/rebuild", new SearchServiceRequest
            { 
                DataSourceLink = dataSourceLink,
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call SearchService" };
        }

        public OperationResult Search(DataSourceLink dataSourceLink, string query, string[] fieldNames, out List<Guid> kbArticles)
        {
            var task = PostAsync<SearchServiceSearchResult, SearchServiceRequest>("searchservice/search", new SearchServiceSearchRequest() 
            { 
                DataSourceLink = dataSourceLink,
                FieldNames = fieldNames,
                query = query,
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                var result = task.Result;
                kbArticles = result.Articles;
                return result.Result;
            }
            kbArticles = null;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call SearchService" };
        }

        public OperationResult SearchGeneral(DataSourceLink dataSourceLink, List<QueryItem> queryList, List<string> tagList, List<int> classList, bool searchFinished, SearchHelper.SearchMode searchMode, out List<FoundObject> foundList)
        {
            var task = PostAsync<SearchServiceGeneralResult, SearchServiceSearchGeneralRequest>("searchservice/search-general", new SearchServiceSearchGeneralRequest() 
            { 
                DataSourceLink = dataSourceLink,
                ClassList = classList,
                QueryList = queryList,
                SearchFinished = searchFinished,
                SearchMode = searchMode,
                TagList = tagList,
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                var result = task.Result;
                foundList = result.FoundObjects;
                return result.Result;
            }
            foundList = null;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call SearchService" };
        }

        public OperationResult Subscribe(Guid applicationID)
        {
            var task = PostAsync<OperationResult, Guid>("searchservice/subscribe", applicationID);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call SearchService" };
        }

        public OperationResult Unsubscribe(Guid applicationID)
        {
            var task = PostAsync<OperationResult, Guid>("searchservice/unsubscribe", applicationID);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call SearchService" };
        }

        public OperationResult UpdateDocument(DataSourceLink dataSourceLink, Document document)
        {
            var task = PostAsync<OperationResult, SearchServiceDocumentRequest>("searchservice/update", new SearchServiceDocumentRequest()
            {
                DataSourceLink = dataSourceLink,
                Document = new DocumentModel(document),
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call SearchService" };
        }
    }
}
