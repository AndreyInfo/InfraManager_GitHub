using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.ProductCatalog;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace IM.Core.Import.BLL.Import.ProductCatalog;
internal class PeripheralsBLL : IPeripheralsBLL, ISelfRegisteredService<IPeripheralsBLL>
{
    private readonly IRepository<Peripheral> _repository;
    private readonly IRepository<PeripheralType> _repositoryType;
    private readonly IUnitOfWork _saveChanges;

    public PeripheralsBLL(IRepository<Peripheral> repository,
     IRepository<PeripheralType> repositoryType,
     IUnitOfWork saveChanges)
    {
        _repository = repository;
        _repositoryType = repositoryType;
        _saveChanges = saveChanges;
    }

    // TODO: когда будет готово jwt - подключить IInsertEntityBLL
    public async Task<Peripheral> CreateAsync(Peripheral peripheral, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default)
    {
        try
        {
            _repository.Insert(peripheral);

            await _saveChanges.SaveAsync(cancellationToken);

            return peripheral;
        }
        catch (Exception e)
        {
            protocolLogger.Information($"ERR Ошибка добавления переферийного оборудования");
            protocolLogger.Error(e, $"Error Create peripheral");
            throw;
        }
    }

    // TODO: когда будет готово jwt - подключить IInsertEntityBLL
    public async Task<PeripheralType> CreateTypeAsync(PeripheralType peripheralType, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default)
    {
        try
        {
            _repositoryType.Insert(peripheralType);

            await _saveChanges.SaveAsync(cancellationToken);

            return peripheralType;
        }
        catch (Exception e)
        {
            protocolLogger.Information($"ERR Ошибка добавления модели переферийного оборудования");
            protocolLogger.Error(e, $"Error Create peripheral model");
            throw;
        }
    }
}
