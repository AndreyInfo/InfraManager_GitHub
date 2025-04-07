using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using InfraManager.BLL.AccessManagement;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.Sessions;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.Sessions;

internal class UserPersonalLicenceBLL : IUserPersonalLicenceBLL, ISelfRegisteredService<IUserPersonalLicenceBLL>
{
    private readonly IRepository<UserPersonalLicence> _repository;
    private readonly IPagingQueryCreator _queryCreator;
    private readonly IUnitOfWork _saveChanges;
    private readonly IGuidePaggingFacade<UserPersonalLicence, UserPersonalLicenceListItem> _paggingFacade;
    private readonly IMapper _mapper;
    private readonly IUserAccessBLL _access;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<UserPersonalLicenceBLL> _logger;

    public UserPersonalLicenceBLL(
        IRepository<UserPersonalLicence> repository,
        IPagingQueryCreator queryCreator,
        IUnitOfWork saveChanges,
        IGuidePaggingFacade<UserPersonalLicence, UserPersonalLicenceListItem> paggingFacade,
        IMapper mapper,
        IUserAccessBLL access,
        ICurrentUser currentUser, 
        ILogger<UserPersonalLicenceBLL> logger)
    {
        _repository = repository;
        _queryCreator = queryCreator;
        _saveChanges = saveChanges;
        _paggingFacade = paggingFacade;
        _mapper = mapper;
        _access = access;
        _currentUser = currentUser;
        _logger = logger;
    }


    public async Task DeleteAsync(Guid userID, CancellationToken cancellationToken = default)
    {
        if (!await _access.HasAdminRoleAsync(_currentUser.UserId, cancellationToken))
        {
            _logger.LogWarning(
                $"User with ID = {_currentUser.UserId} tried to delete personal license for user with ID = {userID} without admin rights");
            
            throw new AccessDeniedException(
                $"User with ID {_currentUser.UserId} has no admin rights to delete personal licences");
        }
    
        var licences = await _repository.FirstOrDefaultAsync(x => x.UserID == userID, cancellationToken)
            ?? throw new ObjectNotFoundException($"Personal license for user with ID = {userID} was not found");

        _repository.Delete(licences);
        await _saveChanges.SaveAsync(cancellationToken);

        _logger.LogInformation(
            $"User with ID = {_currentUser.UserId} successfully deleted personal license for user with ID = {userID}");
    }

    public async Task<UserPersonalLicenceDetails[]> ListAsync(BaseFilter filter,
        CancellationToken cancellationToken = default)
    {
        if (!await _access.HasAdminRoleAsync(_currentUser.UserId, cancellationToken))
        {
            throw new AccessDeniedException(
                $"User with ID {_currentUser.UserId} has no admin rights to see personal licences");
        }
    
        var query = _repository.With(x => x.User).ThenWith(x => x.Subdivision).Query();
        var licences = await _paggingFacade.GetPaggingAsync(filter,
            query: query,
            cancellationToken: cancellationToken);

        _logger.LogInformation(
            $"User with ID = {_currentUser.UserId} successfully got users personal licenses list");

        return _mapper.Map<UserPersonalLicenceDetails[]>(licences);
    }

    public async Task InsertAsync(Guid userID, CancellationToken cancellationToken = default)
    {
        if (!await _access.HasAdminRoleAsync(_currentUser.UserId, cancellationToken))
        {
            _logger.LogWarning(
                $"User with ID = {_currentUser.UserId} tried to add personal license for user with ID = {userID} without admin rights");
        
            throw new AccessDeniedException(
                $"User with ID {_currentUser.UserId} has no admin rights to add personal licences");
        }
        
        var maxCounts = 15000; //TODO пока нет механизма получения кол-во лицензий, решено было оставить константу, как появятся ключи, будете переделано под кол-во сессий из ключа

        await InsertAsync(userID, maxCounts, cancellationToken);
    }
    
    private async Task InsertAsync(Guid userID, int maxCounts, CancellationToken cancellationToken = default)
    {
        using (var transaction =
               TransactionScopeCreator.Create(IsolationLevel.Serializable, TransactionScopeOption.Required))
        {
            var licencesCount = await _repository.CountAsync(cancellationToken);
            if (licencesCount >= maxCounts)
            {
                throw new InvalidObjectException(Resources.No_More_Personal_Licences);
            }
            
            _repository.Insert(new UserPersonalLicence(userID));
            await _saveChanges.SaveAsync(cancellationToken, IsolationLevel.Serializable);
            
            transaction.Complete();
        }

        _logger.LogInformation(
            $"User with ID = {_currentUser.UserId} successfully add new user personal license for user with ID = {userID}");
    }
    

    public async Task<bool> HasPersonalLicenceAsync(Guid userID, int maxCount,
        CancellationToken cancellationToken = default)
    {
        var result = _queryCreator.Create(_repository.Query().OrderBy(x => x.UserID));
        var orderedResult = await result.PageAsync(0, maxCount, cancellationToken);

        return orderedResult.Any(x => x.UserID == userID);
    }
}