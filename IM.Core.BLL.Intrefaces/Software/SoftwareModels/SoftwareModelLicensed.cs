using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using System;

namespace InfraManager.BLL.Software.SoftwareModels;

public interface SoftwareModelLicensed
{
    public Guid LicenseSchemeID { get; set; }
    public SoftwareLicenseSchemeDetails LicenseScheme { get; set; }

}
