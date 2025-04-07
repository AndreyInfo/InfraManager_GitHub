using System;
using Inframanager;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.DAL.Asset
{
    [ObjectClassMapping(ObjectClass.AdapterModel)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.AdapterModel_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.AdapterModel_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.AdapterModel_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.AdapterModel_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    [ImportType(Name = "IImportEntity")]
    public partial class AdapterType :
        IMarkableForDelete
        , IProductModel<int>
        , IImportExtendedModel
    {
        public AdapterType()
        {
            IMObjID = Guid.NewGuid();
        }

        public Guid IMObjID { get; init; }
        public int ManufacturerID { get; set; }
        public string Name { get; set; }
        public string Parameters { get; set; }
        public string Note { get; set; }
        public string ProductNumber { get; set; }
        public string ExternalID { get; set; }
        public string Code { get; set; }
        public bool Removed { get; set; }
        public Guid? ComplementaryID { get; set; }
        public Guid ProductCatalogTypeID { get; set; }
        public byte[] RowVersion { get; set; }
        public int? SlotTypeID { get; set; }
        public bool CanBuy { get; set; }

        public virtual ProductCatalogType ProductCatalogType { get; init; }
        public virtual SlotType SlotType { get; init; }
        public virtual Manufacturer Vendor { get; init; }
        public void MarkForDelete() => Removed = true;
    }
}