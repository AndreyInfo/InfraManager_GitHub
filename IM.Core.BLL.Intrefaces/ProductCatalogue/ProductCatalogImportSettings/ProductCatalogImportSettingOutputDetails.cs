using System;
using InfraManager.BLL.Asset.ConnectorTypes;
using InfraManager.BLL.ProductCatalogue.ProductCatalogCsvConfiguration;
using InfraManager.BLL.Technologies;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Configuration;
using InfraManager.DAL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogImportSettings
{
    public class ProductCatalogImportSettingOutputDetails
    {
        public Guid ID { get; init; }

        public string Name { get; init; }

        public string Note { get; init; }

        public bool RestoreRemovedModels { get; init; }

        public int TechnologyTypeID { get; init; }

        public TechnologyTypeDetails TechnologyType { get; set; }

        public int JackTypeID { get; init; }

        public ConnectorTypeData ConnectorType { get; set; }

        public Guid ProductCatalogImportCSVConfigurationID { get; init; }

        public ProductCatalogImportCSVConfigurationDetails ProductCatalogImportCSVConfiguration { get; set; }

        public string Path { get; init; }

        public byte[] RowVersion { get; init; }
    }
}