using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.Asset;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.OrganizationStructure.Groups;

internal class GroupBLL : IGroupBLL, ISelfRegisteredService<IGroupBLL>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _saveChangesCommand;
    private readonly IRepository<Group> _groupRepository;
    private readonly IRepository<GroupUser> _groupUserRepository;
    private readonly IValidatePermissions<Group> _validatePermissions;
    private readonly ILogger<Group> _logger;
    private readonly IObjectResponsibilityAccessBLL _objectResponsibilityAccessBLL;
    private readonly ICurrentUser _currentUser;

    public GroupBLL(IObjectResponsibilityAccessBLL objectResponsibilityAccessBLL
        , IMapper mapper
        , IUnitOfWork saveChangesCommand
        , IRepository<Group> groupRepository
        , IRepository<GroupUser> groupUserRepository
        , IValidatePermissions<Group> validatePermissions
        , ILogger<Group> logger
        , ICurrentUser currentUser)
    {
        _objectResponsibilityAccessBLL = objectResponsibilityAccessBLL;
        _mapper = mapper;
        _saveChangesCommand = saveChangesCommand;
        _groupRepository = groupRepository;
        _groupUserRepository = groupUserRepository;
        _currentUser = currentUser;
        _logger = logger;
        _validatePermissions = validatePermissions;
    }

    public async Task<GroupDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} requested {nameof(Group)} with id = {id}");
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

        var group = await _groupRepository.With(c => c.ResponsibleUser)
                        .FirstOrDefaultAsync(c => c.IMObjID == id, cancellationToken)
                    ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Group);

        return _mapper.Map<GroupDetails>(group);
    }


    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

        var group = await _groupRepository.WithMany(c => c.QueueUsers)
                                          .FirstOrDefaultAsync(c => c.IMObjID == id, cancellationToken)
                                          ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Group);

        using (var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
        {
            _groupRepository.Delete(group);
            await _objectResponsibilityAccessBLL.DeleteByOwnerAsync(id, cancellationToken);
            await _saveChangesCommand.SaveAsync(cancellationToken);
            transaction.Complete();
        }

        _logger.LogTrace($"UserID = {_currentUser.UserId} deleted {nameof(Group)} with id={id}");
    }

    public async Task<GroupDetails[]> GetListAsync(string searchName, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} requested {nameof(Group)}s");
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var result = string.IsNullOrWhiteSpace(searchName)
            ? await _groupRepository.ToArrayAsync(cancellationToken)
            : await _groupRepository.ToArrayAsync(c => c.Name.ToLower().Contains(searchName.ToLower()), cancellationToken);

        return _mapper.Map<GroupDetails[]>(result);
    }
    
    #region Сохранение 

    public async Task<Guid> AddAsync(GroupData data, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);
        var saveModel = _mapper.Map<Group>(data);
        
        saveModel.Type = GroupTypeExtensions.ConvertToGroupType(data.Type);

        _logger.LogTrace($"UserID = {_currentUser.UserId} has permission to insert {nameof(Group)}");

        _groupRepository.Insert(saveModel);
        await SavePerformersAsync(data.PerformersID ?? Array.Empty<Guid>(), saveModel.IMObjID, cancellationToken);
        await _saveChangesCommand.SaveAsync(cancellationToken);


        _logger.LogTrace($"UserID = {_currentUser.UserId} inserted new {nameof(Group)}");

        return saveModel.IMObjID;
    }


    public async Task<Guid> UpdateAsync(GroupDetails details, Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);

        var group = await _groupRepository.FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken)
                                           ?? throw new ObjectNotFoundException($"Group not found. ID = {id}");

        _mapper.Map(details, group);
        group.Type = GroupTypeExtensions.ConvertToGroupType(details.Type);
        
        await SavePerformersAsync(details.PerformersId ?? Array.Empty<Guid>(), id, cancellationToken);
        await _saveChangesCommand.SaveAsync(cancellationToken);

        _logger.LogTrace($"UserID = {_currentUser.UserId} updated {nameof(Group)}");

        return id;
    }


    private async Task SavePerformersAsync(Guid[] performerIDs, Guid queueID, CancellationToken cancellationToken = default)
    {
        var allPerformers = await _groupUserRepository.ToArrayAsync(x => x.GroupID == queueID, cancellationToken);

        var deletedPerformers = allPerformers.Where(x => !performerIDs.Contains(x.UserID)).ToArray();
        var savedPerformers = performerIDs.Where(x => !allPerformers.Select(c => c.UserID).Contains(x)).ToArray();

        RemovePerformers(deletedPerformers);
        InsertPerformers(savedPerformers, queueID);
    }

    private void RemovePerformers(GroupUser[] performers)
    {
        if (performers is null)
            return;

        foreach (var el in performers)
            _groupUserRepository.Delete(el);
    }

    private void InsertPerformers(Guid[] performers, Guid queueID)
    {
        if (performers is null)
            return;

        foreach (var userID in performers)
        {
            var user = new GroupUser { UserID = userID, GroupID = queueID };
            _groupUserRepository.Insert(user);
        }
    }


    
    
    #endregion
}