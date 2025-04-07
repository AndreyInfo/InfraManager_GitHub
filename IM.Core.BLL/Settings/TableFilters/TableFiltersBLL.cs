using Inframanager.BLL.Settings.TableFilters;
using InfraManager.BLL.AccessManagement;
using InfraManager.DAL;
using InfraManager.DAL.Events;
using InfraManager.DAL.Settings;
using InfraManager.ResourcesArea;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings.TableFilters
{
    internal class TableFiltersBLL : ITableFiltersBLL, ISelfRegisteredService<ITableFiltersBLL>
    {
        #region .ctor        

        private readonly IRepository<WebFilter> _filtersRepository;
        private readonly IRepository<Event> _events;
        private readonly IRepository<WebFilterUsing> _filterUsagesRepository;
        private readonly IRepository<WebUserFilterSettings> _userFilterSettingsRepository;
        private readonly IFinder<WebUserFilterSettings> _userFilterSettingsFinder;
        private readonly IFinder<WebFilterUsing> _filterUsageFinder;
        private readonly IFinder<WebFilter> _filterFinder;
        private readonly IFindEntityByGlobalIdentifier<User> _usersFinder;
        private readonly IUnitOfWork _saveChanges;
        private readonly ICurrentUser _currentUser;
        private readonly IServiceMapper<FilterTypes, ICreateFilterElement> _filterElementCreators;
        private readonly ICultureProvider _cultureProvider;
        private readonly IUserAccessBLL _userAccess;

        public TableFiltersBLL(
            IRepository<WebFilter> filtersRepository,
            IRepository<Event> events,
            IRepository<WebFilterUsing> filterUsagesRepository, 
            IRepository<WebUserFilterSettings> userFilterSettingsRepository, 
            IFinder<WebUserFilterSettings> userFilterSettingsFinder,
            IFinder<WebFilterUsing> filterUsageFinder,
            IFinder<WebFilter> filterFinder,
            IFindEntityByGlobalIdentifier<User> usersFinder,
            IUnitOfWork saveChanges, 
            ICurrentUser currentUser,
            IServiceMapper<FilterTypes, ICreateFilterElement> filterElementCreators,
            ICultureProvider cultureProvider,
            IUserAccessBLL userAccess)
        {
            _filtersRepository = filtersRepository;
            _events = events;
            _filterUsagesRepository = filterUsagesRepository;
            _userFilterSettingsRepository = userFilterSettingsRepository;

            _filterUsageFinder = filterUsageFinder;
            _userFilterSettingsFinder = userFilterSettingsFinder;
            _filterFinder = filterFinder;
            _usersFinder = usersFinder;
            _saveChanges = saveChanges;
            _currentUser = currentUser;
            _filterElementCreators = filterElementCreators;
            _cultureProvider = cultureProvider;
            _userAccess = userAccess;
        }

        #endregion

        #region Filters

        public async Task<FilterDetails[]> GetListAsync(string view, CancellationToken cancellationToken = default)
        {
            var viewIsGranted = await _userAccess.ViewIsGrantedAsync(_currentUser.UserId, view);
            if (!viewIsGranted)
            {
                throw new AccessDeniedException($"Get table filters for {view}");
            }

            var uiCulture = await _cultureProvider.GetUiCultureInfoAsync(cancellationToken);

            var currentUserId = _currentUser.UserId;
            var filters = await _filtersRepository.ToArrayAsync(
                f => f.ViewName == view
                    && (f.UserId == null || f.UserId == currentUserId),
                cancellationToken);

            var filterIds = filters.Select(f => f.Id).ToArray();
            var userFilterSettings = await _userFilterSettingsRepository
                .With(x => x.Filter)
                    .ThenWithMany(x => x.Elements)
                .ToArrayAsync(
                    x => x.ViewName == view
                        && filterIds.Contains(x.Filter.Id)
                        && x.Filter != null
                        && x.UserId == (x.Filter.UserId ?? currentUserId));

            var usages = await _filterUsagesRepository.ToArrayAsync(
                x => filterIds.Contains(x.FilterId)
                    && x.UserId == currentUserId);
            
            var resultItems = from filter in filters
                              join userSettings in userFilterSettings 
                                on filter.Id equals userSettings.Filter.Id
                                  into tempUserSettings
                              from userSettings in tempUserSettings.DefaultIfEmpty()
                              join usage in usages on filter.Id equals usage.FilterId
                                  into tempUsages
                              from usage in tempUsages.DefaultIfEmpty()
                              where userSettings == null || !userSettings.Temp
                              select new { Filter = filter, LastUsage = usage?.UtcDateLastUsage };

            return resultItems
                .Select(x => CreateFilterModel(x.Filter, x.LastUsage, uiCulture))
                .ToArray();
        }

        public async Task<FilterDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var filter = await GetOrRaiseErrorAsync(id, cancellationToken);

            return await CreateFilterModelAsync(filter);
        }

        public Task<FilterDetails> GetAsync(ITableFilter listFilter, CancellationToken cancellationToken = default)
        {
            return GetAsync(_currentUser.UserId, listFilter, cancellationToken);
        }

        public async Task<FilterDetails> GetAsync(Guid userId, ITableFilter listFilter, CancellationToken cancellationToken = default)
        {
            if (listFilter.CurrentFilterID.HasValue)
            {
                return await GetAsync(listFilter.CurrentFilterID.Value, cancellationToken);
            }

            if (!string.IsNullOrWhiteSpace(listFilter.StandardFilter)
                || listFilter.CustomFilters.Any())
            {
                return new FilterDetails
                {
                    StandardName = listFilter.StandardFilter,
                    Standart = !string.IsNullOrWhiteSpace(listFilter.StandardFilter),
                    Elements = listFilter.CustomFilters.Select(ConvertFilterElement).ToArray()
                };
            }

            var currentUserFilter = await GetCurrentAsync(userId, listFilter.ViewName, cancellationToken);
            return currentUserFilter.Filter;
        }

        public async Task<FilterDetails> AddAsync(string view, FilterData model, CancellationToken cancellationToken = default)
        {
            var viewIsGranted = await _userAccess.ViewIsGrantedAsync(_currentUser.UserId, view);
            if (!viewIsGranted)
            {
                throw new AccessDeniedException($"Add table filter for {view}");
            }

            ValidateOrRaiseError(model);

            var filter = new WebFilter
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                ViewName = view,
                Standart = false,
                UserId = _currentUser.UserId,
                Others = model.Others
            };

            AddFilterElements(filter, model.Elements);

            _filtersRepository.Insert(filter);
            await _saveChanges.SaveAsync(cancellationToken);
            var uiCulture = await _cultureProvider.GetUiCultureInfoAsync(cancellationToken);

            return CreateFilterModel(filter, lastUsage: null, uiCulture);
        }

        private static Dictionary<string, OperationID> _viewToOperationMapping =
            new Dictionary<string, OperationID>
            {
                { ListView.ClientCallList, OperationID.CommonFilters_EditForMyCalls },
                { ListView.NegotiationList, OperationID.CommonFilters_EditForNegotiations },
                { ListView.MyTasksList, OperationID.CommonFilters_EditForTasks },
                { ListView.AllCallsList, OperationID.CommonFilters_EditForCall },
                { ListView.ChangeRequestList, OperationID.CommonFilters_EditForRFC },
                { ListView.ProblemList, OperationID.CommonFilters_EditForProblems },
                { ListView.WorkOrderList, OperationID.CommonFilters_EditForWorkOrders },
                { ListView.CustomControlList, OperationID.CommonFilters_EditForControl }
            };

        public async Task<FilterDetails> UpdateAsync(Guid id, FilterData model, CancellationToken cancellationToken = default)
        {
            var filter = await GetOrRaiseErrorAsync(id, cancellationToken);

            filter.Name = model.Name ?? filter.Name;

            var operationId = _viewToOperationMapping.ContainsKey(filter.ViewName)
                    ? (int?)_viewToOperationMapping[filter.ViewName]
                    : null;
            var newEvent = new Event("Изменен список фильтрации", operationId, _currentUser.UserId);
            var eventSubject = new EventSubject
            {
                Id = Guid.NewGuid(),
                SubjectName = "Фильтр",
                SubjectValue = filter.Name, 
                ObjectId = id
            };

            bool submitEvent = false;
            if (model.Others.HasValue && filter.Others != model.Others)
            {
                filter.Others = model.Others;
                var listName = filter.Standart ? "Другие встроенные фильтры" : "Мои фильтры";
                eventSubject.EventSubjectParam.Add(
                    new EventSubjectParam(
                        "Изменение списка фильтрации",
                        filter.Others.Value ? listName : "Общие фильтры",
                        filter.Others.Value ? "Общие фильтры" : listName));
                submitEvent = true;
            }

            var ownerId = model.UserId ?? filter.UserId;
            if (ownerId.HasValue)
            {
                var owner = await _usersFinder.FindAsync(ownerId.Value, cancellationToken);
                eventSubject.EventSubjectParam.Add(
                    new EventSubjectParam("Владелец фильтра", null, owner.FullName));
            }

            if (!string.IsNullOrEmpty(filter.ViewName))
            {
                eventSubject.EventSubjectParam.Add(
                    new EventSubjectParam("Расположение фильтра", null, filter.ViewName));
            }

            if (model.UserId.HasValue && model.UserId != filter.UserId)
            {
                filter.UserId = model.UserId ?? filter.UserId;

                eventSubject.EventSubjectParam.Add(
                    new EventSubjectParam("Изменен владелец фильтра", null, filter.ViewName));
                submitEvent = true;
            }

            if (submitEvent)
            {
                _events.Insert(newEvent);
            }

            if (model.Elements != null && model.Elements.Any())
            {
                filter.Elements.Clear();
                AddFilterElements(filter, model.Elements);
            }
            await _saveChanges.SaveAsync(cancellationToken);

            return await CreateFilterModelAsync(filter, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var filter = await GetOrRaiseErrorAsync(id, cancellationToken);
            _filtersRepository.Delete(filter);
            await _saveChanges.SaveAsync(cancellationToken);
        }

        private async Task<WebFilter> GetOrRaiseErrorAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var filter = await _filterFinder.FindAsync(new object[] { id }, cancellationToken);

            if (filter == null)
            {
                throw new ObjectNotFoundException<Guid>(id, "TableFilter");
            }

            var viewIsGranted = await _userAccess.ViewIsGrantedAsync(_currentUser.UserId, filter.ViewName);
            if (!viewIsGranted)
            {
                throw new AccessDeniedException($"Get filter {id} related to view {filter.ViewName}");
            }

            return filter;
        }

        private void ValidateOrRaiseError(FilterData model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new InvalidObjectException("Имя фильтра не задано"); // TODO: Localize exception text
            }

            if (model.Elements == null || !model.Elements.Any())
            {
                throw new InvalidObjectException("Отсутствуют элементы фильтра");
            }
        }

        private void AddFilterElements(WebFilter filter, FilterElementDetails[] elements)
        {
            foreach (var elementModel in elements)
            {
                var creator = _filterElementCreators.Map(elementModel.Type);
                var elementData = creator.CreateFilterElement(elementModel).GetData();
                var filterElement = new WebFilterElement(elementData);                
                filter.Elements.Add(filterElement);
            }
        }

        #endregion

        #region Current filter

        public Task<CurrentFilterDetails> GetCurrentAsync(string viewName, CancellationToken cancellationToken = default)
        {
            return GetCurrentAsync(_currentUser.UserId, viewName, cancellationToken);
        }

        public async Task<CurrentFilterDetails> GetCurrentAsync(Guid userId, string viewName, CancellationToken cancellationToken = default)
        {
            var userSettings = await _userFilterSettingsFinder.FindAsync(
                new object[] { userId, viewName },
                cancellationToken);

            if (userSettings == null)
            {
                return await GetDefaltFilterAsync(viewName, cancellationToken);
            }

            var filter = new FilterDetails 
            { 
                Name = StandardFilters.All, 
                Standart = true, 
                StandardName = StandardFilters.All,
                Elements = Array.Empty<FilterElementBase>()
            };
            if (userSettings.Filter != null)
            {
                filter = await CreateFilterModelAsync(userSettings.Filter, cancellationToken);
            }

            return new CurrentFilterDetails
            {
                AfterUtcModified = userSettings.AfterUtcDateModified,
                WithFinishedWorkflow = userSettings.WithFinishedWorkflow,
                Filter = filter
            };
        }

        public async Task SetCurrentAsync(string viewName, CurrentFilterData currentFilter, CancellationToken cancellationToken = default)
        {
            var userSettings = await _userFilterSettingsFinder.FindAsync(
                new object[] { _currentUser.UserId, viewName },
                cancellationToken);

            if (userSettings == null)
            {
                userSettings = new WebUserFilterSettings(_currentUser.UserId, viewName);
                _userFilterSettingsRepository.Insert(userSettings);
            }

            if (currentFilter.FilterId.HasValue)
            {
                var filter = await _filterFinder.FindAsync(currentFilter.FilterId.Value, cancellationToken);
                userSettings.Filter = filter ?? throw new InvalidObjectException("Выбранный фильтр был удален или не существовал.");
            }

            if (DateTimeExtensions.TryConvertFromMillisecondsAfterMinimumDate(
                currentFilter.AfterUtcModified, 
                out DateTime? modifiedAfter))
            {
                userSettings.AfterUtcDateModified = modifiedAfter;
            }
            else
            {
                throw new InvalidObjectException("Значение {currentFilter.AfterUtcModified} не верно");
            }
            userSettings.WithFinishedWorkflow = currentFilter.WithFinishedWorkflow;
            userSettings.Temp = currentFilter.IsTemp;

            await _saveChanges.SaveAsync(cancellationToken);
        }

        private readonly Dictionary<string, string> _defaultFilters =
            new Dictionary<string, string>
            {
                { ListView.NegotiationList, StandardFilters.NegotiationStartedNeeded },
                { ListView.MyTasksList, StandardFilters.MyTasksAllMyNotClose },
                { ListView.AllCallsList, StandardFilters.CallEngineerAllMyNotAccomplished },
                { ListView.ProblemList, StandardFilters.ProblemAllMyOpened },
                { ListView.WorkOrderList, StandardFilters.WorkOrderAllMyInWorkNotAccomplished }
            };

        private async Task<CurrentFilterDetails> GetDefaltFilterAsync(string viewName, CancellationToken cancellationToken = default)
        {
            var defaultFilterName = _defaultFilters.ContainsKey(viewName)
                ? _defaultFilters[viewName]
                : StandardFilters.All;
            var filters = await _filtersRepository
                .ToArrayAsync(
                    f => f.Standart 
                        && f.Name == defaultFilterName 
                        && f.ViewName == viewName,
                    cancellationToken);
            var defaultFilter = filters.FirstOrDefault() // TODO: Use FindAsync instead of ToArrayAsync(...) + FirstOrDefault()
                ?? throw new Exception($"Default filter for view {viewName} is not found.");

            var usage = await _filterUsageFinder.FindAsync(
                new object[] { defaultFilter.Id, _currentUser.UserId },
                cancellationToken);
            var uiCulture = await _cultureProvider.GetUiCultureInfoAsync(cancellationToken);

            return new CurrentFilterDetails
            {
                Filter = CreateFilterModel(defaultFilter, usage?.UtcDateLastUsage, uiCulture)
            };
        }

        #endregion

        #region Create FilterModel

        private const char resourcesCharacter = '_';

        private async Task<FilterDetails> CreateFilterModelAsync(WebFilter filter, CancellationToken cancellationToken = default)
        {
            var uiCulture = await _cultureProvider.GetUiCultureInfoAsync(cancellationToken);
            var usage = await _filterUsageFinder.FindAsync(
                 filter.Id,
                 _currentUser.UserId,
                cancellationToken);

            return CreateFilterModel(filter, usage?.UtcDateLastUsage, uiCulture);
        }

        private FilterDetails CreateFilterModel(WebFilter filter, DateTime? lastUsage, CultureInfo uiCulture)
        {
            return new FilterDetails
            {
                ID = filter.Id,
                UserID = filter.UserId,
                ViewName = filter.ViewName,
                Name = filter.Name.StartsWith(resourcesCharacter) || filter.Name.EndsWith(resourcesCharacter)
                                    ? (Resources.ResourceManager.GetString(filter.Name, uiCulture) ?? filter.Name.Trim(resourcesCharacter)) // TODO: Resources should be injected
                                    : filter.Name,
                StandardName = filter.Standart ? filter.Name : string.Empty,
                Standart = filter.Standart,
                Others = filter.Others ?? default,
                UtcDateLastUsage = lastUsage.ConvertToMillisecondsAfterMinimumDate(),
                Description = filter.Description,
                Elements = filter
                    .Elements
                    .Select(elem => elem.Parse())
                    .Select(ConvertFilterElement)
                    .ToArray()
            };
        }

        private FilterElementBase ConvertFilterElement(FilterElementData data)
        {
            var filterType = (FilterTypes)data.Type;
            var creator = _filterElementCreators.Map(filterType);

            return creator.CreateFilterElement(data);
        }

        #endregion
    }
}
