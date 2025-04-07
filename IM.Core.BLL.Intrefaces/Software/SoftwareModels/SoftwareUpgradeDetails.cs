using System;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.DAL.Software;

namespace InfraManager.BLL.Software.SoftwareModels;

public class SoftwareUpgradeDetails : SoftwareModelDetailsBase, ISoftwareModelGrouped, ISoftwareModelLanguaged, SoftwareModelLicensed
{
    public string? ModelRedaction { get; init; }
    public SoftwareModelLanguage LanguageID { get ; init; }
    public SoftwareModelLanguageDetails Language { get; set; }
    public SoftwareModelParentDetails Parent { get; init; }
    public Guid? OwnerModelID { get; init; }
    public int? OwnerModelClassID { get; init; }
    public Guid? GroupQueueID { get; set; }
    public Guid LicenseSchemeID { get; set; }
    public SoftwareLicenseSchemeDetails LicenseScheme { get; set; }
}
