using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.Software;

namespace InfraManager.BLL.Software.SoftwareModelDependencies;

public class SoftwareModelDependencyFilter : BaseFilter
{
    public SoftwareModelDependencyType? softwareModelDependencyType { get; init; }
}
