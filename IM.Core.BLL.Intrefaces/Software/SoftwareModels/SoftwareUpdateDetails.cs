using InfraManager.BLL.Software.SoftwareModels.CommonDetails;

namespace InfraManager.BLL.Software.SoftwareModels;

public class SoftwareUpdateDetails : SoftwareModelDetailsBase
{
    public SoftwareModelParentDetails Parent { get; init; }
}
