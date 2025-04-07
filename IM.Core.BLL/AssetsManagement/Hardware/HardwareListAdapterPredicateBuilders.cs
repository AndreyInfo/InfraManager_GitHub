using InfraManager.DAL.Asset;

namespace InfraManager.BLL.AssetsManagement.Hardware;

internal class HardwareListAdapterPredicateBuilders<TListItem> :
    HardwareListPredicateBuilders<Adapter, AdapterType, TListItem>
    where TListItem : IHardwareListItem
{
    public HardwareListAdapterPredicateBuilders()
    {
        AddPredicateBuilder(
            reportItem => reportItem.InvNumber,
            entity => entity.Name);
        AddPredicateBuilder(
            reportItem => reportItem.VendorName,
            entity => entity.Model.Vendor.ID);
    }
}