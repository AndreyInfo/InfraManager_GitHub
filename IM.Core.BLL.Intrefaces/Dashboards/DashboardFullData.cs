using System;

namespace InfraManager.BLL.Dashboards;

public class DashboardFullData
{
    public string Name { get; init; }
    public Guid? DashboardFolderID { get; init; }
    public ObjectClass ObjectClassID { get; init; }
    public string Data { get; set; }
}
