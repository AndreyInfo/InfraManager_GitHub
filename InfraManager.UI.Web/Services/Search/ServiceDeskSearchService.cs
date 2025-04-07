using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL;
using InfraManager.BLL.AccessManagement;
using Inframanager.BLL.ListView;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.BLL.ServiceDesk.CustomControl;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.BLL.ServiceDesk.Negotiations;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.BLL.ServiceDesk.Search;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk;
using InfraManager.Services.SearchService;
using SearchResult = InfraManager.Web.BLL.Search.SearchResult;
using PLFoundObject = InfraManager.Web.BLL.Search.FoundObject;
using FoundObject = InfraManager.BLL.ServiceDesk.Search.FoundObject;

namespace InfraManager.UI.Web.Services.Search
{
    public class ServiceDeskSearchService
    {
        private const int MaxItemsToReturn = 8;
        private readonly IServiceDeskSearchCache _searchCache;
        private readonly ICustomControlBLL _customControlBLL;
        private readonly IReadNegotiationBLL _negotiationBLL;
        private readonly ITableFiltersBLL _filters;
        private readonly IMapper _mapper;

        private readonly ILightSearcher _service;
        private readonly IValidateObjectPermissions<Guid, KBArticle> _kbArticleAccessValidator;
        private readonly IObjectAccessBLL _accessManager;
        private readonly IServiceDeskSearchStrategy<SearchByNumberParameters> _searchByNumberStrategy;
        private readonly IServiceDeskSearchStrategy<SearchByTextParameters> _searchByTextStrategy;
        private readonly IServiceDeskSearchStrategy<SearchNotBoundParameters> _searchNotBoundStrategy;
        private readonly IWorkOrderBLL _woBll;
        private readonly IProblemBLL _problemBll;
        private readonly ICallBLL _callBll;
        private readonly IMassIncidentBLL _massiveIncidentBll;
        private readonly IValidateObjectPermissions<Guid, Problem> _problemAccessValidator;

        public ServiceDeskSearchService(IValidateObjectPermissions<Guid, KBArticle> kbArticleAccessValidator,
            IObjectAccessBLL accessManager,
            IServiceDeskSearchStrategy<SearchByNumberParameters> searchByNumberStrategy,
            IServiceDeskSearchStrategy<SearchByTextParameters> searchByTextStrategy,
            IServiceDeskSearchStrategy<SearchNotBoundParameters> searchNotBoundStrategy,
            IWorkOrderBLL woBll,
            IProblemBLL problemBll,
            ICallBLL callBll,
            IServiceDeskSearchCache searchCache,
            ICustomControlBLL customControlBLL,
            IReadNegotiationBLL negotiationBLL,
            IMassIncidentBLL massiveIncidentBLL,
            ITableFiltersBLL filters,
            IMapper mapper,
            IValidateObjectPermissions<Guid, Problem> problemAccessValidator,
            ILightSearcher service)
        {
            _searchCache = searchCache;
            _customControlBLL = customControlBLL;
            _negotiationBLL = negotiationBLL;
            _filters = filters;
            _mapper = mapper;
            _kbArticleAccessValidator = kbArticleAccessValidator;
            _accessManager = accessManager;
            _searchByNumberStrategy = searchByNumberStrategy;
            _searchByTextStrategy = searchByTextStrategy;
            _searchNotBoundStrategy = searchNotBoundStrategy;
            _woBll = woBll;
            _problemBll = problemBll;
            _callBll = callBll;
            _massiveIncidentBll = massiveIncidentBLL;
            _problemAccessValidator = problemAccessValidator;
            _service = service;
        }

        /// <summary>
        /// Получение следующей части результата поиска
        /// </summary>
        /// <param name="searchKey">Ключ, с которым результаты поиска были записаны в кэш</param>
        /// <param name="searchText">Текст для поиска</param>
        /// <param name="mode">Тип поиска</param>
        /// <param name="classes">Типы объектов</param>
        /// <param name="tags">Тэги</param>
        /// <param name="advancedSearchMode">Область поиска</param>
        /// <param name="shouldSearchFinished">Должен ли результат содержать объекты с  заверённым workflow</param>
        /// <param name="findNotBound">Искать сущности непривязанные к указанной в searchText сущности</param>
        /// <param name="viewName">Идентификатор страницы, фильтр которой будет применен к результатам поиска</param>
        /// <param name="userID">Идентификатор пользователя</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Результаты поиска</returns>
        public Task<IReadOnlyList<SearchResult>> LoadSearchResultAsync(string searchKey, string searchText,
            SearchHelper.SearchMode mode,
            ObjectClass[] classes,
            string[] tags,
            SearchHelper.AdvancedSearchMode advancedSearchMode, bool shouldSearchFinished, bool findNotBound, string viewName,
            Guid userID, CancellationToken cancellationToken = default)
        {
            if (!_searchCache.TryTakeNext(searchKey, MaxItemsToReturn, out var cachedResult))
            {
                return SearchAsync(searchKey, searchText, mode, classes, tags, advancedSearchMode, shouldSearchFinished, findNotBound,
                    viewName, userID, cancellationToken);
            }

            var groupedResult = cachedResult
                .GroupBy(sr => sr.ClassID)
                .ToDictionary(gr => gr.Key, gr => gr.Select(_ => _));

            return GetSearchResultAsync(userID,
                advancedSearchMode == SearchHelper.AdvancedSearchMode.SearchInCurrentList ? viewName : default,
                shouldSearchFinished, groupedResult, cancellationToken);
        }

