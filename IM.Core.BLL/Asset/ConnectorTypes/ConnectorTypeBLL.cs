using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.ProductCatalogue.Manufactures;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.ConnectorTypes;

internal class ConnectorTypeBLL :
    StandardBLL<int, ConnectorType, ConnectorTypeData, ConnectorTypeDetails, BaseFilter>
    , IConnectorTypeBLL
    , ISelfRegisteredService<IConnectorTypeBLL>
{
    private readonly IMapper _mapper;
    private readonly IValidatePermissions<ConnectorType> _validatePermissions;
    private readonly IGuidePaggingFacade<ConnectorType, ConnectorTypeColumns> _guidePaggingFacade;
    private readonly IBuildEntityQuery<ConnectorType, ConnectorTypeDetails, BaseFilter> _buildEntityQuery;

    public ConnectorTypeBLL(IRepository<ConnectorType> repository
        , IMapper mapper
        , ILogger<ConnectorTypeBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IValidatePermissions<ConnectorType> validatePermissions
        , IBuildObject<ConnectorTypeDetails, ConnectorType> detailsBuilder
        , IInsertEntityBLL<ConnectorType, ConnectorTypeData> insertEntityBLL
        , IGuidePaggingFacade<ConnectorType, ConnectorTypeColumns> guidePaggingFacade
        , IModifyEntityBLL<int, ConnectorType, ConnectorTypeData, ConnectorTypeDetails> modifyEntityBLL
        , IRemoveEntityBLL<int, ConnectorType> removeEntityBLL
        , IGetEntityBLL<int, ConnectorType, ConnectorTypeDetails> detailsBLL
        , IGetEntityArrayBLL<int, ConnectorType, ConnectorTypeDetails, BaseFilter> detailsArrayBLL
        , IBuildEntityQuery<ConnectorType, ConnectorTypeDetails, BaseFilter> buildEntityQuery)
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
        _guidePaggingFacade = guidePaggingFacade;
        _buildEntityQuery = buildEntityQuery;
    }

    public async Task<ConnectorTypeDetails[]> GetListAsync(BaseFilter filter, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var connectorTypes = await _guidePaggingFacade.GetPaggingAsync(filter
            , _buildEntityQuery.Query(filter)
            , x => x.Name.ToLower().Contains(filter.SearchString.ToLower())
            , cancellationToken);

        return _mapper.Map<ConnectorTypeDetails[]>(connectorTypes);
    }
}
