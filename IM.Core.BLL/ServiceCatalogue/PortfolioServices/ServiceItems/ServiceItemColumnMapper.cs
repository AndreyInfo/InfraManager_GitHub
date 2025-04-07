using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;

internal class ServiceItemColumnMapper : IColumnMapperSetting<ServiceItem, ServiceItemColumns>
                , ISelfRegisteredService<IColumnMapperSetting<ServiceItem, ServiceItemColumns>>
{
    public void Configure(IColumnMapperSettingsBase<ServiceItem, ServiceItemColumns> configurer)
    {
        configurer.ShouldBe(x => x.StateName, x => x.State);
    }
}