        /// <summary>
        /// Поиск объектов Service Desk
        /// </summary>
        /// <param name="searchKey">Ключ, с которым результаты поиска будут записаны в кэш</param>
        /// <param name="searchText">Текст для поиска</param>
        /// <param name="mode">Тип поиска</param>
        /// <param name="classes">Типы объектов</param>
        /// <param name="tags">Тэги</param>
        /// <param name="advancedSearchMode">Область поиска</param>
        /// <param name="shouldSearchFinished">Должен ли результат содержать объекты с  заверённым workflow</param>
        /// /// <param name="findNotBound">Искать сущности непривязанные к указанной в searchText сущности</param>
        /// <param name="viewName">Идентификатор страницы, фильтр которой будет применен к результатам поиска</param>
        /// <param name="userID">Идентификатор пользователя</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Результаты поиска</returns>
        public async Task<IReadOnlyList<SearchResult>> SearchAsync(string searchKey, string searchText,
            SearchHelper.SearchMode mode,
            ObjectClass[] classes,
            string[] tags,
            SearchHelper.AdvancedSearchMode advancedSearchMode, bool shouldSearchFinished, bool findNotBound, string viewName,
            Guid userID, CancellationToken cancellationToken = default)
        {
            var searchResult = await SearchObjectsAsync(searchText, mode, classes, tags,
                shouldSearchFinished, findNotBound, userID, cancellationToken).ConfigureAwait(false);

            if (searchResult.Count == 0)
                return Array.Empty<SearchResult>();

            if (advancedSearchMode == SearchHelper.AdvancedSearchMode.SearchInSearch)
            {
                if (!_searchCache.TryGet(searchKey, out var prevResult)) return Array.Empty<SearchResult>();

                _searchCache.Cache(searchKey,
                    prevResult.Intersect(searchResult).OrderByDescending(s => s.UtcDateModified).ToArray());
            }
            else
            {
                _searchCache.Cache(searchKey, searchResult.OrderByDescending(s => s.UtcDateModified).ToArray());
            }

            _searchCache.TryTakeNext(searchKey, MaxItemsToReturn, out var cachedResult);

            var groupedResult = cachedResult
                .GroupBy(sr => sr.ClassID)
                .ToDictionary(gr => gr.Key, gr => gr.Select(_ => _));

            return await GetSearchResultAsync(userID,
                advancedSearchMode == SearchHelper.AdvancedSearchMode.SearchInCurrentList ? viewName : default,
                shouldSearchFinished, groupedResult, cancellationToken).ConfigureAwait(false);
        }

        private async Task<IReadOnlyList<FoundObject>> SearchObjectsAsync(string searchText,
            SearchHelper.SearchMode mode,
            IReadOnlyList<ObjectClass> classes,
            IReadOnlyList<string> tags,
            bool shouldSearchFinished,
            bool findNotBound,
            Guid userID, CancellationToken cancellationToken)
        {
            IReadOnlyList<FoundObject> searchResult;
            Guid parentId = Guid.Empty;
            if (mode == SearchHelper.SearchMode.Number)
            {
                searchResult =
                    await _searchByNumberStrategy.SearchAsync(new SearchByNumberParameters(int.Parse(searchText),
                        shouldSearchFinished,
                        classes), cancellationToken);
            }
            else if (findNotBound && (string.IsNullOrEmpty(searchText) || Guid.TryParse(searchText, out parentId)))
            {
                searchResult =
                    await _searchNotBoundStrategy.SearchAsync(new SearchNotBoundParameters(parentId, classes), cancellationToken);
            }
            else
            {
                searchResult = await _searchByTextStrategy.SearchAsync(new SearchByTextParameters(searchText, mode,
                    classes,
                    tags,
                    shouldSearchFinished), cancellationToken);
            }

            var validatedResult = new List<FoundObject>(searchResult.Count);
            foreach (var resultItem in searchResult)
            {
                if (await ValidateAccessAsync(userID, resultItem, cancellationToken))
                    validatedResult.Add(resultItem);
            }

            return validatedResult;
        }

