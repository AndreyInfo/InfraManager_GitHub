using InfraManager.DAL.Asset;
using System;


namespace InfraManager.DAL.Software.Licenses;
public class SoftwareModelLicenseItem
{
    public Guid ID { get; init; }
    public Guid SoftwareModelID { get; init; }
    public string SoftwareModelName { get; init; }
    public string SoftwareModelVersion { get; init; }
    public Guid ManufacturerID { get; init; }
    public string ManufacturerName { get; init; }
    public string SoftwareTypeName { get; init; }
    public Guid SoftwareLicenseSchemeID { get; init; }
    public byte? SoftwareLicenceSchemeEnum { get; init; }
    public string SoftwareLicenseSchemeName { get; init; }
    public string Name { get; init; }
    public string Note { get; init; }
    public Guid? ServiceContractID { get; init; }
    public DateTime? UtcFinishDate { get; init; }
    public string InventoryNumber { get; init; }
    public DateTime? BeginDate { get; init; }
    public DateTime? EndDate { get; init; }
    public int? Count { get; init; }
    public DateTime? Warranty { get; init; }
    public string Supplier { get; init; }
    public string UserName { get; init; }
    public string OwnerName { get; init; }
    public string UtilizerName { get; init; }
    public string LifeCycleStateName { get; init; }
    public decimal? Cost { get; init; }
    public int RoomIntID { get; init; }
    public Guid? RoomIMObjID { get; init; }
    public string OrganizationName { get; set; }
    public string BuildingName { get; set; }
    public string FloorName { get; set; }
    public string RoomName { get; set; }
    public int InUseReferenceCount { get; set; }
    public int Balance { get; set; }
    public LicenceType LicenceType { get; init; }
    public string LicenceTypeName { get; init; }
    public int? LimitInDays { get; init; }
    public string ProductCatalogTypeName { get; init; }
    public string SoftwareLicenceTypeModelName { get; set; }
}
