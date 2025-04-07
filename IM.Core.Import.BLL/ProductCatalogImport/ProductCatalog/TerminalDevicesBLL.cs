using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.ProductCatalog;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Asset;

namespace IM.Core.Import.BLL.Import.ProductCatalog;
internal class TerminalDevicesBLL : ITerminalDevicesBLL, ISelfRegisteredService<ITerminalDevicesBLL>
{
    private readonly IRepository<TerminalDevice> _repository;
    private readonly IRepository<TerminalDeviceModel> _repositoryType;
    private readonly IUnitOfWork _saveChanges;

    public TerminalDevicesBLL(IRepository<TerminalDevice> repository,
     IRepository<TerminalDeviceModel> repositoryType,
     IUnitOfWork saveChanges)
    {
        _repository = repository;
        _repositoryType = repositoryType;
        _saveChanges = saveChanges;
    }

    // TODO: когда будет готово jwt - подключить IInsertEntityBLL
    public async Task<TerminalDevice> CreateAsync(TerminalDevice terminalDevice, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default)
    {
        try
        {
            _repository.Insert(terminalDevice);

            await _saveChanges.SaveAsync(cancellationToken);

            return terminalDevice;
        }
        catch (Exception e)
        {
            protocolLogger.Information($"ERR Ошибка добавления оконечного оборудования");
            protocolLogger.Error(e, $"Error Create terminal device");
            throw;
        }
    }

    // TODO: когда будет готово jwt - подключить IInsertEntityBLL
    public async Task<TerminalDeviceModel> CreateTypeAsync(TerminalDeviceModel terminalDeviceType, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default)
    {
        try
        {
            _repositoryType.Insert(terminalDeviceType);

            await _saveChanges.SaveAsync(cancellationToken);

            return terminalDeviceType;
        }
        catch (Exception e)
        {
            protocolLogger.Information($"ERR Ошибка добавления модели оконечного оборудования");
            protocolLogger.Error(e, $"Error Create terminal device model");
            throw;
        }
    }
}
