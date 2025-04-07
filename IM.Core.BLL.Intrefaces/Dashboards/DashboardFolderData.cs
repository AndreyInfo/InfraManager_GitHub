using System;

namespace InfraManager.BLL.Dashboards;

public class DashboardFolderData
{
    public string Name { get; init; }
    public Guid? ParentDashboardFolderID { get; init; }
}
