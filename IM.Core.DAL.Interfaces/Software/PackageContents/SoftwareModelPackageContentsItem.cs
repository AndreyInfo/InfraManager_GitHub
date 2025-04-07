using System;

namespace InfraManager.DAL.Software.PackageContents;
public class SoftwareModelPackageContentsItem
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string Version { get; init; }
    public Guid ManufacturerID { get; init; }
    public string ManufacturerName { get; init; }
    public string SoftwareTypeName { get; init; }
    public SoftwareModelTemplate Template { get; init; }
    public string ExternalID { get; init; }
    public string Code { get; init; }
    public string? ModelRedaction { get; init; }
    public SoftwareModelLanguage LanguageID { get; init; }
    public string SoftwareModelUsingTypeName { get; init; }
    public int InstallationCount { get; init; }
}
