namespace InfraManager.DAL.Operations;

public class OperationListItem
{
    public OperationID ID { get; init; }

    public string Name { get; init; }
    
    public string Description { get; init; }

    public OperationID ObjectID { get; init; }

    public bool IsSelect { get; set; }
    
    public string ObjectName { get; set; }

}