using InfraManager.DAL.Asset;
using System;
using Inframanager;

namespace InfraManager.DAL.ProductCatalogue
{
    [ObjectClassMapping(ObjectClass.PeripherialModel)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.PeripheralModel_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.PeripheralModel_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.PeripheralModel_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails,OperationID.PeripheralModel_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ProductCatalogType_Details)]
    public class PeripheralType : IMarkableForDelete
        , IProductModel<int>
        , IImportExtendedModel
    {
        public Guid IMObjID { get; init; }
        public int ManufacturerID { get; set; }
        public string Name { get; set; }
        public string Parameters { get; set; }
        public string Note { get; set; }
        public string ProductNumber { get; set; }
        public string ExternalID { get; set; }
        public string Code { get; set; }
        public decimal? MaxLoad { get; set; }
        public decimal? NomLoad { get; set; }
        public byte? CanColorPrint { get; set; }
        public byte? CanFotoPrint { get; set; }
        public Guid? ComplementaryID { get; set; }
        public bool Removed { get; private set; }
        public void MarkForDelete() => Removed = true;
        public Guid ProductCatalogTypeID { get; set; }
        public byte[] RowVersion { get; init; }
        public bool CanBuy { get; set; }

        public virtual ProductCatalogType ProductCatalogType { get; init; }
        public virtual Manufacturer Vendor { get; init; }
    }
}
