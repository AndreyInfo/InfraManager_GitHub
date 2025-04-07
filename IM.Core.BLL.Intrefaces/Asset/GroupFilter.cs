using InfraManager.BLL.ServiceDesk;

namespace InfraManager.BLL.Asset;

public class GroupFilter : ExecutorListFilter
{
    public bool IsCall { get; init; }
    public bool IsWorkOrder { get; init; }
    public bool IsProblem { get; init; }
    public bool IsChangeRequest { get; init; }
    public bool IsMassiveIncident { get; init; }

    public bool IsNeedValidateType => IsCall || IsWorkOrder || IsProblem || IsChangeRequest || IsMassiveIncident;
    public bool IsAll => IsCall && IsWorkOrder && IsProblem && IsChangeRequest && IsMassiveIncident;
    public GroupTypeData GetGroupTypeData => new(IsCall, IsWorkOrder, IsProblem, IsChangeRequest, IsMassiveIncident);
}