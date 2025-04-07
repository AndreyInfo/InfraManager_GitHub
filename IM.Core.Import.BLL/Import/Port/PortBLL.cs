using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.Port;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Asset;

namespace IM.Core.Import.BLL.Import.Port;
internal class PortBLL : IPortBLL, ISelfRegisteredService<IPortBLL>
{
    private readonly IRepository<ActivePort> _portRepository;
    private readonly IRepository<PortTemplate> _portTemplateRepository;
    public PortBLL(IRepository<ActivePort> portRepository, IRepository<PortTemplate> portTemplateRepository)
    {
        _portRepository = portRepository;
        _portTemplateRepository = portTemplateRepository;
    }

    public async Task CreateAsync(ActivePort[] ports, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var port in ports)
                _portRepository.Insert(port);
        }
        catch (Exception e)
        {
            protocolLogger.Information($"ERR Ошибка добавления портов оборудования");
            protocolLogger.Error(e, $"Error Create ports");
            throw;
        }
    }

    public async Task<PortTemplate[]> GetPortTemplateAsync(Guid modelID, CancellationToken cancellationToken = default)
        => await _portTemplateRepository.ToArrayAsync(x => x.ObjectID == modelID, cancellationToken);
}
