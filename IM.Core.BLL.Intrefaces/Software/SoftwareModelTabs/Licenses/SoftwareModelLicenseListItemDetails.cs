using System;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Licenses;

public class SoftwareModelLicenseListItemDetails
{
    public Guid ID { get; init; }
    public Guid SoftwareModelID { get; init; }
    public string SoftwareModelName { get; init; }
    public string SoftwareModelVersion { get; init; }
    public Guid ManufacturerID { get; init; }
    public string ManufacturerName { get; set; }
    public string SoftwareTypeName { get; init; }
    public Guid SoftwareLicenseSchemeID { get; init; }
    public byte? SoftwareLicenceSchemeEnum { get; init; }
    public string SoftwareLicenseSchemeName { get; set; }
    public string Name { get; init; }
    public string Note { get; init; }
    public Guid? ServiceContractID { get; set; }
    public DateTime? UtcFinishDate { get; set; }
    public string InventoryNumber { get; init; }
    public DateTime? BeginDate { get; init; }
    public DateTime? EndDate { get; init; }
    public int? Count { get; set; }
    public DateTime? Warranty { get; set; }
    public string Supplier { get; set; }
    public string UserName { get; set; }
    public string OwnerName { get; set; }
    public string UtilizerName { get; set; }
    public string LifeCycleStateName { get; set; }
    public decimal? Cost { get; set; }
    public int RoomIntID { get; init; }
    public string OrganizationName { get; set; }
    public string BuildingName { get; set; }
    public string FloorName { get; set; }
    public string RoomName { get; set; }
    public int InUseReferenceCount { get; set; }
    public int Balance { get; set; }
    public string LicenceTypeName { get; set; }
    public int? LimitInDays { get; set; }
    public string ProductCatalogTypeName { get; set ;}
    public string SoftwareLicenceTypeModelName { get; set; }
}
