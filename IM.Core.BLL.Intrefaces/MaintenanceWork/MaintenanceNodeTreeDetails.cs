using System;

namespace InfraManager.BLL.MaintenanceWork;

public class MaintenanceNodeTreeDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public Guid? ParentID { get; init; }
    public ObjectClass ClassID { get; init; }
    public byte[] RowVersion { get; init; }
    public bool HasChild { get; init; }
    public string IconName { get; init; }
}
