namespace IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogImportSettings
{
    public class ProductCatalogImportSettingDetails
    {
        public string Name { get; init; }

        public string Note { get; init; }

        public bool RestoreRemovedModels { get; init; }

        public int TechnologyTypeID { get; init; }

        public int JackTypeID { get; init; }

        public Guid ProductCatalogImportCSVConfigurationID { get; init; }

        public string Path { get; init; }

        public byte[] RowVersion { get; init; }
    }
}