using System;
using Inframanager;
using InfraManager.DAL.Finance;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.DAL.Asset
{
    [ObjectClassMapping(ObjectClass.ServiceContract)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.ServiceContract_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.ServiceContract_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.ServiceContract_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.ServiceContract_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ServiceContract_Properties)]
    public class ServiceContract : IProduct<ServiceContractModel>
    {
        public Guid ID { get; init; }
        public int Number { get; init; }
        public DateTime UtcStartDate { get; init; }
        public DateTime UtcFinishDate { get; init; }
        public decimal Cost { get; init; }
        public string Notice { get; init; }
        public Guid? PeripheralDatabaseID { get; init; }
        public Guid? ServiceContractTypeID { get; set; }
        public Guid? LifeCycleStateID { get; set; }
        public Guid? SupplierID { get; set; }
        public Guid? WorkOrderID { get; init; }
        public Guid? FinanceCenterID { get; init; }
        public Guid? ProductCatalogTypeID { get; set; }
        public Guid? ComplementaryID { get; init; }
        public byte[] RowVersion { get; init; }
        public byte TimePeriod { get; init; }
        public Guid? GoodsInvoiceID { get; init; }
        public string ExternalNumber { get; init; }
        public string Description { get; init; }
        public Guid? ManufacturerID { get; init; }
        public Guid? ManagerID { get; init; }
        public ObjectClass? ManagerClassID { get; init; }
        public short UpdateType { get; init; }
        public short UpdatePeriod { get; init; }
        public short NdsType { get; init; }
        public short NdsPercent { get; init; }
        public decimal? NdsCustomValue { get; init; }
        public string AddressLicence { get; init; }
        public string LoginLicence { get; init; }
        public string PasswordLicence { get; init; }
        public string AddressAsset { get; init; }
        public string LoginAsset { get; init; }
        public string PasswordAsset { get; init; }
        public DateTime UtcDateRegistered { get; init; }
        public DateTime UtcInitialDateStart { get; init; }
        public DateTime UtcInitialDateFinish { get; init; }
        public decimal InitialCost { get; init; }
        public DateTime UtcDateCreated { get; init; }
        public Guid? InitiatorID { get; init; }
        public ObjectClass? InitiatorClassID { get; init; }
        public bool? ReInit { get; init; }
        public Guid? ModelID { get; init; }

        public virtual ServiceContractType ServiceContractType { get; init; }
        public virtual LifeCycleState LifeCycleState { get; init; }
        public virtual ProductCatalogType ProductCatalogType { get; init; }
        public virtual Supplier Supplier { get; init; }
        public virtual WorkOrder WorkOrder { get; init; }
        public virtual ServiceContractModel Model { get; init; }
        public virtual FinanceCenter FinanceCenter { get; init; }
    }
}