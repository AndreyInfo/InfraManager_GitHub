using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Interface.Import.Models.ModelSettings
{
    public class ProductCatalogImportSettingTypesOutputDetails
    {
        public Guid ProductCatalogImportSettingID { get; init; }

        public ProductCatalogImportSetting ProductCatalogImportSetting { get; set; }

        public Guid ProductCatalogTypeID { get; init; }

        public ProductCatalogType ProductCatalogType { get; set; }
    }
}