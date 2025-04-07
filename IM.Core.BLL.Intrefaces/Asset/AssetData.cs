using System;

namespace InfraManager.BLL.Asset;
public class AssetData
{
    public int DeviceID { get; init; }
    public Guid ID { get; init; }
    public string Agreement { get; init; }
    public Guid? SupplierID { get; init; }
    public DateTime? Warranty { get; init; }
    public Guid? ServiceContractID { get; init; }
    public int? UserID { get; init; }
    public decimal? Cost { get; init; }
    public string Founding { get; init; }
    public DateTime? AppointmentDate { get; init; }
    public string UserField1 { get; init; }
    public string UserField2 { get; init; }
    public string UserField3 { get; init; }
    public string UserField4 { get; init; }
    public string UserField5 { get; init; }
    public bool OnStore { get; init; }
    public DateTime? DateInquiry { get; init; }
    public Guid? OwnerID { get; init; }
    public ObjectClass? OwnerClassID { get; init; }
    public Guid? UtilizerID { get; set; }
    public ObjectClass? UtilizerClassID { get; set; }
    public Guid? PeripheralDatabaseID { get; init; }
    public int? ComplementaryID { get; init; }
    public Guid? ComplementaryGuidID { get; init; }
    public bool IsWorking { get; init; }
    public DateTime? DateAnnuled { get; init; }
    public Guid? LifeCycleStateID { get; init; }
    public Guid? FixedAsinitID { get; init; }
    public Guid? GoodsInvoiceID { get; init; }
    public Guid? ServiceCenterID { get; init; }
    public Guid? StorageID { get; init; }
}
