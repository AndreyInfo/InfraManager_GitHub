using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.BLL.ProductCatalogue.ProductCatalogModels;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.BLL.ProductCatalogue;
internal static class ServiceCollectionExtensionsProductCatalog
{
    public static IServiceCollection AddProductBLL(this IServiceCollection services)
    {
        //TODO сделать авторегистрацию 
        services.AddMappingScoped(new ServiceMapping<ObjectClass, IProductObjectBLL>()
            .Map<ProductObjectBLL<NetworkDevice, NetworkDeviceModel>>().To(ObjectClass.ActiveDevice)
            .Map<ProductObjectBLL<Material, MaterialModel>>().To(ObjectClass.Material)
            .Map<ProductObjectBLL<MaterialConsumptionRate, MaterialModel>>().To(ObjectClass.MaterialCartridge)
            .Map<ProductObjectBLL<Adapter, AdapterType>>().To(ObjectClass.Adapter)
            .Map<ProductObjectBLL<Rack, CabinetType>>().To(ObjectClass.CabinetType)
            .Map<ProductObjectBLL<TerminalDevice, TerminalDeviceModel>>().To(ObjectClass.TerminalDevice)
            .Map<ProductObjectBLL<ServiceContract, ServiceContractModel>>().To(ObjectClass.ServiceContract)
            .Map<ProductObjectBLL<Peripheral, PeripheralType>>().To(ObjectClass.Peripherial));

        return services;
    }
}
