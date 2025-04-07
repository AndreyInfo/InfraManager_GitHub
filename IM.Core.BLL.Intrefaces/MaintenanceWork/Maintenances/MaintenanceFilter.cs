using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;

namespace InfraManager.BLL.MaintenanceWork.Maintenances;

public class MaintenanceFilter : BaseFilter
{
    public Guid? FolderID { get; init; }
}
