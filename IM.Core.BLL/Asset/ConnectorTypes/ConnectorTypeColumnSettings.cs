using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Asset.ConnectorTypes;
internal sealed class ConnectorTypeColumnSettings : IColumnMapperSetting<ConnectorType, ConnectorTypeColumns>
    , ISelfRegisteredService<IColumnMapperSetting<ConnectorType, ConnectorTypeColumns>>
{
    public void Configure(IColumnMapperSettingsBase<ConnectorType, ConnectorTypeColumns> configurer)
    {
        configurer.ShouldBe(x => x.MediumName, x => x.Medium.Name);
    }
}
