using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.ServiceDesk.Search;
using InfraManager.Core.Logging;
using InfraManager.DAL;
using InfraManager.IM.BusinessLayer.SearchService;
using InfraManager.Services.SearchService;
using Microsoft.Extensions.Logging;
using Document = InfraManager.Services.SearchService.Document;
using ISearchService = InfraManager.BLL.ServiceDesk.Search.ISearchService;
using FoundObject = InfraManager.BLL.ServiceDesk.Search.FoundObject;

namespace InfraManager.UI.Web.Services.Search
{
    public class SearchServiceProxy : ISearchService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<SearchServiceProxy> _logger;

        public SearchServiceProxy(IMapper mapper, ILogger<SearchServiceProxy> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public Task<IReadOnlyList<FoundObject>> SearchAsync(string searchText, SearchHelper.SearchMode mode,
            IReadOnlyList<ObjectClass> classes, IReadOnlyList<string> tags, bool shouldSearchFinished,
            CancellationToken cancellationToken = default)
        {
            if (!SearchManager.IsConnected)
            {
                SearchManager.Connect();
            }

            if (!SearchManager.IsConnected)
            {
                Logger.Trace("SearchService unavailable");
                return null;
            }

            var searchFields = GetSearchFields(mode);

            var queryList = new List<QueryItem> { new(searchText, searchFields) };
            var tagList = new List<string>();
            if (tags != null)
                tagList.AddRange(tags);

            var result = SearchManager.Search(queryList, tagList, classes.Select(c => (int)c).ToList(),
                shouldSearchFinished, mode);
            var searchItems = (IReadOnlyList<FoundObject>)result.Select(ToFoundObject).ToArray();
            return Task.FromResult(searchItems);
        }

        public void Update<T, TNote>(T entity, TNote[] notes) where T : IGloballyIdentifiedEntity
        {
            var document = _mapper.Map(entity, _mapper.Map<Document>(notes));
            Update(entity.IMObjID, document);
        }

        public void Update<T>(T entity) where T : IGloballyIdentifiedEntity
        {
            var document = _mapper.Map<Document>(entity);
            Update(entity.IMObjID, document);
        }

        public void Insert<T>(T entity) where T : IGloballyIdentifiedEntity
        {
            var document = _mapper.Map<Document>(entity);
            Insert(entity.IMObjID, document);
        }

        public void RebuildIndex()
        {
            if (!SearchManager.RebuildIndex())
            {
                throw new InvalidOperationException("Ошибка при перестроении индекса");
            }
        }

        public void OptimizeIndex()
        {
            if (!SearchManager.OptimizeIndex())
            {
                throw new InvalidOperationException("Ошибка при оптимизации индекса");
            }
        }

        public SearchServiceStatus GetStatus()
        {
            var rebuilding = SearchManager.IndexIsRebuilding();
            var optimizing = SearchManager.IndexIsOptimizing();
            
            return new SearchServiceStatus
            {
                LastIndexRebuild = SearchManager.GetLastIndexCreationDate(),
                CurrentTask = rebuilding ? SearchServiceTask.IndexRebuilding :
                    optimizing ? SearchServiceTask.IndexOptimizing : SearchServiceTask.Idle
            };
        }

        private void Update(Guid id, Document document)
        {
            try
            {
                SearchManager.UpdateDocument(id, document);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on update search service");
            }
        }

        private void Insert(Guid id, Document document)
        {
            try
            {
                SearchManager.InsertDocument(id, document);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on insert into search service");
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                SearchManager.DeleteDocument(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on delete from search service");
            }
        }

        private string[] GetSearchFields(SearchHelper.SearchMode mode)
        {
            switch (mode)
            {
                case SearchHelper.SearchMode.Number:
                    return new[] { SearchHelper.LuceneNumber };
                case SearchHelper.SearchMode.Context:
                    return SearchHelper.FieldNamesContextSearch;
                case SearchHelper.SearchMode.User:
                    return SearchHelper.FieldNamesUserSearch;
                case SearchHelper.SearchMode.CallClient:
                    return SearchHelper.FieldNamesCallClientSearch;
                default:
                    throw new NotSupportedException("mode");
            }
        }

        private FoundObject ToFoundObject(InfraManager.Services.SearchService.FoundObject obj) =>
            new()
            {
                ClassID = (ObjectClass)obj.ClassID,
                Name = obj.Name,
                Number = obj.Number,
                EntityStateName = obj.EntityStateName,
                ClientFullName = obj.ClientFullName,
                ClientID = obj.ClientID,
                Description = obj.Description,
                ExecutorFullName = obj.ExecutorFullName,
                ExecutorID = obj.ExecutorID,
                ID = obj.ID,
                UtcDateModified = obj.UtcDateModified,
                UtcDatePromised = obj.UtcDatePromised,
                OwnerID = obj.OwnerID
            };
    }
}