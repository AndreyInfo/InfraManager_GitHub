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
/// Бизнес-логика работы с командами жизненных циклов и алертами при выполнении.
/// </summary>
public abstract class LifeCycleCommandWithAlert : LifeCycleCommand
{
    protected LifeCycleCommandWithAlert(IMapper mapper
        , IUnitOfWork unitOfWork
        , IRepository<AssetEntity> assetRepository
        , IServiceMapper<LifeCycleOperationCommandType, AssetHistorySaveStrategy> assetHistorySaver) 
        : base(mapper, unitOfWork, assetRepository, assetHistorySaver)
    {
    }

    /// <summary>
    /// Выполнение команды с алертом для объекта.
    /// </summary>
    /// <param name="id">Идентификатор объекта.</param>
    /// <param name="operationID">Идентификатор операции.</param>
    /// <param name="data">Данные команды жизненного цикла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Результат алерта.</returns>
    public abstract Task<LifeCycleCommandResultItem> ExecuteWithAlertAsync(Guid id
        , Guid operationID
        , LifeCycleCommandBaseData data
        , CancellationToken cancellationToken);

    public override Task<LifeCycleCommandResultItem> ExecuteAsync(Guid id, Guid operationID, LifeCycleCommandBaseData data, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
