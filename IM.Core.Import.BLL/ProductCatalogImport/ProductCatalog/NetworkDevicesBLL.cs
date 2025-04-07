using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.ProductCatalog;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Asset;

namespace IM.Core.Import.BLL.Import.ProductCatalog;
internal class NetworkDevicesBLL : INetworkDevicesBLL, ISelfRegisteredService<INetworkDevicesBLL>
{
    private readonly IRepository<NetworkDevice> _repository;
    private readonly IUnitOfWork _saveChanges;

    public NetworkDevicesBLL(IRepository<NetworkDevice> repository,
     IUnitOfWork saveChanges)
    {
        _repository = repository;
        _saveChanges = saveChanges;
    }

    // TODO: когда будет готово jwt - подключить IInsertEntityBLL
    public async Task<NetworkDevice> CreateAsync(NetworkDevice networkDevice, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default)
    {
        try
        {
            _repository.Insert(networkDevice);

            await _saveChanges.SaveAsync(cancellationToken);

            return networkDevice;
        }
        catch (Exception e)
        {
            protocolLogger.Information($"ERR Ошибка добавления сетевого оборудования");
            protocolLogger.Error(e, $"Error Create network device");
            throw;
        }
    }
}
