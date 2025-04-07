using System;
using InfraManager;
using InfraManager.DAL.ProductCatalogue.Import;

namespace Inframanager.DAL.ProductCatalogue.Import
{
    [OperationIdMapping(ObjectAction.Insert, OperationID.None)]
    [OperationIdMapping(ObjectAction.Update, OperationID.None)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public class ProductCatalogImportCSVConfigurationConcordance
    {
        public Guid ID { get; init; }

        public virtual ProductCatalogImportCSVConfiguration ProductCatalogImportCSVConfiguration { get; set; }

        public string Field { get; init; }

        public string Expression { get; init; }
        
    }
}