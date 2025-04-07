using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.DAL.Software;

namespace InfraManager.BLL.Software;

public interface ISoftwareModelLanguaged
{
    public SoftwareModelLanguage LanguageID { get; init; }
    public SoftwareModelLanguageDetails Language { get; set; }
}
