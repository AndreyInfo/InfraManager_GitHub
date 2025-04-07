using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset.Subclasses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ModelCharacteristics;
internal class EntityCharacteristicsProvider<TEntity, TDetails> : IEntityCharacteristicsProvider 
    where TEntity : class, IAssetSubclass
    where TDetails : EntityCharacteristicsDetailsBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGetEntityBLL<Guid, TEntity, TDetails> _getEntityBLL;
    private readonly IInsertEntityBLL<TEntity, EntityCharacteristicsDataBase> _insertEntityBLL;
    private readonly IModifyEntityBLL<Guid, TEntity, EntityCharacteristicsDataBase, TDetails> _modifyEntityBLL;

    public EntityCharacteristicsProvider(IMapper mapper
        , IUnitOfWork unitOfWork
        , IGetEntityBLL<Guid, TEntity, TDetails> getEntityBLL
        , IInsertEntityBLL<TEntity, EntityCharacteristicsDataBase> insertEntityBLL
        , IModifyEntityBLL<Guid, TEntity, EntityCharacteristicsDataBase, TDetails> modifyEntityBLL)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _getEntityBLL = getEntityBLL;
        _insertEntityBLL = insertEntityBLL;
        _modifyEntityBLL = modifyEntityBLL;
    }

    public async Task InsertAsync(EntityCharacteristicsDataBase data, CancellationToken cancellationToken)
        => await _insertEntityBLL.CreateAsync(data, cancellationToken);

    public async Task<EntityCharacteristicsDetailsBase> GetAsync(Guid id, CancellationToken cancellationToken)
        => await _getEntityBLL.DetailsAsync(id, cancellationToken);

    public async Task<EntityCharacteristicsDetailsBase> UpdateAsync(Guid id, EntityCharacteristicsDataBase data, CancellationToken cancellationToken)
    {
        var modifyCharacteristics = await _modifyEntityBLL.ModifyAsync(id, data, cancellationToken);

        await _unitOfWork.SaveAsync(cancellationToken);

        return _mapper.Map<TDetails>(modifyCharacteristics);
    }
}
