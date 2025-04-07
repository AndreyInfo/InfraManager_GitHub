using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Asset.Peripherals;
internal class PeripheralColumnsSettings
    : IColumnMapperSetting<Peripheral, PeripheralColumns>
    , ISelfRegisteredService<IColumnMapperSetting<Peripheral, PeripheralColumns>>
{
    public void Configure(IColumnMapperSettingsBase<Peripheral, PeripheralColumns> configurer)
    {
        configurer.ShouldBe(x => x.ProductCatalogTypeName, x => x.Model.ProductCatalogType.Name);
        configurer.ShouldBe(x => x.ProductCatalogModelName, x => x.Model.Name);
    }
}
