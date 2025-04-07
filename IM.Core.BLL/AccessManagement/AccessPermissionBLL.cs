using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.OrganizationStructure;
using System.Transactions;
using InfraManager.BLL.AccessManagement.AccessPermissions;
using InfraManager.BLL.AccessManagement.ForTable;
using InfraManager.BLL.Settings;
using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.Localization;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Logging;
using Inframanager;

namespace InfraManager.BLL.AccessManagement;

internal class AccessPermissionBLL : IAccessPermissionBLL, ISelfRegisteredService<IAccessPermissionBLL>
{
    private readonly IMapper _mapper;
    private readonly IRepository<AccessPermission> _repositoryAccessPermission;
    private readonly IRepository<OrganizationItemGroup> _repositoryOrganizationItemGroups;
    private readonly IReadonlyRepository<User> _users;
    private readonly IReadonlyRepository<Owner> _owners;
    private readonly IReadonlyRepository<Group> _groups;
    private readonly IUnitOfWork _saveChangesCommand;
    private readonly ICurrentUser _currentUser;
    private readonly IOrganizationItemGroupQuery _organizationItemGroupQuery;
    private readonly IUserColumnSettingsBLL _columnBLL;
    private readonly IColumnMapper<AccessPermissionModelItem, AccessPermissionForTable> _columnMapper;
    private readonly ILocalizeText _localizeText;
    private readonly ILogger<AccessPermissionBLL> _logger;


    public AccessPermissionBLL(IMapper mapper
                               , IRepository<AccessPermission> repositoryAccessPermission
                               , IRepository<OrganizationItemGroup> repositoryOrganizationItemGroups
                               , IUnitOfWork saveChangesCommand
                               , IReadonlyRepository<User> users
                               , IReadonlyRepository<Owner> owners
                               , IReadonlyRepository<Group> groups
                               , ICurrentUser currentUser
                               , IOrganizationItemGroupQuery organizationItemGroupQuery
                               , IUserColumnSettingsBLL columnBLL
                               , IColumnMapper<AccessPermissionModelItem, AccessPermissionForTable> columnMapper
                               , ILocalizeText localizeText
                               , ILogger<AccessPermissionBLL> logger)
    {
        _mapper = mapper;
        _repositoryAccessPermission = repositoryAccessPermission;
        _repositoryOrganizationItemGroups = repositoryOrganizationItemGroups;
        _saveChangesCommand = saveChangesCommand;
        _users = users;
        _owners = owners;
        _groups = groups;
        _owners = owners;
        _currentUser = currentUser;
        _organizationItemGroupQuery = organizationItemGroupQuery;
        _columnBLL = columnBLL;
        _columnMapper = columnMapper;
        _localizeText = localizeText;
        _logger = logger;
    }

