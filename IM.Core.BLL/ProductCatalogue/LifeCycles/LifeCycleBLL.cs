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

namespace InfraManager.BLL.ProductCatalogue.LifeCycles;

internal sealed class LifeCycleBLL : StandardBLL<Guid, LifeCycle, LifeCycleData, LifeCycleDetails, LifeCycleFilter>
    , ILifeCycleBLL
    , ISelfRegisteredService<ILifeCycleBLL>
{
    private readonly IMapper _mapper;
    private readonly IValidatePermissions<LifeCycle> _validatePermissions;

    public LifeCycleBLL(IRepository<LifeCycle> repository
        , ILogger<LifeCycleBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<LifeCycleDetails, LifeCycle> detailsBuilder
        , IInsertEntityBLL<LifeCycle, LifeCycleData> insertEntityBLL
        , IModifyEntityBLL<Guid, LifeCycle, LifeCycleData, LifeCycleDetails> modifyEntityBLL
        , IRemoveEntityBLL<Guid, LifeCycle> removeEntityBLL
        , IGetEntityBLL<Guid, LifeCycle, LifeCycleDetails> detailsBLL
        , IGetEntityArrayBLL<Guid, LifeCycle, LifeCycleDetails, LifeCycleFilter> detailsArrayBLL
        , IMapper mapper
        , IValidatePermissions<LifeCycle> validatePermissions)
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
        _validatePermissions = validatePermissions;
    }

    public async Task<LifeCycleDetails> InsertAsAsync(LifeCycleData data, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.InsertAs, cancellationToken);

        var entity = _mapper.Map<LifeCycle>(data);
        Repository.Insert(entity);
        await UnitOfWork.SaveAsync(cancellationToken);

        return _mapper.Map<LifeCycleDetails>(entity);
    }
}
