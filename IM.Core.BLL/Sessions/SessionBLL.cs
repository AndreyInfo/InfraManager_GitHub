using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL;
using InfraManager.DAL.Sessions;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.Sessions;

internal class SessionBLL : ISessionBLL, ISelfRegisteredService<ISessionBLL>
{

    private readonly IRepository<Session> _repository;
    private readonly IActiveEngineerSessionCountQuery _activeEngineerSessionCountQuery;
    private readonly IActivePersonalSessionCountQuery _activePersonalSessionCountQuery;
    private readonly IMapper _mapper;
    private readonly ISessionDeactivator _deactivator;
    private readonly IUserAccessBLL _access;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<SessionBLL> _logger;
    private readonly IOrderedColumnQueryBuilder<Session, SessionListItem> _orderedColumnQueryBuilder;
    private readonly ISessionListQuery _sessionListQuery;
    
    public SessionBLL(IActiveEngineerSessionCountQuery activeEngineerSessionCountQuery,
        IRepository<Session> repository,
        IMapper mapper,
        ISessionDeactivator deactivator,
        IUserAccessBLL access,
        ICurrentUser currentUser,
        ILogger<SessionBLL> logger,
        IActivePersonalSessionCountQuery activePersonalSessionCountQuery,
        IOrderedColumnQueryBuilder<Session, SessionListItem> orderedColumnQueryBuilder,
        ISessionListQuery sessionListQuery)
    {
        _activeEngineerSessionCountQuery = activeEngineerSessionCountQuery;
        _repository = repository;
        _mapper = mapper;
        _deactivator = deactivator;
        _access = access;
        _currentUser = currentUser;
        _logger = logger;
        _activePersonalSessionCountQuery = activePersonalSessionCountQuery;
        _orderedColumnQueryBuilder = orderedColumnQueryBuilder;
        _sessionListQuery = sessionListQuery;
    }

    public async Task<SessionDetails[]> ListAsync(SessionFilter filter, CancellationToken cancellationToken = default)
    {
        await CheckAdminRoleAsync(cancellationToken);

        var query = _repository.With(x => x.User).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
            .DisableTrackingForQuery().Query();

        if (filter.UserID.HasValue)
        {
            query = query.Where(x => x.UserID == filter.UserID);
        }

        if (!string.IsNullOrEmpty(filter.UserAgent))
        {
            query = query.Where(x => x.UserAgent == filter.UserAgent);
        }

        var orderedQuery = await _orderedColumnQueryBuilder.BuildQueryAsync(filter.ViewName, query, cancellationToken);

        var sessions = await _sessionListQuery.ExecuteAsync(orderedQuery, filter.CountRecords,
            filter.StartRecordIndex, cancellationToken);

        _logger.LogInformation(
            $"User with ID = {_currentUser.UserId} successfully got sessions list");

        return _mapper.Map<SessionDetails[]>(sessions);
    }

    public async Task DeactivateSessionAsync(Guid userID, string userAgent, CancellationToken cancellationToken = default)    
    {
        await CheckAdminRoleAsync(cancellationToken);
        
        await _deactivator.DeactivateAsync(userID, userAgent, cancellationToken);

        _logger.LogInformation(
            $"User with ID = {_currentUser.UserId} successfully deactivated session from userID = {userID} and user agent = {userAgent}");
    }

    public async Task<SessionsStatisticDetails> SessionStatisticAsync(CancellationToken cancellationToken = default)
    {
        await CheckAdminRoleAsync(cancellationToken);

        var activeEngineerSessionCount = await _activeEngineerSessionCountQuery.ExecuteAsync(cancellationToken);
        var activePersonalSessionCount = await _activePersonalSessionCountQuery.ExecuteAsync(cancellationToken);
        var activeSessionsCount = await ActiveSessionsCount(cancellationToken);
        var AvailableConcurrentSessionCount = 15_000; //TODO пока нет механизма получения кол-во лицензий, решено было оставить константу, как появятся ключи, будете переделано под кол-во сессий из ключа
        var AvailablePersonalSessionCount = 15_000; //TODO пока нет механизма получения кол-во лицензий, решено было оставить константу, как появятся ключи, будете переделано под кол-во сессий из ключа
        
        _logger.LogInformation(
            $"User with ID = {_currentUser.UserId} successfully got sessions statistic");
        
        return new SessionsStatisticDetails(
            activeEngineerSessionCount,
            activePersonalSessionCount,
            activeSessionsCount,
            AvailableConcurrentSessionCount,
            AvailablePersonalSessionCount);
    }

    private async Task<int> ActiveSessionsCount(CancellationToken cancellationToken = default)
    {
        return await _repository.CountAsync(x => x.UtcDateClosed == null, cancellationToken);
    }
    
    private async Task CheckAdminRoleAsync(CancellationToken cancellationToken = default)
    {
        if (!await _access.HasAdminRoleAsync(_currentUser.UserId, cancellationToken))
        {
            throw new AccessDeniedException(
                $"User with ID {_currentUser.UserId} has no admin rights to work with sessions");
        }
        
        _logger.LogInformation($"User with ID = {_currentUser.UserId} has admin rights to work with user's sessions");
    }
}