using System;

namespace InfraManager.BLL.Dashboards;

public class DashboardData
{
    public string Name { get; init; }
    public Guid? DashboardFolderID { get; init; }
    public ObjectClass ObjectClassID { get; init; }

}
