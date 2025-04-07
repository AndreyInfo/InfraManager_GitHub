using System;

namespace InfraManager.BLL.Dashboards;

public class DashboardFolderDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public Guid? ParentDashboardFolderID { get; init; }
}
