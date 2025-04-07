using System;

namespace InfraManager.BLL.Software.SoftwareModelTabs.PackageContents
{
    public class SoftwareModelPackageContentsListItemDetails
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string Version { get; init; }
        public Guid ManufacturerID { get; init; }
        public string ManufacturerName { get; set; }
        public string SoftwareTypeName { get; init; }
        public string TemplateName { get; init; }
        public string ExternalID { get; init; }
        public string Code { get; init; }
        public string? ModelRedaction { get; init; }
        public string LanguageName { get; init; }
        public string SoftwareModelUsingTypeName { get; init; }
        public int InstallationCount { get; set; }
    }
}
