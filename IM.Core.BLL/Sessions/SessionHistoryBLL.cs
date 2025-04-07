using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.AccessManagement;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.Sessions;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.Sessions;

internal class SessionHistoryBLL : ISessionHistoryBLL, ISelfRegisteredService<ISessionHistoryBLL>
{
    private readonly IRepository<UserSessionHistory> _repository;
    private readonly IGuidePaggingFacade<UserSessionHistory, UserSessionHistoryListItem> _paggingFacade;
    private readonly IMapper _mapper;
    private readonly IUserAccessBLL _access;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<SessionHistoryBLL> _logger;

    public SessionHistoryBLL(IRepository<UserSessionHistory> repository,
        IGuidePaggingFacade<UserSessionHistory, UserSessionHistoryListItem> paggingFacade,
        IMapper mapper,
        IUserAccessBLL access,
        ICurrentUser currentUser,
        ILogger<SessionHistoryBLL> logger)
    {
        _repository = repository;
        _paggingFacade = paggingFacade;
        _mapper = mapper;
        _access = access;
        _currentUser = currentUser;
        _logger = logger;
    }
    
    public async Task<UserSessionHistoryDetails[]> ListAsync(BaseFilter filter,
        CancellationToken cancellationToken = default)
    {
        if (!await _access.HasAdminRoleAsync(_currentUser.UserId, cancellationToken))
        {
            throw new AccessDeniedException($"User with ID {_currentUser.UserId} has no admin rights to see sessions");
        }

        var query = _repository.DisableTrackingForQuery().With(x => x.User).With(x => x.Executor).Query();

        var sessions = await _paggingFacade.GetPaggingAsync(filter,
            query: query,
            cancellationToken: cancellationToken);

        _logger.LogInformation(
            $"User with ID = {_currentUser.UserId} successfully got users session history list");

        return _mapper.Map<UserSessionHistoryDetails[]>(sessions);
    }
}