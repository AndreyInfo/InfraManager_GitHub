using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;

namespace InfraManager.BLL.Dashboards;

public class DashboardListFilter : BaseFilter
{
    public Guid? FolderID { get; init; }

    public bool ByAccess { get; init; }
}
