using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL;
using InfraManager.DAL.Sessions;

namespace InfraManager.BLL.Sessions;

internal class CookieSessionDeactivator : ISessionDeactivator, ISelfRegisteredService<ISessionDeactivator>
{
    private readonly IRepository<Session> _repository;
    private readonly IRepository<UserSessionHistory> _repositorySessionHistory;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _saveChanges;
    
    public CookieSessionDeactivator(IRepository<Session> repository,
        IRepository<UserSessionHistory> repositorySessionHistory,
        ICurrentUser currentUser,
        IUnitOfWork saveChanges)
    {
        _repository = repository;
        _repositorySessionHistory = repositorySessionHistory;
        _currentUser = currentUser;
        _saveChanges = saveChanges;
    }
    
    public async Task DeactivateAsync(Guid userID, string userAgent, CancellationToken cancellationToken = default)
    {
        var activeSession = await _repository.FirstOrDefaultAsync(x => x.UserID == userID && x.UserAgent == userAgent,
            cancellationToken) ?? throw new ObjectNotFoundException(
            $"Session with id = {userID} was not found to deactivate it");

        activeSession.SecurityStamp = string.Empty;
        activeSession.UtcDateClosed = DateTime.UtcNow;

        _repositorySessionHistory.Insert(new UserSessionHistory(SessionHistoryType.KillConnection, userID, userAgent,
            _currentUser.UserId));

        await _saveChanges.SaveAsync(cancellationToken);
    }
}