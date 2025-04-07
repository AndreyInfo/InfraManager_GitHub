using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using InfraManager.BLL.Settings;
using InfraManager.BLL.Users;
using InfraManager.DAL;
using InfraManager.DAL.Sessions;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.Sessions;

internal class SystemSessionBLL : ISystemSessionBLL, ISelfRegisteredService<ISystemSessionBLL>
{
    private readonly IUserBLL _userBLL;
    private readonly IRepository<Session> _repository;
    private readonly IRepository<UserSessionHistory> _sessionHistoryRepository;
    private readonly IActiveEngineerSessionCountQuery _activeEngineerSessionCountQuery;
    private readonly IUserPersonalLicenceBLL _userPersonalLicenceBLL;
    private readonly IUnitOfWork _saveChanges;
    private readonly ILogger<SystemSessionBLL> _logger;
    private readonly ISettingsBLL _settings;
    private readonly IConvertSettingValue<int> _intConverter;

    private const int
        MaxConcurrentEngineerSessionCount =
            15000; //TODO пока нет механизма получения кол-во лицензий, решено было оставить константу, как появятся ключи, будете переделано под кол-во сессий из ключа
    
    public SystemSessionBLL(IUserBLL userBLL,
        IRepository<Session> repository,
        IRepository<UserSessionHistory> sessionHistoryRepository,
        IActiveEngineerSessionCountQuery activeEngineerSessionCountQuery,
        IUserPersonalLicenceBLL userPersonalLicenceBLL,
        IUnitOfWork saveChanges,
        ILogger<SystemSessionBLL> logger,
        ISettingsBLL settings,
        IConvertSettingValue<int> intConverter)
    {
        _userBLL = userBLL;
        _repository = repository;
        _sessionHistoryRepository = sessionHistoryRepository;
        _activeEngineerSessionCountQuery = activeEngineerSessionCountQuery;
        _userPersonalLicenceBLL = userPersonalLicenceBLL;
        _saveChanges = saveChanges;
        _logger = logger;
        _settings = settings;
        _intConverter = intConverter;
    }

    public async Task DeactivateInactiveSessionsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting to deactivate inactive sessions");
            
        var inactiveTime =
            _intConverter.Convert(await _settings.GetValueAsync(SystemSettings.InactiveUserTime, cancellationToken));
            
        var utcNow = DateTime.UtcNow.AddMinutes(-inactiveTime);

        var inactiveSessions = await _repository.ToArrayAsync(
            x => x.UtcDateClosed == null && x.UtcDateLastActivity < utcNow, cancellationToken);

        _logger.LogTrace($"{inactiveSessions.Length} inactive sessions was found");

        foreach (var el in inactiveSessions)
        {
            el.UtcDateClosed = DateTime.UtcNow;
            el.SecurityStamp = String.Empty;

            _sessionHistoryRepository.Insert(new UserSessionHistory(SessionHistoryType.InactiveDisabled, el.UserID,
                el.UserAgent));

            _logger.LogTrace($"Session for user with ID = {el.UserID} and User Agent = {el.UserAgent} was deactivated");
        }

        await _saveChanges.SaveAsync(cancellationToken);
        
