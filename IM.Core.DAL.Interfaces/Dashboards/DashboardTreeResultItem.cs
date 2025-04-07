using System;

namespace InfraManager.DAL.Dashboards;

public sealed class DashboardTreeResultItem
{
    public Guid ID { get; init; }
    public Guid? ParentFolderID { get; init; }
    public string Name { get; init; }
    public bool HasChilds { get; init; }
    public bool IsDashboard { get; init; }
}