using AutoMapper;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.BLL.ProductCatalogue.Models.AdditionalFields;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.PeripherialTypes;
internal sealed class PeripheralTypeProfile : Profile
{
    public PeripheralTypeProfile()
    {
        CreateMap<ProductCatalogModelData, PeripheralType>()
            .ForMember(dst => dst.Parameters, mapper => mapper.MapFrom(src => src.Properties.Parameters))
            .ForMember(dst => dst.CanBuy, mapper => mapper.MapFrom(src => src.Properties.CanBuy))
            .ForMember(dst => dst.ManufacturerID, mapper => mapper.MapFrom(src => src.VendorID));

        CreateMap<PeripheralType, ProductModelOutputDetails>()
            .ForMember(x => x.ID, x => x.MapFrom(y => y.IMObjID))
            .ForMember(x => x.ProductCatalogTypeName, x => x.MapFrom(y => y.ProductCatalogType.Name))
            .ForMember(x => x.VendorName, x => x.MapFrom(y => y.Vendor.Name))
            .ForMember(x => x.TemplateID, x => x.MapFrom(y => y.ProductCatalogType.ProductCatalogTemplateID))
            .ForMember(x => x.CategoryName, x => x.MapFrom(y => y.ProductCatalogType.ProductCatalogCategory.Name))
            .ForMember(x => x.LifeCycleName, x => x.MapFrom(y => y.ProductCatalogType.LifeCycle.Name))
            .ForMember(x => x.IsLogical, x => x.MapFrom(y => y.ProductCatalogType.IsLogical))
            .ForMember(x => x.TemplateClassName,
                x => x.MapFrom(y => y.ProductCatalogType.ProductCatalogTemplate.Name))
            .ForMember(x => x.TemplateClassID,
                x => x.MapFrom(y => y.ProductCatalogType.ProductCatalogTemplate.ClassID))
            .ForMember(dest => dest.Properties, mapper => mapper.MapFrom(src => new PeripheralTypeAdditionalFields(src)));
    }
}
