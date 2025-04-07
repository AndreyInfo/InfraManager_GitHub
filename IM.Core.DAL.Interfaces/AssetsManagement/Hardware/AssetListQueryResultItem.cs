using System;

namespace InfraManager.DAL.AssetsManagement.Hardware;

public class AssetListQueryResultItem
{
    public int DeviceID { get; init; }
    public Guid? LifeCycleStateID { get; init; }
    public string LifeCycleStateName { get; init; }
    public string Agreement { get; init; }
    public Guid? UserID { get; init; }
    public string UserName { get; init; }
    public string Founding { get; init; }
    public Guid? OwnerID { get; init; }
    public string OwnerName { get; init; }
    public Guid? UtilizerID { get; init; }
    public string UtilizerName { get; init; }
    public DateTime? AppointmentDate { get; init; }
    public decimal? Cost { get; init; }
    public Guid? ServiceCenterID { get; init; }
    public string ServiceCenterName { get; init; }
    public Guid? ServiceContractID { get; init; }
    public int? ServiceContractNumber { get; init; }
    public DateTime? Warranty { get; init; }
    public Guid? SupplierID { get; init; }
    public string SupplierName { get; init; }
    public DateTime? DateReceived { get; init; }
    public DateTime? DateInquiry { get; init; }
    public DateTime? DateAnnuled { get; init; }
    public string UserField1 { get; init; }
    public string UserField2 { get; init; }
    public string UserField3 { get; init; }
    public string UserField4 { get; init; }
    public string UserField5 { get; init; }
    public bool IsWorking { get; init; }
    public Guid? ServiceContractLifeCycleStateID { get; init; }
    public string ServiceContractLifeCycleStateName { get; init; }
    public DateTime? ServiceContractUtcFinishDate { get; init; }
    public bool OnStore { get; init; }
}