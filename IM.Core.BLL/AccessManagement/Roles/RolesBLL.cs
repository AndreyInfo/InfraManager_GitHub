using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.Roles;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Localization;
using InfraManager.BLL.Users;
using InfraManager.Core.Extensions;
using InfraManager.DataStructures.Graphs;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Caching.Memory;

namespace InfraManager.BLL.AccessManagement.Roles;

internal class RolesBLL : IRolesBLL, ISelfRegisteredService<IRolesBLL>
{
    private readonly IRepository<Role> _repositoryRole;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _saveChangesCommand;
    private readonly IRepository<UserRole> _userRolesRepository;
    private readonly IValidatePermissions<Role> _validatePermissions;
    private readonly ILogger<Role> _logger;
    private readonly ICurrentUser _currentUser;
    private readonly IReadonlyRepository<UserRole> _userRoles;
    private readonly ILocalizeText _localizer;
    private readonly IMemoryCache _cache;
    private readonly IGetEntityArrayBLL<Guid, Role, RoleDetails, RoleFilter> _detailsArrayBLL;
    private readonly IUserAccessBLL _access;

    public RolesBLL(
        IRepository<Role> repositoryRole,
        IMapper mapper,
        IUnitOfWork saveChangesCommand,
        IRepository<UserRole> userRolesRepository,
        IValidatePermissions<Role> validatePermissions,
        ILogger<Role> logger,
        IReadonlyRepository<UserRole> userRoles,
        ICurrentUser currentUser,
        ILocalizeText localizer,
        IMemoryCache cache,
        IGetEntityArrayBLL<Guid, Role, RoleDetails, RoleFilter> detailsArrayBLL,
        IUserAccessBLL access)
    {
        _repositoryRole = repositoryRole;
        _mapper = mapper;
        _saveChangesCommand = saveChangesCommand;
        _userRolesRepository = userRolesRepository;
        _validatePermissions = validatePermissions;
        _logger = logger;
        _userRoles = userRoles;
        _currentUser = currentUser;
        _localizer = localizer;
        _cache = cache;
        _detailsArrayBLL = detailsArrayBLL;
        _access = access;
    }

    public async Task<RoleDetails> AddAsync(RoleInsertDetails model, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert,
            cancellationToken);
        var saveModel = _mapper.Map<Role>(model);
        saveModel.Operations.ForEach(c => c.RoleID = saveModel.ID);
        saveModel.LifeCycleStateOperations.ForEach(c => c.RoleID = saveModel.ID);
        
        _repositoryRole.Insert(saveModel);
        await _saveChangesCommand.SaveAsync(cancellationToken);

        _logger.LogInformation($"User ID = {_currentUser.UserId} add new Role with Name = [{model.Name}] and ID = {saveModel.ID}");
        
