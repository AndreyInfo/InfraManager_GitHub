using AutoMapper;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.BLL.ProductCatalogue.Models.AdditionalFields;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.TerminalModels;
internal sealed class TerminalDeviceModelProfile : Profile
{
    public TerminalDeviceModelProfile()
    {
        CreateMap<ProductCatalogModelData, TerminalDeviceModel>()
            .ForMember(dst => dst.TechnologyTypeID, mapper => mapper.MapFrom(src => src.Properties.TechnologyTypeID))
            .ForMember(dst => dst.ConnectorTypeID, mapper => mapper.MapFrom(src => src.Properties.ConnectorTypeID))
            .ForMember(dst => dst.CanBuy, mapper => mapper.MapFrom(src => src.Properties.CanBuy))
            .ForMember(dst => dst.ManufacturerID, mapper => mapper.MapFrom(src => src.VendorID));

        CreateMap<TerminalDeviceModel, ProductModelOutputDetails>()
            .ForMember(x => x.VendorID, x => x.MapFrom(y => y.ManufacturerID))
            .ForMember(x => x.ID, x => x.MapFrom(y => y.IMObjID))
            .ForMember(x => x.ProductCatalogTypeName, x => x.MapFrom(y => y.ProductCatalogType.Name))
            .ForMember(x => x.VendorName, x => x.MapFrom(y => y.Manufacturer.Name))
            .ForMember(x => x.TemplateID, x => x.MapFrom(y => y.ProductCatalogType.ProductCatalogTemplateID))
            .ForMember(x => x.LifeCycleName, x => x.MapFrom(y => y.ProductCatalogType.LifeCycle.Name))
            .ForMember(x => x.IsLogical, x => x.MapFrom(y => y.ProductCatalogType.IsLogical))
            .ForMember(x => x.TemplateClassName,
                x => x.MapFrom(y => y.ProductCatalogType.ProductCatalogTemplate.Name))
            .ForMember(x => x.TemplateClassID,
                x => x.MapFrom(y => y.ProductCatalogType.ProductCatalogTemplate.ClassID))
            .ForMember(dst => dst.Properties, mapper => mapper.MapFrom(src => new TerminalDeviceModelAdditionalFields(src)));
    }
}
