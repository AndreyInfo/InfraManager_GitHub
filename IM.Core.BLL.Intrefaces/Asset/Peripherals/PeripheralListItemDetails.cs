using System;

namespace InfraManager.BLL.Asset.Peripherals;

public class PeripheralListItemDetails
{
    public string ProductCatalogTypeName { get; init; }
    public string ProductCatalogModelName { get; init; }
    public string TerminalDeviceName { get; init; }
    public string NetworkDeviceName { get; init; }
    public string SerialNumber { get; init; }
    public string Code { get; init; }
    public string Parameters { get; init; }
    public decimal? BWLoad { get; init; }
    public decimal? ColorLoad { get; init; }
    public decimal? PhotoLoad { get; init; }
    public string Note { get; init; }
    public string InquiryState { get; init; }
    public string ProductCatalogTemplateName { get; init; }
    public string LifeCycleStateName { get; init; }
    public bool IsWorking { get; init; }
    public decimal? Cost { get; init; }
    public string Founding { get; init; }
    public DateTime? AppointmentDate { get; init; }
    public string UserName { get; init; }
    public string SupplierName { get; init; }
    public DateTime? DateReceived { get; init; }
    public DateTime? DateAnnuled { get; init; }
    public string Agreement { get; init; }
    public DateTime? Warranty { get; init; }
    public string ServiceCenterName { get; init; }
    public int? ServiceContractNumber { get; init; }
    public string OwnerName { get; init; }
    public string UtilizerName { get; init; }
    public string ExternalID { get; init; }
}
