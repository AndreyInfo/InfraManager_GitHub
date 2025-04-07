using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.Asset;
using PortAdapterEntity = InfraManager.DAL.Asset.PortAdapter;

namespace InfraManager.BLL.Asset.PortAdapter;

internal sealed class PortAdapterCollumnSetting : 
    IColumnMapperSetting<PortAdapterEntity, PortAdapterColumns>
    , ISelfRegisteredService<IColumnMapperSetting<PortAdapterEntity, PortAdapterColumns>>
{
    public void Configure(IColumnMapperSettingsBase<PortAdapterEntity, PortAdapterColumns> configurer)
    {
        configurer.ShouldBe(x => x.PortNumber, x => x.PortNumber);
        configurer.ShouldBe(x => x.JackTypeName, x => x.JackType.Name);
        configurer.ShouldBe(x => x.TechnologyTypeName, x => x.TechnologyType.Name);
    }
}

