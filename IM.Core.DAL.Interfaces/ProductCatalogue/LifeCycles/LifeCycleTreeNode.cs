using System;

namespace InfraManager.DAL.ProductCatalogue.LifeCycles;
public class LifeCycleTreeNode
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public ObjectClass? ClassID { get; init; }

    public Guid? ParentID { get; init; }

    public bool HasChild { get; init; }

    public bool FullSelect { get; init; }

    public bool PartSelect { get; init; }
}
