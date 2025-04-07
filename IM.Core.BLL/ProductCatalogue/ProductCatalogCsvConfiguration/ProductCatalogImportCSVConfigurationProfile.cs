using AutoMapper;
using InfraManager.DAL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCsvConfiguration;

internal class ProductCatalogImportCSVConfigurationProfile : Profile
{
    public ProductCatalogImportCSVConfigurationProfile()
    {
        CreateMap<ProductCatalogImportCSVConfiguration, ProductCatalogImportCSVConfigurationOutputDetails>()
            ;

        CreateMap<ProductCatalogImportCSVConfigurationDetails, ProductCatalogImportCSVConfiguration>();
        
        CreateMap<ProductCatalogImportInsertCSVConfigurationDetails, ProductCatalogImportCSVConfiguration>();
    }
}