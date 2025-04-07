using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;

internal class LifeCycleStateBLL : StandardBLL<Guid, LifeCycleState, LifeCycleStateData, LifeCycleStateDetails, LifeCycleStateFilter>
    , ILifeCycleStateBLL
    , ISelfRegisteredService<ILifeCycleStateBLL>
{
    private readonly IMapper _mapper;
    private readonly IGuidePaggingFacade<LifeCycleState, LifeCycleStateColumns> _guidePaggingFacade;
    private readonly IValidatePermissions<LifeCycleState> _validatePermissions;

    public LifeCycleStateBLL(IRepository<LifeCycleState> repository
        , ILogger<LifeCycleStateBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<LifeCycleStateDetails, LifeCycleState> detailsBuilder
        , IInsertEntityBLL<LifeCycleState, LifeCycleStateData> insertEntityBLL
        , IModifyEntityBLL<Guid, LifeCycleState, LifeCycleStateData, LifeCycleStateDetails> modifyEntityBLL
        , IRemoveEntityBLL<Guid, LifeCycleState> removeEntityBLL
        , IGetEntityBLL<Guid, LifeCycleState, LifeCycleStateDetails> detailsBLL
        , IGetEntityArrayBLL<Guid, LifeCycleState, LifeCycleStateDetails, LifeCycleStateFilter> detailsArrayBLL
        , IMapper mapper
        , IGuidePaggingFacade<LifeCycleState, LifeCycleStateColumns> guidePaggingFacade
        , IValidatePermissions<LifeCycleState> validatePermissions)
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
        _mapper = mapper;
        _guidePaggingFacade = guidePaggingFacade;
        _validatePermissions = validatePermissions;
    }

    public async Task<LifeCycleStateDetails[]> GetByLifeCycleIDAsync(LifeCycleStateFilter filter, CancellationToken cancellationToken)
    {
        Logger.LogTrace($"UserID = {CurrentUser.UserId} request {ObjectAction.ViewDetailsArray} {nameof(LifeCycleState)}");

        await _validatePermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        Logger.LogTrace($"UserID = {CurrentUser.UserId} start {ObjectAction.ViewDetailsArray} {nameof(LifeCycleState)}");


        var query = Repository.WithMany(c => c.LifeCycleStateOperations).Query();
        if(filter.LifeCycleID.HasValue)
            query = query.Where(c => c.LifeCycleID == filter.LifeCycleID);

        var states = await _guidePaggingFacade.GetPaggingAsync(filter
            , query
            , c => c.Name.ToLower().Contains(filter.SearchString.ToLower())
            , cancellationToken);

        return _mapper.Map<LifeCycleStateDetails[]>(states);
    }
}
