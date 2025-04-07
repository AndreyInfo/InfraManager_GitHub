
		using AutoMapper;
        using Inframanager.DAL.ProductCatalogue.Import;

        namespace InfraManager.BLL.ProductCatalogue.Import;

    internal class ProductCatalogImportSettingTypesProfile:Profile
    {
        public ProductCatalogImportSettingTypesProfile()
        {
            CreateMap<ProductCatalogImportSettingTypes,ProductCatalogImportSettingTypesOutputDetails>()
                ;
                
            CreateMap<ProductCatalogImportSettingTypesDetails,ProductCatalogImportSettingTypes>();
        }
	}
	