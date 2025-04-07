using AutoMapper;
using Inframanager.BLL;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.Asset;
internal class EquipmentBaseBLL<TKey, TEntity, TData, TDetails> 
    : StandardBLL<TKey, TEntity, TData, TDetails, BaseFilter>
    , IEquipmentBaseBLL<TKey, TEntity, TData, TDetails>
    where TEntity : class
    where TDetails : class
{
    private readonly IMapper _mapper;
    private readonly IAssetBLL _assetBLL;
    private readonly IInsertEntityBLL<TEntity, TData> _insertEntityBLL;

    public EquipmentBaseBLL(IRepository<TEntity> repository
        , ILogger<EquipmentBaseBLL<TKey, TEntity, TData, TDetails>> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<TDetails, TEntity> detailsBuilder
        , IInsertEntityBLL<TEntity, TData> insertEntityBLL
        , IModifyEntityBLL<TKey, TEntity, TData, TDetails> modifyEntityBLL
        , IRemoveEntityBLL<TKey, TEntity> removeEntityBLL
        , IGetEntityBLL<TKey, TEntity, TDetails> detailsBLL
        , IGetEntityArrayBLL<TKey, TEntity, TDetails, BaseFilter> detailsArrayBLL
        , IMapper mapper
        , IAssetBLL assetBLL) 
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
        _assetBLL = assetBLL;
        _insertEntityBLL = insertEntityBLL;
    }

    public async new Task<TDetails> AddAsync(TData data, CancellationToken cancellationToken)
    {
        using (var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
        {
            var entity = await _insertEntityBLL.CreateAsync(data, cancellationToken);
            await UnitOfWork.SaveAsync(cancellationToken);

            var assetData = _mapper.Map<AssetData>(entity);
            _mapper.Map(data, assetData);
            var assetDetails = await _assetBLL.AddAsync(assetData, cancellationToken);

            var details = _mapper.Map<TDetails>(entity);
            transaction.Complete();

            return _mapper.Map(assetDetails, details);
        }
    }
}
