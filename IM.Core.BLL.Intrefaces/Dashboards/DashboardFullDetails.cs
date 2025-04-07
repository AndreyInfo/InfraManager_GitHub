using System;

namespace InfraManager.BLL.Dashboards;

public class DashboardFullDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public Guid? DashboardFolderID { get; init; }
    public ObjectClass ObjectClassID { get; init; }
    public string Data { get; set; }
}
