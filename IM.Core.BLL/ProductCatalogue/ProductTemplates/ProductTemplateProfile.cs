using AutoMapper;
using InfraManager.BLL.ProductCatalogue.Classes;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductTemplates;

internal sealed class ProductTemplateProfile : Profile
{
    public ProductTemplateProfile()
    {
        CreateMap<ProductCatalogTemplate, ProductTemplateInfo>();
    }
}