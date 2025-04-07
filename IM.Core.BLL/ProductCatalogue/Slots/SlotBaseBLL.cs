using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.Slots;
internal class SlotBaseBLL<TEntity, TData, TDetails, TTable>
    : StandardBLL<SlotBaseKey, TEntity, TData, TDetails, SlotBaseFilter>
    , ISlotBaseBLL<TEntity, TData, TDetails, TTable>
    where TEntity : class
    where TDetails : class
{
    private readonly IMapper _mapper;
    private readonly IValidatePermissions<TEntity> _validatePermissions;
    private readonly IGuidePaggingFacade<TEntity, TTable> _guidePaggingFacade;
    private readonly IBuildEntityQuery<TEntity, TDetails, SlotBaseFilter> _buildEntityQuery;

    public SlotBaseBLL(IRepository<TEntity> repository
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<TDetails, TEntity> detailsBuilder
        , IInsertEntityBLL<TEntity, TData> insertEntityBLL
        , IValidatePermissions<TEntity> validatePermissions
        , IGuidePaggingFacade<TEntity, TTable> guidePaggingFacade
        , IRemoveEntityBLL<SlotBaseKey, TEntity> removeEntityBLL
        , IGetEntityBLL<SlotBaseKey, TEntity, TDetails> detailsBLL
        , ILogger<SlotBaseBLL<TEntity, TData, TDetails, TTable>> logger
        , IBuildEntityQuery<TEntity, TDetails, SlotBaseFilter> buildEntityQuery
        , IModifyEntityBLL<SlotBaseKey, TEntity, TData, TDetails> modifyEntityBLL
        , IGetEntityArrayBLL<SlotBaseKey, TEntity, TDetails, SlotBaseFilter> detailsArrayBLL) 
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

    public async Task<TDetails[]> GetListAsync(SlotBaseFilter filter, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var slotTemplates = await _guidePaggingFacade.GetPaggingAsync(filter
            , _buildEntityQuery.Query(filter)
            , null
            , cancellationToken);

        return _mapper.Map<TDetails[]>(slotTemplates);
    }
}
