using AutoMapper;
using InfraManager.BLL.Asset.History;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
using System.Threading;
using System.Threading.Tasks;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
/// <summary>
/// Бизнес-логика работы с командами жизненных циклов.
/// </summary>
public abstract class LifeCycleCommand
{
    protected readonly IMapper Mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<AssetEntity> _assetRepository;
    private readonly IServiceMapper<LifeCycleOperationCommandType, AssetHistorySaveStrategy> _assetHistorySaver;

    public LifeCycleCommand(IMapper mapper
        , IUnitOfWork unitOfWork
        , IRepository<AssetEntity> assetRepository
        , IServiceMapper<LifeCycleOperationCommandType, AssetHistorySaveStrategy> assetHistorySaver)
    {
        Mapper = mapper;
        _unitOfWork = unitOfWork;
        _assetRepository = assetRepository;
        _assetHistorySaver = assetHistorySaver;
    }

    /// <summary>
    /// Выполнение команды для объекта.
    /// </summary>
    /// <param name="id">Идентификатор объекта.</param>
    /// <param name="operationID">Идентификатор операции.</param>
    /// <param name="data">Данные команды жизненного цикла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат выполнения команды.</returns>
    public abstract Task<LifeCycleCommandResultItem> ExecuteAsync(Guid id, Guid operationID, LifeCycleCommandBaseData data, CancellationToken cancellationToken);

    /// <summary>
    /// Изменение состояния жизненного цикла оборудования.
    /// </summary>
    /// <param name="id">Идентификатор оборудования.</param>
    /// <param name="lifeCycleStateID">Идентификатор состояния жизненного цикла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    protected async Task ChangeLifeCycleStateAsync(Guid id, Guid? lifeCycleStateID, CancellationToken cancellationToken)
    {
        var asset = await _assetRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
        if (lifeCycleStateID is not null)
        {
            asset.LifeCycleStateID = lifeCycleStateID;
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Сохранение истории выполнения команд для объекта в зависимости от типа команды.
    /// </summary>
    /// <param name="id">Идентификатор объекта.</param>
    /// <param name="commandType">Тип команды.</param>
    /// <param name="data">Данные команды жизненного цикла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task SaveHistoryAsync(Guid id, LifeCycleOperationCommandType commandType
        , AssetHistoryBaseData data, CancellationToken cancellationToken)
    {
        await _assetHistorySaver.Map(commandType).SaveAsync(id, data, cancellationToken);
    }
}
