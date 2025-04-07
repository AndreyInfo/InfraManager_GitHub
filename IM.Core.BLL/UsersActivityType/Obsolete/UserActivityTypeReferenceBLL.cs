using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.Settings;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;

namespace InfraManager.BLL.UsersActivityType.Obsolete;

[Obsolete("Use InfraManager.BLL.UsersActivityType.UserActivityTypeBLL instead.")]
internal class UserActivityTypeReferenceBLL : IUserActivityTypeReferenceBLL
    , ISelfRegisteredService<IUserActivityTypeReferenceBLL>
{
    private readonly IReadonlyRepository<UserActivityTypeReference> _readOnlyUserActivityTypeReferenceRepository;
    private readonly IReadonlyRepository<Group> _readOnlyGroupRepository;
    private readonly IRepository<UserActivityTypeReference> _repository;
    private readonly IUnitOfWork _saveChangesCommand;

    private readonly IUserColumnSettingsBLL _columnBLL;
    private readonly ICurrentUser _currentUser;

    private readonly IMapper _mapper;
    public UserActivityTypeReferenceBLL(IMapper mapper,
        IReadonlyRepository<UserActivityTypeReference> readOnlyUserActivityTypeReferenceRepository,
        IReadonlyRepository<Group> readOnlyGroupRepository,
        IRepository<UserActivityTypeReference> repository,
        IUnitOfWork saveChangesCommand,
        IUserColumnSettingsBLL columnBLL,
        ICurrentUser currentUser)
    {
        _mapper = mapper;
        _readOnlyUserActivityTypeReferenceRepository = readOnlyUserActivityTypeReferenceRepository;
        _readOnlyGroupRepository = readOnlyGroupRepository;
        _repository = repository;
        _saveChangesCommand = saveChangesCommand;
        _columnBLL = columnBLL;
        _currentUser = currentUser;
    }

    public async Task DeleteAsync(Guid[] ids, CancellationToken cancellationToken)
    {
        foreach (var id in ids)
        {
            var userActivityTypeReference =
                _readOnlyUserActivityTypeReferenceRepository.FirstOrDefault(x => x.ID == id)
                ?? throw new ObjectNotFoundException($"User activity type reference (ID = {id})");

            _repository.Delete(userActivityTypeReference);
        }

        await _saveChangesCommand.SaveAsync(cancellationToken);
    }


    public async Task<UserActivityTypeReferenceDetails[]> GetListByIdAsync(Guid userOrGroupId,
        CancellationToken cancellationToken)
    {
        var userActivityTypeReferenceList =
            await _readOnlyUserActivityTypeReferenceRepository.ToArrayAsync(x => x.ObjectID == userOrGroupId,
                cancellationToken);

        return _mapper.Map<UserActivityTypeReferenceDetails[]>(userActivityTypeReferenceList);
    }

    public async Task<UserActivityTypePathDetails[]> GetListUserActivityTypeByIdAsync(Guid userOrGroupId,
        BaseFilter filter, CancellationToken cancellationToken)
    {
        var userActivityTypeReferenceList =
            await _readOnlyUserActivityTypeReferenceRepository.With(c=> c.Type)
                                                              .ToArrayAsync(x => x.ObjectID == userOrGroupId, cancellationToken);

        var userActivityTypeWithChildsList =
            _mapper.Map<UserActivityTypeWithChildsDetails[]>(userActivityTypeReferenceList);

        foreach (var item in userActivityTypeWithChildsList)
        {
            item.BuildParents();
            item.Path = string.Join(" / ", item.Types.Select(c=> c.Name));
        }

        var userActivityTypePathList = _mapper.Map<UserActivityTypePathDetails[]>(userActivityTypeWithChildsList);

        var columns = await _columnBLL.GetAsync(_currentUser.UserId, filter.ViewName, cancellationToken);
        var orderColumn = columns.GetSortColumn();
        return ApplyFilter(userActivityTypePathList, filter, orderColumn);
    }

    //TODO отрефакторить, должно происходить на уровне БД
    private UserActivityTypePathDetails[] ApplyFilter(IEnumerable<UserActivityTypePathDetails> userActivityTypePathList,
        BaseFilter filter, Sort orderColumn)
    {
        if (!string.IsNullOrEmpty(filter.SearchString))
            userActivityTypePathList = userActivityTypePathList.Where(x => x.Path.Contains(filter.SearchString));

        var prop = typeof(UserActivityTypePathDetails).GetProperty(orderColumn.PropertyName);
        userActivityTypePathList = orderColumn.Ascending
            ? userActivityTypePathList.OrderBy(x => prop.GetValue(x, null))
            : userActivityTypePathList.OrderByDescending(x => prop.GetValue(x, null));

        if (filter.StartRecordIndex >= 1)
            userActivityTypePathList = userActivityTypePathList.Skip(filter.StartRecordIndex);

        if (filter.CountRecords > 0)
            userActivityTypePathList = userActivityTypePathList.Take(filter.CountRecords);

        return userActivityTypePathList.ToArray();
    }

    public async Task<Guid[]> InsertAsync(UserActivityTypeReferenceDetails[] models,
        CancellationToken cancellationToken)
    {
        var ids = new Guid[models.Length];

        for (var i = 0; i < models.Length; i++) 
            ids[i] = await InsertOneTypeAsync(models[i], cancellationToken);

        await _saveChangesCommand.SaveAsync(cancellationToken);


        return ids;
    }

    private async Task<Guid> InsertOneTypeAsync(UserActivityTypeReferenceDetails model,
        CancellationToken cancellationToken)
    {
        var queue = await _readOnlyGroupRepository.With(x => x.QueueUsers)
            .FirstOrDefaultAsync(x => x.IMObjID == model.ObjectId, cancellationToken);

        if (queue is not null)
            await AddTypeToUsersIsQueueAsync(queue, model.UserActivityTypeId, cancellationToken);

        var entity = queue is null
            ? _mapper.Map<UserActivityTypeReference>(model, opt => opt.Items["ObjectClassId"] = ObjectClass.Group)
            : _mapper.Map<UserActivityTypeReference>(model);

        _repository.Insert(entity);

        return entity.ID;
    }

    private async Task AddTypeToUsersIsQueueAsync(Group queue, Guid userActivityTypeID,
        CancellationToken cancellationToken)
    {
        foreach (var user in queue.QueueUsers)
        {
            var model = new UserActivityTypeReferenceDetails(userActivityTypeID, ObjectClass.User, user.UserID);

            var isExistsActivityTypeUser = await _repository.AnyAsync(c => c.ObjectClassID == ObjectClass.User 
                                                                           && c.ObjectID == user.UserID, cancellationToken);
            if (!isExistsActivityTypeUser)
                await InsertOneTypeAsync(model, cancellationToken);
        }
    }
}