using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.AssetsManagement.Hardware;

[ListViewItem(ListView.AssetSearch)]
public class AssetSearchListItem : IHardwareListItem
{
    public Guid ID { get; init; }
    public ObjectClass ClassID { get; init; }

    [ColumnSettings(0, 100)]
    [Label(nameof(Resources.AssetNumber_Name))]
    public string Name { get; init; }

    [ColumnSettings(3)]
    [Label(nameof(Resources.AssetNumber_Note))]
    public string Note { get; init; }
    
    [ColumnSettings(4)]
    [Label(nameof(Resources.AssetNumber_TypeName))]
    public string TypeName { get; init; }

    [ColumnSettings(5)]
    [Label(nameof(Resources.AssetNumber_ModelName))]
    public string ModelName { get; init; }

    [ColumnSettings(6)]
    [Label(nameof(Resources.AssetNumber_AssetStateName))]
    public string LifeCycleStateName { get; init; }

    [ColumnSettings(7)]
    [Label(nameof(Resources.AssetNumber_RoomName))]
    public string RoomName { get; init; }

    [ColumnSettings(8)]
    [Label(nameof(Resources.AssetNumber_RackName))]
    public string RackName { get; init; }

    [ColumnSettings(9)]
    [Label(nameof(Resources.AssetNumber_WorkplaceName))]
    public string WorkplaceName { get; init; }

    [ColumnSettings(10)]
    [Label(nameof(Resources.AssetNumber_FloorName))]
    public string FloorName { get; init; }

    [ColumnSettings(11)]
    [Label(nameof(Resources.AssetNumber_BuildingName))]
    public string BuildingName { get; init; }

    [ColumnSettings(12)]
    [Label(nameof(Resources.AssetNumber_OrganizationName))]
    public string OrganizationName { get; init; }

    [ColumnSettings(13)]
    [Label(nameof(Resources.AssetNumber_VendorName))]
    public string VendorName { get; init; }

    [ColumnSettings(14)]
    [Label(nameof(Resources.AssetNumber_SerialNumber))]
    public string SerialNumber { get; init; }

    [ColumnSettings(15)]
    [Label(nameof(Resources.AssetNumber_InvNumber))]
    public string InvNumber { get; init; }

    [ColumnSettings(16)]
    [Label(nameof(Resources.AssetNumber_Code))]
    public string Code { get; init; }

    [ColumnSettings(17)]
    [Label(nameof(Resources.ServiceContractNumber))]
    public string ServiceContractNumber { get; init; }

    [ColumnSettings(18)]
    [Label(nameof(Resources.ServiceContractState))]
    public string ServiceContractLifeCycleStateName { get; init; }

    [ColumnSettings(19)]
    [Label(nameof(Resources.ServiceContractFinishDate))]
    public string ServiceContractUtcFinishDate { get; init; }

    [ColumnSettings(20)]
    [Label(nameof(Resources.AssetNumber_UserName))]
    public string UserName { get; init; }

    [ColumnSettings(21)]
    [Label(nameof(Resources.AssetNumber_OwnerName))]
    public string OwnerName { get; init; }

    [ColumnSettings(22)]
    [Label(nameof(Resources.AssetNumber_UtilizerName))]
    public string UtilizerName { get; init; }

    [ColumnSettings(23)]
    [Label(nameof(Resources.IPAddress))]
    public string IPAddress { get; init; }

    [ColumnSettings(24)]
    [Label(nameof(Resources.NetworkName))]
    public string ConfigurationUnitName { get; init; }

    public string FullObjectLocation { get; init; }
    public string FullObjectName { get; init; }

    public string ProductCatalogTemplateName => string.Empty;
    public string ServiceCenterName => string.Empty;
    public string SupplierName => string.Empty;
    public decimal? Cost => null;
    public string AppointmentDate => null;
    public string Warranty => null;
    public string DateReceived => null;
    public string DateInquiry => null;
    public string DateAnnuled => null;
}