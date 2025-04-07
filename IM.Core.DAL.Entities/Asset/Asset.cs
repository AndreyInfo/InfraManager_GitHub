using System;
using Inframanager;
using InfraManager.DAL.Finance;
using InfraManager.DAL.Location;
using InfraManager.DAL.ProductCatalogue.LifeCycles;

namespace InfraManager.DAL.Asset
{
    [ObjectClassMapping(ObjectClass.Asset)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.Asset_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Asset_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Asset_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.Asset_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Asset_Properties)]
    public partial class Asset
    {
        public Asset()
        {
            DateReceived = DateTime.UtcNow;
            AppointmentDate = DateTime.UtcNow;
            IsWorking = true;
        }

        public const int NonExistentDeviceID = 0;

        public int DeviceID { get; set; }
        public DateTime? DateReceived { get; set; }
        public string Agreement { get; set; }
        public Guid? SupplierID { get; set; }
        public DateTime? Warranty { get; set; }
        public Guid? ServiceContractID { get; set; }
        public int? UserID { get; set; }
        public decimal? Cost { get; set; }
        public string Founding { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string UserField1 { get; set; }
        public string UserField2 { get; set; }
        public string UserField3 { get; set; }
        public string UserField4 { get; set; }
        public string UserField5 { get; set; }
        public bool OnStore { get; set; } // При добавлении в бд OnStore = true
        public byte[] tstamp { get; set; }
        public Guid ID { get; set; }
        public DateTime? DateInquiry { get; set; }
        public Guid? OwnerID { get; set; }
        public ObjectClass? OwnerClassID { get; set; }
        public Guid? UtilizerID { get; set; }
        public ObjectClass? UtilizerClassID { get; set; }
        public Guid? PeripheralDatabaseID { get; set; }
        public int? ComplementaryID { get; set; }
        public Guid? ComplementaryGuidID { get; set; }
        public bool IsWorking { get; set; }
        public DateTime? DateAnnuled { get; set; }
        public Guid? LifeCycleStateID { get; set; }
        public Guid? FixedAssetID { get; set; }
        public Guid? GoodsInvoiceID { get; set; }
        public Guid? ServiceCenterID { get; set; }
        public Guid? StorageID { get; set; }
        
        public virtual User Owner { get; init; }
        public virtual User Utilizer { get; init; }
        public virtual LifeCycleState LifeCycleState { get; set; }
        public virtual StorageLocation StorageLocation { get; init; }
        public virtual User User { get; init; }
        public virtual Supplier Supplier { get; init; }
        public virtual Supplier ServiceCenter { get; init; }
        public virtual ServiceContract ServiceContract { get; init; }
    }
}