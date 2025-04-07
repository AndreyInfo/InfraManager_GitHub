using System;

namespace InfraManager.BLL.AssetsManagement.Hardware;

public class HardwareListItemModel
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string SerialNumber { get; init; }
    public string Code { get; init; }
    public string Note { get; init; }
    public string TypeName { get; init; }
    public string ModelName { get; init; }
    public string VendorName { get; init; }
    public string RoomName { get; init; }
    public string RackName { get; init; }
    public string WorkplaceName { get; init; }
    public string FloorName { get; init; }
    public string BuildingName { get; init; }
    public string OrganizationName { get; init; }
    public string LifeCycleStateName { get; init; }
    public string InvNumber { get; init; }
    public string Agreement { get; init; }
    public string UserName { get; init; }
    public string Founding { get; init; }
    public string OwnerName { get; init; }
    public string UtilizerName { get; init; }
    public string AppointmentDate { get; init; }
    public decimal? Cost { get; init; }
    public string ServiceCenterName { get; init; }
    public string ServiceContractNumber { get; init; }
    public string Warranty { get; init; }
    public string SupplierName { get; init; }
    public string DateReceived { get; init; }
    public string DateInquiry { get; init; }
    public string DateAnnuled { get; init; }
    public string UserField1 { get; init; }
    public string UserField2 { get; init; }
    public string UserField3 { get; init; }
    public string UserField4 { get; init; }
    public string UserField5 { get; init; }
    public string IsWorking { get; init; }
    public string ProductCatalogTemplateName { get; init; }
    public string LocationOnStore { get; init; }
    public string ServiceContractLifeCycleStateName { get; init; }
    public string ServiceContractUtcFinishDate { get; init; }
}