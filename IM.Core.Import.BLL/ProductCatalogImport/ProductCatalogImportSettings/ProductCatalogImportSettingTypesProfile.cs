using AutoMapper;
using IM.Core.Import.BLL.Interface.Import.Models.ModelSettings;
using Inframanager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Import.SettingsBLL.ImportSettings;

    internal class ProductCatalogImportSettingTypesProfile:Profile
    {
        public ProductCatalogImportSettingTypesProfile()
        {
            CreateMap<ProductCatalogImportSettingTypes,ProductCatalogImportSettingTypesOutputDetails>()
                ;
                
            CreateMap<ProductCatalogImportSettingTypesDetails,ProductCatalogImportSettingTypes>();
        }
	}
	