        private async Task<bool> ValidateAccessAsync(Guid userID, FoundObject foundObject,
            CancellationToken cancellationToken)
        {
            if (foundObject.ClassID == ObjectClass.KBArticle)
            {
                return await _kbArticleAccessValidator.ObjectIsAvailableAsync(userID, foundObject.ID, cancellationToken)
                    .ConfigureAwait(false);
            }

            if (foundObject.ClassID == ObjectClass.Problem)
            {
                return await _problemAccessValidator.ObjectIsAvailableAsync(userID, foundObject.ID, cancellationToken);
            }

            return await _accessManager.AccessIsGrantedAsync(userID, foundObject.ID, foundObject.ClassID)
                .ConfigureAwait(false);
        }

        private async Task<IReadOnlyList<SearchResult>> GetSearchResultAsync(Guid userID, string viewName,
            bool shouldSearchFinished,
            IReadOnlyDictionary<ObjectClass, IEnumerable<FoundObject>> foundObjectsGroups,
            CancellationToken cancellationToken)
        {
            var result = new List<SearchResult>(foundObjectsGroups.Count);

            if (string.IsNullOrEmpty(viewName))
            {
                result.AddRange(foundObjectsGroups.Select(f => new SearchResult
                {
                    ClassID = (int)f.Key,
                    FoundObjectList = f.Value.Select(x => _mapper.Map<PLFoundObject>(x)).ToList()
                }));
            }
            else
            {
                var currentFilter = await _filters.GetCurrentAsync(userID, viewName, cancellationToken)
                    .ConfigureAwait(false);

                switch (viewName)
                {
                    case ListView.MyTasksList:
                        foreach (var gr in foundObjectsGroups)
                        {
                            var filter = new ListViewFilterData<ServiceDeskListFilter>
                            {
                                ViewName = viewName,
                                StandardFilter = currentFilter.Filter.StandardName,
                                ExtensionFilter = new ServiceDeskListFilter
                                {
                                    IDList = gr.Value.Select(v => v.ID).ToArray(),
                                    WithFinishedWorkflow = shouldSearchFinished
                                }
                            };
                            switch (gr.Key)
                            {
                                case ObjectClass.Call:
                                {
                                    result.Add(new SearchResult
                                    {
                                        ClassID = (int)gr.Key,
                                        FoundObjectList =
                                            await GetFoundObjectsAsync(_callBll, filter, cancellationToken)
                                                .ConfigureAwait(false)
                                    });
                                    break;
                                }
                                case ObjectClass.WorkOrder:
                                {
                                    result.Add(new SearchResult
                                    {
                                        ClassID = (int)gr.Key,
                                        FoundObjectList = await GetFoundObjectsAsync(_woBll, filter, cancellationToken)
                                            .ConfigureAwait(false)
                                    });
                                    break;
                                }
                                case ObjectClass.Problem:
                                {
                                    result.Add(new SearchResult
                                    {
                                        ClassID = (int)gr.Key,
                                        FoundObjectList =
                                            await GetFoundObjectsAsync(_problemBll, filter, cancellationToken)
                                                .ConfigureAwait(false)
                                    });
                                    break;
                                }
                                case ObjectClass.MassIncident:
                                {
                                    result.Add(new SearchResult
                                    {
                                        ClassID = (int)gr.Key,
                                        FoundObjectList =
                                            await GetFoundObjectsAsync(_massiveIncidentBll, filter, cancellationToken)
                                                .ConfigureAwait(false)
                                    });
                                    break;
                                }
                            }
                        }

                        break;
                    case ListView.ClientCallList when foundObjectsGroups.TryGetValue(ObjectClass.Call, out var objs):
                    {
                        var filter = new ListViewFilterData<CallFromMeListFilter>
                        {
                            ViewName = viewName,
                            CurrentFilterID = currentFilter.Filter.ID,
                            ExtensionFilter = new CallFromMeListFilter
                            {
                                IDList = objs.Select(v => v.ID).ToArray(),
                                WithFinishedWorkflow = shouldSearchFinished
                            }
                        };

                        result.Add(new SearchResult
                        {
                            ClassID = (int)ObjectClass.Call,
                            FoundObjectList = await GetMyFoundObjectsAsync(_callBll, filter, cancellationToken)
                                .ConfigureAwait(false)
                        });
                        break;
                    }
                    case ListView.AllCallsList when foundObjectsGroups.TryGetValue(ObjectClass.Call, out var objs):
                    {
                        var filter = new ListViewFilterData<ServiceDeskListFilter>
                        {
                            ViewName = viewName,
                            CurrentFilterID = currentFilter.Filter.ID,
                            ExtensionFilter = new ServiceDeskListFilter
                            {
                                IDList = objs.Select(v => v.ID).ToArray(),
                                WithFinishedWorkflow = shouldSearchFinished
                            }
                        };

                        result.Add(new SearchResult
                        {
                            ClassID = (int)ObjectClass.Call,
                            FoundObjectList = await GetFoundObjectsAsync(_callBll, filter, cancellationToken)
                                .ConfigureAwait(false)
                        });
                        break;
                    }

                    case ListView.WorkOrderList
                        when foundObjectsGroups.TryGetValue(ObjectClass.WorkOrder, out var objs):
                    {
                        var filter = new ListViewFilterData<ServiceDeskListFilter>
                        {
                            ViewName = viewName,
                            CurrentFilterID = currentFilter.Filter.ID,
                            ExtensionFilter = new ServiceDeskListFilter
                            {
                                IDList = objs.Select(v => v.ID).ToArray(),
                                WithFinishedWorkflow = shouldSearchFinished
                            }
                        };

                        result.Add(new SearchResult
                        {
                            ClassID = (int)ObjectClass.WorkOrder,
                            FoundObjectList = await GetFoundObjectsAsync(_woBll, filter, cancellationToken)
                                .ConfigureAwait(false)
                        });
                        break;
                    }
                    case ListView.ProblemList when foundObjectsGroups.TryGetValue(ObjectClass.Problem, out var objs):
                    {
                        var filter = new ListViewFilterData<ServiceDeskListFilter>
                        {
                            ViewName = viewName,
                            CurrentFilterID = currentFilter.Filter.ID,
                            ExtensionFilter = new ServiceDeskListFilter
                            {
                                IDList = objs.Select(v => v.ID).ToArray(),
                                WithFinishedWorkflow = shouldSearchFinished
                            }
                        };

                        result.Add(new SearchResult
                        {
                            ClassID = (int)ObjectClass.Problem,
                            FoundObjectList = await GetFoundObjectsAsync(_problemBll, filter, cancellationToken)
                                .ConfigureAwait(false)
                        });
                        break;
                    }
                    case ListView.NegotiationList:
                    {
                        var filter = new ListViewFilterData<NegotiationListFilter>
                        {
                            ViewName = viewName,
                            CurrentFilterID = currentFilter.Filter.ID,
                            ExtensionFilter = new NegotiationListFilter
                            {
                                IDList = foundObjectsGroups.Values.SelectMany(v => v).Select(v => v.ID).ToArray(),
                                WithFinishedWorkflow = shouldSearchFinished
                            }
                        };
                        var foundNegotiation = await GetFoundObjectsAsync(_negotiationBLL, filter,
                            foundObjectsGroups.Values.SelectMany(v => v), cancellationToken).ConfigureAwait(false);
                        var foundNegotiationGrouped = foundNegotiation.GroupBy(f => f.ClassID);
                        foreach (var gr in foundNegotiationGrouped)
                        {
                            result.Add(new SearchResult
                            {
                                ClassID = gr.Key,
                                FoundObjectList = gr.ToList()
                            });
                        }

                        break;
                    }
                    case ListView.CustomControlList:
                    {
                        var filter = new ListViewFilterData<ServiceDeskListFilter>
                        {
                            ViewName = viewName,
                            CurrentFilterID = currentFilter.Filter.ID,
                            ExtensionFilter = new ServiceDeskListFilter
                            {
                                IDList = foundObjectsGroups.Values.SelectMany(v => v).Select(v => v.ID).ToArray(),
                                WithFinishedWorkflow = shouldSearchFinished
                            }
                        };
                        var foundOnControl = await GetFoundObjectsAsync(_customControlBLL, filter,
                            foundObjectsGroups.Values.SelectMany(v => v), cancellationToken).ConfigureAwait(false);
                        var foundOnControlGrouped = foundOnControl.GroupBy(f => f.ClassID);
                        foreach (var gr in foundOnControlGrouped)
                        {
                            result.Add(new SearchResult
                            {
                                ClassID = gr.Key,
                                FoundObjectList = gr.ToList()
                            });
                        }

                        break;
                    }
                    case ListView.AllMassIncidents when foundObjectsGroups.TryGetValue(ObjectClass.MassIncident, out var objs):
                    {
                        var filter = new ListViewFilterData<ServiceDeskListFilter>
                        {
                            ViewName = viewName,
                            CurrentFilterID = currentFilter.Filter.ID,
                            ExtensionFilter = new ServiceDeskListFilter
                            {
                                IDList = objs.Select(v => v.ID).ToArray(),
                                WithFinishedWorkflow = shouldSearchFinished
                            }
                        };

                        result.Add(new SearchResult
                        {
                            ClassID = (int)ObjectClass.MassIncident,
                            FoundObjectList = await GetFoundObjectsAsync(_massiveIncidentBll, filter, cancellationToken)
                                .ConfigureAwait(false)
                        });
                        break;
                    }
                }
            }

            return result;
        }

