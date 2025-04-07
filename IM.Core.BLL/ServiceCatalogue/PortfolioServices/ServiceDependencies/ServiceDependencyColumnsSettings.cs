using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.ServiceCatalog;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceDependencies;

internal sealed class ServiceDependencyColumnsSettings : IColumnMapperSetting<ServiceModelItem, ServiceDependencyForTable>
    , ISelfRegisteredService<IColumnMapperSetting<ServiceModelItem, ServiceDependencyForTable>>
{
    public void Configure(IColumnMapperSettingsBase<ServiceModelItem, ServiceDependencyForTable> configurer)
    {
        configurer.ShouldBe(x => x.TypeName, x => x.Type);
        configurer.ShouldBe(x => x.StateName, x => x.State);
    }
}
