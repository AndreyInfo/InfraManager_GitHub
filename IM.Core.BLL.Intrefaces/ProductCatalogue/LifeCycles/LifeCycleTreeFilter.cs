using System;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles;

public class LifeCycleTreeFilter
{
    public Guid? ParentID { get; init; }
    public ObjectClass ClassID { get; init; }
    public Guid? RoleID { get; init; }
}
