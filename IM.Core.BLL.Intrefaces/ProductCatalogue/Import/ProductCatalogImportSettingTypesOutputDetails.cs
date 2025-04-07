using System;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.Import
{
    public class ProductCatalogImportSettingTypesOutputDetails
    {
        public Guid ProductCatalogImportSettingID { get; init; }

        public ProductCatalogImportSetting ProductCatalogImportSetting { get; set; }

        public Guid ProductCatalogTypeID { get; init; }

        public ProductCatalogType ProductCatalogType { get; set; }
    }
}