        return _mapper.Map<RoleDetails>(saveModel);
    }

    public async Task<RoleDetails> UpdateAsync(Guid id, RoleData model, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update,
            cancellationToken);

        var foundEntity = await _repositoryRole.FirstOrDefaultAsync(x => x.ID == id, cancellationToken) ??
                          throw new ObjectNotFoundException<Guid>(id, ObjectClass.Role);
        
        _mapper.Map(model, foundEntity);
        await _saveChangesCommand.SaveAsync(cancellationToken);
        
        _logger.LogInformation($"User ID = {_currentUser.UserId} Saved new Role with ID = {foundEntity.ID}");
        return _mapper.Map<RoleDetails>(foundEntity);
    }

    public async Task<RoleDetails> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

        var entity = await _repositoryRole
                         .WithMany(c => c.LifeCycleStateOperations)
                            .ThenWith(c => c.LifeCycleStateOperation)
                         .WithMany(c => c.Operations)
                            .ThenWith(c => c.Operation)
                         .FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
                     ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Role);
        
        return _mapper.Map<RoleDetails>(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

        if (id == Role.AdminRoleId) // TODO: ObjectReadonlyException и Http Code будет другой в web api
            throw new InvalidObjectException(await _localizer.LocalizeAsync(nameof(Resources.CanNotDeleteAdminRole), cancellationToken));

        await ThrowIfNotExistsAsync(id, cancellationToken);

        var userRole = await _userRolesRepository.FirstOrDefaultAsync(x => x.RoleID == id, cancellationToken);

        if (userRole is not null) // TODO: Переделать на Foreign Key с OnDelete = No Action
            throw new InvalidObjectException(await _localizer.LocalizeAsync(nameof(Resources.RoleUsedInSystem), cancellationToken));
        
        
        var entity = await _repositoryRole.WithMany(c => c.Operations)
                                          .WithMany(c => c.LifeCycleStateOperations)
                                          .FirstOrDefaultAsync(c => c.ID == id, cancellationToken);

        _repositoryRole.Delete(entity);

        await _saveChangesCommand.SaveAsync(cancellationToken);
    }

    private async Task ThrowIfNotExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        if (!await _repositoryRole.AnyAsync(c => c.ID == id, cancellationToken))
            throw new ObjectNotFoundException<Guid>(id, ObjectClass.Role);
    }

    public async Task<UserRolesWithSelectedDetails[]> GetUserRolesAsync(Guid userID,
        CancellationToken cancellationToken = default)
    {
        var allRoles = await _repositoryRole.ToArrayAsync(cancellationToken);
        var userRoles = await _userRolesRepository.ToArrayAsync(x => x.UserID == userID, cancellationToken);

        var rolesWithSelected = _mapper.Map<UserRolesWithSelectedDetails[]>(allRoles);

        userRoles.ForEach(x =>
        {
            var selectedRole = rolesWithSelected.FirstOrDefault(z => z.ID == x.RoleID);
            if (selectedRole != null)
            {
                selectedRole.Selected = true;
            }
        });
        
        return rolesWithSelected;
    }

    public async Task SetRoleForUserAsync(UserRolesWithSelectedData[] userRoles, Guid userID,
        CancellationToken cancellationToken = default)
    {
        if (!await _access.UserHasOperationAsync(_currentUser.UserId, OperationID.Role_SetRole,
                cancellationToken))
        {
            throw new AccessDeniedException(
                $"User with id = {_currentUser.UserId} cant set role for users(ID = {userID})");
        }

        var rolesToDelete = userRoles.Where(x => !x.IsSelected).Select(x => new UserRole(x.ID, userID)).ToArray();
        var rolesToAdd = userRoles.Where(x => x.IsSelected).Select(x => new UserRole(x.ID, userID)).ToArray();

        rolesToDelete.ForEach(x => _userRolesRepository.Delete(x));
        rolesToAdd.ForEach(x => _userRolesRepository.Insert(x));
        
        await _saveChangesCommand.SaveAsync(cancellationToken);
        
        _cache.Remove(UserBLL.DetailsKey(userID));
        _cache.Remove(AccessManagementCacheKeys.GetUserOperationsCacheKey(userID));
        _cache.Remove(AccessManagementCacheKeys.GetUserRolesCacheKey(userID));
    }

    public async Task<RoleDetails[]> GetByUserAsync(Guid userID, CancellationToken cancellationToken)
    {
        var query = _repositoryRole.WithMany(c => c.LifeCycleStateOperations).ThenWith(c => c.LifeCycleStateOperation)
                                   .WithMany(c => c.Operations).ThenWith(c => c.Operation);

        var roles = await query.Query().Where(c => _userRoles.Query().Any(y => y.RoleID == c.ID && y.UserID == userID)).ExecuteAsync(cancellationToken);
        return roles.Select(x => _mapper.Map<RoleDetails>(x)).ToArray();
    }
    
    public async Task<RoleDetails[]> GetDetailsArrayAsync(RoleFilter filterBy, CancellationToken cancellationToken = default)
        => await _detailsArrayBLL.ArrayAsync(filterBy, cancellationToken);

    public async Task<RoleDetails[]> GetDetailsPageAsync(RoleFilter filterBy, ClientPageFilter<Role> pageBy,
        CancellationToken cancellationToken)
        => await _detailsArrayBLL.PageAsync(filterBy, pageBy, cancellationToken);
}