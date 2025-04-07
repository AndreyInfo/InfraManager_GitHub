using AutoMapper;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.BLL.ProductCatalogue.Models.AdditionalFields;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.NetworkDeviceModels;
internal sealed class NetworkDeviceModelProfile : Profile
{
    public NetworkDeviceModelProfile()
    {
        CreateMap<ProductCatalogModelData, NetworkDeviceModel>()
            .ForMember(dst => dst.Height, mapper => mapper.MapFrom(src => src.Properties.Height))
            .ForMember(dst => dst.HeightInUnits, mapper => mapper.MapFrom(src => src.Properties.HeightInUnits))
            .ForMember(dst => dst.Width, mapper => mapper.MapFrom(src => src.Properties.Width))
            .ForMember(dst => dst.Depth, mapper => mapper.MapFrom(src => src.Properties.Depth))
            .ForMember(dst => dst.IsRackmount, mapper => mapper.MapFrom(src => src.Properties.IsRackMount))
            .ForMember(dst => dst.CanBuy, mapper => mapper.MapFrom(src => src.Properties.CanBuy))
            .ForMember(dst => dst.ManufacturerID, mapper => mapper.MapFrom(src => src.VendorID));

        CreateMap<NetworkDeviceModel, ProductModelOutputDetails>()
            .ForMember(x => x.VendorID, x => x.MapFrom(y => y.ManufacturerID))
            .ForMember(x => x.ID, x => x.MapFrom(y => y.IMObjID))
            .ForMember(x => x.ProductCatalogTypeName, x => x.MapFrom(y => y.ProductCatalogType.Name))
            .ForMember(x => x.VendorName, x => x.MapFrom(y => y.Manufacturer.Name))
            .ForMember(x => x.TemplateID, x => x.MapFrom(y => y.ProductCatalogType.ProductCatalogTemplateID))
            .ForMember(x => x.CategoryName, x => x.MapFrom(y => y.ProductCatalogType.ProductCatalogCategory.Name))
            .ForMember(x => x.LifeCycleName, x => x.MapFrom(y => y.ProductCatalogType.LifeCycle.Name))
            .ForMember(x => x.IsLogical, x => x.MapFrom(y => y.ProductCatalogType.IsLogical))
            .ForMember(x => x.TemplateClassName,
                x => x.MapFrom(y => y.ProductCatalogType.ProductCatalogTemplate.Name))
            .ForMember(x => x.TemplateClassID,
                x => x.MapFrom(y => y.ProductCatalogType.ProductCatalogTemplate.ClassID))
            .ForMember(dst => dst.Properties, mapper => mapper.MapFrom(src => new NetworkDeviceModelAdditionalFields(src)));
    }
}