        private async Task<List<PLFoundObject>> GetFoundObjectsAsync(IWorkOrderBLL bll,
            ListViewFilterData<ServiceDeskListFilter> filter, CancellationToken cancellationToken)
        {
            var wos = await bll.GetAllWorkOrdersAsync(filter, cancellationToken).ConfigureAwait(false);
            return wos.Select(x => _mapper.Map<PLFoundObject>(x)).ToList();
        }

        private async Task<List<PLFoundObject>> GetFoundObjectsAsync(IProblemBLL bll,
            ListViewFilterData<ServiceDeskListFilter> filter, CancellationToken cancellationToken)
        {
            var problems = await bll.AllProblemsArrayAsync(filter, cancellationToken);
            return problems.Select(x => _mapper.Map<PLFoundObject>(x)).ToList();
        }

        private async Task<List<PLFoundObject>> GetFoundObjectsAsync(ICallBLL bll,
            ListViewFilterData<ServiceDeskListFilter> filter, CancellationToken cancellationToken)
        {
            var calls = await bll.AllCallsAsync(filter, cancellationToken).ConfigureAwait(false);
            return calls.Select(x => _mapper.Map<PLFoundObject>(x)).ToList();
        }

        private async Task<List<PLFoundObject>> GetMyFoundObjectsAsync(ICallBLL bll,
            ListViewFilterData<CallFromMeListFilter> filter, CancellationToken cancellationToken)
        {
            var calls = await bll.CallsFromMeAsync(filter, cancellationToken).ConfigureAwait(false);
            return calls.Select(x => _mapper.Map<PLFoundObject>(x)).ToList();
        }

