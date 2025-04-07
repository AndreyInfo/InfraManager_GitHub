using Inframanager.BLL;
using InfraManager.DAL.Asset;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset;

internal class TerminalDeviceBLL :
    ITerminalDeviceBLL,
    ISelfRegisteredService<ITerminalDeviceBLL>
{
    private readonly IGetEntityBLL<int, TerminalDevice, TerminalDeviceDetails> _terminalDevices;

    public TerminalDeviceBLL(IGetEntityBLL<int, TerminalDevice, TerminalDeviceDetails> terminalDevices)
    {
        _terminalDevices = terminalDevices;
    }

    public async Task<TerminalDeviceDetails> DetailsAsync(int id, CancellationToken cancellationToken)
        => await _terminalDevices.DetailsAsync(id, cancellationToken);
    
}
