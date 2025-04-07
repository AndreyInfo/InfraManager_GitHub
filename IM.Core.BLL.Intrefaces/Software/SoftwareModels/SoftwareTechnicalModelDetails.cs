using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.DAL.Software;
using System;

namespace InfraManager.BLL.Software.SoftwareModels;

public class SoftwareTechnicalModelDetails : SoftwareModelDetailsBase, ISoftwareModelLanguaged
{
    public string? ModelRedaction { get; init; }
    public SoftwareModelLanguage LanguageID { get ; init; }
    public SoftwareModelLanguageDetails Language { get; set; }
    public Guid? CommercialModelID { get; init; }
    public CommercialModelDetails CommercialModel { get; set; }
    public string? ModelDistribution { get; init; }
    public string ProcessNames { get; init; }
}
