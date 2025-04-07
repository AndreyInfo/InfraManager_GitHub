using System;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
public class LifeCycleCommandResultItem
{
    public LifeCycleCommandResultItem()
    {
        
    }
    public LifeCycleCommandResultItem(Guid? lifeCycleStateID)
    {
        LifeCycleStateID = lifeCycleStateID;
    }

    public Guid? LocationID { get; init; }
    public ObjectClass? LocationClassID { get; init; }
    public Guid? LifeCycleStateID { get; set; }
}
