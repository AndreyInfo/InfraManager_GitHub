using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;

public class OperationalLevelAgreementServiceColumnMapper : 
    IColumnMapperSetting<OperationalLevelAgreementServiceListItem, OperationLevelAgreementServiceListItem>,
    ISelfRegisteredService<IColumnMapperSetting<OperationalLevelAgreementServiceListItem,
        OperationLevelAgreementServiceListItem>>
{
    public void Configure(IColumnMapperSettingsBase<OperationalLevelAgreementServiceListItem, OperationLevelAgreementServiceListItem> configurer)
    {
        configurer.ShouldBe(x => x.StateName, x => x.State);
        configurer.ShouldBe(x => x.Category, x => x.Category);
        configurer.ShouldBe(x => x.Name, x => x.Name);
        configurer.ShouldBe(x => x.OwnerName, x => x.OwnerName);
    }
}