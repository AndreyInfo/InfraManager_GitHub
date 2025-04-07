using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.Users;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Operations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.AccessManagement.Operations;

internal class OperationsBLL : IOperationsBLL, ISelfRegisteredService<IOperationsBLL>
{

    private readonly IOperationGetQuery _getQuery;
    private readonly IRepository<RoleOperation> _repositoryRoleOperations;
    private readonly IUnitOfWork _saveChanges;
    private readonly IValidatePermissions<RoleOperation> _permissionsValidator;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<RoleOperation> _logger;
    private readonly IRepository<UserRole> _userRolesRepository;
    private readonly IMemoryCache _memoryCache;

    private Guid currentUserID => _currentUser.UserId;

    public OperationsBLL(IOperationGetQuery getQuery,
        IRepository<RoleOperation> repositoryRoleOperations,
        IUnitOfWork saveChanges,
        IValidatePermissions<RoleOperation> permissionsValidator,
        ICurrentUser currentUser,
        ILogger<RoleOperation> logger,
        IRepository<UserRole> userRolesRepository,
        IMemoryCache memoryCache)
    {
        _getQuery = getQuery;
        _repositoryRoleOperations = repositoryRoleOperations;
        _saveChanges = saveChanges;
        _permissionsValidator = permissionsValidator;
        _currentUser = currentUser;
        _logger = logger;
        _userRolesRepository = userRolesRepository;
        _memoryCache = memoryCache;
    }

    public async Task<GroupedOperationListItem[]> GetOperationsListAsync(OperationFilter filter,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"User ID = {currentUserID} requested all operations");
        await _permissionsValidator.ValidateOrRaiseErrorAsync(_logger, currentUserID, ObjectAction.ViewDetails,
            cancellationToken);

        _logger.LogInformation($"User ID = {currentUserID} got all operations");
        return await _getQuery.ExecuteAsync(filter.RoleID, filter.OnlySelectedForRole, cancellationToken);
    }

    public async Task UpdateOperationsRolesAsync(Guid roleID, OperationData[] data, CancellationToken cancellationToken)
    {
        if (roleID == Role.AdminRoleId)
        {
            throw new ObjectReadonlyException("Admin Role", roleID);
        }
        
        switch (data[0]?.IsSelect)
        {
            case true:
            {
                await InsertRoleOperationsAsync(roleID, data, cancellationToken);
            }break;
            case false:
            {
                await DeleteRoleOperationsAsync(roleID, data, cancellationToken);
            }break;
        }
        
        await RemoveCacheAsync(roleID, cancellationToken);
        await _saveChanges.SaveAsync(cancellationToken);
    }

    private async Task InsertRoleOperationsAsync(Guid roleID, OperationData[] data,
        CancellationToken cancellationToken = default)
    {
        await _permissionsValidator.ValidateOrRaiseErrorAsync(_logger, currentUserID, ObjectAction.Insert,
            cancellationToken);
        
        foreach (var el in data)
        {
            if (_repositoryRoleOperations
                    .Any(x => x.OperationID == el.ID && x.RoleID == roleID))
            {
                continue;
            }

            var roleOperation = new RoleOperation(roleID, el.ID);

            _logger.LogInformation(
                $"User ID = {currentUserID} insert operation with ID = {el.ID} from Role with ID = {roleID}");
                
            _repositoryRoleOperations.Insert(roleOperation);
        }
    }

    private async Task RemoveCacheAsync(Guid roleID, CancellationToken cancellationToken = default)
    {
        var users = await _userRolesRepository.ToArrayAsync(x => x.RoleID == roleID, cancellationToken);
        foreach (var user in users)
        {
            _memoryCache.Remove(UserBLL.DetailsKey(user.UserID));
            _memoryCache.Remove(AccessManagementCacheKeys.GetUserOperationsCacheKey(user.UserID));
            _memoryCache.Remove(AccessManagementCacheKeys.GetUserRolesCacheKey(user.UserID));
        }
    } // TODO: Переписать весь метод

    private async Task DeleteRoleOperationsAsync(Guid roleID, OperationData[] data,
        CancellationToken cancellationToken = default)
    {
        await _permissionsValidator.ValidateOrRaiseErrorAsync(_logger, currentUserID, ObjectAction.Delete,
            cancellationToken);
        
        foreach (var el in data)
        {
            if (!_repositoryRoleOperations
                    .Any(x => x.OperationID == el.ID && x.RoleID == roleID))
            {
                continue;
            }

            var entity = _repositoryRoleOperations.FirstOrDefault(x =>
                x.OperationID == el.ID && x.RoleID == roleID);

            _logger.LogInformation(
                $"User ID = {currentUserID} deleted operation with ID = {el.ID} from Role with ID = {roleID}");
            
            _repositoryRoleOperations.Delete(entity);
        }
    }
}
