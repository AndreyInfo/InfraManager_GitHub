using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.Dashboards.ForTable;
using InfraManager.DAL.Dashboards;

namespace InfraManager.BLL.Dashboards;

public class DashboardColumnSettings : IColumnMapperSetting<Dashboard, DashboardsForTable>, ISelfRegisteredService<IColumnMapperSetting<Dashboard, DashboardsForTable>>
{
    public void Configure(IColumnMapperSettingsBase<Dashboard, DashboardsForTable> configurer)
    {
        configurer.ShouldBe(x => x.StringFolder, x => x.Folder.Name);
    }
}
