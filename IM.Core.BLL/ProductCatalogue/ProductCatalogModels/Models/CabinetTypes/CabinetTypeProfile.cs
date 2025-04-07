using AutoMapper;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.BLL.ProductCatalogue.Models.AdditionalFields;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.CabinetTypes;
internal sealed class CabinetTypeProfile : Profile
{
    public CabinetTypeProfile()
    {
        CreateMap<ProductCatalogModelData, CabinetType>()
            .ForMember(dst => dst.ManufacturerID, mapper => mapper.MapFrom(src => src.VendorID));

        CreateMap<CabinetType, ProductModelOutputDetails>()
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
            .ForMember(dst => dst.Properties, mapper => mapper.MapFrom(src => new CabinetTypeAdditionalFields(src)));
    }
}
