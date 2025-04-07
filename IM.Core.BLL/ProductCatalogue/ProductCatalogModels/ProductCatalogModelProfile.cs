using AutoMapper;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.DAL.ProductCatalogue.Tree;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels;

internal sealed class ProductCatalogModelProfile:Profile
{
    public ProductCatalogModelProfile()
    {
        CreateMap<ProductModelOutputDetails, ProductCatalogNode>()
            .ForMember(x => x.CanContainsSubNodes, x => x.MapFrom(y => false));
    }

}