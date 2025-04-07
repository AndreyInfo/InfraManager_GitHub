using AutoMapper;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.BLL.ProductCatalogue.Models.AdditionalFields;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.AdapterTypes;
internal sealed class AdapterTypeProfile : Profile
{
    public AdapterTypeProfile()
    {
        CreateMap<ProductCatalogModelData, AdapterType>()
            .ForMember(dst => dst.SlotTypeID, mapper => mapper.MapFrom(src => src.Properties.SlotTypeID))
            .ForMember(dst => dst.CanBuy, mapper => mapper.MapFrom(src => src.Properties.CanBuy))
            .ForMember(dst => dst.ManufacturerID, mapper => mapper.MapFrom(src => src.VendorID))
            .ForMember(dst => dst.Parameters, mapper => mapper.MapFrom(src => src.Properties.Parameters));

        CreateMap<AdapterType, ProductModelOutputDetails>()
           .ForMember(x => x.ID, x => x.MapFrom(y => y.IMObjID))
           .ForMember(x => x.ProductCatalogTypeName, x => x.MapFrom(y => y.ProductCatalogType.Name))
           .ForMember(x => x.VendorName, x => x.MapFrom(y => y.Vendor.Name))
           .ForMember(x => x.TemplateID, x => x.MapFrom(y => y.ProductCatalogType.ProductCatalogTemplateID))
           .ForMember(x => x.LifeCycleName, x => x.MapFrom(y => y.ProductCatalogType.LifeCycle.Name))
           .ForMember(x => x.IsLogical, x => x.MapFrom(y => y.ProductCatalogType.IsLogical))
           .ForMember(x => x.ProductNumber, x => x.MapFrom(y => y.ProductNumber))
           .ForMember(x => x.TemplateClassName,
               x => x.MapFrom(y => y.ProductCatalogType.ProductCatalogTemplate.Name))
           .ForMember(x => x.TemplateClassID,
               x => x.MapFrom(y => y.ProductCatalogType.ProductCatalogTemplate.ClassID))
           .ForMember(dst => dst.Properties, mapper => mapper.MapFrom(src => new AdapterTypeAdditionalFields(src)));
    }
}
