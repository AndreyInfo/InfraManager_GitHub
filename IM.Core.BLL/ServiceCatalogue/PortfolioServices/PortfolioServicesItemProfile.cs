using System.Linq;
using AutoMapper;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.SLA;
using InfraManager.DAL.ServiceCatalog;
using InfraManager.BLL.AutoMapper;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

internal class PortfolioServicesItemProfile : Profile
{
    public PortfolioServicesItemProfile()
    {
        CreateMap<ServiceCategory, PortfolioServicesItem>()
            .ForMember(dst => dst.IconName, m => m.MapFrom(scr => scr.IconName))
            .ForMember(c => c.ClassId, m => m.MapFrom(scr => ObjectClass.ServiceCategory))
            .ForMember(dst => dst.HasChild, m => m.MapFrom(scr => scr.Services.Any()))
            .ForMember(dst => dst.Note, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.Note) ? "" : scr.Note));

        CreateMap<Service, PortfolioServicesItem>()
            .ForMember(dst => dst.IconName, m => m.MapFrom(scr => scr.IconName))
            .ForMember(c => c.ClassId, m => m.MapFrom(scr => ObjectClass.Service))
            .ForMember(c => c.ParentId, m => m.MapFrom(scr => scr.CategoryID))
            .ForMember(dst => dst.Note, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.Note) ? "" : scr.Note));

        CreateMap<ServiceItem, PortfolioServicesItem>()
            .ForMember(c => c.ClassId, m => m.MapFrom(scr => ObjectClass.ServiceItem))
            .ForMember(c => c.ParentId, m => m.MapFrom(scr => scr.ServiceID))
            .ForMember(dst => dst.Note, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.Note) ? "" : scr.Note))
            .ForMember(dst => dst.HasChild, m => m.MapFrom(scr => false));

        CreateMap<ServiceAttendance, PortfolioServicesItem>()
            .ForMember(c => c.ClassId, m => m.MapFrom(scr => ObjectClass.ServiceAttendance))
            .ForMember(c => c.ParentId, m => m.MapFrom(scr => scr.ServiceID))
            .ForMember(dst => dst.Note, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.Note) ? "" : scr.Note))
            .ForMember(dst => dst.HasChild, m => m.MapFrom(scr => false));


        CreateMap<Service, PortfolioServiceItemTable>()
            .ForMember(dst => dst.ServiceCategoryName, m => m.MapFrom(scr => scr.Category.Name))
            .ForMember(c => c.StateName, m => m.MapFrom<LocalizedEnumResolver<Service, PortfolioServiceItemTable, CatalogItemState>, CatalogItemState>(
                        entity => entity.State))
           .ForMember(c => c.TypeName, m => m.MapFrom<LocalizedEnumResolver<Service, PortfolioServiceItemTable, ServiceType>, ServiceType>(
                        entity => entity.Type));


        CreateMap<ServiceModelItem, PortfolioServiceItemTable>()
            .ForMember(dst => dst.OwnerID, m => m.MapFrom(scr => scr.OrganizationItemObjectIDCustomer))
            .ForMember(dst => dst.OwnerClassID, m => m.MapFrom(scr => scr.OrganizationItemClassIDCustomer))
            .ForMember(c => c.TypeName, m => m.MapFrom<LocalizedEnumResolver<ServiceModelItem, PortfolioServiceItemTable, ServiceType>, ServiceType>(
                        entity => entity.Type))
            .ForMember(c => c.StateName, m => m.MapFrom<LocalizedEnumResolver<ServiceModelItem, PortfolioServiceItemTable, CatalogItemState>, CatalogItemState>(
                        entity => entity.State.Value));
    }
}
