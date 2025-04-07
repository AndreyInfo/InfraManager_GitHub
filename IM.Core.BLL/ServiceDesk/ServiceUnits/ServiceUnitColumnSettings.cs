using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.ServiceUnits;

internal sealed class ServiceUnitColumnSettings : IColumnMapperSetting<ServiceUnit, ServiceUnitColumns>
    , ISelfRegisteredService<IColumnMapperSetting<ServiceUnit, ServiceUnitColumns>>
{
    public void Configure(IColumnMapperSettingsBase<ServiceUnit, ServiceUnitColumns> configurer)
    {
        configurer.ShouldBe(x => x.ResponsibleName, x => x.ResponsibleUser.Surname)
            .Then(x=> x.ResponsibleUser.Name)
            .Then(x=> x.ResponsibleUser.Patronymic);
    }
}
