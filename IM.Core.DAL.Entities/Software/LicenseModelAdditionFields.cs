using System;

namespace InfraManager.DAL.Software;

public class LicenseModelAdditionFields
{
    public Guid SoftwareModelID { get; set; }
    public int? LicenseControlID { get; init; }
    public SoftwareModelLanguage? LanguageID { get; init; }

    public virtual SoftwareModel SoftwareModel { get; set; }
}
