using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.ProductCatalog;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using AdapterEntity = InfraManager.DAL.Asset.Adapter;

namespace IM.Core.Import.BLL.Import.Adapter;
internal class AdaptersBLL : IAdaptersBLL, ISelfRegisteredService<IAdaptersBLL>
{
    private readonly IRepository<AdapterEntity> _repository;
    private readonly IRepository<AdapterType> _repositoryType;
    private readonly IUnitOfWork _saveChanges;

    public AdaptersBLL(IRepository<AdapterEntity> repository,
     IRepository<AdapterType> repositoryType,
     IUnitOfWork saveChanges)
    {
        _repository = repository;
        _repositoryType = repositoryType;
        _saveChanges = saveChanges;
    }

    // TODO: когда будет готово jwt - подключить IInsertEntityBLL
    public async Task<AdapterEntity> CreateAsync(AdapterEntity adapter, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default)
    {
        try
        {
            _repository.Insert(adapter);

            await _saveChanges.SaveAsync(cancellationToken);

            return adapter;
        }
        catch (Exception e)
        {
            protocolLogger.Information($"ERR Ошибка добавления адаптера");
            protocolLogger.Error(e, $"Error Create adapter");
            throw;
        }
    }

    // TODO: когда будет готово jwt - подключить IInsertEntityBLL
    public async Task<AdapterType> CreateTypeAsync(AdapterType adapterType, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default)
    {
        try
        {
            _repositoryType.Insert(adapterType);

            await _saveChanges.SaveAsync(cancellationToken);

            return adapterType;
        }
        catch (Exception e)
        {
            protocolLogger.Information($"ERR Ошибка добавления модели адаптера");
            protocolLogger.Error(e, $"Error Create adapter model");
            throw;
        }
    }
}