    public async Task<AccessPermission> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} request {ObjectAction.ViewDetails} AccessPermission with id = {id}");

        return await _repositoryAccessPermission.FirstOrDefaultAsync(c=> c.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.AccessPermission);
    }

    public async Task<AccessPermissionDetails[]> GetDataTableAsync(BaseFilterWithClassIDAndID<Guid> filter, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} request {ObjectAction.ViewDetailsArray } AccessPermission for object {filter.ClassID} with id = {filter.ObjectID}");


        _logger.LogTrace($"UserID = {_currentUser.UserId} start get columns for AccessPermission");
        
        var columns = await _columnBLL.GetAsync(_currentUser.UserId, filter.ViewName, cancellationToken);
        var orderColumn = columns.GetSortColumn();
        var mappedValues = _columnMapper.MapToStringArray(orderColumn.PropertyName);
        orderColumn.PropertyName = mappedValues.First();

        _logger.LogTrace($"UserID = {_currentUser.UserId} finish get columns for AccessPermission");



        _logger.LogTrace($"UserID = {_currentUser.UserId} start get data from DataBase for AccessPermission");
        var organizationItemGroup = await _organizationItemGroupQuery.ExecuteAsync(filter.ObjectID,
            filter.ClassID,
            filter.SearchString,
            orderColumn,
            mappedValues,
            filter.CountRecords,
            filter.StartRecordIndex,
            cancellationToken);
        _logger.LogTrace($"UserID = {_currentUser.UserId} finish get data from DataBase for AccessPermission");

        return _mapper.Map<AccessPermissionDetails[]>(organizationItemGroup);
    }

    public async Task RemoveAsync(Guid accessPermissionID, Guid objectID, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} start {ObjectAction.Delete} AccessPermission accessPermissionID = {accessPermissionID} for objectID = {objectID}");

        var organizationItemGroup = await _repositoryOrganizationItemGroups.FirstOrDefaultAsync(c => c.ID == accessPermissionID && c.ItemID == objectID, cancellationToken)
                        ?? throw new ObjectNotFoundException($"Not found OrganizationItemGroup for AccessPermission id = {accessPermissionID}");

        _repositoryOrganizationItemGroups.Delete(organizationItemGroup);

        await _saveChangesCommand.SaveAsync(cancellationToken);
        _logger.LogTrace($"UserID = {_currentUser.UserId} finish {ObjectAction.Delete} AccessPermission accessPermissionID = {accessPermissionID} for objectID = {objectID}");
    }

    public async Task<Guid> AddAsync(AccessPermissionDetails model, CancellationToken cancellationToken = default)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} start {ObjectAction.Insert} to object {model.ObjectClassId} for {model.OwnerClassId}");


        await ThrowIfExistsSameAccessPermissionAsync(model, cancellationToken);

        using var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required);
        var saveModel = _mapper.Map<AccessPermission>(model);
        _repositoryAccessPermission.Insert(saveModel);
        await _saveChangesCommand.SaveAsync(cancellationToken);

        await InsertOrganizateItemGroupAsync(saveModel.ID, model.OwnerId, model.OwnerClassId, cancellationToken);
        await _saveChangesCommand.SaveAsync(cancellationToken);
        transaction.Complete();

        _logger.LogTrace($"UserID = {_currentUser.UserId} finish {ObjectAction.Insert} to object {model.ObjectClassId} for {model.OwnerClassId}");
        return saveModel.ID;
    }

    public async Task<Guid> UpdateAsync(AccessPermissionDetails model, CancellationToken cancellationToken = default)
    {
        _logger.LogTrace($"UserID = {_currentUser.UserId} start {ObjectAction.Update} to object {model.ObjectClassId} for {model.OwnerClassId}");

        var foundEntity = await _repositoryAccessPermission.FirstOrDefaultAsync(x => x.ID.Equals(model.Id), cancellationToken)
                                                           ?? throw new ObjectNotFoundException<Guid>(model.Id, ObjectClass.AccessPermission);

        _mapper.Map(model, foundEntity);

        await _saveChangesCommand.SaveAsync(cancellationToken);

        _logger.LogTrace($"UserID = {_currentUser.UserId} finish {ObjectAction.Update} to object {model.ObjectClassId} for {model.OwnerClassId}");
        return foundEntity.ID;
    }

    // TODO передлать на индексы
    private async Task ThrowIfExistsSameAccessPermissionAsync(AccessPermissionDetails model, CancellationToken cancellationToken)
    {
        var isExsitsSame = await _repositoryOrganizationItemGroups.AnyAsync(c => c.ItemClassID == model.OwnerClassId
                                                             && c.ItemID == model.OwnerId
                                                             && c.AccessPermission.ObjectID == model.ObjectId
                                                             && c.AccessPermission.ObjectClassID == model.ObjectClassId
                                                             , cancellationToken);

        if (isExsitsSame)
            throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.ErrorExistsWithSameParametr), cancellationToken));
    }
   
    private async Task InsertOrganizateItemGroupAsync(Guid id, Guid ownerID, ObjectClass ownerClassID, CancellationToken cancellationToken)
    {
        if (await DoesExistOrganizateItemGroupAsync(id, ownerID, ownerClassID, cancellationToken))
            return;

        var saveModelOrganizationItemGroups = new OrganizationItemGroup(id, ownerID, ownerClassID);
        _repositoryOrganizationItemGroups.Insert(saveModelOrganizationItemGroups);
    }

    private async Task<bool> DoesExistOrganizateItemGroupAsync(Guid accessPermissionID, Guid ownerID, ObjectClass classID, CancellationToken cancellationToken)
        => await _repositoryOrganizationItemGroups.AnyAsync(c => 
                        c.ID == accessPermissionID
                        && c.ItemID == ownerID
                        && c.ItemClassID == classID,
                cancellationToken);


    public async Task<AccessPermissionData> GetAccessUserToObjectByIDAsync(Guid objectID, ObjectClass objectClassID, CancellationToken cancellationToken)
    {
        var result = new AccessPermissionData()
        {
            ObjectID = objectID,
            ClassID = objectClassID,
            OwnerID = _currentUser.UserId,
            Rights = new AccessPermissionRightsDetails()
        };

        var accessPermissions = await _repositoryAccessPermission.ToArrayAsync(c => c.ObjectClassID == objectClassID && c.ObjectID == objectID, cancellationToken);

        foreach (var item in accessPermissions)
        {
            var itemGroup = await _repositoryOrganizationItemGroups.FirstOrDefaultAsync(c => c.ID == item.ID, cancellationToken);
            if (itemGroup is null || !await HasAccessToServiceAsync(itemGroup.ItemID, itemGroup.ItemClassID, cancellationToken))
                continue;

            CalculateRights(result.Rights, item);
        }

        return result;
    }

    private void CalculateRights(AccessPermissionRightsDetails rights, AccessPermission item)
    {
        rights.HasUpdatePermissions= rights.HasUpdatePermissions|| item.Update.GetValueOrDefault();
        rights.HasDeletePermissions= rights.HasDeletePermissions || item.Delete.GetValueOrDefault();
        rights.HasAddPermissions= rights.HasAddPermissions || item.Add.GetValueOrDefault();
        rights.HasPropertiesPermissions = rights.HasPropertiesPermissions || item.Properties.GetValueOrDefault();
        rights.HasAccessManagePermissions = rights.HasAccessManagePermissions || item.AccessManage.GetValueOrDefault();
    }


    private async Task<bool> HasAccessToServiceAsync(Guid itemID, ObjectClass itemClassID, CancellationToken cancellationToken) =>
         itemClassID switch
         {
             ObjectClass.User => _currentUser.UserId == itemID,
             ObjectClass.Group => await _groups.AnyAsync(c => c.IMObjID == itemID
                                                                       && (c.ResponsibleID == _currentUser.UserId
                                                                             || c.QueueUsers.Any(v => v.UserID == _currentUser.UserId))
                                                                 , cancellationToken),
             ObjectClass.Division => await SubdivisionsContainsUsers(itemID, cancellationToken),
             ObjectClass.Organizaton => await _users.AnyAsync(c => c.IMObjID == _currentUser.UserId
                                                                           && c.Subdivision.OrganizationID == itemID
                                                                       , cancellationToken),
             ObjectClass.Owner => await _owners.AnyAsync(c=> c.IMObjID == _currentUser.UserId),
             _ => throw new Exception($"Нет поддержки доступа к сервису для {itemClassID}")
         };


    /// <summary>
    /// Проверяет является ли пользователь участником подраздления (учитывая вложенность подраздалений)
    /// </summary>
    /// <param name="subdivisionID">идентификатор подразделений</param>
    /// <param name="cancellationToken"></param>
    /// <returns>является участником подразделения или нет</returns>
    private async Task<bool> SubdivisionsContainsUsers(Guid subdivisionID, CancellationToken cancellationToken)
    {
        var user = await _users.With(c => c.Subdivision)
            .ThenWith(c => c.ParentSubdivision)
            .FirstOrDefaultAsync(c => c.IMObjID == _currentUser.UserId, cancellationToken);

        var subdivision = user.Subdivision;
        do
        {
            if (subdivision.ID == subdivisionID || subdivision.SubdivisionID == subdivisionID)
                return true;

            subdivision = subdivision.ParentSubdivision;
        }
        while (subdivision is not null && subdivision.SubdivisionID.HasValue);

        return false;
    }
}
