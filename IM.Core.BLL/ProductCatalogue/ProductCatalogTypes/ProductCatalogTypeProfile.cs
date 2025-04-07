using AutoMapper;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue.Tree;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;

internal sealed class ProductCatalogTypeProfile : Profile
{
    public ProductCatalogTypeProfile()
    {
        CreateMap<ProductCatalogType, ProductCatalogTypeDetails>()
            .ForMember(x => x.ID, x => x.MapFrom(x=>x.IMObjID))
            .ForMember(x => x.ParentID, x => x.MapFrom(x=>x.ProductCatalogCategoryID))
            .ForMember(x => x.TemplateID, x => x.MapFrom(y => y.ProductCatalogTemplateID))
            .ForMember(x => x.TemplateName, x => x.MapFrom(y => y.ProductCatalogTemplate.Name))
            .ForMember(x => x.TemplateClassID, x => x.MapFrom(y => y.ProductCatalogTemplate.ClassID))
            .ForMember(x => x.LifeCycleName, x => x.MapFrom(y => y.LifeCycle.Name))
            .ForMember(x => x.LifeCycleType, x => x.MapFrom(y => y.LifeCycle.Type))
            .ForMember(x => x.IsSubjectAsset,x => x.MapFrom(scr => scr.IsAccountingAsset))
            .ForMember(x => x.Agreement,x => x.MapFrom(scr => scr.ServiceContractTypeAgreement));

        CreateMap<ProductCatalogTypeData, ProductCatalogType>()
            .ForMember(x => x.IsAccountingAsset, x => x.MapFrom(scr => scr.IsSubjectAsset))
            .ForMember(dst => dst.ProductCatalogCategoryID, m => m.MapFrom(scr => scr.ParentID))
            .ForMember(dst => dst.ProductCatalogTemplateID, m => m.MapFrom(scr => scr.TemplateID))
            .ForMember(dst => dst.ServiceContractTypeAgreement, m => m.MapFrom(scr => scr.Agreement))
            .ForMember(dst => dst.ServiceContractFeatures, m => m.MapFrom(scr => scr.ServiceContractFeatures));

        CreateMap<ProductCatalogType, ProductCatalogNode>()
            .ForMember(x => x.ID, x => x.MapFrom(y => y.IMObjID))
            .ForMember(x => x.CanContainsSubNodes, x => x.MapFrom(y => false))
            .ForMember(x => x.TemplateClassID, x => x.MapFrom(y => y.ProductCatalogTemplate.ClassID))
            .ForMember(x => x.ClassID, x => x.MapFrom(y => ObjectClass.ProductCatalogType))
            .ForMember(x => x.ParentID, x => x.MapFrom(y => y.ProductCatalogCategoryID));
    }
}