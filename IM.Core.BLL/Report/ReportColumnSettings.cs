using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.Report;

public class ReportColumnSettings: IColumnMapperSetting<Reports, ReportsForTable>, ISelfRegisteredService<IColumnMapperSetting<Reports, ReportsForTable>>
{
    public void Configure(IColumnMapperSettingsBase<Reports, ReportsForTable> configurer)
    {
        configurer.ShouldBe(x => x.StringFolder, x => x.Folder.Name);
    }
}