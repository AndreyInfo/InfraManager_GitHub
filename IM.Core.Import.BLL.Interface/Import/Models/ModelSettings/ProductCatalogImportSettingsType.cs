using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Interface.Import.Models.ModelSettings
{
    public record ProductCatalogImportSettingsType
    {
        public Guid ProductCatalogImportSettingID { get; init; }

        public Guid ProductCatalogTypeID { get; init; }
    }
}