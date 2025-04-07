using InfraManager.BLL.Software.SoftwareLicenses;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.DAL.Software;
using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Software.SoftwareModels;

public class SoftwareCommercialModelDetails : SoftwareModelDetailsBase, ISoftwareModelGrouped, ISoftwareModelLanguaged, SoftwareModelLicensed
{
    public Guid SoftwareModelUsingTypeID { get; init; }
    public SoftwareModelUsingTypeDetails SoftwareModelUsingType { get; init; }
    public string? ModelRedaction { get; init; }
    public Guid? OwnerModelID { get; init; }
    public int? OwnerModelClassID { get; init; }
    public Guid? GroupQueueID { get; set; }
    public int VersionRecognitionID { get; init; }
    public int VersionRecognitionLvl { get; init; }
    public int RedactionRecognition { get; init; }
    public SoftwareModelLanguage LanguageID { get ; init; }
    public SoftwareModelLanguageDetails Language { get ; set; }
    public Guid LicenseSchemeID { get; set; }
    public SoftwareLicenseSchemeDetails LicenseScheme { get; set; }
}
