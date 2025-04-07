using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.AssetsManagement.Hardware;

internal class HardwareListPeripheralPredicateBuilders<TListItem> :
    HardwareListPredicateBuilders<Peripheral, PeripheralType, TListItem>
    where TListItem : IHardwareListItem
{
    public HardwareListPeripheralPredicateBuilders()
    {
        AddPredicateBuilder(
            reportItem => reportItem.InvNumber,
            entity => entity.Name);
        AddPredicateBuilder(
            reportItem => reportItem.VendorName,
            entity => entity.Model.Vendor.ID);
    }
}