using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;

namespace InfraManager.BLL.MaintenanceWork.MaintenanceDependencies;

public class MaintenanceDependencyFilter : BaseFilter
{
    public Guid MaintenanceID { get; init; }
}
