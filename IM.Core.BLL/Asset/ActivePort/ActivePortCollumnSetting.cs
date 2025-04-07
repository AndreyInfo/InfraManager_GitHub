using InfraManager.BLL.ColumnMapper;
using ActivePortEntity = InfraManager.DAL.Asset.ActivePort;

namespace InfraManager.BLL.Asset.ActivePort;

internal sealed class ActivePortCollumnSetting :
    IColumnMapperSetting<ActivePortEntity, ActivePortColumns>
    , ISelfRegisteredService<IColumnMapperSetting<ActivePortEntity, ActivePortColumns>>
{
    public void Configure(IColumnMapperSettingsBase<ActivePortEntity, ActivePortColumns> configurer)
    {
        configurer.ShouldBe(x => x.JackTypeName, x => x.JackType.Name);
        configurer.ShouldBe(x => x.TechnologyTypeName, x => x.TechnologyType.Name);
    }
}