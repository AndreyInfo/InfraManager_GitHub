using AutoMapper;
using IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogImportSettings;
using InfraManager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Import.SettingsBLL.ImportSettings.ProductCatalogImportSettings;

internal class ProductCatalogImportSettingProfile : Profile
{
    public ProductCatalogImportSettingProfile()
    {
        CreateMap<ProductCatalogImportSetting, ProductCatalogImportSettingOutputDetails>();

        CreateMap<ProductCatalogImportSettingDetails, ProductCatalogImportSetting>();
    }
}