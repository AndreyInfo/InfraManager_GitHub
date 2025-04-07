using System;

namespace InfraManager.DAL.Software.Installation
{
    public partial class ViewSoftwareInstallation
    {
        public Guid Id { get; set; }
        public Guid SoftwareModelId { get; set; }
        public string SoftwareModelName { get; set; }
        public string SoftwareModelVersion { get; set; }
        public Guid SoftwareTypeId { get; set; }
        public Guid SoftwareModelUsingTypeId { get; set; }
        public string ManufacturerName { get; set; }
        public string SoftwareModelUsingTypeName { get; set; }
        public string SoftwareTypeName { get; set; }
        public Guid? CommercialModelId { get; set; }
        public string CommercialModelName { get; set; }
        public string CommercialModelVersion { get; set; }
        public Guid? CommercialTypeId { get; set; }
        public Guid? CommercialModelUsingTypeId { get; set; }
        public string CommercialManufacturerName { get; set; }
        public string CommercialModelUsingTypeName { get; set; }
        public string CommercialTypeName { get; set; }
        public string UniqueNumber { get; set; }
        public DateTime? InstallDate { get; set; }
        public string InstallPath { get; set; }
        public Guid DeviceId { get; set; }
        public int DeviceClassId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceOwnerName { get; set; }
        public string DeviceUtilizerName { get; set; }
        public string DeviceOrganizationName { get; set; }
        public Guid? SoftwareLicenceId { get; set; }
        public string SoftwareLicenceName { get; set; }
        public Guid? SoftwareLicenceSerialNumberId { get; set; }
        public Guid? SoftwareLicenceScheme { get; set; }
        public string SoftwareLicenceSchemeName { get; set; }
        public string SerialNumber { get; set; }
        public int? SoftwareExecutionCount { get; set; }
        public byte[] RowVersion { get; set; }
        public DateTime UtcDateCreated { get; set; }
        public DateTime? UtcDateLastDetected { get; set; }
        public byte State { get; set; }
    }
}