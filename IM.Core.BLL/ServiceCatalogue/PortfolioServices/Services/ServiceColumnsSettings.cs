using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.ServiceCatalog;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;

internal class ServiceColumnsSettings : IColumnMapperSetting<ServiceModelItem, PortfolioServiceForTable>
    , ISelfRegisteredService<IColumnMapperSetting<ServiceModelItem, PortfolioServiceForTable>>
{
    public void Configure(IColumnMapperSettingsBase<ServiceModelItem, PortfolioServiceForTable> configurer)
    {
        configurer.ShouldBe(x => x.TypeName, x => x.Type);
        configurer.ShouldBe(x => x.StateName, x => x.State);
    }
}
