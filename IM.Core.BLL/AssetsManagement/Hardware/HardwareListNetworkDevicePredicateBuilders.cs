using InfraManager.DAL.Asset;

namespace InfraManager.BLL.AssetsManagement.Hardware;

internal class HardwareListNetworkDevicePredicateBuilders<TListItem> :
    HardwareListPredicateBuilders<NetworkDevice, NetworkDeviceModel, TListItem>
    where TListItem : IHardwareListItem
{
    public HardwareListNetworkDevicePredicateBuilders()
    {
        AddPredicateBuilder(entity => entity.InvNumber);
        AddPredicateBuilder(
            reportItem => reportItem.VendorName,
            entity => entity.Model.Manufacturer.ID);
    }
}