        private async Task<List<PLFoundObject>> GetFoundObjectsAsync(
            ICustomControlBLL bll,
            ListViewFilterData<ServiceDeskListFilter> filter, 
            IEnumerable<FoundObject> foundObjects,
            CancellationToken cancellationToken)
        {
            var customControl = await bll.GetListAsync(filter, cancellationToken).ConfigureAwait(false);
            var foundIds = new HashSet<Guid>(customControl.Select(c => c.ID));
            return foundObjects.Where(f => foundIds.Contains(f.ID)).Select(f => _mapper.Map<PLFoundObject>(f)).ToList();
        }

        private async Task<List<PLFoundObject>> GetFoundObjectsAsync(
            IReadNegotiationBLL bll,
            ListViewFilterData<NegotiationListFilter> filter, 
            IEnumerable<FoundObject> foundObjects,
            CancellationToken cancellationToken)
        {
            var negotiations = await bll.GetReportAsync(filter, cancellationToken).ConfigureAwait(false);
            var foundIds = new HashSet<Guid>(negotiations.Select(c => c.ObjectID));
            return foundObjects.Where(f => foundIds.Contains(f.ID)).Select(f => _mapper.Map<PLFoundObject>(f)).ToList();
        }

        private async Task<List<PLFoundObject>> GetFoundObjectsAsync(IMassIncidentBLL bll,
            ListViewFilterData<ServiceDeskListFilter> filter, CancellationToken cancellationToken)
        {
            var massiveIncidents = await bll.AllMassIncidentsAsync(filter, cancellationToken).ConfigureAwait(false);
            return massiveIncidents.Select(f => _mapper.Map<PLFoundObject>(f)).ToList();
        }

        public async Task<MyTasksReportItem[]> SearchAsync(SearchFilter filter,
            CancellationToken cancellationToken = default)
        {
            if (filter.SearchMode != SearchHelper.SearchMode.Number)
            {
                filter.Text = "";
                filter.IDs = (await _searchByTextStrategy.SearchAsync(new SearchByTextParameters(filter.Text,
                            filter.SearchMode, filter.Classes, new List<string>(), filter.SearchFinished),
                        cancellationToken))
                    .Select(x => x.ID).ToArray();
            }

            return await _service.SearchAsync(filter, cancellationToken);
        }
    }
}