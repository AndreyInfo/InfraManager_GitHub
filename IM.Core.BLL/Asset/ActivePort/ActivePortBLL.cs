using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ActivePortEntity = InfraManager.DAL.Asset.ActivePort;

namespace InfraManager.BLL.Asset.ActivePort;

internal class ActivePortBLL : StandardBLL<int, ActivePortEntity, ActivePortData, ActivePortDetails, ActivePortFilter>
    , IActivePortBLL
    , ISelfRegisteredService<IActivePortBLL>
{
    private readonly IMapper _mapper;
    private readonly IValidatePermissions<ActivePortEntity> _validatePermissions;
    private readonly IGuidePaggingFacade<ActivePortEntity, ActivePortColumns> _guidePaggingFacade;
    private readonly IBuildEntityQuery<ActivePortEntity, ActivePortDetails, ActivePortFilter> _buildEntityQuery;
    private readonly IRepository<ActivePortEntity> _repository;

    public ActivePortBLL(
          IRepository<ActivePortEntity> repository
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , ILogger<ActivePortBLL> logger
        , IValidatePermissions<ActivePortEntity> validatePermissions
        , IBuildObject<ActivePortDetails, ActivePortEntity> detailsBuilder
        , IInsertEntityBLL<ActivePortEntity, ActivePortData> insertEntityBLL
        , IRemoveEntityBLL<int, ActivePortEntity> removeEntityBLL
        , IGuidePaggingFacade<ActivePortEntity, ActivePortColumns> guidePaggingFacade
        , IGetEntityBLL<int, ActivePortEntity, ActivePortDetails> detailsBLL
        , IBuildEntityQuery<ActivePortEntity, ActivePortDetails, ActivePortFilter> buildEntityQuery
        , IGetEntityArrayBLL<int, ActivePortEntity, ActivePortDetails, ActivePortFilter> detailsArrayBLL
        , IModifyEntityBLL<int, ActivePortEntity, ActivePortData, ActivePortDetails> modifyEntityBLL)
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
        _repository = repository;
    }

    public async Task<int> GetCountPortsAsync(int id, CancellationToken cancellationToken)
    {
        return await _repository.CountAsync(x => x.ActiveEquipmentID == id, cancellationToken);
    }

    public async Task<ActivePortDetails[]> GetListAsync(ActivePortFilter filter, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var ports = await _guidePaggingFacade.GetPaggingAsync(filter
            , _buildEntityQuery.Query(filter)
            , null
            , cancellationToken);

        return _mapper.Map<ActivePortDetails[]>(ports);
    }
}