using AutoMapper;
using InfraManager.DAL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogImportSettings;

internal class ProductCatalogImportSettingProfile : Profile
{
    public ProductCatalogImportSettingProfile()
    {
        CreateMap<ProductCatalogImportSetting, ProductCatalogImportSettingOutputDetails>()
            ;

        CreateMap<ProductCatalogImportSettingDetails, ProductCatalogImportSetting>();
    }
}