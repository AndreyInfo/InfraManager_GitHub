using InfraManager.DAL.Software;
using System;

namespace InfraManager.BLL.Software.SoftwareModelDependencies;

public class SoftwareModelDependencyDetails
{
    public Guid ParentSoftwareModelID { get; init; }
    public Guid ChildSoftwareModelID { get; init; }
    public SoftwareModelDependencyType Type { get; init; }
}