        _logger.LogInformation($"{inactiveSessions.Length} sessions was deactivated");
    }

    public async Task CreateOrRestoreAsync(Guid userID, string securityStamp, string userAgent,
        SessionLocationType locationType, CancellationToken cancellationToken = default)
    {
        var user = await _userBLL.AnonymousDetailsAsync(userID, cancellationToken);

        using (var transaction =
               TransactionScopeCreator.Create(IsolationLevel.Serializable, TransactionScopeOption.Required))
        {
            var sessionsCount = await _activeEngineerSessionCountQuery.ExecuteAsync(cancellationToken);
            var hasPersonalLicence =
                await _userPersonalLicenceBLL.HasPersonalLicenceAsync(userID, int.MaxValue, cancellationToken);

            var needLicencesCount = hasPersonalLicence ? 1 : 0;

            if (!hasPersonalLicence
                && MaxConcurrentEngineerSessionCount > 0
                && user.HasRoles
                && sessionsCount + needLicencesCount > MaxConcurrentEngineerSessionCount)
            {
                _sessionHistoryRepository.Insert(new UserSessionHistory(SessionHistoryType.ConnectionFail, userID,
                    userAgent));

                await _saveChanges.SaveAsync(cancellationToken, IsolationLevel.Serializable);
                throw new InvalidObjectException(Resources.Session_NotEnoughLicences);
            }

            if (hasPersonalLicence)
            {
                await DisableAllButThisAsync(userID, userAgent, userID, cancellationToken);
            }

            await UpdateOrCreateSessionAsync(userID, userAgent, securityStamp, locationType, hasPersonalLicence,
                cancellationToken);

            await _saveChanges.SaveAsync(cancellationToken, IsolationLevel.Serializable);
            transaction.Complete();
        }
    }

    private async Task UpdateOrCreateSessionAsync(Guid userID, string userAgent, string securityStamp,
        SessionLocationType locationType, bool isPersonal, CancellationToken cancellationToken = default)
    {
        var activeSession = await _repository.FirstOrDefaultAsync(x => x.UserID == userID && x.UserAgent == userAgent,
            cancellationToken);

        var licenceType = isPersonal ? SessionLicenceType.Personal : SessionLicenceType.Concurrency;
        
        if (activeSession == null)
        {
            _logger.LogTrace(
                $"New session was created for user with ID = {userID}, user agent = {userAgent}");

            _repository.Insert(new Session(userID, userAgent, securityStamp, locationType, licenceType));
        }
        else
        {
            _logger.LogTrace(
                $"Existing session was updated for user with ID = {userID}, user agent = {userAgent}");
            
            activeSession.SecurityStamp = securityStamp;
            activeSession.UtcDateLastActivity = DateTime.UtcNow;
            activeSession.UtcDateOpened = activeSession.UtcDateLastActivity;
            activeSession.UtcDateClosed = null;
            activeSession.LicenceType = licenceType;
            activeSession.Location = locationType;
        }

        _sessionHistoryRepository.Insert(new UserSessionHistory(SessionHistoryType.Connect, userID, userAgent));

        _logger.LogInformation(
            $"User with ID = {userID} successfully created or updated his session, user agent = {userAgent}");
    }
    
    private async Task DisableAllButThisAsync(Guid userID, string userAgent, Guid executorID,
        CancellationToken cancellationToken = default)
    {
        var userSessions = await
            _repository.ToArrayAsync(x => x.UserID == userID && x.UserAgent != userAgent, cancellationToken);

        foreach (var el in userSessions)
        {
            el.SecurityStamp = string.Empty;
            el.UtcDateClosed = DateTime.UtcNow;

            _sessionHistoryRepository.Insert(new UserSessionHistory(SessionHistoryType.KillConnection, userID,
                userAgent, executorID));
        }
    }

    public async Task AbortAsync(Guid userID, string userAgent, string securityStamp,
        CancellationToken cancellationToken = default)
    {
        var activeSession = await
            _repository.FirstOrDefaultAsync(x => x.UserID == userID && x.UserAgent == userAgent, cancellationToken);

        if (activeSession != null)
        {
            activeSession.UtcDateClosed = DateTime.UtcNow;
            activeSession.SecurityStamp = securityStamp;
        }

        _sessionHistoryRepository.Insert(new UserSessionHistory(SessionHistoryType.Disconnect, userID, userAgent));
        
        await _saveChanges.SaveAsync(cancellationToken);
        
        _logger.LogInformation(
            $"User with ID = {userID} successfully aborted his session");
    }

    public async Task ExtendAsync(Guid userID, string userAgent, string securityStamp,
        CancellationToken cancellationToken)
    {
        var activeSession = await _repository.FirstOrDefaultAsync(
                                x => x.UserID == userID && x.UserAgent == userAgent && x.UtcDateClosed == null &&
                                     x.SecurityStamp == securityStamp, cancellationToken)
                            ?? throw new ObjectNotFoundException("Session was not found to extend");

        activeSession.UtcDateLastActivity = DateTime.UtcNow;
        
        await _saveChanges.SaveAsync(cancellationToken);
        
        _logger.LogInformation(
            $"User with ID = {userID} successfully extended his session");
    }
}