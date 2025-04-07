using Inframanager.BLL;
using InfraManager.BLL.Asset.Peripherals;
using InfraManager.DAL.Asset;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.Peripherals;

internal class PeripheralBLL :
    IPeripheralBLL,
    ISelfRegisteredService<IPeripheralBLL>
{
    private readonly IGetEntityBLL<Guid, Peripheral, PeripheralDetails> _peripherals;

    public PeripheralBLL(IGetEntityBLL<Guid, Peripheral, PeripheralDetails> peripherals)
    {
        _peripherals = peripherals;
    }

    public async Task<PeripheralDetails> DetailsAsync(Guid id, CancellationToken cancellationToken)
        => await _peripherals.DetailsAsync(id, cancellationToken);
    
}
