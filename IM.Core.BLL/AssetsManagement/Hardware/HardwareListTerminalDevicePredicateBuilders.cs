using InfraManager.DAL.Asset;

namespace InfraManager.BLL.AssetsManagement.Hardware;

internal class HardwareListTerminalDevicePredicateBuilders<TListItem> :
    HardwareListPredicateBuilders<TerminalDevice, TerminalDeviceModel, TListItem>
    where TListItem : IHardwareListItem
{
    public HardwareListTerminalDevicePredicateBuilders()
    {
        AddPredicateBuilder(entity => entity.InvNumber);
        AddPredicateBuilder(
            reportItem => reportItem.VendorName,
            entity => entity.Model.Manufacturer.ID);
    }
}