using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceAttendances;

internal sealed class ServiceAttendanceColumnMapper : IColumnMapperSetting<ServiceAttendance, ServiceAttendanceForTable>
    , ISelfRegisteredService<IColumnMapperSetting<ServiceAttendance, ServiceAttendanceForTable>>
{
    public void Configure(IColumnMapperSettingsBase<ServiceAttendance, ServiceAttendanceForTable> configurer)
    {
        configurer.ShouldBe(x => x.StateName, x => x.State);
    }
}

