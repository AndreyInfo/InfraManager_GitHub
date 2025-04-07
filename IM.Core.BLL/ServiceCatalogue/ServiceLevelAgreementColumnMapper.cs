using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue;

public class ServiceLevelAgreementColumnMapper : IColumnMapperSetting<ServiceLevelAgreement, SLAForTable>,
    ISelfRegisteredService<IColumnMapperSetting<ServiceLevelAgreement, SLAForTable>>
{
    public void Configure(IColumnMapperSettingsBase<ServiceLevelAgreement, SLAForTable> configurer)
    {
        configurer.ShouldBe(x => x.CalendarWorkSchedule, x => x.CalendarWorkSchedule.Name);
        configurer.ShouldBe(x => x.TimeZoneName, x => x.TimeZone.Name);
    }
}