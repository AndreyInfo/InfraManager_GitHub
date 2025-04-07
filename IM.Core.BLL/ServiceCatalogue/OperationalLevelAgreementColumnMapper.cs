using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;

namespace InfraManager.BLL.ServiceCatalogue;

public class OperationalLevelAgreementColumnMapper :
    IColumnMapperSetting<OperationalLevelAgreement, OperationLevelAgreementListItem>,
    ISelfRegisteredService<IColumnMapperSetting<OperationalLevelAgreement, OperationLevelAgreementListItem>>
{
    public void Configure(IColumnMapperSettingsBase<OperationalLevelAgreement, OperationLevelAgreementListItem> configurer)
    {
        configurer.ShouldBe(x => x.CalendarWorkSchedule, x => x.CalendarWorkSchedule.Name);
        configurer.ShouldBe(x => x.TimeZoneName, x => x.TimeZone.Name);
    }
}