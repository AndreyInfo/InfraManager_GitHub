using IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogCsvConfiguration;

namespace IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogImportSettings
{
    public class ProductCatalogImportSettingOutputDetails
    {
        public Guid ID { get; init; }

        public string Name { get; init; }

        public string Note { get; init; }

        public bool RestoreRemovedModels { get; init; }

        public int TechnologyTypeID { get; init; }

        public TechnologyTypeOutputDetails TechnologyType { get; set; }

        public int JackTypeID { get; init; }

        public ConnectorTypeOutputDetails ConnectorType { get; set; }

        public Guid ProductCatalogImportCSVConfigurationID { get; init; }

        public string Path { get; init; }

        public byte[] RowVersion { get; init; }
    }
}