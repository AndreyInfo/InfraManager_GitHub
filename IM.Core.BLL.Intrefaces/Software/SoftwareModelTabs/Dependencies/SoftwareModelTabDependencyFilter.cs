using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.Software;
using System;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Dependencies;

public class SoftwareModelTabDependencyFilter : BaseFilter
{
    public Guid? ID { get; init; }
    public SoftwareModelDependencyType Type { get; init; }
}
