using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk;

internal class FindExecutorBLL<TListItem, TQueryResultItem, TFilter, TExecutor> : IFindExecutorBLL<TListItem, TFilter>
    where TExecutor : class
    where TFilter : ExecutorListFilter
{
    private readonly ICurrentUser _currentUser;
    private readonly IValidatePermissions<TExecutor> _permissionValidator;
    private readonly IValidateObject<ExecutorListFilter> _filterValidator;
    private readonly IServiceMapper<ObjectClass, IExecutorListQuery<TQueryResultItem, TExecutor>> _queryMapper;
    private readonly IMapper _mapper;

    public FindExecutorBLL(
        ICurrentUser currentUser,
        IValidatePermissions<TExecutor> permissionValidator,
        IValidateObject<ExecutorListFilter> filterValidator,
        IServiceMapper<ObjectClass, IExecutorListQuery<TQueryResultItem, TExecutor>> queryMapper,
        IMapper mapper)
    {
        _currentUser = currentUser;
        _permissionValidator = permissionValidator;
        _filterValidator = filterValidator;
        _queryMapper = queryMapper;
        _mapper = mapper;
    }

    public async Task<TListItem[]> FindAsync(TFilter filter, CancellationToken cancellationToken = default)
    {
        await _permissionValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);
        await _filterValidator.ValidateOrRaiseErrorAsync(filter, cancellationToken);

        var query = _queryMapper.Map(filter.HasAccessToObjectClassID.Value);
        var queryFilter = new ExecutorListQueryFilter
        {
            ObjectID = filter.HasAccessToObjectID.Value,
            UserIDs = filter.UserIDList?.ToArray() ?? Array.Empty<Guid>(),
            QueueIDs = filter.QueueID.HasValue
                ? new [] { filter.QueueID.Value, }
                : filter.QueueIDList ?? Array.Empty<Guid>(),
            TOZEnabled = filter.TOZEnabled,
            TTZEnabled = filter.TTZEnabled,
            ServiceResponsibilityEnabled = filter.ServiceResponsibilityEnabled,
            ShouldParticipateAutoAssign = filter.ShouldParticipateAutoAssign,
        };

        var items = await query.ExecuteAsync(queryFilter, cancellationToken);

        return _mapper.Map<TListItem[]>(items);
    }
}