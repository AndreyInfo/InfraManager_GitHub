using System;

namespace InfraManager.DAL.AssetsManagement.Hardware;

public class HardwareListQueryResultItemBase
{
    public Guid ID { get; init; }
    public ObjectClass ClassID { get; init; }
    public string Name { get; init; }
    public string SerialNumber { get; init; }
    public string Code { get; init; }
    public string Note { get; init; }
    public string TypeName { get; init; }
    public string ModelName { get; init; }
    public Guid ModelID { get; init; }
    public string VendorName { get; init; }
    public Guid? RoomID { get; init; }
    public string RoomName { get; init; }
    public string InvNumber { get; init; }
    public string ProductCatalogTemplateName { get; init; }
    public Guid? RackID { get; init; }
    public string RackName { get; init; }
    public Guid? WorkplaceID { get; init; }
    public string WorkplaceName { get; init; }
    public string FloorName { get; init; }
    public string BuildingName { get; init; }
    public string OrganizationName { get; init; }
    public bool LocationOnStore { get; init; }
    public AssetListQueryResultItem AssetItem { get; init; } = new();
    public string FullObjectName { get; init; }
    public string FullObjectLocation { get; init; }
    public string LifeCycleStateName { get; init; }
    public Guid? LifeCycleStateID { get; init; }
    public string Agreement { get; init; }
    public string UserName { get; init; }
    public string Founding { get; init; }
    public string OwnerName { get; init; }
    public string UtilizerName { get; init; }
    public DateTime? AppointmentDate { get; init; }
    public decimal? Cost { get; init; }
    public string ServiceCenterName { get; init; }
    public int? ServiceContractNumber { get; init; }
    public DateTime? Warranty { get; init; }
    public string SupplierName { get; init; }
    public DateTime? DateReceived { get; init; }
    public DateTime? DateInquiry { get; init; }
    public DateTime? DateAnnuled { get; init; }
    public bool IsWorking { get; init; }
    public string ServiceContractLifeCycleStateName { get; init; }
    public DateTime? ServiceContractUtcFinishDate { get; init; }
    public string IPAddress { get; init; }
    public string ConfigurationUnitName { get; init; }
}