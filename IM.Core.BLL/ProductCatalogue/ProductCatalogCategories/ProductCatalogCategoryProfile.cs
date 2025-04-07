using AutoMapper;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCategories;

internal sealed class ProductCatalogCategoryProfile : Profile
{
    public ProductCatalogCategoryProfile()
    {
        CreateMap<ProductCatalogCategory, ProductCatalogCategoryDetails>()
            .ForMember(x => x.ParentID, x => x.MapFrom(y => y.ParentProductCatalogCategoryID));

        CreateMap<ProductCatalogCategoryData, ProductCatalogCategory>()
            .ConstructUsing(c=> new ProductCatalogCategory(c.Name))
            .ForMember(x => x.ParentProductCatalogCategoryID, x => x.MapFrom(y => y.ParentID));
    }
}
