using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Licenses;

[ListViewItem(ListView.SoftwareModelLicenseForTable)]
public class SoftwareModelLicenseForTable
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.SoftwareModelName))]
    public string SoftwareModelName { get; set; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.SoftwareModelVersion))]
    public string SoftwareModelVersion { get; set; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.SoftwareLicence_ManufacturerName))]
    public string ManufacturerName { get; set; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.SoftwareTypeName))]
    public string SoftwareTypeName { get; set; }

    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.LicenceScheme))]
    public string SoftwareLicenseSchemeName { get; set; }

    [ColumnSettings(6, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; set; }

    [ColumnSettings(7, 100)]
    [Label(nameof(Resources.Note))]
    public string Note { get; set; }

    [ColumnSettings(8, 100)]
    [Label(nameof(Resources.ServiceContractNumber))]
    public string ServiceContractID { get; set; }

    [ColumnSettings(9, 100)]
    [Label(nameof(Resources.ServiceContractFinishDate))]
    public string UtcFinishDate { get; set; }

    [ColumnSettings(10, 100)]
    [Label(nameof(Resources.InventarNumber))]
    public string InventoryNumber { get; set; }

    [ColumnSettings(11, 100)]
    [Label(nameof(Resources.SoftwareLicence_BeginDate))]
    public string BeginDate { get; set; }

    [ColumnSettings(12, 100)]
    [Label(nameof(Resources.SoftwareLicence_EndDate))]
    public string EndDate { get; set; }

    [ColumnSettings(13, 100)]
    [Label(nameof(Resources.LicenceCount))]
    public string Count { get; set; }

    [ColumnSettings(14, 100)]
    [Label(nameof(Resources.Warranty))]
    public string Warranty { get; set; }

    [ColumnSettings(15, 100)]
    [Label(nameof(Resources.Supplier))]
    public string Supplier { get; set; }

    [ColumnSettings(16, 100)]
    [Label(nameof(Resources.MOL))]
    public string UserName { get; set; }

    [ColumnSettings(17, 100)]
    [Label(nameof(Resources.Owner))]
    public string OwnerName { get; set; }

    [ColumnSettings(18, 100)]
    [Label(nameof(Resources.UtilizerName))]
    public string UtilizerName { get; set; }

    [ColumnSettings(19, 100)]
    [Label(nameof(Resources.LifeCycleState))]
    public string LifeCycleStateName { get; set; }

    [ColumnSettings(20, 100)]
    [Label(nameof(Resources.ContractLicence_Cost))]
    public string Cost { get; set; }

    [ColumnSettings(21, 100)]
    [Label(nameof(Resources.Organization_Name))]
    public string OrganizationName { get; set; }

    [ColumnSettings(22, 100)]
    [Label(nameof(Resources.AssetNumber_BuildingName))]
    public string BuildingName { get; set; }

    [ColumnSettings(23, 100)]
    [Label(nameof(Resources.AssetNumber_FloorName))]
    public string FloorName { get; set; }

    [ColumnSettings(24, 100)]
    [Label(nameof(Resources.AssetNumber_RoomName))]
    public string RoomName { get; set; }

    [ColumnSettings(25, 100)]
    [Label(nameof(Resources.InUseReferenceCount))]
    public string InUseReferenceCount { get; set; }

    [ColumnSettings(26, 100)]
    [Label(nameof(Resources.Balance))]
    public string Balance { get; set; }

    [ColumnSettings(27, 100)]
    [Label(nameof(Resources.LicenceType))]
    public string LicenceTypeName { get; set; }

    [ColumnSettings(28, 100)]
    [Label(nameof(Resources.SoftwareLicence_LimitInDays))]
    public string LimitInDays { get; set; }

    [ColumnSettings(29, 100)]
    [Label(nameof(Resources.SoftwareLicence_ModelType))]
    public string SoftwareLicenceTypeModelName { get; set; }
}
