using System;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
public class LifeCycleCommandAlertDetails : LifeCycleCommandResultItem
{
    public LifeCycleCommandAlertDetails(Guid? lifeCycleStateID) : base(lifeCycleStateID)
    {
    }

    public LifeCycleCommandAlertType? LifeCycleCommandAlertType { get; init; }
    public Guid? ComponentID { get; init; }
}
