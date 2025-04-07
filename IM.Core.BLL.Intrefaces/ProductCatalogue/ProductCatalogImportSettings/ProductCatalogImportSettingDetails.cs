using System;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Configuration;
using InfraManager.DAL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogImportSettings
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