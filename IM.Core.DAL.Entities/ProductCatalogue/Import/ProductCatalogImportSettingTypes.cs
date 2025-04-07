using System;
using InfraManager;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue.Import;

namespace Inframanager.DAL.ProductCatalogue.Import
{
    [OperationIdMapping(ObjectAction.Insert, OperationID.None)]
    [OperationIdMapping(ObjectAction.Update, OperationID.None)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public class ProductCatalogImportSettingTypes
    {
        public Guid ProductCatalogImportSettingID { get; init; }

        public virtual ProductCatalogImportSetting ProductCatalogImportSetting { get; set; }

        public Guid ProductCatalogTypeID { get; init; }

        public virtual ProductCatalogType ProductCatalogType { get; set; }
    }
}