using System;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
public class LifeCycleCommandAlertData
{
    public LifeCycleCommandAlertType LifeCycleCommandAlertType { get; init; }
    public Guid? ComponentID { get; init; }
    public bool? IsSkip { get; init; }
    public bool? IsReplace { get; init; }
    public bool? IsCancel { get; init; }
}
