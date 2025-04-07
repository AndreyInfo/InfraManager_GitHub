namespace InfraManager.BLL.AccessManagement.Operations;

public class RoleOperationNode<TID> where TID : struct
{
    public TID ID { get; init; }

    public string Name { get; init; }

    public string Note { get; init; }

    public ObjectClass? ClassID { get; init; } 

    public TID? ParentID { get; init; }
    
    public bool HasChild { get; set; }

    public bool FullSelect { get; set; }

    public bool PartSelect { get; set; }
}
