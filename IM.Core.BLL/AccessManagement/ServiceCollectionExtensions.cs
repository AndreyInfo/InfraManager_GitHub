using InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess;
using InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.DeviceCatalog;
using InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.ServiceCatalog;
using InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.TOZ_org;
using InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.TOZ_sks;
using InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.TTZ_sks;
using InfraManager.DAL.AccessManagement;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.BLL.AccessManagement;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddAccessManagement(this IServiceCollection services)
    {
        services.AddMappingScoped(
                new ServiceMapping<(AccessTypes, ObjectClass), ISaveAccess>()

                // Дерево местоположений до комнат
                .Map<SaveAccessOwnerTOZ>().To((AccessTypes.TOZ_sks, ObjectClass.Owner))
                .Map<SaveAccessOrganizationTOZ>().To((AccessTypes.TOZ_sks, ObjectClass.Organizaton))
                .Map<SaveAccessBuildingTOZ>().To((AccessTypes.TOZ_sks, ObjectClass.Building))
                .Map<SaveAccessFloorTOZ>().To((AccessTypes.TOZ_sks, ObjectClass.Floor))
                .Map<SaveAccessRoomTOZ>().To((AccessTypes.TOZ_sks, ObjectClass.Room))
                
                // Дерево местоположений с шкафами
                .Map<SaveAccessOwnerTTZ>().To((AccessTypes.TTZ_sks, ObjectClass.Owner))
                .Map<SaveAccessOrganizationTTZ>().To((AccessTypes.TTZ_sks, ObjectClass.Organizaton))
                .Map<SaveAccessBuildingTTZ>().To((AccessTypes.TTZ_sks, ObjectClass.Building))
                .Map<SaveAccessFloorTTZ>().To((AccessTypes.TTZ_sks, ObjectClass.Floor))
                .Map<SaveAccessRoomTTZ>().To((AccessTypes.TTZ_sks, ObjectClass.Room))
                .Map<SaveAccessRackTTZ>().To((AccessTypes.TTZ_sks, ObjectClass.Rack))

                // дерево каталога сервисов/портефеля сервисов
                .Map<SaveAccessServiceCatalog>().To((AccessTypes.ServiceCatalogue, ObjectClass.ServiceCatalogue))
                .Map<SaveAccessServiceCategory>().To((AccessTypes.ServiceCatalogue, ObjectClass.ServiceCategory))
                .Map<SaveAccessService>().To((AccessTypes.ServiceCatalogue, ObjectClass.Service))
                .Map<SaveAccessServiceItem>().To((AccessTypes.ServiceCatalogue, ObjectClass.ServiceItem))
                .Map<SaveAccessServiceAttendance>().To((AccessTypes.ServiceCatalogue, ObjectClass.ServiceAttendance))

                //TOZ_org дерево оргструктуры
                .Map<SaveAccessOwnerOrgTOZ>().To((AccessTypes.TOZ_org, ObjectClass.Owner))
                .Map<SaveAccessOrganizationTOZORG>().To((AccessTypes.TOZ_org, ObjectClass.Organizaton))
                .Map<SaveAccessDivisionTOZ>().To((AccessTypes.TOZ_org, ObjectClass.Division))

                //DeviceCatalogue дерево каталога продуктов
                .Map<SaveAccessOwnerDeviceCatalog>().To((AccessTypes.DeviceCatalogue, ObjectClass.ProductCatalogue))
                .Map<SaveAccessProductCatalogCategory>().To((AccessTypes.DeviceCatalogue, ObjectClass.ProductCatalogCategory))
                .Map<SaveAccessProductCatalogType>().To((AccessTypes.DeviceCatalogue, ObjectClass.ProductCatalogType))
                );

        return services;
    }
}