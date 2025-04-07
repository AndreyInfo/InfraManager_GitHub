using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using PortAdapterEntity = InfraManager.DAL.Asset.PortAdapter;

namespace InfraManager.BLL.Asset.PortAdapter;

internal class PortAdapterBLL : StandardBLL<Guid, PortAdapterEntity, PortAdapterData, PortAdapterDetails, PortAdapterFilter>
    , IPortAdapterBLL
    , ISelfRegisteredService<IPortAdapterBLL>
{
    private readonly IMapper _mapper;
    private readonly IValidatePermissions<PortAdapterEntity> _validatePermissions;
    private readonly IGuidePaggingFacade<PortAdapterEntity, PortAdapterColumns> _guidePaggingFacade;
    private readonly IBuildEntityQuery<PortAdapterEntity, PortAdapterDetails, PortAdapterFilter> _buildEntityQuery;

    public PortAdapterBLL(
          IRepository<PortAdapterEntity> repository
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , ILogger<PortAdapterBLL> logger
        , IValidatePermissions<PortAdapterEntity> validatePermissions
        , IBuildObject<PortAdapterDetails, PortAdapterEntity> detailsBuilder
        , IInsertEntityBLL<PortAdapterEntity, PortAdapterData> insertEntityBLL
        , IRemoveEntityBLL<Guid, PortAdapterEntity> removeEntityBLL
        , IGuidePaggingFacade<PortAdapterEntity, PortAdapterColumns> guidePaggingFacade
        , IGetEntityBLL<Guid, PortAdapterEntity, PortAdapterDetails> detailsBLL
        , IBuildEntityQuery<PortAdapterEntity, PortAdapterDetails, PortAdapterFilter> buildEntityQuery
        , IGetEntityArrayBLL<Guid, PortAdapterEntity, PortAdapterDetails, PortAdapterFilter> detailsArrayBLL
        , IModifyEntityBLL<Guid, PortAdapterEntity, PortAdapterData, PortAdapterDetails> modifyEntityBLL)
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
        _buildEntityQuery = buildEntityQuery;
        _guidePaggingFacade = guidePaggingFacade;
        _validatePermissions = validatePermissions;
    }

    public async Task<PortAdapterDetails[]> GetListAsync(PortAdapterFilter filter, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var ports = await _guidePaggingFacade.GetPaggingAsync(filter
            , _buildEntityQuery.Query(filter)
            , null
            , cancellationToken);

        return _mapper.Map<PortAdapterDetails[]>(ports);
    }
}