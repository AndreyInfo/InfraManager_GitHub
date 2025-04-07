using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.MaintenanceWork;


namespace InfraManager.BLL.MaintenanceWork.MaintenanceDependencies;

internal sealed class MaintenanceDependencyColumnMapperSetting : IColumnMapperSetting<MaintenanceDependency, MaintenanceDependencyListItem>
    , ISelfRegisteredService<IColumnMapperSetting<MaintenanceDependency, MaintenanceDependencyListItem>>
{
    public void Configure(IColumnMapperSettingsBase<MaintenanceDependency, MaintenanceDependencyListItem> configurer)
    {
        configurer.ShouldBe(c => c.ClassName, x => x.ObjectClass.Name);
    }
}
