using AutoMapper;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.BLL.ProductCatalogue.Models.AdditionalFields;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.SoftwareLicenseModels;
internal sealed class SoftwareLicenseModelProfile : Profile
{
    public SoftwareLicenseModelProfile()
    {
        CreateMap<ProductCatalogModelData, SoftwareLicenseModel>();

        CreateMap<SoftwareLicenseModel, ProductModelOutputDetails>()
            .ForMember(x => x.ManufactureID, x => x.MapFrom(y => y.ManufacturerID))
            .ForMember(x => x.VendorID, x => x.MapFrom(y => y.Manufacturer.ID))
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
            .ForMember(dst => dst.Properties, mapper => mapper.MapFrom(src => new SoftwareLicenseModelAdditionalFields(src)));
    }
}
