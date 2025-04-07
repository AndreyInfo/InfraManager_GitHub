using System;
using System.Collections.Generic;
using Inframanager;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Configuration;

namespace InfraManager.DAL.ProductCatalogue.Import
{
    [ObjectClassMapping(ObjectClass.ProductCatalogImportSetting)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.None)]
    [OperationIdMapping(ObjectAction.Update, OperationID.None)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public class ProductCatalogImportSetting
    {
        public Guid ID { get; init; }

        public string Name { get; init; }

        public string Note { get; init; }

        public bool RestoreRemovedModels { get; init; }

        public int TechnologyTypeID { get; init; }

        public virtual TechnologyType TechnologyType { get; set; }

        public int JackTypeID { get; init; }

        public virtual ConnectorType ConnectorType { get; set; }

        public Guid ProductCatalogImportCSVConfigurationID { get; init; }

        public virtual ProductCatalogImportCSVConfiguration ProductCatalogImportCSVConfiguration { get; set; }

        public string Path { get; init; }

        public byte[] RowVersion { get; init; }
    }
}