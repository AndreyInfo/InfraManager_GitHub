using Inframanager.BLL;
using InfraManager.BLL.AccessManagement.Operations;
using InfraManager.BLL.Roles;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Operations;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations;

internal class LifeCycleStateOperationBLL : StandardBLL<Guid, LifeCycleStateOperation, LifeCycleStateOperationData, LifeCycleStateOperationDetails, LifeCycleStateOperationFilter>
    , ILifeCycleStateOperationBLL
    , ISelfRegisteredService<ILifeCycleStateOperationBLL>
{
    private readonly ILifeCycleListOperationQuery _queryListOperations;
    private readonly IRepository<RoleLifeCycleStateOperation> _roleLifeCycleStateOperationsRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IRolesBLL _rolesBLL;

    public LifeCycleStateOperationBLL(IRepository<LifeCycleStateOperation> repository
        , ILogger<LifeCycleStateOperationBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<LifeCycleStateOperationDetails, LifeCycleStateOperation> detailsBuilder
        , IInsertEntityBLL<LifeCycleStateOperation, LifeCycleStateOperationData> insertEntityBLL
        , IModifyEntityBLL<Guid, LifeCycleStateOperation, LifeCycleStateOperationData, LifeCycleStateOperationDetails> modifyEntityBLL
        , IRemoveEntityBLL<Guid, LifeCycleStateOperation> removeEntityBLL
        , IGetEntityBLL<Guid, LifeCycleStateOperation, LifeCycleStateOperationDetails> detailsBLL
        , IGetEntityArrayBLL<Guid, LifeCycleStateOperation, LifeCycleStateOperationDetails, LifeCycleStateOperationFilter> detailsArrayBLL
        , ILifeCycleListOperationQuery queryListOperations
        , IRepository<RoleLifeCycleStateOperation> roleLifeCycleStateOperationsRepository
        , IRolesBLL rolesBLL)
        : base(repository
            , logger
            , unitOfWork
            , currentUser
            , detailsBuilder
            , insertEntityBLL
            , modifyEntityBLL
            , removeEntityBLL
            , detailsBLL
            , detailsArrayBLL)
    {
        _queryListOperations = queryListOperations;
        _roleLifeCycleStateOperationsRepository = roleLifeCycleStateOperationsRepository;
        _currentUser = currentUser;
        _rolesBLL = rolesBLL;
    }

    public async Task<GroupedLifeCycleListItem[]> GetOperationsAsync(LifeCycleStateOperationFilter filter, CancellationToken cancellationToken = default)
    {
        Guid[] rolesID = null;

        if (filter.LifeCycleStateID is not null)
        {
            var roles = await _rolesBLL.GetByUserAsync(_currentUser.UserId, cancellationToken);
            rolesID = roles.Select(x => x.ID).ToArray();
        }
        else if(filter.RoleID is not null)
        {
            rolesID = new Guid[1] {(Guid)filter.RoleID};
        }

        return await _queryListOperations.ExecuteAsync(rolesID, filter.LifeCycleStateID, cancellationToken);
    }

    public async Task SaveOperationsAsync(Guid roleID, LifeCycleOperationsData[] data, CancellationToken cancellationToken = default)
    {
        foreach (var el in data)
        {
            if (el.IsSelected)
            {
                await InsertOperationAsync(roleID, el, cancellationToken);
            }
            else
            {
                await DeleteOperationAsync(roleID, el, cancellationToken);
            }
        }

        await UnitOfWork.SaveAsync(cancellationToken);
    }

    private async Task DeleteOperationAsync(Guid roleID, LifeCycleOperationsData data,
        CancellationToken cancellationToken = default)
    {
        var item = await _roleLifeCycleStateOperationsRepository.FirstOrDefaultAsync(
            x => x.RoleID == roleID && x.LifeCycleStateOperationID == data.ID, cancellationToken);

        if (item != null)
        {
            _roleLifeCycleStateOperationsRepository.Delete(item);
        }
    }

    private async Task InsertOperationAsync(Guid roleID, LifeCycleOperationsData data,
        CancellationToken cancellationToken = default)
    {
        var item = await _roleLifeCycleStateOperationsRepository.FirstOrDefaultAsync(
            x => x.RoleID == roleID && x.LifeCycleStateOperationID == data.ID, cancellationToken);

        if (item == null)
        {
            _roleLifeCycleStateOperationsRepository.Insert(new RoleLifeCycleStateOperation(roleID, data.ID));
        }
    }
}
