using System;

namespace InfraManager.BLL.Dashboards;

public sealed class DashboardTreeItemDetails
{
    public Guid ID { get; init; }
    public Guid? ParentID { get; init; }
    public string Name { get; init; }
    public bool HasChilds { get; init; }
    public bool IsDashboard { get; init; }
}