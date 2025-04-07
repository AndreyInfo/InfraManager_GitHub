using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.ConfigurationUnit;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.ConfigurationUnits;

namespace IM.Core.Import.BLL.Import.ConfigurationUnit;
internal class ConfigurationUnitsBLL : IConfigurationUnitsBLL, ISelfRegisteredService<IConfigurationUnitsBLL>
{
    private readonly IRepository<NetworkNode> _repository;

    public ConfigurationUnitsBLL(IRepository<NetworkNode> repository)
    {
        _repository = repository;
    }
    public async Task CreateAsync(NetworkNode[] cuEntities, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var cu in cuEntities)
                _repository.Insert(cu);
        }
        catch (Exception e)
        {
            protocolLogger.Information($"ERR Ошибка добавления Узла сети");
            protocolLogger.Error(e, $"Error Create Network node");
            throw;
        }
    }
}
