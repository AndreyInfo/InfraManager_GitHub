using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL;
using InfraManager.DAL.Events;
using System.Linq;
using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.UserUniqueFiltrations;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.Events
{
    internal class EventBLL : IEventBLL, ISelfRegisteredService<IEventBLL>
    {
        private readonly IRepository<Event> _events;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IMapper _mapper;
        private readonly IPagingQueryCreator _pagging;
        private readonly IUserFiltrationQueryBuilder<Event, EventSubjectFiltrationOptions> _builderQuery;
        private readonly IOrderedColumnQueryBuilder<Event, EventSubjectListItem> _orderedColumnQueryBuilder;
        private readonly IValidatePermissions<Event> _permissionValidator;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<EventBLL> _logger;

        public EventBLL(IRepository<Event> events,
            IUnitOfWork saveChangesCommand,
            IMapper mapper,
            IPagingQueryCreator pagging,
            IUserFiltrationQueryBuilder<Event, EventSubjectFiltrationOptions> builderQuery,
            IOrderedColumnQueryBuilder<Event, EventSubjectListItem> orderedColumnQueryBuilder,
            IValidatePermissions<Event> permissionValidator,
            ICurrentUser currentUser,
            ILogger<EventBLL> logger)
        {
            _events = events;
            _saveChangesCommand = saveChangesCommand;
            _mapper = mapper;
            _pagging = pagging;
            _builderQuery = builderQuery;
            _orderedColumnQueryBuilder = orderedColumnQueryBuilder;
            _permissionValidator = permissionValidator;
            _currentUser = currentUser;
            _logger = logger;
        }

        public void AddEvent(Event @event)
        {
            _events.Insert(@event);
        }

        public async Task<Event[]> GetEventsAsync(Guid id, DateTime? dateFrom, DateTime? dateTo,
            CancellationToken cancellationToken)
        {
            var query = _events.Query().Where(x => x.EventSubject.Any(t => t.ObjectId == id));
            
            if (dateFrom.HasValue)
            {
                query = query.Where(x => x.Date >= dateFrom.Value);
            }
            
            if (dateTo.HasValue)
            {
                query = query.Where(x => x.Date <= dateTo.Value);
            }
            
            return await query.ExecuteAsync(cancellationToken);
        }

        public async Task AddEventSaveAsync(Event @event, CancellationToken cancellationToken = default)
        {
            AddEvent(@event);
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task<EventDetails[]> GetEventSubjectsAsync(EventSubjectFilter filter,
            CancellationToken cancellationToken)
        {
            await _permissionValidator.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId,
                ObjectAction.ViewDetailsArray, cancellationToken);
            
            var query = _events.With(x => x.User).WithMany(x => x.EventSubject).DisableTrackingForQuery().Query();
            
            query = _builderQuery.Build(filter.EventSubjectSearchColumns, query);

            if (filter.DateFrom.HasValue)
            {
                query = query.Where(x => x.Date >= filter.DateFrom.Value);
            }
            
            if (filter.DateTo.HasValue)
            {
                query = query.Where(x => x.Date <= filter.DateTo.Value);
            }

            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(x => x.Message.ToLower().Contains(filter.SearchString.ToLower())
                                         ||
                                         (x.User.Surname + " " + x.User.Name + " " +
                                          x.User.Patronymic).Contains(filter.SearchString.ToLower())
                                         ||
                                         x.EventSubject.Any(x => x.SubjectName.Contains(filter.SearchString.ToLower()))
                                         ||
                                         x.EventSubject.Any(x =>
                                             x.SubjectValue.Contains(filter.SearchString.ToLower())));
            }
            
            var orderedQuery =
                await _orderedColumnQueryBuilder.BuildQueryAsync(filter.ViewName, query, cancellationToken);
            
            var paggingQuery = _pagging.Create(orderedQuery);
            
            var elements =
                await paggingQuery.PageAsync(filter.StartRecordIndex, filter.CountRecords, cancellationToken);
            
            _logger.LogInformation($"User with id = {_currentUser.UserId} got Event List");
            
            return _mapper.Map<EventDetails[]>(elements);
        }

      
        public async Task<EventDetails> GetEventSubjectAsync(Guid id, CancellationToken cancellationToken)
        {
            await _permissionValidator.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId,
                ObjectAction.ViewDetailsArray, cancellationToken);
            
            var @event = await _events.WithMany(x => x.ChildEvents).WithMany(x => x.EventSubject)
                .ThenWithMany(x => x.EventSubjectParam).DisableTrackingForQuery()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            _logger.LogInformation($"User with id = {_currentUser.UserId} got information about Event with id = {id}");
            
            return _mapper.Map<EventDetails>(@event);
        }
    }
}
