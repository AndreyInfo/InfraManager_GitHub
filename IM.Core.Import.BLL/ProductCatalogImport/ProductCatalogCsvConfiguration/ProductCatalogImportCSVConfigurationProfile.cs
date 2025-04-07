using AutoMapper;
using IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogCsvConfiguration;
using InfraManager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Import.SettingsBLL.ImportSettings.ProductCatalogCsvConfiguration;

internal class ProductCatalogImportCSVConfigurationProfile : Profile
{
    public ProductCatalogImportCSVConfigurationProfile()
    {
        CreateMap<ProductCatalogImportCSVConfiguration, ProductCatalogImportCSVConfigurationOutputDetails>();

        CreateMap<ProductCatalogImportCSVConfigurationDetails, ProductCatalogImportCSVConfiguration>();
        
        CreateMap<ProductCatalogImportInsertCSVConfigurationDetails, ProductCatalogImportCSVConfiguration>();
    }
}