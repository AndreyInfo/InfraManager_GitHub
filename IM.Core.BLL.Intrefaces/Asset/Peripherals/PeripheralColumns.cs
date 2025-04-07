using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.Asset.Peripherals;

[ListViewItem(ListView.PeripheralColumns)]
public class PeripheralColumns
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Type))]
    public string ProductCatalogTypeName { get; init; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.Model))]
    public string ProductCatalogModelName { get; init; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.TerminalDevice))]
    public string TerminalDeviceName { get; init; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.NetworkDevice))]
    public string NetworkDeviceName { get; init; }

    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.SerialNumber))]
    public string SerialNumber { get; init; }

    [ColumnSettings(6, 100)]
    [Label(nameof(Resources.Code))]
    public string Code { get; init; }

    [ColumnSettings(7, 100)]
    [Label(nameof(Resources.ParametersDefaultGroupName))]
    public string Parameters { get; init; }

    [ColumnSettings(8, 100)]
    [Label(nameof(Resources.Peripheral_BWLoad))]
    public decimal? BWLoad { get; init; }

    [ColumnSettings(9, 100)]
    [Label(nameof(Resources.Peripheral_ColorLoad))]
    public decimal? ColorLoad { get; init; }

    [ColumnSettings(10, 100)]
    [Label(nameof(Resources.Peripheral_PhotoLoad))]
    public decimal? PhotoLoad { get; init; }

    [ColumnSettings(11, 100)]
    [Label(nameof(Resources.AssetNumber_Note))]
    public string Note { get; init; }

    [ColumnSettings(12, 100)]
    [Label(nameof(Resources.InquiryState))]
    public string InquiryState { get; init; }

    [ColumnSettings(13, 100)]
    [Label(nameof(Resources.ProductCatalogueModel_Class))]
    public string ProductCatalogTemplateName { get; init; }

    [ColumnSettings(14, 100)]
    [Label(nameof(Resources.AssetNumber_AssetStateName))]
    public string LifeCycleStateName { get; init; }

    [ColumnSettings(15, 100)]
    [Label(nameof(Resources.AssetNumber_IsWorking))]
    public bool IsWorking { get; init; }

    [ColumnSettings(16, 100)]
    [Label(nameof(Resources.AssetNumber_Cost))]
    public decimal? Cost { get; init; }

    [ColumnSettings(17, 100)]
    [Label(nameof(Resources.AssetNumber_Founding))]
    public string Founding { get; init; }

    [ColumnSettings(18, 100)]
    [Label(nameof(Resources.AssetNumber_AppointmentDate))]
    public DateTime? AppointmentDate { get; init; }

    [ColumnSettings(19, 100)]
    [Label(nameof(Resources.MOL))]
    public string UserName { get; init; }

    [ColumnSettings(20, 100)]
    [Label(nameof(Resources.Supplier))]
    public string SupplierName { get; init; }

    [ColumnSettings(21, 100)]
    [Label(nameof(Resources.AssetNumber_DateReceived))]
    public DateTime? DateReceived { get; init; }

    [ColumnSettings(22, 100)]
    [Label(nameof(Resources.AssetNumber_DateAnnuled))]
    public DateTime? DateAnnuled { get; init; }

    [ColumnSettings(23, 100)]
    [Label(nameof(Resources.AssetNumber_Agreement))]
    public string Agreement { get; init; }

    [ColumnSettings(24, 100)]
    [Label(nameof(Resources.AssetNumber_Warranty))]
    public DateTime? Warranty { get; init; }

    [ColumnSettings(25, 100)]
    [Label(nameof(Resources.AssetNumber_ServiceCenterName))]
    public string ServiceCenterName { get; init; }

    [ColumnSettings(26, 100)]
    [Label(nameof(Resources.AssetNumber_ServiceContractNumber))]
    public string ServiceContractNumber { get; init; }

    [ColumnSettings(27, 100)]
    [Label(nameof(Resources.AssetNumber_OwnerName))]
    public string OwnerName { get; init; }

    [ColumnSettings(28, 100)]
    [Label(nameof(Resources.AssetNumber_UtilizerName))]
    public string UtilizerName { get; init; }

    [ColumnSettings(29, 100)]
    [Label(nameof(Resources.ExternalID))]
    public string ExternalID { get; init; }
}
