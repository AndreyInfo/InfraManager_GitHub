using System;
using Inframanager;

namespace InfraManager.DAL.ProductCatalogue.Import
{
    [ObjectClassMapping(ObjectClass.ProductCatalogImportCSVConfiguration)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.None)]
    [OperationIdMapping(ObjectAction.Update, OperationID.None)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public class ProductCatalogImportCSVConfiguration
    {
        public Guid ID { get; init; }

        public string Name { get; init; }

        public string Note { get; init; }

        public char Delimeter { get; init; }

        public byte[] RowVersion { get; init; }
        
        public virtual ProductCatalogImportSetting ProductCatalogImportSetting { get; init; }
    }
}